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
using WindowsInput;
using WindowsInput.Native;

namespace POExileDirection
{
    public partial class HideoutForm : Form
    {
        #region ⨌⨌ DllImport for Invoke ⨌⨌
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        #endregion

        private int nMoving = 0;
        private int nMovePosX = 0;
        private int nMovePosY = 0;

        public HideoutForm()
        {
            InitializeComponent();
        }

        private void HideoutForm_Load(object sender, EventArgs e)
        {
            Visible = false;
            this.StartPosition = FormStartPosition.Manual;

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
                string sLeft = parser.GetSetting("MISC", "HIDEOUTLEFT");
                string sTop = parser.GetSetting("MISC", "HIDEOUTTOP");

                Left = Convert.ToInt32(sLeft);
                Top = Convert.ToInt32(sTop);
            }
            catch
            {
                MSGForm frmMSG = new MSGForm();
                frmMSG.lbMsg.Text = "환경 파일을 읽을 수 없습니다.\r\n\r\nini 파일이 손상되었거나 삭제되었습니다.";
                frmMSG.ShowDialog();
            }

            toolTip1.SetToolTip(this.pictureBox1, "더블클릭 : 은신처로 이동 (Double Click : Visit Hideout)");

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

        private void HideoutForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks > 1)
            {
                IntPtr handlePOE = IntPtr.Zero;
                handlePOE = FindWindow("POEWindowClass", "Path of Exile"); // ClassName = POEWindowClass

                SetForegroundWindow(handlePOE);

                InputSimulator iSim = new InputSimulator();

                iSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);

                iSim.Keyboard.TextEntry("/hideout");

                // Send RETURN
                iSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);

                iSim = null;

                SetForegroundWindow(handlePOE);
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

        private void HideoutForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (nMoving == 1)
            {
                this.SetDesktopLocation(MousePosition.X - nMovePosX, MousePosition.Y - nMovePosY);
            }
        }

        private void HideoutForm_MouseUp(object sender, MouseEventArgs e)
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
            parser.AddSetting("MISC", "HIDEOUTLEFT", Left.ToString());
            parser.AddSetting("MISC", "HIDEOUTTOP", Top.ToString());
            parser.SaveSettings();
        }
    }
}
