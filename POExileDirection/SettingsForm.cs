using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using System.Reflection;

namespace POExileDirection
{
    public partial class SettingsForm : Form
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public string fsRemains;
        public string fsJUN;
        public string fsALVA;
        public string fsZANA;
        public string fsHideout; // hideout

        public string keyRemains;
        public string keyJUN;
        public string keyALVA;
        public string keyZANA;
        public string keyHideout; // hideout
        public string keySearchbyPosition;
        public string keyEXIT;

        public string colorStringRGB1;
        public string colorStringRGB2;
        public string colorStringRGB3;
        public string colorStringRGB4;
        public string colorStringRGB5;

        public string colorStringRGBQ;
        public string colorStringRGBW;
        public string colorStringRGBE;
        public string colorStringRGBR;
        public string colorStringRGBT;

        public SettingsForm()
        {
            InitializeComponent();
            Text = "DeadlyTradeForPOE";
            this.ShowInTaskbar = false;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            Visible = false;

            this.BackColor = Color.Wheat;
            this.TransparencyKey = Color.Wheat;
            this.TopMost = true;
            //this.FormBorderStyle = FormBorderStyle.None;

            if(LauncherForm.g_strUILang=="ENG")
            {
                btnEXIT.Text = "/exit";
            }

            // HOTKEYS
            textRemains.Text = keyRemains;
            textJUN.Text = keyJUN;
            textALVA.Text = keyALVA;
            textZANA.Text = keyZANA;
            textHideout.Text = keyHideout; // Hideout
            textBoxPositionSearch.Text = keySearchbyPosition;
            textBoxEXIT.Text = keyEXIT;

            // FLASK and TRADING PANEL
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
                // FLASK
                string sColor1 = string.Empty;
                string sColor2 = string.Empty;
                string sColor3 = string.Empty;
                string sColor4 = string.Empty;
                string sColor5 = string.Empty;

                sColor1 = parser.GetSetting("MISC", "FLASK1COLOR");
                sColor2 = parser.GetSetting("MISC", "FLASK2COLOR");
                sColor3 = parser.GetSetting("MISC", "FLASK3COLOR");
                sColor4 = parser.GetSetting("MISC", "FLASK4COLOR");
                sColor5 = parser.GetSetting("MISC", "FLASK5COLOR");

                colorStringRGB1 = sColor1;
                colorStringRGB2 = sColor2;
                colorStringRGB3 = sColor3;
                colorStringRGB4 = sColor4;
                colorStringRGB5 = sColor5;

                panelCO1.BackColor = StringRGBToColor(sColor1);
                panelCO2.BackColor = StringRGBToColor(sColor2);
                panelCO3.BackColor = StringRGBToColor(sColor3);
                panelCO4.BackColor = StringRGBToColor(sColor4);
                panelCO5.BackColor = StringRGBToColor(sColor5);

                sColor1 = parser.GetSetting("SKILL", "SKILL1COLOR");
                sColor2 = parser.GetSetting("SKILL", "SKILL2COLOR");
                sColor3 = parser.GetSetting("SKILL", "SKILL3COLOR");
                sColor4 = parser.GetSetting("SKILL", "SKILL4COLOR");
                sColor5 = parser.GetSetting("SKILL", "SKILL5COLOR");

                colorStringRGBQ = sColor1;
                colorStringRGBW = sColor2;
                colorStringRGBE = sColor3;
                colorStringRGBR = sColor4;
                colorStringRGBT = sColor5;

                panelQ.BackColor = StringRGBToColor(sColor1);
                panelW.BackColor = StringRGBToColor(sColor2);
                panelE.BackColor = StringRGBToColor(sColor3);
                panelR.BackColor = StringRGBToColor(sColor4);
                panelT.BackColor = StringRGBToColor(sColor5);

                textBoxSEC1.Text = parser.GetSetting("MISC", "FLASKTIME1"); // LauncherForm.g_FlaskTime1
                textBoxSEC2.Text = parser.GetSetting("MISC", "FLASKTIME2"); // LauncherForm.g_FlaskTime2
                textBoxSEC3.Text = parser.GetSetting("MISC", "FLASKTIME3"); // LauncherForm.g_FlaskTime3
                textBoxSEC4.Text = parser.GetSetting("MISC", "FLASKTIME4"); // LauncherForm.g_FlaskTime4
                textBoxSEC5.Text = parser.GetSetting("MISC", "FLASKTIME5"); // LauncherForm.g_FlaskTime5

                labelFL1.Text = ((Keys)LauncherForm.g_Flask1).ToString();
                labelFL2.Text = ((Keys)LauncherForm.g_Flask2).ToString();
                labelFL3.Text = ((Keys)LauncherForm.g_Flask3).ToString();
                labelFL4.Text = ((Keys)LauncherForm.g_Flask4).ToString();
                labelFL5.Text = ((Keys)LauncherForm.g_Flask5).ToString();

                textBoxQ.Text = parser.GetSetting("SKILL", "SKILLTIME1");
                textBoxW.Text = parser.GetSetting("SKILL", "SKILLTIME2");
                textBoxE.Text = parser.GetSetting("SKILL", "SKILLTIME3");
                textBoxR.Text = parser.GetSetting("SKILL", "SKILLTIME4");
                textBoxT.Text = parser.GetSetting("SKILL", "SKILLTIME5");

                lbSkillQ.Text = ((Keys)LauncherForm.g_Skill1).ToString();
                lbSkillW.Text = ((Keys)LauncherForm.g_Skill2).ToString();
                lbSkillE.Text = ((Keys)LauncherForm.g_Skill3).ToString();
                lbSkillR.Text = ((Keys)LauncherForm.g_Skill4).ToString();
                lbSkillT.Text = ((Keys)LauncherForm.g_Skill5).ToString();

                // TRADING PANEL
                textBoxCharacterNick.Text = parser.GetSetting("CHARACTER", "MYNICK");
                if (parser.GetSetting("CHARACTER", "AUTOKICK") == "Y")
                    checkBoxAutoKick.Checked = true;
                else
                    checkBoxAutoKick.Checked = false;

                try
                {
                    List<DeadlyAtlas.NotifyMSGCollection> NotifyMSG =
                        LauncherForm.deadlyInformationData.InformationMSG.NotifyMSG.Where(retval => retval.Id == "THX").ToList();

                    textBoxDone.Text = NotifyMSG[0].Msg;
                }
                catch
                {
                    textBoxDone.Text = "thanks. gl hf~.";
                }

                try
                {
                    List<DeadlyAtlas.NotifyMSGCollection> NotifyMSG =
                        LauncherForm.deadlyInformationData.InformationMSG.NotifyMSG.Where(retval => retval.Id == "WILLING").ToList();

                    textBoxResend.Text = NotifyMSG[0].Msg;
                }
                catch
                {
                    textBoxResend.Text = "still willing to buy? 'Your Offer : '";
                }

                try
                {
                    List<DeadlyAtlas.NotifyMSGCollection> NotifyMSG =
                        LauncherForm.deadlyInformationData.InformationMSG.NotifyMSG.Where(retval => retval.Id == "WAIT").ToList();

                    textBoxWait.Text = NotifyMSG[0].Msg;
                }
                catch
                {
                    textBoxWait.Text = "wait a sec plz.";
                }

                try
                {
                    List<DeadlyAtlas.NotifyMSGCollection> NotifyMSG =
                        LauncherForm.deadlyInformationData.InformationMSG.NotifyMSG.Where(retval => retval.Id == "SOLD").ToList();

                    textBoxSold.Text = NotifyMSG[0].Msg;
                }
                catch
                {
                    textBoxSold.Text = "sold already. Sry.";
                }
            }
            catch
            {
                MSGForm frmMSG = new MSGForm();
                frmMSG.lbMsg.Text = "환경 파일을 읽을 수 없습니다.\r\n\r\nini 파일이 손상되었거나 삭제되었습니다.";
                frmMSG.ShowDialog();
            }

