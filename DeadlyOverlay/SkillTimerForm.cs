using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using System.Reflection;

namespace POExileDirection
{
    public partial class SkillTimerForm : Form
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private int m_nExStyleNum = -20;
        private const uint WS_EX_LAYERED = 0x00080000;
        private const uint WS_EX_TRANSPARENT = 0x00000020;
        private const int LWA_ALPHA = 0x2;
        private const int LWA_COLORKEY = 0x1;

        private int nMoving = 0;
        private int nMovePosX = 0;
        private int nMovePosY = 0;

        public int nSkillNumber = 0;
        public double lnSkillTimer = 0.0;
        private double lnMaxValue = 0.0;

        public SkillTimerForm()
        {
            InitializeComponent();
            Text = "DeadlyTradeForPOE";
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var Params = base.CreateParams;
                Params.ExStyle |= 0x80;
                return Params;
            }
        }

        private void SkillTimerForm_Load(object sender, EventArgs e)
        {
            Visible = false;
            this.StartPosition = FormStartPosition.Manual;

            uint exstyleGet = InteropCommon.GetWindowLong(this.Handle, m_nExStyleNum);
            uint nRet = InteropCommon.SetWindowLong(this.Handle, m_nExStyleNum, exstyleGet | WS_EX_LAYERED | WS_EX_TRANSPARENT);

            #region ⨌⨌ Get Information from ConfigPath.ini ⨌⨌
            string strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath.ini");

            if (LauncherForm.resolution_width < 1920 && LauncherForm.resolution_height < 1080)
            {
                strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath_1600_1024.ini");
                if (LauncherForm.resolution_width < 1600 && LauncherForm.resolution_height < 1024)
                    strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath_1280_768.ini");
                else if (LauncherForm.resolution_width < 1280)
                    strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath_LOW.ini");
            }
            else if (LauncherForm.resolution_width > 1920)
                strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath_HIGH.ini");

            IniParser parser = new IniParser(strINIPath);
            log.Info($"{MethodBase.GetCurrentMethod().Name} RESOLUTION : " + strINIPath);

            try
            {
                /*
                [SKILL]
                SKILLTIME2=9.5
                SKILLTIME5=9.5
                TOGGLESKILL3ON=FALSE
                TOGGLESKILL1ON=FALSE
                SKILLTIME1=9.5
                SKILLTIME4=9.5
                TOGGLESKILL5ON=FALSE
                SKILLTIME3=9.5
                TOGGLESKILL2ON=FALSE
                TOGGLESKILL4ON=FALSE
                SKILL1LEFT=314
                SKILL1TOP=1019
                SKILL1COLOR=255,0,255
                SKILL2LEFT=364
                SKILL2TOP=1019
                SKILL2COLOR=255,0,255
                SKILL3LEFT=404
                SKILL3TOP=1019
                SKILL3COLOR=255,0,255
                SKILL4LEFT=454
                SKILL4TOP=1019
                SKILL4COLOR=255,0,255
                SKILL5LEFT=494
                SKILL5TOP=1019
                SKILL5COLOR=255,0,255
                */
                string sLeft = string.Empty;
                string sTop = string.Empty;
                string sColor = string.Empty;
                if (nSkillNumber == 1)
                {
                    sLeft = parser.GetSetting("SKILL", "SKILL1LEFT");
                    sTop = parser.GetSetting("SKILL", "SKILL1TOP");
                    sColor = parser.GetSetting("SKILL", "SKILL1COLOR");
                }
                else if (nSkillNumber == 2)
                {
                    sLeft = parser.GetSetting("SKILL", "SKILL2LEFT");
                    sTop = parser.GetSetting("SKILL", "SKILL2TOP");
                    sColor = parser.GetSetting("SKILL", "SKILL2COLOR");
                }
                else if (nSkillNumber == 3)
                {
                    sLeft = parser.GetSetting("SKILL", "SKILL3LEFT");
                    sTop = parser.GetSetting("SKILL", "SKILL3TOP");
                    sColor = parser.GetSetting("SKILL", "SKILL3COLOR");
                }
                else if (nSkillNumber == 4)
                {
                    sLeft = parser.GetSetting("SKILL", "SKILL4LEFT");
                    sTop = parser.GetSetting("SKILL", "SKILL4TOP");
                    sColor = parser.GetSetting("SKILL", "SKILL4COLOR");
                }
                else if (nSkillNumber == 5)
                {
                    sLeft = parser.GetSetting("SKILL", "SKILL5LEFT");
                    sTop = parser.GetSetting("SKILL", "SKILL5TOP");
                    sColor = parser.GetSetting("SKILL", "SKILL5COLOR");
                }

                Left = Convert.ToInt32(sLeft);
                Top = Convert.ToInt32(sTop);

                xuiGauge1.FilledColor = StringRGBToColor(sColor);
                //xuiGauge1.ForeColor = Color.Red;

            }
            catch
            {
                MSGForm frmMSG = new MSGForm();
                frmMSG.lbMsg.Text = "SKILL 환경 파일을 읽을 수 없습니다.\r\n\r\nini 파일이 손상되었거나 삭제되었습니다.";
                frmMSG.ShowDialog();
            }
            #endregion

            lnMaxValue = lnSkillTimer;
            //xuiGauge1.MaxValue = Convert.ToInt32(lnSkillTimer);
            xuiGauge1.Percentage = 100;

            BringToFront();

            timer1.Start();
            Visible = true;
        }

        private Color StringRGBToColor(string color)
        {
            var arrColorFragments = color?.Split(',').Select(sFragment => { int.TryParse(sFragment, out int fragment); return fragment; }).ToArray();

            switch (arrColorFragments?.Length)
            {
                case 3:
                    return Color.FromArgb(255, arrColorFragments[0], arrColorFragments[1], arrColorFragments[2]);
                case 4:
                    return Color.FromArgb(arrColorFragments[0], arrColorFragments[1], arrColorFragments[2], arrColorFragments[3]);
                default:
                    return Color.Transparent;
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!LauncherForm.g_pinLOCK)
                {
                    // Reverse Style
                    uint exstyleGet = InteropCommon.GetWindowLong(this.Handle, m_nExStyleNum);
                    InteropCommon.SetWindowLong(this.Handle, m_nExStyleNum, exstyleGet & ~(WS_EX_LAYERED | WS_EX_TRANSPARENT));
                    Width = 40;
                    Height = 32;
                }
                else
                {
                    // Set Style : Can't Click (Layered Transparent)
                    uint exstyleGet = InteropCommon.GetWindowLong(this.Handle, m_nExStyleNum);
                    InteropCommon.SetWindowLong(this.Handle, m_nExStyleNum, exstyleGet | WS_EX_LAYERED | WS_EX_TRANSPARENT);
                    Width = 40;
                    Height = 32;
                }

                /*using (Graphics gr = xuiGauge1.CreateGraphics())
                {
                    Font f = new Font(FontFamily.GenericSerif, 10);
                    SizeF len = gr.MeasureString(lnSkillTimer.ToString("N1"), f);
                    Point location = new Point(Convert.ToInt32((xuiGauge1.Width / 2) - len.Width / 2), 
                                               Convert.ToInt32((xuiGauge1.Height / 2) - len.Height / 2));
                    gr.DrawString(lnSkillTimer.ToString("N1"), f, Brushes.Red, location);
                }*/

                //
                int nPercent = Convert.ToInt32(lnSkillTimer / lnMaxValue * 100);
                lbTime.Text = lnSkillTimer.ToString("N1");
                xuiGauge1.Percentage = nPercent;

                lnSkillTimer = lnSkillTimer - 0.1; // 100ms
                if (lnSkillTimer <= 0.0)
                {
                    timer1.Stop();

                    switch (nSkillNumber)
                    {
                        case 1:
                            ControlForm.frmSkillK1 = null;
                            break;
                        case 2:
                            ControlForm.frmSkillK2 = null;
                            break;
                        case 3:
                            ControlForm.frmSkillK3 = null;
                            break;
                        case 4:
                            ControlForm.frmSkillK4 = null;
                            break;
                        case 5:
                            ControlForm.frmSkillK5 = null;
                            break;
                        default:
                            break;
                    }
                    xuiGauge1.Dispose();
                    xuiGauge1 = null;
                    this.Dispose();
                    this.BeginInvoke(new MethodInvoker(Close)); // Close();
                }
            }
            catch
            {
                ;
            }
        }

        private void XuiGauge1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!LauncherForm.g_pinLOCK)
            {
                nMoving = 1;
                nMovePosX = e.X;
                nMovePosY = e.Y;
            }
        }

        private void XuiGauge1_MouseMove(object sender, MouseEventArgs e)
        {
            if (nMoving == 1)
            {
                this.SetDesktopLocation(MousePosition.X - nMovePosX, MousePosition.Y - nMovePosY);
            }
        }

        private void XuiGauge1_MouseUp(object sender, MouseEventArgs e)
        {
            nMoving = 0;

            string strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath.ini");

            if (LauncherForm.resolution_width < 1920 && LauncherForm.resolution_height < 1080)
            {
                strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath_1600_1024.ini");
                if (LauncherForm.resolution_width < 1600 && LauncherForm.resolution_height < 1024)
                    strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath_1280_768.ini");
                else if (LauncherForm.resolution_width < 1280)
                    strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath_LOW.ini");
            }
            else if (LauncherForm.resolution_width > 1920)
                strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath_HIGH.ini");

            IniParser parser = new IniParser(strINIPath);
            log.Info($"{MethodBase.GetCurrentMethod().Name} RESOLUTION : " + strINIPath);

            switch (nSkillNumber)
            {
                case 1:
                    parser.AddSetting("SKILL", "SKILL1LEFT", Left.ToString());
                    parser.AddSetting("SKILL", "SKILL1TOP", Top.ToString());
                    break;
                case 2:
                    parser.AddSetting("SKILL", "SKILL2LEFT", Left.ToString());
                    parser.AddSetting("SKILL", "SKILL2TOP", Top.ToString());
                    break;
                case 3:
                    parser.AddSetting("SKILL", "SKILL3LEFT", Left.ToString());
                    parser.AddSetting("SKILL", "SKILL3TOP", Top.ToString());
                    break;
                case 4:
                    parser.AddSetting("SKILL", "SKILL4LEFT", Left.ToString());
                    parser.AddSetting("SKILL", "SKILL4TOP", Top.ToString());
                    break;
                case 5:
                    parser.AddSetting("SKILL", "SKILL5LEFT", Left.ToString());
                    parser.AddSetting("SKILL", "SKILL5TOP", Top.ToString());
                    break;
                default:
                    break;
            }

            parser.SaveSettings();
        }

        private void SkillTimerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                switch (nSkillNumber)
                {
                    case 1:
                        ControlForm.frmSkillK1 = null;
                        break;
                    case 2:
                        ControlForm.frmSkillK2 = null;
                        break;
                    case 3:
                        ControlForm.frmSkillK3 = null;
                        break;
                    case 4:
                        ControlForm.frmSkillK4 = null;
                        break;
                    case 5:
                        ControlForm.frmSkillK5 = null;
                        break;
                    default:
                        break;
                }

                if(xuiGauge1 != null) xuiGauge1.Dispose();
                this.Dispose();
            }
            catch
            {
                ;
            }
        }
    }
}
