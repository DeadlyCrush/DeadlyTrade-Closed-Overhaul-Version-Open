using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POExileDirection
{
    public partial class FlaskTimerCircleForm : Form
    {
        private int nMoving = 0;
        private int nMovePosX = 0;
        private int nMovePosY = 0;

        public int nFlaskNumber = 0;
        public double lnFlaskTimer = 0.0;

        public FlaskTimerCircleForm()
        {
            InitializeComponent();
        }

        private void FlaskTimerCircleForm_Load(object sender, EventArgs e)
        {
            Visible = false;
            this.StartPosition = FormStartPosition.Manual;

            #region ⨌⨌ Get Information from ConfigPath.ini ⨌⨌
            string strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath.ini");

            if (LauncherForm.resolution_width < 1920 && LauncherForm.resolution_height < 1080)
            {
                strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath_1600_1024.ini");
                if (LauncherForm.resolution_width < 1600 && LauncherForm.resolution_height < 1024)
                    strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath_1280_768.ini");
            }
            IniParser parser = new IniParser(strINIPath);

            try
            {
                string sLeft = string.Empty;
                string sTop = string.Empty;
                string sColor = string.Empty;
                if (nFlaskNumber == 1)
                {
                    sLeft = parser.GetSetting("MISC", "FLASK1LEFT");
                    sTop = parser.GetSetting("MISC", "FLASK1TOP");
                    sColor = parser.GetSetting("MISC", "FLASK1COLOR");
                }
                else if (nFlaskNumber == 2)
                {
                    sLeft = parser.GetSetting("MISC", "FLASK2LEFT");
                    sTop = parser.GetSetting("MISC", "FLASK2TOP");
                    sColor = parser.GetSetting("MISC", "FLASK2COLOR");
                }
                else if (nFlaskNumber == 3)
                {
                    sLeft = parser.GetSetting("MISC", "FLASK3LEFT");
                    sTop = parser.GetSetting("MISC", "FLASK3TOP");
                    sColor = parser.GetSetting("MISC", "FLASK3COLOR");
                }
                else if (nFlaskNumber == 4)
                {
                    sLeft = parser.GetSetting("MISC", "FLASK4LEFT");
                    sTop = parser.GetSetting("MISC", "FLASK4TOP");
                    sColor = parser.GetSetting("MISC", "FLASK4COLOR");
                }
                else if (nFlaskNumber == 5)
                {
                    sLeft = parser.GetSetting("MISC", "FLASK5LEFT");
                    sTop = parser.GetSetting("MISC", "FLASK5TOP");
                    sColor = parser.GetSetting("MISC", "FLASK5COLOR");
                }

                Left = Convert.ToInt32(sLeft);
                Top = Convert.ToInt32(sTop);

                circularProgressBar1.ProgressColor = StringRGBToColor(sColor);

            }
            catch
            {
                MSGForm frmMSG = new MSGForm();
                frmMSG.lbMsg.Text = "FLASK 환경 파일을 읽을 수 없습니다.\r\n\r\nini 파일이 손상되었거나 삭제되었습니다.";
                frmMSG.ShowDialog();
            }
            #endregion

            circularProgressBar1.Maximum = Convert.ToInt32(lnFlaskTimer);

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
            lnFlaskTimer = lnFlaskTimer - 0.1; // 100ms
            if (lnFlaskTimer < 0.1)
            {
                timer1.Stop();
                Close();
            }

            circularProgressBar1.Text = lnFlaskTimer.ToString("N1");
            circularProgressBar1.Value = Convert.ToInt32(lnFlaskTimer);
            panel1.Invalidate();
        }

        private void CircularProgressBar1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!LauncherForm.g_pinLOCK)
            {
                nMoving = 1;
                nMovePosX = e.X;
                nMovePosY = e.Y;
            }
        }

        private void CircularProgressBar1_MouseMove(object sender, MouseEventArgs e)
        {
            if (nMoving == 1)
            {
                this.SetDesktopLocation(MousePosition.X - nMovePosX, MousePosition.Y - nMovePosY);
            }
        }

        private void CircularProgressBar1_MouseUp(object sender, MouseEventArgs e)
        {
            nMoving = 0;

            string strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath.ini");

            if (LauncherForm.resolution_width < 1920 && LauncherForm.resolution_height < 1080)
            {
                strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath_1600_1024.ini");
                if (LauncherForm.resolution_width < 1600 && LauncherForm.resolution_height < 1024)
                    strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath_1280_768.ini");
            }
            IniParser parser = new IniParser(strINIPath);

            switch (nFlaskNumber)
            {
                case 1:
                    parser.AddSetting("MISC", "FLASK1LEFT", Left.ToString());
                    parser.AddSetting("MISC", "FLASK1TOP", Top.ToString());
                    break;
                case 2:
                    parser.AddSetting("MISC", "FLASK2LEFT", Left.ToString());
                    parser.AddSetting("MISC", "FLASK2TOP", Top.ToString());
                    break;
                case 3:
                    parser.AddSetting("MISC", "FLASK3LEFT", Left.ToString());
                    parser.AddSetting("MISC", "FLASK3TOP", Top.ToString());
                    break;
                case 4:
                    parser.AddSetting("MISC", "FLASK4LEFT", Left.ToString());
                    parser.AddSetting("MISC", "FLASK4TOP", Top.ToString());
                    break;
                case 5:
                    parser.AddSetting("MISC", "FLASK5LEFT", Left.ToString());
                    parser.AddSetting("MISC", "FLASK5TOP", Top.ToString());
                    break;
                default:
                    break;
            }

            parser.SaveSettings();
        }
    }
}