            panel1.Visible = true;
            panel2.Visible = false;
            panel3.Visible = false;
            panelDonateContact.Visible = false;
            panelHallOfFame.Visible = false;
            panelSkillMain.Visible = false;

            panel1.Left = 0;
            panel1.Top = 0;
            panel1.Width = 589;
            panel1.Height = 483;

            panel2.Left = 0;
            panel2.Top = 0;
            panel2.Width = 589;
            panel2.Height = 483;

            panel3.Left = 0;
            panel3.Top = 0;
            panel3.Width = 589;
            panel3.Height = 483;

            panelDonateContact.Left = 0;
            panelDonateContact.Top = 0;
            panelDonateContact.Width = 589;
            panelDonateContact.Height = 483;

            panelHallOfFame.Left = 0;
            panelHallOfFame.Top = 0;
            panelHallOfFame.Width = 589;
            panelHallOfFame.Height = 483;

            panelSkillMain.Left = 0;
            panelSkillMain.Top = 0;
            panelSkillMain.Width = 589;
            panelSkillMain.Height = 483;

            xuiSlider1.Percentage = LauncherForm.g_NotifyVolume;
            labelVolume.Text = "Volume = " + xuiSlider1.Percentage.ToString();

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

        private void BtnClose_Click(object sender, EventArgs e)
        {
            double dValidate1 = 0.0;
            double dValidate2 = 0.0;
            double dValidate3 = 0.0;
            double dValidate4 = 0.0;
            double dValidate5 = 0.0;

            // FLASK TIMER
            if (textBoxSEC1.Text.Length<0 || textBoxSEC2.Text.Length < 0 || textBoxSEC3.Text.Length < 0 || textBoxSEC4.Text.Length < 0 || textBoxSEC5.Text.Length < 0)
            {
                MSGForm frmMSG = new MSGForm();
                frmMSG.lbMsg.Text = "플라스크 타이머 시간(초)을 입력하세요.\r\n\r\nFlask Timer Sec. field value is Empty.";
                frmMSG.ShowDialog();

                return;
            }

            dValidate1 = Convert.ToDouble(textBoxSEC1.Text);
            dValidate2 = Convert.ToDouble(textBoxSEC2.Text);
            dValidate3 = Convert.ToDouble(textBoxSEC3.Text);
            dValidate4 = Convert.ToDouble(textBoxSEC4.Text);
            dValidate5 = Convert.ToDouble(textBoxSEC5.Text);

            if (dValidate1<0.1 || dValidate2 < 0.1 || dValidate3 < 0.1 || dValidate4 < 0.1 || dValidate5 < 0.1)
            {
                MSGForm frmMSG = new MSGForm();
                frmMSG.lbMsg.Text = "시간(초)은 0.1보다 커야합니다.\r\n\r\n'Sec.' field value must bigger than 0.1";
                frmMSG.ShowDialog();

                return;
            }

            // SKILL TIMER
            if (textBoxQ.Text.Length < 0 || textBoxW.Text.Length < 0 || textBoxE.Text.Length < 0 || textBoxR.Text.Length < 0 || textBoxT.Text.Length < 0)
            {
                MSGForm frmMSG = new MSGForm();
                frmMSG.lbMsg.Text = "스킬 타이머 시간(초)을 입력하세요.\r\n\r\nSkill Timer Sec. field value is Empty.";
                frmMSG.ShowDialog();

                return;
            }

            dValidate1 = 0.0;
            dValidate2 = 0.0;
            dValidate3 = 0.0;
            dValidate4 = 0.0;
            dValidate5 = 0.0;

            dValidate1 = Convert.ToDouble(textBoxQ.Text);
            dValidate2 = Convert.ToDouble(textBoxW.Text);
            dValidate3 = Convert.ToDouble(textBoxE.Text);
            dValidate4 = Convert.ToDouble(textBoxR.Text);
            dValidate5 = Convert.ToDouble(textBoxT.Text);

            if (dValidate1 < 0.1 || dValidate2 < 0.1 || dValidate3 < 0.1 || dValidate4 < 0.1 || dValidate5 < 0.1)
            {
                MSGForm frmMSG = new MSGForm();
                frmMSG.lbMsg.Text = "시간(초)은 0.1보다 커야합니다.\r\n\r\n'Sec.' field value must bigger than 0.1";
                frmMSG.ShowDialog();

                return;
            }

            // NICKNAME
            if(!String.IsNullOrEmpty(textBoxCharacterNick.Text))
                LauncherForm.g_strMyNickName = textBoxCharacterNick.Text;

            // NOTIFICATION MESSAGE
            /*
            public static string g_strnotiWAIT { get; set; }
            public static string g_strnotiSOLD { get; set; }
            public static string g_strnotiDONE { get; set; }
            public static string g_strnotiRESEND { get; set; }
            */
            if (textBoxWait.Text.Length > 0)
                LauncherForm.g_strnotiWAIT = textBoxWait.Text;

            if (textBoxSold.Text.Length > 0)
                LauncherForm.g_strnotiSOLD = textBoxSold.Text;

            if (textBoxDone.Text.Length > 0)
                LauncherForm.g_strnotiDONE = textBoxDone.Text;

            if (textBoxResend.Text.Length > 0)
                LauncherForm.g_strnotiRESEND = textBoxResend.Text;

            // AUTO KICK
            if(checkBoxAutoKick.Checked)
                LauncherForm.g_strTRAutoKick = "Y";
            else
                LauncherForm.g_strTRAutoKick = "N";

            btnClose.DialogResult = DialogResult.OK;
            this.Hide();
        }

