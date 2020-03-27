using Newtonsoft.Json;
using Ninja_Price.API.PoeNinja;
using Ninja_Price.API.PoeNinja.Classes;
using POExileDirection.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace POExileDirection
{
    public partial class ControlForm : Form
    {
        #region ⨌⨌ DllImport for Invoke ⨌⨌
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("User32.Dll", EntryPoint = "PostMessageA")]
        static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        static extern byte VkKeyScan(char ch);

        [DllImport("imm32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr ImmGetContext(IntPtr hWnd);

        [DllImport("imm32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr ImmReleaseContext(IntPtr hWnd, IntPtr hImc);

        [DllImport("imm32.dll", CharSet = CharSet.Auto)]
        public static extern bool ImmGetConversionStatus(IntPtr hImc, out int fdwConversion, out int fdwSentence);

        [DllImport("imm32.dll")]
        public static extern bool ImmSetConversionStatus(IntPtr hIMC, int fdwConversion, int fdwSentence);
        #endregion

        public const string WINDOW_NAME = "Path of Exile"; // POE Window Title
        IntPtr handlePOE = FindWindow(null, WINDOW_NAME);
        IntPtr g_thisH;// = FindWindow(null, "POExileDirection");

        // Hot Keys
        fsModifiers m_unMod = 0;
        Keys m_HotKey = 0;
        const int WM_MOUSEWHEEL = 0x020A;
        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;
        const int WM_CTRL = 0x11;
        const int VK_LEFT = 0x25;
        const int VK_RIGHT = 0x26;

        int nMoving = 0;
        int nMovePosX = 0;
        int nMovePosY = 0;

        string currentDirectory = null;

        MainForm frmMainForm = new MainForm();
        bool bMainFormActivated = false;

        ImageOverlayForm frmIMGOverlay = null; // new ImageOverlayForm();
        public static bool bIMGOvelayActivated = false;

        ImageOverlayFormAlva frmIMGOverlayALVA = null; // new ImageOverlayFormAlva();
        public static bool bIMGOvelayActivatedALVA = false;

        ImageOverlayFormMap frmIMGOverlayMAP = null; // new ImageOverlayFormMap();
        public static bool bIMGOvelayActivatedMAP = false;

        string[] strImagePath = new string[3];

        static public string keyMAINRemains;// = "특수키없음 + F2";
        static public string keyMAINJUN;// = "특수키없음 + F3";
        static public string keyMAINALVA;// = "특수키없음 + F4";
        static public string keyMAINZANA;// = "특수키없음 + F6";

        OverlayHotkeys ovHRemains = new OverlayHotkeys();
        OverlayHotkeys ovHJUN = new OverlayHotkeys();
        OverlayHotkeys ovHALVA = new OverlayHotkeys();
        OverlayHotkeys ovHZANA = new OverlayHotkeys();

        GlobalLowLevelHooks.MouseHook mouseHook = new GlobalLowLevelHooks.MouseHook();
        GlobalLowLevelHooks.KeyboardHook keyHook = new GlobalLowLevelHooks.KeyboardHook();
        bool IsControlKeyDown = false;

        const int IME_CMODE_ALPHANUMERIC = 0;
        const int IME_CMODE_KOR = 1;

        NinjaForm frmNinja = new NinjaForm();
        public static bool bShowNinja = false;

        bool bIsMinimized = false;

        public ControlForm()
        {
            InitializeComponent();
            this.Text = "POExileDirection";

            Init_Controls();
            currentDirectory = Application.StartupPath;
        }

        public string g_strISKAKAOUSER = "YES";

        #region ⨌⨌ Init. Controls. ⨌⨌
        public void Init_Controls()
        {
            // btnClose
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.BackColor = Color.Transparent;
            btnClose.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnClose.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnClose.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.TabStop = false;

            //
            button1.FlatStyle = FlatStyle.Flat;
            button1.BackColor = Color.Transparent;
            button1.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button1.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button1.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            button1.FlatAppearance.BorderSize = 0;
            button1.TabStop = false;
            //
            button2.FlatStyle = FlatStyle.Flat;
            button2.BackColor = Color.Transparent;
            button2.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button2.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button2.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            button2.FlatAppearance.BorderSize = 0;
            button2.TabStop = false;
            //
            button3.FlatStyle = FlatStyle.Flat;
            button3.BackColor = Color.Transparent;
            button3.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button3.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button3.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            button3.FlatAppearance.BorderSize = 0;
            button3.TabStop = false;
            //
            button4.FlatStyle = FlatStyle.Flat;
            button4.BackColor = Color.Transparent;
            button4.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button4.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button4.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            button4.FlatAppearance.BorderSize = 0;
            button4.TabStop = false;
            //
            button5.FlatStyle = FlatStyle.Flat;
            button5.BackColor = Color.Transparent;
            button5.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button5.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button5.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            button5.FlatAppearance.BorderSize = 0;
            button5.TabStop = false;
            //
            button6.FlatStyle = FlatStyle.Flat;
            button6.BackColor = Color.Transparent;
            button6.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button6.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button6.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            button6.FlatAppearance.BorderSize = 0;
            button6.TabStop = false;
        }
        #endregion

        private void ControlForm_Load(object sender, EventArgs e)
        {
            DialogResult rsKakao = MessageBox.Show("카카오 클라이언트 유저이신가요?", "DeadlyCrush", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rsKakao == DialogResult.Yes)
                g_strISKAKAOUSER = "YES";
            else
                g_strISKAKAOUSER = "NO";

            Init_ControlFormPosition();

            Register_HotKeys();

            mouseHook.Install();
            mouseHook.MouseWheel += MouseHook_MouseWheel;

            keyHook.Install();
            keyHook.KeyDown += KeyHook_KeyDown;
            keyHook.KeyUp += KeyHook_KeyUp;

            // for ReadStream Client.txt
            frmMainForm.strMainFromKAKAOUSER = g_strISKAKAOUSER;
            frmMainForm.Show();
            frmMainForm.Hide();
        }

        #region ⨌⨌ Form Closed. - Dispose ⨌⨌
        private void ControlForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            UnregisterHotKey(g_thisH, 2);
            UnregisterHotKey(g_thisH, 3);
            UnregisterHotKey(g_thisH, 4);
            UnregisterHotKey(g_thisH, 6);

            btnClose.Dispose();
            button1.Dispose();
            button2.Dispose();
            button3.Dispose();
            button4.Dispose();
            button5.Dispose();
            button6.Dispose();

            frmMainForm.Dispose();
            if (frmIMGOverlay!=null) frmIMGOverlay.Dispose();
            if (frmIMGOverlayALVA != null) frmIMGOverlayALVA.Dispose();
            if (frmIMGOverlayMAP != null) frmIMGOverlayMAP.Dispose();

            mouseHook.MouseWheel -= MouseHook_MouseWheel;
            mouseHook.Uninstall();

            keyHook.KeyDown -= KeyHook_KeyDown;
            keyHook.KeyUp -= KeyHook_KeyUp;
            keyHook.Uninstall();

            if (frmNinja != null) frmNinja.Dispose();
        }
        #endregion

        #region ⨌⨌ Hooking - Mouse, Keyboard ⨌⨌
        private void KeyHook_KeyDown(GlobalLowLevelHooks.KeyboardHook.VKeys key)
        {
            if (key == GlobalLowLevelHooks.KeyboardHook.VKeys.LCONTROL)
            {
                IsControlKeyDown = true;
            }
        }

        private void KeyHook_KeyUp(GlobalLowLevelHooks.KeyboardHook.VKeys key)
        {
            if (key == GlobalLowLevelHooks.KeyboardHook.VKeys.LCONTROL)
            {
                IsControlKeyDown = false;
            }
        }

        private void MouseHook_MouseWheel(GlobalLowLevelHooks.MouseHook.MSLLHOOKSTRUCT mouseStruct)
        {
            if (mouseStruct.mouseData.ToString() == "7864320")
            {
                // Up
                if (IsControlKeyDown)
                {
                    if (handlePOE != null)
                    {
                        // CTRL + ←
                        SetForegroundWindow(handlePOE);
                        SendKeys.Send("^{LEFT}");
                        
                        /*PostMessage(handlePOE, WM_KEYDOWN, WM_CTRL, 0);
                        PostMessage(handlePOE, 0x0111, VK_LEFT, 0);
                        PostMessage(handlePOE, WM_KEYUP, WM_CTRL, 0);
                        PostMessage(handlePOE, WM_KEYUP, VK_LEFT, 0);*/
                    }
                }
            }

            // VK_RIGHT : 0x26
            IntPtr vkRight = (IntPtr)0x26;
            if (mouseStruct.mouseData.ToString() == "4287102976")
            {
                // Down
                if (IsControlKeyDown)
                {
                    if (handlePOE != null)
                    {
                        // CTRL + →
                        SetForegroundWindow(handlePOE);
                        SendKeys.Send("^{RIGHT}");

                        /*PostMessage(handlePOE, WM_KEYDOWN, WM_CTRL, 0);
                        PostMessage(handlePOE, WM_KEYDOWN, VK_RIGHT, 0);
                        PostMessage(handlePOE, WM_KEYUP, WM_CTRL, 0);
                        PostMessage(handlePOE, WM_KEYUP, VK_RIGHT, 0);*/
                    }
                }
            }
        }
        #endregion

        #region ⨌⨌ Register Hot Keys ⨌⨌
        public void Register_HotKeys()
        {
            g_thisH = FindWindow(null, "POExileDirection");

            Parse_StringToHotKey(keyMAINRemains);
            bool bRetHOT = false;
            bRetHOT = RegisterHotKey(g_thisH, 2, (uint)m_unMod, (uint)m_HotKey);
            if (!bRetHOT)
            {
                MSGForm frmMSG = new MSGForm();
                frmMSG.lbMsg.Text = "단축키 설정에 실패하였습니다.\r\n\r\n단축키를 제외한 다른 기능은 정상작동합니다.";
                frmMSG.ShowDialog();
            }
            ovHRemains.fsMod = m_unMod;
            ovHRemains.hotKeys = m_HotKey;

            Parse_StringToHotKey(keyMAINJUN);
            bRetHOT = false;
            bRetHOT = RegisterHotKey(g_thisH, 3, (uint)m_unMod, (uint)m_HotKey);
            if (!bRetHOT)
            {
                MSGForm frmMSG = new MSGForm();
                frmMSG.lbMsg.Text = "단축키 설정에 실패하였습니다.\r\n\r\n단축키를 제외한 다른 기능은 정상작동합니다.";
                frmMSG.ShowDialog();
            }
            ovHJUN.fsMod = m_unMod;
            ovHJUN.hotKeys = m_HotKey;

            Parse_StringToHotKey(keyMAINALVA);
            bRetHOT = false;
            bRetHOT = RegisterHotKey(g_thisH, 4, (uint)m_unMod, (uint)m_HotKey);
            if (!bRetHOT)
            {
                MSGForm frmMSG = new MSGForm();
                frmMSG.lbMsg.Text = "단축키 설정에 실패하였습니다.\r\n\r\n단축키를 제외한 다른 기능은 정상작동합니다.";
                frmMSG.ShowDialog();
            }
            ovHALVA.fsMod = m_unMod;
            ovHALVA.hotKeys = m_HotKey;

            Parse_StringToHotKey(keyMAINZANA);
            bRetHOT = false;
            bRetHOT = RegisterHotKey(g_thisH, 6, (uint)m_unMod, (uint)m_HotKey);
            if (!bRetHOT)
            {
                MSGForm frmMSG = new MSGForm();
                frmMSG.lbMsg.Text = "단축키 설정에 실패하였습니다.\r\n\r\n단축키를 제외한 다른 기능은 정상작동합니다.";
                frmMSG.ShowDialog();
            }
            ovHZANA.fsMod = m_unMod;
            ovHZANA.hotKeys = m_HotKey;
        }

        public void Parse_StringToHotKey(string text)
        {
            fsModifiers Modifier = fsModifiers.None;
            Keys key = 0;

            bool HasControl = false;
            bool HasAlt = false;
            bool HasShift = false;

            string[] result;
            string[] separators = new string[] { " + " };
            result = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            //Iterate through the keys and find the modifier.
            foreach (string entry in result)
            {
                //Find the Control Key.
                if (entry.Trim() == Keys.Control.ToString())
                {
                    HasControl = true;
                }
                //Find the Alt key.
                if (entry.Trim() == Keys.Alt.ToString())
                {
                    HasAlt = true;
                }
                //Find the Shift key.
                if (entry.Trim() == Keys.Shift.ToString())
                {
                    HasShift = true;
                }
            }

            if (HasControl) { Modifier |= fsModifiers.Control; }
            if (HasAlt) { Modifier |= fsModifiers.Alt; }
            if (HasShift) { Modifier |= fsModifiers.Shift; }

            //Get the last key in the shortcut
            KeysConverter keyconverter = new KeysConverter();
            key = (Keys)keyconverter.ConvertFrom(result.GetValue(result.Length - 1));

            m_HotKey = key;
            m_unMod = Modifier;
        }
        #endregion

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312)
            {
                Keys keyHot = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                fsModifiers modifier = (fsModifiers)((int)m.LParam & 0xFFFF);

                if (modifier == ovHRemains.fsMod && keyHot == ovHRemains.hotKeys)
                {
                    SetForegroundWindow(handlePOE);
                    Get_Remaining();
                }

                if (modifier == ovHJUN.fsMod && keyHot == ovHJUN.hotKeys)
                {
                    if (!bIMGOvelayActivated)
                        frmIMGOverlay = new ImageOverlayForm();
                    frmIMGOverlay.m_strImagePath = strImagePath[0];
                    frmIMGOverlay.nZoom = 0;
                    frmIMGOverlay.Load_Image();
                    IMGOverlayForm_Show_Hide((int)OVERLAY_WHAT.OVER_JUN);
                }

                if (modifier == ovHALVA.fsMod && keyHot == ovHALVA.hotKeys)
                {
                    if (!bIMGOvelayActivatedALVA)
                        frmIMGOverlayALVA = new ImageOverlayFormAlva();
                    frmIMGOverlayALVA.m_strImagePath = strImagePath[1];
                    frmIMGOverlayALVA.nZoom = 0;
                    frmIMGOverlayALVA.Load_Image();
                    IMGOverlayForm_Show_Hide((int)OVERLAY_WHAT.OVER_ALVA);
                }

                if (modifier == ovHZANA.fsMod && keyHot == ovHZANA.hotKeys)
                {
                    if (!bIMGOvelayActivatedMAP)
                        frmIMGOverlayMAP = new ImageOverlayFormMap();
                    frmIMGOverlayMAP.m_strImagePath = strImagePath[2];
                    frmIMGOverlayMAP.nZoom = 0;
                    frmIMGOverlayMAP.Load_Image();
                    IMGOverlayForm_Show_Hide((int)OVERLAY_WHAT.OVER_MAP);
                }
            }

            base.WndProc(ref m);
        }

        public void ForceHideOverlay()
        {
            bIMGOvelayActivated = false;
            frmIMGOverlay.Hide();
        }

        public void Get_Remaining()
        {
            /*IntPtr hIMC = ImmGetContext(handlePOE);
            
            // Force to English
            ImmSetConversionStatus(hIMC, IME_CMODE_ALPHANUMERIC, 0);
            SendKeys.Send("{Enter}/remaining{Enter}");
            ImmReleaseContext(handlePOE, hIMC);
            */
            InputSimulator iSim = new InputSimulator();

            // Need to press ALT because the SetForegroundWindow sometimes does not work
            iSim.Keyboard.KeyPress(VirtualKeyCode.MENU);

            // Make POE the foreground application and send input
            SetForegroundWindow(handlePOE);

            iSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);

            // Send the input
            iSim.Keyboard.TextEntry("/remaining");

            // Send RETURN
            iSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);

            iSim = null;
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            Go_HideOut();
        }

        public void Go_HideOut()
        {
            InputSimulator iSim = new InputSimulator();

            // Need to press ALT because the SetForegroundWindow sometimes does not work
            // RM_2017_0713 iSim.Keyboard.KeyPress(VirtualKeyCode.MENU);

            // Make POE the foreground application and send input
            SetForegroundWindow(handlePOE);

            iSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);

            // Send the input
            iSim.Keyboard.TextEntry("/hideout");

            // Send RETURN
            iSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);

            iSim = null;
        }

        #region ⨌⨌ Init. Form Location ⨌⨌
        public void Init_ControlFormPosition()
        {
            string strINIPath = String.Format("{0}\\{1}", currentDirectory, "ConfigPath.ini");
            IniParser parser = new IniParser(strINIPath);

            try
            {
                string sLeft = parser.GetSetting("LOCATIONMAIN", "LEFT");
                string sTop = parser.GetSetting("LOCATIONMAIN", "TOP");

                if (sLeft != "CENTER" && sTop != "CENTER")
                {
                    this.StartPosition = FormStartPosition.Manual;
                    this.Left = Int32.Parse(sLeft);
                    this.Top = Int32.Parse(sTop);
                }

                string strPath = "";
                strPath = parser.GetSetting("DIRECTIONHELPER", "POELOGPATH");

                // Get Image Path
                strImagePath[0] = parser.GetSetting("OVERLAY", "JUN"); // @".\DeadlyInform\Betrayal.png";   // JUN
                if(strImagePath[0]=="")
                    strImagePath[0] = @".\DeadlyInform\Betrayal.png";

                strImagePath[1] = parser.GetSetting("OVERLAY", "ALVA"); // @".\DeadlyInform\Incursion.png";  // ALVA
                if (strImagePath[1] == "")
                    strImagePath[1] = @".\DeadlyInform\Incursion.png";

                strImagePath[2] = parser.GetSetting("OVERLAY", "ZANA"); // @".\DeadlyInform\Atlas.png";      // ZANA
                if (strImagePath[2] == "")
                    strImagePath[2] = @".\DeadlyInform\Atlas.png";

                // HOT KEYS
                keyMAINRemains = parser.GetSetting("HOTKEY", "R");
                keyMAINJUN = parser.GetSetting("HOTKEY", "J");
                keyMAINALVA = parser.GetSetting("HOTKEY", "A");
                keyMAINZANA = parser.GetSetting("HOTKEY", "Z");

                if (File.Exists(strPath))
                {
                    ;
                }
                else
                {
                    MSGForm frmMSG = new MSGForm();
                    frmMSG.lbMsg.Text = "패스 오브 엑자일 경로가 맞지 않습니다.\r\n경로를 설정해주세요.";
                    frmMSG.ShowDialog();
                    FolderBrowserDialog dlgFolder = new FolderBrowserDialog();
                    DialogResult dr = dlgFolder.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        if(g_strISKAKAOUSER == "YES")
                            strPath = String.Format("{0}\\{1}", dlgFolder.SelectedPath, "logs\\KakaoClient.txt");
                        else
                            strPath = String.Format("{0}\\{1}", dlgFolder.SelectedPath, "logs\\Client.txt");
                    }
                    else
                    {
                        this.Close();
                        Environment.Exit(0);
                    }

                    // Set Ini.
                    parser.AddSetting("DIRECTIONHELPER", "POELOGPATH", strPath);
                    parser.SaveSettings();
                }
            }
            catch
            {
                MSGForm frmMSG = new MSGForm();
                frmMSG.lbMsg.Text = "환경 파일을 읽을 수 없습니다.\r\n\r\nini 파일이 손상되었거나 삭제되었습니다.";
                frmMSG.ShowDialog();
                this.Close();
                Environment.Exit(0);
            }
        }
        #endregion

        private void Button1_Click(object sender, EventArgs e)
        {
            // Quest Helper
            MainForm_Show_Hide("DEFAULT");
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (!bIMGOvelayActivated)
                frmIMGOverlay = new ImageOverlayForm();
            // JUN
            frmIMGOverlay.m_strImagePath = strImagePath[0];
            frmIMGOverlay.nZoom = 0;
            frmIMGOverlay.Load_Image();
            IMGOverlayForm_Show_Hide((int)OVERLAY_WHAT.OVER_JUN);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (!bIMGOvelayActivatedALVA)
                frmIMGOverlayALVA = new ImageOverlayFormAlva();
            // ALVA
            frmIMGOverlayALVA.m_strImagePath = strImagePath[1];
            frmIMGOverlayALVA.nZoom = 0;
            frmIMGOverlayALVA.Load_Image();
            IMGOverlayForm_Show_Hide((int)OVERLAY_WHAT.OVER_ALVA);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            if (!bIMGOvelayActivatedMAP)
                frmIMGOverlayMAP = new ImageOverlayFormMap();
            // ZANA
            frmIMGOverlayMAP.m_strImagePath = strImagePath[2];
            frmIMGOverlayMAP.nZoom = 0;
            frmIMGOverlayMAP.Load_Image();
            IMGOverlayForm_Show_Hide((int)OVERLAY_WHAT.OVER_MAP);
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            // TO DO
            if (!bShowNinja)
            {
                DialogResult rsGet = MessageBox.Show("데이터를 수신합니다. 약간의 시간이 소요됩니다.\r\n진행하시겠습니까?", "DeadlyCrush", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (rsGet == DialogResult.Yes)
                {
                    frmNinja = new NinjaForm();
                    bShowNinja = true;
                    frmNinja.Show();
                }
                else
                    return;
            }
            else
            {
                bShowNinja = false;
                frmNinja.Close();
            }
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            // TO DO
            SettingsForm frmSettings = new SettingsForm();
                frmSettings.keyRemains = keyMAINRemains;
                frmSettings.keyJUN = keyMAINJUN;
                frmSettings.keyALVA = keyMAINALVA;
                frmSettings.keyZANA = keyMAINZANA;
            if (frmSettings.ShowDialog() == DialogResult.OK)
            {
                keyMAINRemains = frmSettings.keyRemains;
                keyMAINJUN = frmSettings.keyJUN;
                keyMAINALVA = frmSettings.keyALVA;
                keyMAINZANA = frmSettings.keyZANA;

                UnregisterHotKey(g_thisH, 2);
                UnregisterHotKey(g_thisH, 3);
                UnregisterHotKey(g_thisH, 4);
                UnregisterHotKey(g_thisH, 6);

                Register_HotKeys();

                string strINIPath = String.Format("{0}\\{1}", currentDirectory, "ConfigPath.ini");
                IniParser parser = new IniParser(strINIPath);
                parser.AddSetting("HOTKEY", "R", keyMAINRemains);
                parser.AddSetting("HOTKEY", "J", keyMAINJUN);
                parser.AddSetting("HOTKEY", "A", keyMAINALVA);
                parser.AddSetting("HOTKEY", "Z", keyMAINZANA);
                parser.SaveSettings();
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            Environment.Exit(0);
        }

        private void PanelTop_MouseDown(object sender, MouseEventArgs e)
        {
            nMoving = 1;
            nMovePosX = e.X;
            nMovePosY = e.Y;
        }

        private void PanelTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (nMoving == 1)
            {
                this.SetDesktopLocation(MousePosition.X - nMovePosX, MousePosition.Y - nMovePosY);
            }
        }

        private void PanelTop_MouseUp(object sender, MouseEventArgs e)
        {
            nMoving = 0;
            string strINIPath = String.Format("{0}\\{1}", currentDirectory, "ConfigPath.ini");
            IniParser parser = new IniParser(strINIPath);
            parser.AddSetting("LOCATIONMAIN", "LEFT", this.Left.ToString());
            parser.AddSetting("LOCATIONMAIN", "TOP", this.Top.ToString());
            parser.SaveSettings();
        }

        public void MainForm_Show_Hide(string strDoShow)
        {
            SetForegroundWindow(handlePOE);
            if (strDoShow == "Y")
            {
                bMainFormActivated = true;
                frmMainForm.Show();
            }
            else if (strDoShow == "N")
            {
                bMainFormActivated = false;
                frmMainForm.Hide();
            }
            else
            {
                if (!bMainFormActivated)
                {
                    bMainFormActivated = true;
                    frmMainForm.Show();
                }
                else
                {
                    bMainFormActivated = false;
                    frmMainForm.Hide();
                }
            }
        }

        #region ⨌⨌ Image Overlay Show/Hide ⨌⨌
        public void IMGOverlayForm_Show_Hide(int nWhat)
        {
            SetForegroundWindow(handlePOE);

            switch (nWhat)
            {
                case (int)OVERLAY_WHAT.OVER_JUN:
                    if (!bIMGOvelayActivated)
                    {
                        bIMGOvelayActivated = true;
                        frmIMGOverlay.Show();
                    }
                    else
                    {
                        bIMGOvelayActivated = false;
                        frmIMGOverlay.Close();
                    }
                    break;
                case (int)OVERLAY_WHAT.OVER_ALVA:
                    if (!bIMGOvelayActivatedALVA)
                    {
                        bIMGOvelayActivatedALVA = true;
                        frmIMGOverlayALVA.Show();
                    }
                    else
                    {
                        bIMGOvelayActivatedALVA = false;
                        frmIMGOverlayALVA.Close();
                    }
                    break;
                case (int)OVERLAY_WHAT.OVER_MAP:
                    if (!bIMGOvelayActivatedMAP)
                    {
                        bIMGOvelayActivatedMAP = true;
                        frmIMGOverlayMAP.Show();
                    }
                    else
                    {
                        bIMGOvelayActivatedMAP = false;
                        frmIMGOverlayMAP.Close();
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion

        private void BtnMinimize_Click(object sender, EventArgs e)
        {
            if (bIsMinimized == false)
            {
                bIsMinimized = true;

                this.Size = new Size(56, 66);

                this.btnClose.Hide();
                this.btnMinimize.Location = new Point(35, 0);
                this.btnMinimize.BackgroundImage = Properties.Resources.sysMaxPOEBg;
            }
            else
            {
                bIsMinimized = false;

                this.Size = new Size(336, 66);
                this.btnClose.Show();
                this.btnMinimize.Location = new Point(295, 0);
                this.btnMinimize.BackgroundImage = Properties.Resources.sysMinPOEBg1;
            }
        }
    }
}
