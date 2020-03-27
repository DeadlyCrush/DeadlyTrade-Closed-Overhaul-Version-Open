using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POExileDirection
{
    public partial class toggleForm : Form
    {
        private int nMoving = 0;
        private int nMovePosX = 0;
        private int nMovePosY = 0;

        public int nToggleNumber = 0;
        public static bool bisON { get; set; }

        public toggleForm()
        {
            InitializeComponent();
        }

        private void ToggleForm_Load(object sender, EventArgs e)
        {
            Visible = false;
            this.StartPosition = FormStartPosition.Manual;

            bisON = false;

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
                if (nToggleNumber == 1)
                {
                    sLeft = parser.GetSetting("MISC", "TOGGLE1LEFT");
                    sTop = parser.GetSetting("MISC", "TOGGLE1TOP");
                    if (LauncherForm.g_bToggle1 == true)
                    {
                        pictureBox1.Image = Properties.Resources.Toggle_ON_new;
                        bisON = true;
                    }
                }
                else if (nToggleNumber == 2)
                {
                    sLeft = parser.GetSetting("MISC", "TOGGLE2LEFT");
                    sTop = parser.GetSetting("MISC", "TOGGLE2TOP");
                    if (LauncherForm.g_bToggle2 == true)
                    {
                        pictureBox1.Image = Properties.Resources.Toggle_ON_new;
                        bisON = true;
                    }
                }
                else if (nToggleNumber == 3)
                {
                    sLeft = parser.GetSetting("MISC", "TOGGLE3LEFT");
                    sTop = parser.GetSetting("MISC", "TOGGLE3TOP");
                    if (LauncherForm.g_bToggle3 == true)
                    {
                        pictureBox1.Image = Properties.Resources.Toggle_ON_new;
                        bisON = true;
                    }
                }
                else if (nToggleNumber == 4)
                {
                    sLeft = parser.GetSetting("MISC", "TOGGLE4LEFT");
                    sTop = parser.GetSetting("MISC", "TOGGLE4TOP");
                    if (LauncherForm.g_bToggle4 == true)
                    {
                        pictureBox1.Image = Properties.Resources.Toggle_ON_new;
                        bisON = true;
                    }
                }
                else if (nToggleNumber == 5)
                {
                    sLeft = parser.GetSetting("MISC", "TOGGLE5LEFT");
                    sTop = parser.GetSetting("MISC", "TOGGLE5TOP");
                    if (LauncherForm.g_bToggle5 == true)
                    {
                        pictureBox1.Image = Properties.Resources.Toggle_ON_new;
                        bisON = true;
                    }
                }

                Left = Convert.ToInt32(sLeft);
                Top = Convert.ToInt32(sTop);
            }
            catch
            {
                MSGForm frmMSG = new MSGForm();
                frmMSG.lbMsg.Text = "FLASK 환경 파일을 읽을 수 없습니다. Can't Read Flask Timer Config.\r\n\r\n" +
                    "ini 파일이 손상되었거나 삭제되었습니다. ini File is corrupted or deleted.";
                frmMSG.ShowDialog();
            }

            toolTip1.SetToolTip(this.pictureBox1, "더블클릭 : ON/OFF (Double Click : ON/OFF)");

            Visible = true;
            BringToFront();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // turn on WS_EX_TOOLWINDOW style bit
                cp.ExStyle |= 0x80;
                return cp;
            }
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks > 1)
            {
                if (!bisON)
                {
                    pictureBox1.Image = Properties.Resources.Toggle_ON_new;
                    bisON = true;
                }
                else
                {
                    pictureBox1.Image = Properties.Resources.Toggle_OFF_new;
                    bisON = false;
                }

                string strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath.ini");

                if (LauncherForm.resolution_width < 1920 && LauncherForm.resolution_height < 1080)
                {
                    strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath_1600_1024.ini");
                    if (LauncherForm.resolution_width < 1600 && LauncherForm.resolution_height < 1024)
                        strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath_1280_768.ini");
                }
                IniParser parser = new IniParser(strINIPath);

                switch (nToggleNumber)
                {
                    case 1:
                        parser.AddSetting("MISC", "TOGGLE1ON", bisON.ToString());
                        LauncherForm.g_bToggle1 = bisON;
                        break;
                    case 2:
                        parser.AddSetting("MISC", "TOGGLE2ON", bisON.ToString());
                        LauncherForm.g_bToggle2 = bisON;
                        break;
                    case 3:
                        parser.AddSetting("MISC", "TOGGLE3ON", bisON.ToString());
                        LauncherForm.g_bToggle3 = bisON;
                        break;
                    case 4:
                        parser.AddSetting("MISC", "TOGGLE4ON", bisON.ToString());
                        LauncherForm.g_bToggle4 = bisON;
                        break;
                    case 5:
                        parser.AddSetting("MISC", "TOGGLE5ON", bisON.ToString());
                        LauncherForm.g_bToggle5 = bisON;
                        break;
                    default:
                        break;
                }

                parser.SaveSettings();

                this.Hide();
                this.Show();
            }
            else
            {
                if (!LauncherForm.g_pinLOCK)
                {
                    nMoving = 1;
                    nMovePosX = e.X;
                    nMovePosY = e.Y;
                }
            }
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (nMoving == 1)
            {
                this.SetDesktopLocation(MousePosition.X - nMovePosX, MousePosition.Y - nMovePosY);
            }
        }

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
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

            switch (nToggleNumber)
            {
                case 1:
                    parser.AddSetting("MISC", "TOGGLE1LEFT", Left.ToString());
                    parser.AddSetting("MISC", "TOGGLE1TOP", Top.ToString());
                    break;
                case 2:
                    parser.AddSetting("MISC", "TOGGLE2LEFT", Left.ToString());
                    parser.AddSetting("MISC", "TOGGLE2TOP", Top.ToString());
                    break;
                case 3:
                    parser.AddSetting("MISC", "TOGGLE3LEFT", Left.ToString());
                    parser.AddSetting("MISC", "TOGGLE3TOP", Top.ToString());
                    break;
                case 4:
                    parser.AddSetting("MISC", "TOGGLE4LEFT", Left.ToString());
                    parser.AddSetting("MISC", "TOGGLE4TOP", Top.ToString());
                    break;
                case 5:
                    parser.AddSetting("MISC", "TOGGLE5LEFT", Left.ToString());
                    parser.AddSetting("MISC", "TOGGLE5TOP", Top.ToString());
                    break;
                default:
                    break;
            }

            parser.SaveSettings();
        }
    }
}