        public void GetSet_HotKey(KeyEventArgs e, string strWhich)
        {
            e.SuppressKeyPress = true;  //Suppress the key from being processed by the underlying control.
            if (strWhich == "REMAINS")
                textRemains.Text = string.Empty;
            else if (strWhich == "JUN")
                textJUN.Text = string.Empty;
            else if (strWhich == "ALVA")
                textALVA.Text = string.Empty;
            else if (strWhich == "ZANA")
                textZANA.Text = string.Empty;
            else if (strWhich == "HIDEOUT")
                textHideout.Text = string.Empty;
            else if (strWhich == "SEARCH")
                textBoxPositionSearch.Text = string.Empty;
            else if (strWhich == "EXIT")
                textBoxEXIT.Text = string.Empty;

            //Set the backspace button to specify that the user does not want to use a shortcut.
            if (e.KeyData == Keys.Back)
            {
                return;
            }

            // A modifier is present. Process each modifier.
            // Modifiers are separated by a ",". So we'll split them and write each one to the textbox.
            foreach (string modifier in e.Modifiers.ToString().Split(new Char[] { ';' }))
            {
                if (!modifier.Equals("none", StringComparison.OrdinalIgnoreCase))
                {
                    if (strWhich == "REMAINS")
                        textRemains.Text = modifier.ToUpper() + ";" + e.KeyCode.ToString();
                    else if (strWhich == "JUN")
                        textJUN.Text = modifier.ToUpper() + ";" + e.KeyCode.ToString();
                    else if (strWhich == "ALVA")
                        textALVA.Text = modifier.ToUpper() + ";" + e.KeyCode.ToString();
                    else if (strWhich == "ZANA")
                        textZANA.Text = modifier.ToUpper() + ";" + e.KeyCode.ToString();
                    else if (strWhich == "HIDEOUT")
                        textHideout.Text = modifier.ToUpper() + ";" + e.KeyCode.ToString();
                    else if (strWhich == "SEARCH")
                        textBoxPositionSearch.Text = modifier.ToUpper() + ";" + e.KeyCode.ToString();
                    else if (strWhich == "EXIT")
                        textBoxEXIT.Text = modifier.ToUpper() + ";" + e.KeyCode.ToString();
                }
                else
                {
                    if (strWhich == "REMAINS")
                        textRemains.Text = "NONE;" + e.KeyCode.ToString();
                    else if (strWhich == "JUN")
                        textJUN.Text = "NONE;" + e.KeyCode.ToString();
                    else if (strWhich == "ALVA")
                        textALVA.Text = "NONE;" + e.KeyCode.ToString();
                    else if (strWhich == "ZANA")
                        textZANA.Text = "NONE;" + e.KeyCode.ToString();
                    else if (strWhich == "HIDEOUT")
                        textHideout.Text = "NONE;" + e.KeyCode.ToString();
                    else if (strWhich == "SEARCH")
                        textBoxPositionSearch.Text = "NONE;" + e.KeyCode.ToString();
                    else if (strWhich == "EXIT")
                        textBoxEXIT.Text = "NONE;" + e.KeyCode.ToString();
                }
            }
            if (e.KeyCode == Keys.ShiftKey | e.KeyCode == Keys.ControlKey | e.KeyCode == Keys.Menu)
            {
                if (strWhich == "REMAINS")
                    textRemains.Text = keyRemains;
                else if (strWhich == "JUN")
                    textJUN.Text = keyJUN;
                else if (strWhich == "ALVA")
                    textALVA.Text = keyALVA;
                else if (strWhich == "ZANA")
                    textZANA.Text = keyZANA;
                else if (strWhich == "HIDEOUT")
                    textHideout.Text = keyHideout;
                else if (strWhich == "SEARCH")
                    textBoxPositionSearch.Text = keySearchbyPosition;
                else if (strWhich == "EXIT")
                    textBoxEXIT.Text = keySearchbyPosition;
            }

            keyRemains = textRemains.Text;
            keyJUN = textJUN.Text;
            keyALVA = textALVA.Text;
            keyZANA = textZANA.Text;
            keyHideout = textHideout.Text;
            keySearchbyPosition = textBoxPositionSearch.Text;
            keyEXIT = textBoxEXIT.Text;            
        }

        private void TextRemains_KeyDown(object sender, KeyEventArgs e)
        {
            GetSet_HotKey(e, "REMAINS");
        }

        private void TextJUN_KeyDown(object sender, KeyEventArgs e)
        {
            GetSet_HotKey(e, "JUN");
        }

        private void TextALVA_KeyDown(object sender, KeyEventArgs e)
        {
            GetSet_HotKey(e, "ALVA");
        }

        private void TextZANA_KeyDown(object sender, KeyEventArgs e)
        {
            GetSet_HotKey(e, "ZANA");
        }

        private void TextHideout_KeyUp(object sender, KeyEventArgs e)
        {
            GetSet_HotKey(e, "HIDEOUT");
        }

        private void TextBoxPositionSearch_KeyUp(object sender, KeyEventArgs e)
        {
            GetSet_HotKey(e, "SEARCH");
        }

        private void TextBoxEXIT_KeyUp(object sender, KeyEventArgs e)
        {
            GetSet_HotKey(e, "EXIT");
        }

        private void BtnHotKey_Click(object sender, EventArgs e)
        {
            panel1.Visible = true; // HOT KEY
            panel2.Visible = false; // FLASK
            panel3.Visible = false; // TRADE
            panelDonateContact.Visible = false; // TRADE
            panelHallOfFame.Visible = false; // HALL OF FAME
            panelSkillMain.Visible = false;

            panelHotKey.BackColor = Color.Khaki;
            panelFlask.BackColor = Color.FromArgb(22, 22, 24);
            panelTrading.BackColor = Color.FromArgb(22, 22, 24);
            panelSkill.BackColor = Color.FromArgb(22, 22, 24);
            panelHall.BackColor = Color.FromArgb(22, 22, 24);
            panelDonate.BackColor = Color.FromArgb(22, 22, 24);
        }

        private void BtnFlask_Click(object sender, EventArgs e)
        {
            panel1.Visible = false; // HOT KEY
            panel2.Visible = true; // FLASK
            panel3.Visible = false; // TRADE
            panelDonateContact.Visible = false; // TRADE
            panelHallOfFame.Visible = false; // HALL OF FAME
            panelSkillMain.Visible = false;

            panelHotKey.BackColor = Color.FromArgb(22, 22, 24);
            panelFlask.BackColor = Color.Khaki;
            panelTrading.BackColor = Color.FromArgb(22, 22, 24);
            panelSkill.BackColor = Color.FromArgb(22, 22, 24);
            panelHall.BackColor = Color.FromArgb(22, 22, 24);
            panelDonate.BackColor = Color.FromArgb(22, 22, 24);
        }

        private void BtnTrading_Click(object sender, EventArgs e)
        {
            panel1.Visible = false; // HOT KEY
            panel2.Visible = false; // FLASK
            panel3.Visible = true; // TRADE
            panelDonateContact.Visible = false; // TRADE
            panelHallOfFame.Visible = false; // HALL OF FAME
            panelSkillMain.Visible = false;

            panelHotKey.BackColor = Color.FromArgb(22, 22, 24);
            panelFlask.BackColor = Color.FromArgb(22, 22, 24);
            panelTrading.BackColor = Color.Khaki;
            panelSkill.BackColor = Color.FromArgb(22, 22, 24);
            panelHall.BackColor = Color.FromArgb(22, 22, 24);
            panelDonate.BackColor = Color.FromArgb(22, 22, 24);
        }

        private void ButtonDonate_Click(object sender, EventArgs e)
        {
            panel1.Visible = false; // HOT KEY
            panel2.Visible = false; // FLASK
            panel3.Visible = false; // TRADE
            panelDonateContact.Visible = true; // TRADE
            panelHallOfFame.Visible = false; // HALL OF FAME
            panelSkillMain.Visible = false;

            panelHotKey.BackColor = Color.FromArgb(22, 22, 24);
            panelFlask.BackColor = Color.FromArgb(22, 22, 24);
            panelTrading.BackColor = Color.FromArgb(22, 22, 24);
            panelDonate.BackColor = Color.Khaki;
            panelHall.BackColor = Color.FromArgb(22, 22, 24);
            panelSkill.BackColor = Color.FromArgb(22, 22, 24);
        }

        private void BtnHall_Click(object sender, EventArgs e)
        {
            panel1.Visible = false; // HOT KEY
            panel2.Visible = false; // FLASK
            panel3.Visible = false; // TRADE
            panelDonateContact.Visible = false; // TRADE
            panelHallOfFame.Visible = true; // HALL OF FAME
            panelSkillMain.Visible = false;

            panelHotKey.BackColor = Color.FromArgb(22, 22, 24);
            panelFlask.BackColor = Color.FromArgb(22, 22, 24);
            panelTrading.BackColor = Color.FromArgb(22, 22, 24);
            panelSkill.BackColor = Color.FromArgb(22, 22, 24);
            panelHall.BackColor = Color.Khaki;
            panelDonate.BackColor = Color.FromArgb(22, 22, 24);
        }

        private void BtnSkillTimer_Click(object sender, EventArgs e)
        {
            panel1.Visible = false; // HOT KEY
            panel2.Visible = false; // FLASK
            panel3.Visible = false; // TRADE
            panelDonateContact.Visible = false; // TRADE
            panelHallOfFame.Visible = false; // HALL OF FAME
            panelSkillMain.Visible = true;

            panelHotKey.BackColor = Color.FromArgb(22, 22, 24);
            panelFlask.BackColor = Color.FromArgb(22, 22, 24);
            panelTrading.BackColor = Color.FromArgb(22, 22, 24);
            panelSkill.BackColor = Color.Khaki;
            panelHall.BackColor = Color.FromArgb(22, 22, 24);
            panelDonate.BackColor = Color.FromArgb(22, 22, 24);
        }

        private void panelCO1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DialogResult dRes = colorDialog1.ShowDialog();
            
            if (dRes == DialogResult.OK)
            {
                panelCO1.BackColor = colorDialog1.Color;

                string strR = colorDialog1.Color.R.ToString();
                string strG = colorDialog1.Color.G.ToString();
                string strB = colorDialog1.Color.B.ToString();
                colorStringRGB1 = String.Format("{0},{1},{2}", strR, strG, strB);
            }
        }

        private void panelCO2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DialogResult dRes = colorDialog1.ShowDialog();

            if (dRes == DialogResult.OK)
            {
                panelCO2.BackColor = colorDialog1.Color;
                string strR = colorDialog1.Color.R.ToString();
                string strG = colorDialog1.Color.G.ToString();
                string strB = colorDialog1.Color.B.ToString();
                colorStringRGB2 = String.Format("{0},{1},{2}", strR, strG, strB);
            }
        }

        private void panelCO3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DialogResult dRes = colorDialog1.ShowDialog();

            if (dRes == DialogResult.OK)
            {
                panelCO3.BackColor = colorDialog1.Color;
                string strR = colorDialog1.Color.R.ToString();
                string strG = colorDialog1.Color.G.ToString();
                string strB = colorDialog1.Color.B.ToString();
                colorStringRGB3 = String.Format("{0},{1},{2}", strR, strG, strB);
            }
        }

        private void panelCO4_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DialogResult dRes = colorDialog1.ShowDialog();

            if (dRes == DialogResult.OK)
            {
                panelCO4.BackColor = colorDialog1.Color;
                string strR = colorDialog1.Color.R.ToString();
                string strG = colorDialog1.Color.G.ToString();
                string strB = colorDialog1.Color.B.ToString();
                colorStringRGB4 = String.Format("{0},{1},{2}", strR, strG, strB);
            }
        }

        private void panelCO5_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DialogResult dRes = colorDialog1.ShowDialog();

            if (dRes == DialogResult.OK)
            {
                panelCO5.BackColor = colorDialog1.Color;
                string strR = colorDialog1.Color.R.ToString();
                string strG = colorDialog1.Color.G.ToString();
                string strB = colorDialog1.Color.B.ToString();
                colorStringRGB5 = String.Format("{0},{1},{2}", strR, strG, strB);
            }
        }

        private void textBoxSEC1_TextChanged(object sender, EventArgs e)
        {
            LauncherForm.g_FlaskTime1 = textBoxSEC1.Text;
        }

        private void textBoxSEC2_TextChanged(object sender, EventArgs e)
        {
            LauncherForm.g_FlaskTime2 = textBoxSEC2.Text;
        }

        private void textBoxSEC3_TextChanged(object sender, EventArgs e)
        {
            LauncherForm.g_FlaskTime3 = textBoxSEC3.Text;
        }

        private void textBoxSEC4_TextChanged(object sender, EventArgs e)
        {
            LauncherForm.g_FlaskTime4 = textBoxSEC4.Text;
        }

        private void textBoxSEC5_TextChanged(object sender, EventArgs e)
        {
            LauncherForm.g_FlaskTime5 = textBoxSEC5.Text;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            btnCancel.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void PictureBox5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.buymeacoffee.com/UzY5dr7");
        }

        private void PictureBox6_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://discord.gg/ryjUA7r"); 
        }

        private void PictureBox7_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://discord.gg/ryjUA7r");
        }

        private void PictureBox12_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.patreon.com/bePatron?u=25155273"); 
        }
        
        private void PictureBox15_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/channel/UCRG83nTzKgfIkGEeTU0bmXg");
        }

        private void PictureBox16_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.twitch.tv/deadlycrush");
        }

        private void PictureBox17_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/Deadly-Trade-102279784448097"); 
        }

        private void PictureBoxToon_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://toon.at/donate/deadly_trade"); 
        }

        private void PictureBox12_MouseHover(object sender, EventArgs e)
        {
            pictureBox12.Cursor = Cursors.Hand;
        }

        private void PictureBox5_MouseHover(object sender, EventArgs e)
        {
            pictureBox5.Cursor = Cursors.Hand;
        }

        private void PictureBoxToon_MouseHover(object sender, EventArgs e)
        {
            // TODOTODOTODTODO pictureBoxToon.Cursor = Cursors.Hand;
        }

        private void PictureBox17_MouseHover(object sender, EventArgs e)
        {
            pictureBox17.Cursor = Cursors.Hand;
        }

        private void PictureBox10_MouseHover(object sender, EventArgs e)
        {
            pictureBox10.Cursor = Cursors.Hand;
        }

        private void PictureBox16_MouseHover(object sender, EventArgs e)
        {
            pictureBox16.Cursor = Cursors.Hand;
        }

        private void PictureBox15_MouseHover(object sender, EventArgs e)
        {
            pictureBox15.Cursor = Cursors.Hand;
        }

        private void TextBoxSEC1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void TextBoxSEC2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void TextBoxSEC3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void TextBoxSEC4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void TextBoxSEC5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void PanelQ_DoubleClick(object sender, EventArgs e)
        {
            DialogResult dRes = colorDialog1.ShowDialog();

            if (dRes == DialogResult.OK)
            {
                panelQ.BackColor = colorDialog1.Color;
                string strR = colorDialog1.Color.R.ToString();
                string strG = colorDialog1.Color.G.ToString();
                string strB = colorDialog1.Color.B.ToString();
                colorStringRGBQ = String.Format("{0},{1},{2}", strR, strG, strB);
            }
        }

        private void PanelW_DoubleClick(object sender, EventArgs e)
        {
            DialogResult dRes = colorDialog1.ShowDialog();

            if (dRes == DialogResult.OK)
            {
                panelW.BackColor = colorDialog1.Color;
                string strR = colorDialog1.Color.R.ToString();
                string strG = colorDialog1.Color.G.ToString();
                string strB = colorDialog1.Color.B.ToString();
                colorStringRGBW = String.Format("{0},{1},{2}", strR, strG, strB);
            }
        }

        private void PanelE_DoubleClick(object sender, EventArgs e)
        {
            DialogResult dRes = colorDialog1.ShowDialog();

            if (dRes == DialogResult.OK)
            {
                panelE.BackColor = colorDialog1.Color;
                string strR = colorDialog1.Color.R.ToString();
                string strG = colorDialog1.Color.G.ToString();
                string strB = colorDialog1.Color.B.ToString();
                colorStringRGBE = String.Format("{0},{1},{2}", strR, strG, strB);
            }
        }

        private void PanelR_DoubleClick(object sender, EventArgs e)
        {
            DialogResult dRes = colorDialog1.ShowDialog();

            if (dRes == DialogResult.OK)
            {
                panelR.BackColor = colorDialog1.Color;
                string strR = colorDialog1.Color.R.ToString();
                string strG = colorDialog1.Color.G.ToString();
                string strB = colorDialog1.Color.B.ToString();
                colorStringRGBR = String.Format("{0},{1},{2}", strR, strG, strB);
            }
        }

        private void PanelT_DoubleClick(object sender, EventArgs e)
        {
            DialogResult dRes = colorDialog1.ShowDialog();

            if (dRes == DialogResult.OK)
            {
                panelT.BackColor = colorDialog1.Color;
                string strR = colorDialog1.Color.R.ToString();
                string strG = colorDialog1.Color.G.ToString();
                string strB = colorDialog1.Color.B.ToString();
                colorStringRGBT = String.Format("{0},{1},{2}", strR, strG, strB);
            }
        }

        private void TextBoxQ_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void TextBoxW_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void TextBoxE_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void TextBoxR_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void TextBoxT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void TextBoxQ_TextChanged(object sender, EventArgs e)
        {
            LauncherForm.g_SkillTime1 = textBoxQ.Text;
        }

        private void TextBoxW_TextChanged(object sender, EventArgs e)
        {
            LauncherForm.g_SkillTime2 = textBoxW.Text;
        }

        private void TextBoxE_TextChanged(object sender, EventArgs e)
        {
            LauncherForm.g_SkillTime3 = textBoxE.Text;
        }

        private void TextBoxR_TextChanged(object sender, EventArgs e)
        {
            LauncherForm.g_SkillTime4 = textBoxR.Text;
        }

        private void TextBoxT_TextChanged(object sender, EventArgs e)
        {
            LauncherForm.g_SkillTime5 = textBoxT.Text;
        }

        private void PictureBox27_Click(object sender, EventArgs e)
        {
            // YouTube
            System.Diagnostics.Process.Start("https://www.youtube.com/channel/UCRG83nTzKgfIkGEeTU0bmXg");
        }

        private void PictureBox26_Click(object sender, EventArgs e)
        {
            // Twitch
            System.Diagnostics.Process.Start("https://www.twitch.tv/deadlycrush");
        }

        private void PictureBox25_Click(object sender, EventArgs e)
        {
            // FaceBook
            System.Diagnostics.Process.Start("https://www.facebook.com/Deadly-Trade-102279784448097");
        }

        private void PictureBox28_Click(object sender, EventArgs e)
        {
            // Discord
            System.Diagnostics.Process.Start("https://discord.gg/ryjUA7r");
        }

        private void PictureBox23_Click(object sender, EventArgs e)
        {
            // Toonation
            System.Diagnostics.Process.Start("https://toon.at/donate/deadly_trade");
        }

        private void pictureBox5_Click_1(object sender, EventArgs e)
        {
            // Toonation
            System.Diagnostics.Process.Start("https://toon.at/donate/deadly_trade");
        }

        private void PictureBox24_Click(object sender, EventArgs e)
        {
            // donorbox
            System.Diagnostics.Process.Start("https://donorbox.org/deadly-trade-poe");
        }

        private void xuiSlider1_MouseUp(object sender, MouseEventArgs e)
        {
            LauncherForm.g_NotifyVolume = xuiSlider1.Percentage;
        }

        private void xuiSlider1_MouseMove(object sender, MouseEventArgs e)
        {
            LauncherForm.g_NotifyVolume = xuiSlider1.Percentage;
            labelVolume.Text = "Volume = " + xuiSlider1.Percentage.ToString();
        }

        private void btnPaypalSub_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.jumpleasure.me/deadlytrade/?page_id=455");
        }

        private void btnPaypalMain_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.jumpleasure.me/deadlytrade/?page_id=455");
        }
    }
}
