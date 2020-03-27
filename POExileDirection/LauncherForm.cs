using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using AutoUpdaterDotNET;
using Newtonsoft.Json;
using Ninja_Price.API.PoeNinja.Classes;
using log4net;
using System.Net;
using static System.Environment;

namespace POExileDirection
{
    public partial class LauncherForm : Form
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static DeadlyTradeUpdate.RootObjectNotice _updateCurrentData; // Added by 1.3.9.2 Ver.

        public static IntPtr g_handlePathOfExile { get; set; }

        public static string g_strDonator;

        NinjaForm frmNinja = null;

        private int nMoving = 0;
        private int nMovePosX = 0;
        private int nMovePosY = 0;

        public static int CNT_NINJACATEGORIES = 19;
        public static int g_NinjaFileMakeAndUpdateCNT = 0; // Make 16 + Update 16 ( 16 = CNT_NINJACATEGORIES )
        public static string g_NinjaUpdatedTime { get; set; }

        public static string g_CurrentLeague { get; set; }
        public static string g_POELogPath { get; set; }
        public static string g_POELogFileName { get; set; }
        public static string g_strUILang { get; set; }
        public static bool g_bShowLocalChat { get; set; }

        public static string g_strYNMouseWheelStashTab { get; set; } // Added 1.3.9.6 Ver.

        public static string g_strExplanationLANG { get; set; }

        private bool g_bCanLaunchAddon = false;
        private bool g_bAddonLaunched = false;
        ControlForm frmMainControl = null;

        Common commonClass = new Common();

        private string g_NinjaDirectory = null;

        #region ⨌⨌ for FLASK TIMER ⨌⨌
        public static int g_Flask1 { get; set; }
        public static int g_Flask2 { get; set; }
        public static int g_Flask3 { get; set; }
        public static int g_Flask4 { get; set; }
        public static int g_Flask5 { get; set; }

        public static string g_FlaskTime1 { get; set; }
        public static string g_FlaskTime2 { get; set; }
        public static string g_FlaskTime3 { get; set; }
        public static string g_FlaskTime4 { get; set; }
        public static string g_FlaskTime5 { get; set; }

        public static bool g_bToggle1 { get; set; }
        public static bool g_bToggle2 { get; set; }
        public static bool g_bToggle3 { get; set; }
        public static bool g_bToggle4 { get; set; }
        public static bool g_bToggle5 { get; set; }
        #endregion

        #region ⨌⨌ for SKILL TIMER ⨌⨌
        public static int g_Skill1 { get; set; }
        public static int g_Skill2 { get; set; }
        public static int g_Skill3 { get; set; }
        public static int g_Skill4 { get; set; }
        public static int g_Skill5 { get; set; }

        public static string g_SkillTime1 { get; set; }
        public static string g_SkillTime2 { get; set; }
        public static string g_SkillTime3 { get; set; }
        public static string g_SkillTime4 { get; set; }
        public static string g_SkillTime5 { get; set; }

        public static bool g_bToggleSkill1 { get; set; }
        public static bool g_bToggleSkill2 { get; set; }
        public static bool g_bToggleSkill3 { get; set; }
        public static bool g_bToggleSkill4 { get; set; }
        public static bool g_bToggleSkill5 { get; set; }
        #endregion

        public static string g_strMyNickName { get; set; }
        public static string g_strTRAutoKick { get; set; }

        public static int resolution_height { get; set; }
        public static int resolution_width { get; set; }

        public static int g_nGridWidth { get; set; }
        public static int g_nGridHeight { get; set; }

        public static bool g_FocusLosing { get; set; }
        public static bool g_FocusOnAddon { get; set; }

        public static bool g_pinLOCK { get; set; }

        public static bool g_ZoneInfoExpanded { get; set; }

        public static int g_NotifyVolume { get; set; }

        #region ⨌⨌ Declaration for NINJA API ⨌⨌
        // POE.NINJA
        public static string PoeLeagueApiList = "http://api.pathofexile.com/leagues?type=main&compact=1";

        // 19 Types
        public static string CurrencyURL = "https://poe.ninja/api/data/currencyoverview?type=Currency&league=";
        public static string Fragments_URL = "https://poe.ninja/api/data/currencyoverview?type=Fragment&league=";
        public static string Incubators_URL = "https://poe.ninja/api/data/itemoverview?type=Incubator&league=";
        public static string Scarabs_URL = "https://poe.ninja/api/data/itemoverview?type=Scarab&league=";
        public static string Fossils_URL = "https://poe.ninja/api/data/itemoverview?type=Fossil&league=";
        public static string Resonators_URL = "https://poe.ninja/api/data/itemoverview?type=Resonator&league=";
        
        public static string Essences_URL = "https://poe.ninja/api/data/itemoverview?type=Essence&league=";
        public static string DivinationCards_URL = "https://poe.ninja/api/data/itemoverview?type=DivinationCard&league=";
        public static string Prophecies_URL = "https://poe.ninja/api/data/itemoverview?type=Prophecy&league=";
        
        public static string UniqueMaps_URL = "https://poe.ninja/api/data/itemoverview?type=UniqueMap&league=";
        public static string WhiteMaps_URL = "https://poe.ninja/api/data/itemoverview?type=Map&league=";
        
        public static string UniqueJewels_URL = "https://poe.ninja/api/data/itemoverview?type=UniqueJewel&league=";
        public static string UniqueFlasks_URL = "https://poe.ninja/api/data/itemoverview?type=UniqueFlask&league=";
        
        public static string UniqueWeapons_URL = "https://poe.ninja/api/data/itemoverview?type=UniqueWeapon&league=";
        public static string UniqueArmours_URL = "https://poe.ninja/api/data/itemoverview?type=UniqueArmour&league=";
        public static string UniqueAccessories_URL = "https://poe.ninja/api/data/itemoverview?type=UniqueAccessory&league=";

        public static string BlightOil_URL = "https://poe.ninja/api/data/itemoverview?type=Oil&league="; // Added 1.3.9.0 Version
        public static string Watchstones_URL = "https://poe.ninja/api/data/itemoverview?type=Watchstone&league="; // Added 1.3.9.0 Version
        public static string Beasts_URL = "https://poe.ninja/api/data/itemoverview?type=Beast&league="; // Added 1.3.9.1 Version

        // Ninja Object 19 Types
        public class NinJaAPIData
        {
            public Currency.RootObject Currency { get; set; } = new Currency.RootObject();
            public DivinationCards.RootObject DivinationCards { get; set; } = new DivinationCards.RootObject();
            public Essences.RootObject Essences { get; set; } = new Essences.RootObject();
            public Fragments.RootObject Fragments { get; set; } = new Fragments.RootObject();
            public Prophecies.RootObject Prophecies { get; set; } = new Prophecies.RootObject();
            public UniqueAccessories.RootObject UniqueAccessories { get; set; } = new UniqueAccessories.RootObject();
            public UniqueArmours.RootObject UniqueArmours { get; set; } = new UniqueArmours.RootObject();
            public UniqueFlasks.RootObject UniqueFlasks { get; set; } = new UniqueFlasks.RootObject();
            public UniqueJewels.RootObject UniqueJewels { get; set; } = new UniqueJewels.RootObject();
            public UniqueMaps.RootObject UniqueMaps { get; set; } = new UniqueMaps.RootObject();
            public UniqueWeapons.RootObject UniqueWeapons { get; set; } = new UniqueWeapons.RootObject();
            public WhiteMaps.RootObject WhiteMaps { get; set; } = new WhiteMaps.RootObject();
            public Resonators.RootObject Resonators { get; set; } = new Resonators.RootObject();
            public Fossils.RootObject Fossils { get; set; } = new Fossils.RootObject();
            public Scarab.RootObject Scarabs { get; set; } = new Scarab.RootObject();
            public Incubators.RootObject Incubators { get; set; } = new Incubators.RootObject();

            public Oils.RootObject Oils { get; set; } = new Oils.RootObject(); // Added 1.3.9.0 Version : Blight Oils
            public Watchstones.RootObject Watchstones { get; set; } = new Watchstones.RootObject(); // Added 1.3.9.0 Version : 3.9 Watchstones
            public Beasts.RootObject Beasts { get; set; } = new Beasts.RootObject(); // Beasts_URL : Added 1.3.9.1 Version
        }
        #endregion

        
        public static NinJaAPIData ninjaData { get; set; }// = new NinJaAPIData();

        // DeadlyOverlay : Syndicate, and TO DO...
        public class DeadlyOverlay
        {
            public Syndicate.RootObject SyndicateDeadly { get; set; } = new Syndicate.RootObject();
        }
        public static DeadlyOverlay deadlyOverlayData { get; set; }

        #region [[[[[ Jason Class : DeadlyInformation ]]]]
        // DeadlyInformation for Atlas MAP ( Data From ggpk : aNitMotD )
        public class DeadlyInformation
        {
            public DeadlyAtlas.RootObject InformationDeadly { get; set; } = new DeadlyAtlas.RootObject();
            public DeadlyAtlas.RootObjectMap InformationMaps { get; set; } = new DeadlyAtlas.RootObjectMap();
            public DeadlyAtlas.RootObjectCurruncy InformationCurrency { get; set; } = new DeadlyAtlas.RootObjectCurruncy();
            public DeadlyAtlas.RootObjectDivinationCard InformationDivinationCard { get; set; } = new DeadlyAtlas.RootObjectDivinationCard();
            public DeadlyAtlas.RootObjectDelve InformationDelve { get; set; } = new DeadlyAtlas.RootObjectDelve();
            public DeadlyAtlas.RootObjectScarab InformationScarab { get; set; } = new DeadlyAtlas.RootObjectScarab();
            public DeadlyAtlas.RootObjectMapFragment InformationMapFragment { get; set; } = new DeadlyAtlas.RootObjectMapFragment();
            public DeadlyAtlas.RootObjectProphecy InformationProphecy { get; set; } = new DeadlyAtlas.RootObjectProphecy();
            public DeadlyAtlas.RootObjectUniqueMap InformationUniqueMap { get; set; } = new DeadlyAtlas.RootObjectUniqueMap();
            public DeadlyAtlas.RootObjectUnique InformationUniqueItem { get; set; } = new DeadlyAtlas.RootObjectUnique();
            public DeadlyAtlas.RootObjectNotifyMSG InformationMSG { get; set; } = new DeadlyAtlas.RootObjectNotifyMSG();
            public DeadlyAtlas.RootObjectMapAlertMSG MapAlertMSG { get; set; } = new DeadlyAtlas.RootObjectMapAlertMSG();
            public DeadlyAtlas.RootObjectOilsPassive OilPassiveJsonData { get; set; } = new DeadlyAtlas.RootObjectOilsPassive();
        }
        public static DeadlyInformation deadlyInformationData { get; set; }
        #endregion

        // DeadlyCrush ENG_KOR Matching Data ( Data From ggpk : aNitMotD )
        public class DeadlyENGKORMatching
        {
            public ConvertKOR.RootObject engkorMatching { get; set; } = new ConvertKOR.RootObject();
        }
        public static DeadlyENGKORMatching matchingENGKORData { get; set; }

        public string g_strAddonUILang { get; set; }

        // NOTIFICATION MESSAGE
        public static string g_strnotiWAIT { get; set; }
        public static string g_strnotiSOLD { get; set; }
        public static string g_strnotiDONE { get; set; }
        public static string g_strnotiRESEND { get; set; }

        public static List<string> g_strArrREDAlert { get; set; }
        public static List<string> g_strArrGREENAlert { get; set; }

        private DateTime ScrollTick;

        public LauncherForm()
        {            
            InitializeComponent();
            Text = "DeadlyTradeForPOE";
        }

        private void LauncherForm_Load(object sender, EventArgs e)
        {
            g_handlePathOfExile = IntPtr.Zero;

            g_NinjaDirectory = Application.StartupPath + "\\NinjaData\\";
            // g_CurrentLeague = "Legion";

            g_ZoneInfoExpanded = true;

            Init_Controls();
            commonClass.DeleteAllFilesInFoder(g_NinjaDirectory);

            string strThisAssemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.labelCurrentVer.Text = strThisAssemblyVersion;
            this.labelServerVer.Text = strThisAssemblyVersion;
            this.labelVersionState.Text = "Current Version is the Newest.";
            this.labelNeedToUpdate.Text = "No Update Needed.";

            g_strArrREDAlert = new List<string>();
            g_strArrGREENAlert = new List<string>();

            bool bFontcheck1 = Check_FontInstalled("굴림");
            bool bFontcheck2 = Check_FontInstalled("Gulim");
            bool bFontcheck3 = Check_FontInstalled("gulim");
            if (!bFontcheck1 && !bFontcheck2 && !bFontcheck3)
            {
                log.Info("FONT not found");
                MSGForm frmMSG = new MSGForm();
                frmMSG.lbMsg.Text = "If UI showing wrong font\r\nplease install Gulim font that located in sub folder 'FONT'\r\ngulim.ttc\r\n\r\nIt use gulim and arial font.";
                DialogResult dr = frmMSG.ShowDialog();
                /*MessageBox.Show("NOT FOUND");
                var info = new ProcessStartInfo()
                {
                    FileName = Application.StartupPath + "\\FONT\\FontReg.exe",
                    Arguments = "/copy",
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden

                };

                Process.Start(info);*/
            }
            else
            {
                /*try
                {
                    var info = new ProcessStartInfo()
                    {
                        FileName = Application.StartupPath + "\\FONT\\FontReg.exe",
                        Arguments = "/copy",
                        UseShellExecute = false,
                        Verb = "runas",
                        WindowStyle = ProcessWindowStyle.Hidden
                    };
                    Process.Start(info);
                    log.Info("HI " + info.FileName);
                }
                catch (Exception ex)
                {
                    log.Error($"catch {MethodBase.GetCurrentMethod().Name}", ex);
                }*/

                log.Info("FONT Exist");
            }
            //this.Close();
            //Application.Exit();
            
            // AutoUpdater check server's xml.
            AutoUpdateCheck();

            //TEST TTTTTTT
            // TTTTT panelWaiting.Visible = false;
            // TTTTT Get_NinjaData();
        }

        private bool Check_FontInstalled(string fontName)
        {
            bool bFound = false;
            float fontSize = 12;
            using (Font fontTester = new Font(
                   fontName,
                   fontSize,
                   FontStyle.Regular,
                   GraphicsUnit.Pixel))
            {
                if (fontTester.Name == fontName)
                    bFound = true;
                else
                    bFound = false;
            }

            return bFound;
        }

        public void AutoUpdateCheck()
        {
            AutoUpdater.ShowSkipButton = false;
            AutoUpdater.ShowRemindLaterButton = false;
            AutoUpdater.Mandatory = true;
            AutoUpdater.RunUpdateAsAdmin = true;
            AutoUpdater.OpenDownloadPage = false;
            AutoUpdater.DownloadPath = Application.StartupPath;
            AutoUpdater.Start("https://www.jumpleasure.me/deadlytrade/repository/DeadlyTradeVersion.xml");
            AutoUpdater.CheckForUpdateEvent += AutoUpdater_CheckForUpdateEvent;
        }

        #region ⨌⨌ Init. Controls ⨌⨌
        private void Init_Controls()
        {
            this.Text = "Deadly Trade";
            ShowInTaskbar = true;

            // NOTIFY, TRAY
            this.notifyIconDeadlyTrade.BalloonTipTitle = "Deadly Trade";
            this.notifyIconDeadlyTrade.BalloonTipText = "Deadly Trade Still Working... (이미 실행중입니다.)";
            this.notifyIconDeadlyTrade.Text = "Double Click to Show";
            this.notifyIconDeadlyTrade.BalloonTipIcon = ToolTipIcon.Info;

            // Waiting Panel : 558, 573
            this.panelWaiting.Width = 558;
            this.panelWaiting.Height = 573;

            // btnClose
            this.btnStartAddon.FlatStyle = FlatStyle.Flat;
            this.btnStartAddon.BackColor = Color.Transparent;
            this.btnStartAddon.FlatAppearance.MouseDownBackColor = Color.Transparent;
            this.btnStartAddon.FlatAppearance.MouseOverBackColor = Color.Transparent;
            this.btnStartAddon.FlatAppearance.BorderColor = Color.FromArgb(0, 39, 44, 56);
            this.btnStartAddon.FlatAppearance.BorderSize = 0;
            this.btnStartAddon.TabStop = false;

            // btnStartAddon
            this.btnStartAddon.FlatStyle = FlatStyle.Flat;
            this.btnStartAddon.BackColor = Color.Transparent;
            this.btnStartAddon.FlatAppearance.MouseDownBackColor = Color.Transparent;
            this.btnStartAddon.FlatAppearance.MouseOverBackColor = Color.Transparent;
            this.btnStartAddon.FlatAppearance.BorderColor = Color.FromArgb(0, 39, 44, 56);
            this.btnStartAddon.FlatAppearance.BorderSize = 0;
            this.btnStartAddon.TabStop = false;

            // PROGRESS BAR
            xuiFlatProgressBar1.MaxValue = CNT_NINJACATEGORIES; // Make and Update
            xuiFlatProgressBar2.MaxValue = 20;

            labelAddonStatus.Text = "Addon Data (Not loaded yet)";

            // TOOLTIP
            DeadlyToolTip.SetToolTip(this.btnMinimize, "Minimize (최소화)");
            DeadlyToolTip.SetToolTip(this.btnClose, "Close (닫기)");
            DeadlyToolTip.SetToolTip(this.btnStartAddon, "Waiting for Path Of Exile Launch Finished.");
        }
        #endregion

        private void ReadyToStartAddon()
        {
            btnStartAddon.Image = Properties.Resources.DeadlyTradeStartYellowButton;
            DeadlyToolTip.SetToolTip(btnStartAddon, "Start Addon 'Deadly Trade'");

            g_bCanLaunchAddon = true;
        }

        #region ⨌⨌ Check. Auto Update ⨌⨌

        private class WebClientWithTimeout : WebClient
        {
            protected override WebRequest GetWebRequest(Uri address)
            {
                WebRequest wr = base.GetWebRequest(address);
                wr.Timeout = 5000; // timeout in milliseconds (ms)
                return wr;
            }
        }

        private bool bProceedUpdate = false;
        private void AutoUpdater_CheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            this.btnClose.Enabled = false;
            if (args != null)
            {
                if (args.IsUpdateAvailable)
                {
                    // args.CurrentVersion means Server's Current Version.
                    this.labelServerVer.Text = args.CurrentVersion.ToString();
                    this.labelVersionState.Text = "Current Vrsion is not up to date.";
                    this.labelNeedToUpdate.Text = "Update Nedeed.";

                    #region ⨌⨌ Sample Code ⨌⨌
                    /*DialogResult dialogResult;
                    if (args.Mandatory)
                    {
                        dialogResult =
                            MessageBox.Show(
                                $@"There is new version {args.CurrentVersion} available. You are using version {args.InstalledVersion}. This is required update. Press Ok to begin updating the application.", @"Update Available",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                    }
                    else
                    {
                        dialogResult =
                            MessageBox.Show(
                                $@"There is new version {args.CurrentVersion} available. You are using version {
                                        args.InstalledVersion
                                    }. Do you want to update the application now?", @"Update Available",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information);
                    }*/
                    #endregion

                    // Uncomment the following line if you want to show standard update dialog instead.
                    // AutoUpdater.ShowUpdateForm();

                    try
                    {
                        this.TopMost = false;
                        panelWaiting.Visible = false;
                        #region [[[[[ Delete Useless Files ]]]]]
                        DirectoryInfo di = new DirectoryInfo(Application.StartupPath);
                        FileInfo[] files = di.GetFiles("*.zip").Where(p => p.Extension == ".zip").ToArray();
                        foreach (FileInfo file in files)
                        {
                            try
                            {
                                file.Attributes = FileAttributes.Normal;
                                File.Delete(file.FullName);
                            }
                            catch(Exception ex)
                            {
                                log.Error($"catch {MethodBase.GetCurrentMethod().Name}", ex);
                            }
                        }
                        // Specific File : File.Delete(Application.StartupPath + "\\");
                        #endregion

                        if (AutoUpdater.DownloadUpdate())
                        {
                            this.TopMost = true;
                            #region [[[[[ Show Update Contents. ]]]]]
                            try
                            {
                                string strRead = String.Empty;
                                // Read Update Contents.
                                try
                                {
                                    WebClient wc = new WebClientWithTimeout();
                                    if (LauncherForm.g_strUILang == "KOR")
                                    {
                                        var readData = wc.DownloadData("https://www.jumpleasure.me/deadlytrade/repository/UpdateContents.txt");
                                        strRead = Encoding.UTF8.GetString(readData);
                                    }

                                    else
                                    {
                                        var readData = wc.DownloadData("https://www.jumpleasure.me/deadlytrade/repository/UpdateContentsEN.txt");
                                        strRead = Encoding.UTF8.GetString(readData);
                                    }
                                }
                                catch (WebException ex)
                                {
                                    log.Error($"Read Contents. {MethodBase.GetCurrentMethod().Name}", ex);
                                }
                                this.TopMost = true;
                                labelUpdateTitle.Text = "DeadlyTrade " + strRead.Substring(0, 7) + " Updated contents.";
                                labelPatchNote.Text = strRead;
                                panelUpdateContents.Left = 0;
                                panelUpdateContents.Top = 1;
                                panelUpdateContents.Width = 558;
                                panelUpdateContents.Height = 573;
                                panelUpdateContents.Visible = true;
                                btnExitAndUpdate.Visible = true;
                                pictureBox2.Dispose();
                                panelWaiting.Dispose();
                            }
                            catch (Exception ex)
                            {
                                log.Error($"Update Msg. {MethodBase.GetCurrentMethod().Name}", ex);
                            }
                            this.TopMost = true;
                            Thread.Sleep(1000);
                            #endregion
                        }
                    }
                    catch(Exception exception)
                    {
                        /*MessageBox.Show(exception.Message, exception.GetType().ToString(), MessageBoxButtons.OK,
                            MessageBoxIcon.Error);*/
                        MSGForm frmMSG = new MSGForm();
                        frmMSG.lbMsg.Text = "An error occrred while checking update, please try again later\r\n\r\n(업데이트 확인 중 오류가 발생했습니다. 잠시 후 다시 시도해주세요.)" +
                            "\r\n\r\nERROR MESSAGE : " + exception.Message;
                        DialogResult dr = frmMSG.ShowDialog();

                        log.Error($"Update {MethodBase.GetCurrentMethod().Name}", exception);

                        // Force Terminate Launcher.
                        if (dr == DialogResult.OK)
                            Application.Exit();
                        else
                            Application.Exit();
                    }
                }
                else
                {
                    AutoUpdater.CheckForUpdateEvent -= AutoUpdater_CheckForUpdateEvent;
                    g_bCanLaunchAddon = false;

                    // args.CurrentVersion means Server's Current Version.
                    this.labelCurrentVer.Text = args.InstalledVersion.ToString();
                    this.labelServerVer.Text = args.CurrentVersion.ToString();
                    this.labelVersionState.Text = "Current Version is the Newest.";
                    this.labelNeedToUpdate.Text = "No Update Needed.";

                    Thread.Sleep(500);
                    this.panelWaiting.Visible = false;

                    g_NinjaUpdatedTime = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");
                    labelAddonStatus.Text = String.Format("Addon Data ({0})", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));

                    #region ⨌⨌ Parsing POE production_Config.ini ⨌⨌
                    // Get Addon Data & Pesonal Setting ( From My Games - Path of Exile )               
                    String strPathPOEConifg = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    strPathPOEConifg = strPathPOEConifg + "\\My Games\\Path of Exile\\production_Config.ini";
                    IniParser parser = new IniParser(strPathPOEConifg);

                    try
                    {
                        string strLanguage = parser.GetSetting("LANGUAGE", "language");
                        if (strLanguage.Equals("ko-KR", StringComparison.OrdinalIgnoreCase))
                            g_strUILang = "KOR";
                        else if (strLanguage.Equals("en", StringComparison.OrdinalIgnoreCase))
                            g_strUILang = "ENG";
                        else
                            g_strUILang = "UNKNOWN";

                        // STEP #1 Done.
                        xuiFlatProgressBar2.Value = 1;
                        labelAddonStatus.Text = String.Format("Addon Data ({0})", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));

                        string strLocalChat = parser.GetSetting("UI", "show_local_chat");
                        if (strLocalChat.Equals("true", StringComparison.OrdinalIgnoreCase))
                            g_bShowLocalChat = true;
                        else
                            g_bShowLocalChat = false;

                        // STEP #2 Done.
                        xuiFlatProgressBar2.Value = 2;
                        labelAddonStatus.Text = String.Format("Addon Data ({0})", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));

                        if (g_strUILang == "UNKNOWN")
                        {
                            MSGForm frmMSG = new MSGForm();
                            frmMSG.btmConfirm.Visible = false;
                            frmMSG.btnENG.Visible = true;
                            frmMSG.btnKOR.Visible = true;
                            frmMSG.lbMsg.Text = "Can't find POE UI Configuration. What is your OPTION-UI Languge in POE?" +
                                "\r\n\r\n(언어 설정을 확인할 수 없습니다. 옵션-UI에서 어떤 언어를 사용하고 계신가요?)";
                            DialogResult dr = frmMSG.ShowDialog();
                            if (dr == DialogResult.Yes)
                                g_strUILang = "KOR";
                            else
                                g_strUILang = "ENG";
                        }

                        #region ⨌⨌ POE KEYS : FLASK, SKILL ⨌⨌
                        // FLASK KEYS
                        string strFlask1 = parser.GetSetting("ACTION_KEYS", "use_flask_in_slot1");
                        string strFlask2 = parser.GetSetting("ACTION_KEYS", "use_flask_in_slot2");
                        string strFlask3 = parser.GetSetting("ACTION_KEYS", "use_flask_in_slot3");
                        string strFlask4 = parser.GetSetting("ACTION_KEYS", "use_flask_in_slot4");
                        string strFlask5 = parser.GetSetting("ACTION_KEYS", "use_flask_in_slot5");
                        g_Flask1 = Convert.ToInt32(strFlask1);
                        g_Flask2 = Convert.ToInt32(strFlask2);
                        g_Flask3 = Convert.ToInt32(strFlask3);
                        g_Flask4 = Convert.ToInt32(strFlask4);
                        g_Flask5 = Convert.ToInt32(strFlask5);

                        // SKILL KEYS use_bound_skill4 ~ use_bound_skill8 ( Default : QWERT )
                        string strSkill1 = parser.GetSetting("ACTION_KEYS", "use_bound_skill4");
                        string strSkill2 = parser.GetSetting("ACTION_KEYS", "use_bound_skill5");
                        string strSkill3 = parser.GetSetting("ACTION_KEYS", "use_bound_skill6");
                        string strSkill4 = parser.GetSetting("ACTION_KEYS", "use_bound_skill7");
                        string strSkill5 = parser.GetSetting("ACTION_KEYS", "use_bound_skill8");
                        g_Skill1 = Convert.ToInt32(strSkill1);
                        g_Skill2 = Convert.ToInt32(strSkill2);
                        g_Skill3 = Convert.ToInt32(strSkill3);
                        g_Skill4 = Convert.ToInt32(strSkill4);
                        g_Skill5 = Convert.ToInt32(strSkill5);
                        #endregion

                        resolution_height = Convert.ToInt32(parser.GetSetting("DISPLAY", "resolution_height"));
                        resolution_width = Convert.ToInt32(parser.GetSetting("DISPLAY", "resolution_width"));
                    }
                    catch(Exception ex)
                    {
                        /*g_strUILang = "UNKNOWN";
                        g_bShowLocalChat = false;

                        MSGForm frmMSG = new MSGForm();
                        frmMSG.lbMsg.Text = "Can't read POE Configuration.\r\nPlease check POE UI Setting and Local Chat Enabled & Save Game Options.\r\nAnd Try again Please." +
                            "\r\n\r\n(게임 설정 정보를 확인할 수 없습니다.\r\n게임 실행 후, UI언어와 지역채팅 활성화를 확인하고 게임 옵션을 저장 후에\r\n다시 실행해주세요.)";
                        DialogResult dr = frmMSG.ShowDialog();

                        // Force Terminate Launcher.
                        if (dr == DialogResult.OK)
                            Application.Exit();
                        else
                            Application.Exit();*/
                        if (g_strUILang == "UNKNOWN")
                        {
                            MSGForm frmMSG = new MSGForm();
                            frmMSG.btmConfirm.Visible = false;
                            frmMSG.btnENG.Visible = true;
                            frmMSG.btnKOR.Visible = true;
                            frmMSG.lbMsg.Text = "Can't find POE UI Configuration. What is your OPTION-UI Languge in POE?" +
                                "\r\n\r\n(언어 설정을 확인할 수 없습니다. 옵션-UI에서 어떤 언어를 사용하고 계신가요?)";
                            DialogResult dr = frmMSG.ShowDialog();
                            if (dr == DialogResult.Yes)
                                g_strUILang = "KOR";
                            else
                                g_strUILang = "ENG";
                        }
                        log.Error($"catch {MethodBase.GetCurrentMethod().Name}", ex);
                    }
                    #endregion

                    // STEP #3 Done.
                    xuiFlatProgressBar2.Value = 3;
                    labelAddonStatus.Text = String.Format("Addon Data ({0})", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));

                    Get_NinjaData();
                }
            }
            else
            {
                /*MessageBox.Show(
                        @"There is a problem reaching update server please check your internet connection and try again later.",
                        @"Update check failed", MessageBoxButtons.OK, MessageBoxIcon.Error);*/
                MSGForm frmMSG = new MSGForm();
                frmMSG.lbMsg.Text = "There is a problem reaching update server.\r\nPlease check your internet connection and try again later" +
                    "\r\n\r\n(업데이트 서버에 연결할 수 없습니다.\r\n인터넷 연결을 확인하신 후 다시 시도해주세요.)";
                DialogResult dr = frmMSG.ShowDialog();

                // Force Terminate Launcher.
                if (dr == DialogResult.OK)
                    Application.Exit();
                else
                    Application.Exit();
            }
            this.btnClose.Enabled = true;
        }
        #endregion

        private void btnExitAndUpdate_Click(object sender, EventArgs e)
        {
            pictureBox2.Dispose();
            panelWaiting.Dispose();
            this.Close();
            Application.Exit();
        }

        private void Get_NinjaData()
        {
            Thread.Sleep(300);

            #region ⨌⨌ Parsing ADDON ConfigPath.ini ⨌⨌
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
                string strUserChoideLeague = parser.GetSetting("LEAGUE", "USERCHOICE");
                int nUserChoice = Convert.ToInt32(strUserChoideLeague);

                LEAGUE_STRING enumUerChoice = LEAGUE_STRING.LEAGUE_CURRENT;
                switch (nUserChoice)
                {
                    case 0:
                        enumUerChoice = LEAGUE_STRING.LEAGUE_CURRENT;
                        break;
                    case 1:
                        enumUerChoice = LEAGUE_STRING.LEAGUE_STANDARD;
                        break;
                    case 2:
                        enumUerChoice = LEAGUE_STRING.LEAGUE_HDCORE_CURRENT;
                        break;
                    case 3:
                        enumUerChoice = LEAGUE_STRING.LEAGUE_HDCORE_STANDARD;
                        break;
                    default:
                        enumUerChoice = LEAGUE_STRING.LEAGUE_CURRENT;
                        break;
                }

                g_CurrentLeague = enumUerChoice.ToDescriptionString();
                labelUserLeague.Text = String.Format("Last Your currecny checked league is '{0}'. ( 최근 시세 검색 '{1}' )", g_CurrentLeague, g_CurrentLeague);

                #region ⨌⨌ GET TIMER SETTING : FLASK, SKILL ⨌⨌
                // FLASK TIMER
                g_FlaskTime1 = parser.GetSetting("MISC", "FLASKTIME1");
                g_FlaskTime2 = parser.GetSetting("MISC", "FLASKTIME2");
                g_FlaskTime3 = parser.GetSetting("MISC", "FLASKTIME3");
                g_FlaskTime4 = parser.GetSetting("MISC", "FLASKTIME4");
                g_FlaskTime5 = parser.GetSetting("MISC", "FLASKTIME5");

                if (parser.GetSetting("MISC", "TOGGLE1ON") == "TRUE")
                    g_bToggle1 = true;
                else
                    g_bToggle1 = false;

                if (parser.GetSetting("MISC", "TOGGLE2ON") == "TRUE")
                    g_bToggle2 = true;
                else
                    g_bToggle2 = false;

                if (parser.GetSetting("MISC", "TOGGLE3ON") == "TRUE")
                    g_bToggle3 = true;
                else
                    g_bToggle3 = false;

                if (parser.GetSetting("MISC", "TOGGLE4ON") == "TRUE")
                    g_bToggle4 = true;
                else
                    g_bToggle4 = false;

                if (parser.GetSetting("MISC", "TOGGLE5ON") == "TRUE")
                    g_bToggle5 = true;
                else
                    g_bToggle5 = false;

                // SKILL TIMER
                g_SkillTime1 = parser.GetSetting("SKILL", "SKILLTIME1");
                g_SkillTime2 = parser.GetSetting("SKILL", "SKILLTIME2");
                g_SkillTime3 = parser.GetSetting("SKILL", "SKILLTIME3");
                g_SkillTime4 = parser.GetSetting("SKILL", "SKILLTIME4");
                g_SkillTime5 = parser.GetSetting("SKILL", "SKILLTIME5");

                if (parser.GetSetting("SKILL", "TOGGLESKILL1ON") == "TRUE")
                    g_bToggleSkill1 = true;
                else
                    g_bToggleSkill1 = false;
                
                if (parser.GetSetting("SKILL", "TOGGLESKILL2ON") == "TRUE")
                    g_bToggleSkill2 = true;
                else
                    g_bToggleSkill2 = false;

                if (parser.GetSetting("SKILL", "TOGGLESKILL3ON") == "TRUE")
                    g_bToggleSkill3 = true;
                else
                    g_bToggleSkill3 = false;

                if (parser.GetSetting("SKILL", "TOGGLESKILL4ON") == "TRUE")
                    g_bToggleSkill4 = true;
                else
                    g_bToggleSkill4 = false;

                if (parser.GetSetting("SKILL", "TOGGLESKILL5ON") == "TRUE")
                    g_bToggleSkill5 = true;
                else
                    g_bToggleSkill5 = false;
                #endregion

                g_strMyNickName = parser.GetSetting("CHARACTER", "MYNICK");
                g_strTRAutoKick = parser.GetSetting("CHARACTER", "AUTOKICK");

                string strGridWidth = parser.GetSetting("LOCATIONGRID", "RIGHT");
                string strGridHeight = parser.GetSetting("LOCATIONGRID", "BOTTOM");
                g_nGridWidth = Convert.ToInt32(parser.GetSetting("LOCATIONGRID", "RIGHT"));
                g_nGridHeight = Convert.ToInt32(parser.GetSetting("LOCATIONGRID", "BOTTOM"));

                g_strYNMouseWheelStashTab = parser.GetSetting("MISC", "MOUSESTASHTAB"); ; // Addded 1.3.9.6 Ver.
                if (String.IsNullOrEmpty(g_strYNMouseWheelStashTab))
                    g_strYNMouseWheelStashTab = "Y";

                if (g_strYNMouseWheelStashTab=="Y")
                    checkUseWheelStash.Checked = true;
                else
                    checkUseWheelStash.Checked = false;

                log.Info("CTRL+MOUSEWHEEL : " + g_strYNMouseWheelStashTab);

                // push pin LOCK, UNLOCK
                string strTemp = parser.GetSetting("MISC", "PUSHPINLOCK");
                if (strTemp.ToUpper() == "N")
                {
                    log.Info("PUSHPINLOCK = N");
                    g_pinLOCK = false;
                }
                else
                {
                    log.Info("PUSHPINLOCK = Y");
                    g_pinLOCK = true;
                }
                
                g_NotifyVolume = Convert.ToInt32(parser.GetSetting("LOCATIONNOTIFY", "VOLUME"));
            }
            catch(Exception ex)
            {
                log.Error($"catch {MethodBase.GetCurrentMethod().Name}", ex);

                MSGForm frmMSG = new MSGForm();
                frmMSG.lbMsg.Text = "Can't read conguration. File seems to be corrupt or delete.\r\nPlease Try again.\r\n\r\n" +
                    "(환경 파일이 손상되었거나 삭제되었습니다.\r\n프로그램을 종료하고 다시시도해주세요.)";
                DialogResult dr = frmMSG.ShowDialog();

                // Force Terminate Launcher.
                if (dr == DialogResult.OK)
                    Application.Exit();
                else
                    Application.Exit();
            }
            #endregion

            g_NinjaFileMakeAndUpdateCNT = 0;
            launcherTimer.Start();
            frmNinja = new NinjaForm();
            frmNinja.GetNinJaDataSync();
        }

        private void LauncherTimer_Tick(object sender, EventArgs e)
        {
            Thread.Sleep(100);
            // int nCnt = commonClass.GetFileCountFromFolder(g_NinjaDirectory, false);
            labelNINJASTATUS.Text = "POE.NINJA Data ("+ g_NinjaUpdatedTime + ")";

            xuiFlatProgressBar1.Value = g_NinjaFileMakeAndUpdateCNT;
            if (g_NinjaFileMakeAndUpdateCNT >= CNT_NINJACATEGORIES)
            {
                launcherTimer.Stop();
                Thread.Sleep(100);
                frmNinja.Hide();

                Get_DeadlyOverlayData(); // STEP #4~14 Done.

                Get_deadlyInformationData();

                // STEP #15 Done.
                xuiFlatProgressBar2.Value = 19;
                labelAddonStatus.Text = String.Format("Addon Data ({0})", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));

                Get_matchingENGKORData();

                // STEP #16 Done.
                xuiFlatProgressBar2.Value = 20;
                labelAddonStatus.Text = String.Format("Addon Data ({0})", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));

                g_strExplanationLANG = g_strUILang;
                Thread.Sleep(500);
                timerDetect.Start();
            }

            //log.Info("LauncherTimer_Tick : " + g_NinjaFileMakeAndUpdateCNT.ToString()); // Temporary.
        }

        private void Start_ControlForm()
        {
            g_bAddonLaunched = true;

            g_handlePathOfExile = InteropCommon.FindWindow("POEWindowClass", null); // ClassName = POEWindowClass
            // g_handleDeadlyTrade = FindWindow("WindowsForms10.Window.8.app.0.141b42a_r6_ad1", null); // DeadlyTrade CLASSID = "WindowsForms10.Window.8.app.0.141b42a_r6_ad1"

            Thread.Sleep(10);
            frmMainControl = new ControlForm();
            frmMainControl.ShowIcon = false;
            frmMainControl.ShowInTaskbar = false;
            //frmMainControl.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            frmMainControl.Show();

            // Load_MiscForms();

            labelReady.Text = "DeadlyTrade is running...";
            btnStartAddon.Image = Properties.Resources.DeadlyTradeStartButtonDisabled;
            WindowState = FormWindowState.Minimized;
            Hide();

            InteropCommon.SetForegroundWindow(g_handlePathOfExile);
        }

        private void TimerDetect_Tick(object sender, EventArgs e)
        {
            // TTTTT
            // Show Start Button
            // TTTTT ReadyToStartAddon();
            // TTTTT return;

            Thread.Sleep(100);

            #region ⨌⨌ Wait for POE Launching ⨌⨌
            g_handlePathOfExile = InteropCommon.FindWindow("POEWindowClass", "Path of Exile"); // ClassName = POEWindowClass
            if (g_handlePathOfExile!= IntPtr.Zero)
            {
                timerDetect.Stop();
                Thread.Sleep(100);

                g_POELogPath = InteropCommon.GetPathFromHandle(LauncherForm.g_handlePathOfExile);

                bool containsKG = Regex.IsMatch(g_POELogPath, Regex.Escape("KG"), RegexOptions.IgnoreCase);

                if (containsKG)
                {
                    g_POELogFileName = "KakaoClient.txt";
                    labelReady.Text = "POE [KAKAO Client] Readly to";

                    labelClient.Text = "CLIENT : KAKAO";
                }
                else
                {
                    g_POELogFileName = "Client.txt";
                    labelReady.Text = "POE [GGG Client] Readly to";

                    labelClient.Text = "CLIENT : GGG";
                }

                if (g_strUILang == "KOR")
                    pictureBoxKOREA.BackgroundImage = Properties.Resources.flag_korea;
                else if (g_strUILang == "ENG")
                    pictureBoxKOREA.BackgroundImage = Properties.Resources.flag_united_kingdom;
                else
                {
                    if(containsKG)
                    {
                        g_strUILang = "KOR";
                        pictureBoxKOREA.BackgroundImage = Properties.Resources.flag_korea;
                    }
                    else
                    {
                        g_strUILang = "ENG";
                        pictureBoxKOREA.BackgroundImage = Properties.Resources.flag_united_kingdom;
                    }
                }

                // Show resolution_width*resolution_height to Launcher : Modified 1.3.9.6 Ver.
                labelUI.Text = String.Format("({0}*{1}) Your UI : {2}", resolution_width.ToString(), resolution_height.ToString(), g_strUILang);
                labelClient.Visible = true;
                labelUI.Visible = true;
                pictureBoxKOREA.Visible = true;

                g_POELogPath = Path.GetDirectoryName(g_POELogPath);               
                
                labelReady.Visible = true;
                labelPOEAddonNotice.Text = "Path of Exile Detected. (게임 감지 완료)";

                g_POELogPath = String.Format("{0}\\Logs\\{1}", g_POELogPath, g_POELogFileName);
                labelPOERealPath.Text = g_POELogPath;

                // TTTTTTT // TTTTTTTg_POELogPath = @"D:\DAUM GAMES\PATH OF EXILE\LOGS\TESTCLIENT.TXT"; 

                string strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath.ini");

                if (resolution_width < 1920 && resolution_height < 1080)
                {
                    strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath_1600_1024.ini");
                    if (resolution_width < 1600 && resolution_height < 1024)
                        strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath_1280_768.ini");
                    else if (LauncherForm.resolution_width < 1280)
                        strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath_LOW.ini");
                }
                else if (resolution_width > 1920)
                    strINIPath = String.Format("{0}\\{1}", Application.StartupPath, "ConfigPath_HIGH.ini");

                IniParser parser = new IniParser(strINIPath);
                log.Info($"{MethodBase.GetCurrentMethod().Name} RESOLUTION : " + strINIPath);

                parser.AddSetting("CHARACTER", "MYNICK", g_strMyNickName);
                parser.AddSetting("CHARACTER", "AUTOKICK", g_strTRAutoKick);

                parser.AddSetting("DIRECTIONHELPER", "POELOGPATH", g_POELogPath);
                parser.AddSetting("MISC", "PUSHPINLOCK", g_pinLOCK ? "Y" : "N");

                parser.AddSetting("LOCATIONGRID", "RIGHT", g_nGridWidth.ToString());
                parser.AddSetting("LOCATIONGRID", "BOTTOM", g_nGridHeight.ToString());
                parser.AddSetting("MISC", "MOUSESTASHTAB", g_strYNMouseWheelStashTab);

                log.Info($"{MethodBase.GetCurrentMethod().Name} CTRL+MOUSEWHEEL : " + g_strYNMouseWheelStashTab);
                parser.SaveSettings();

                InteropCommon.SetForegroundWindow(LauncherForm.g_handlePathOfExile);

                // Set Foreground
                /*this.WindowState = FormWindowState.Minimized;
                this.Show();
                this.WindowState = FormWindowState.Normal;*/
                this.BringToFront();

                ScrollTick = DateTime.Now;
                // Show Start Button
                ReadyToStartAddon();
                btnCleaner.Enabled = true; // Added 1.3.9.4 Ver.
                IsExistPOESettingBackup();

                // Start_ControlForm(); // Added 1.3.9.0 Ver
                CheckUpdateLoop(); // Added 1.3.9.2 Ver.

                g_strDonator = String.Empty;
                // Read Update Contents.
                try
                {
                    WebClient wc = new WebClientWithTimeout();
                    var readData = wc.DownloadData("https://www.jumpleasure.me/deadlytrade/repository/Supporters.txt");
                    g_strDonator = Encoding.UTF8.GetString(readData);
                }
                catch (WebException ex)
                {
                    log.Error($"Read Contents. {MethodBase.GetCurrentMethod().Name}", ex);
                }

                ScrollingText();
            }
            #endregion
        }

        #region [[[[[ Real Time Supporters Scrolling ]]]]]
        private async Task ScrollingText()
        {
            while (this.Visible)
            {
                try
                {
                    DateTime nowTime = DateTime.Now;
                    double elapsed = ((TimeSpan)(nowTime - ScrollTick)).TotalMilliseconds;
                    if (elapsed > 200)
                    {
                        char[] chars = g_strDonator.ToCharArray();
                        char[] newChar = new char[chars.Length];
                        int l = chars.Length;
                        int k = 0;
                        for (int j = 0; j < chars.Length; j++)
                        {

                            if (j + 1 < chars.Length)
                                newChar[j] = chars[j + 1];
                            else
                                newChar[l - 1] = chars[k];
                        }
                        labelSupportersRealTime.Text = new string(newChar);
                        ScrollTick = DateTime.Now;
                    }
                }
                catch (Exception ex)
                {
                    log.Error($"catch {MethodBase.GetCurrentMethod().Name}", ex);
                }

                await Task.Delay(200);
            }
        }
        #endregion

        // Added 1.3.9.2
        private async Task CheckUpdateLoop()
        {
            while (true)
            {
                try
                {
                    #region [[[[[ Show Update Contents. ]]]]]
                    string strRead = String.Empty;
                    // Read Update Contents.
                    try
                    {
                        WebClient wc = new WebClientWithTimeout();
                        if (LauncherForm.g_strUILang == "KOR")
                        {
                            var readData = wc.DownloadData("https://www.jumpleasure.me/deadlytrade/repository/UpdateAvailable.txt");
                            strRead = Encoding.UTF8.GetString(readData);
                        }
                        else
                        {
                            var readData = wc.DownloadData("https://www.jumpleasure.me/deadlytrade/repository/UpdateAvailableEN.txt");
                            strRead = Encoding.UTF8.GetString(readData);
                        }
                    }
                    catch (WebException ex)
                    {
                        log.Error($"Read Contents. {MethodBase.GetCurrentMethod().Name}", ex);
                    }

                    Assembly assembly = Assembly.GetExecutingAssembly();
                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                    string version = fvi.FileVersion;

                    log.Info("strRead : " + strRead + " Your version : " + version);
                    if (strRead.Trim() != version) // 1.3.9.4
                    {
                        if (ControlForm.g_strZoneName.ToUpper().Contains("HIDEOUT") || ControlForm.g_strZoneName.Contains("은신처"))
                        {
                            ControlForm.bNeedtoShowAvailabeUpdate = true;
                        }
                    }                   
                    #endregion
                }
                catch (Exception ex)
                {
                    log.Error($"catch {MethodBase.GetCurrentMethod().Name}", ex);
                }

                await Task.Delay(1000*60*60*1); // 1000ms(1s) * 60 = 60s(1m) * 60 = 60m(1h) * 1 = 1h
            }
        }

        private void Get_DeadlyOverlayData()
        {
            var tmpData = new DeadlyOverlay();

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            try
            {
                using (var r = new StreamReader(Application.StartupPath + "\\SyndicateMember\\SyndicateInform.json"))
                {
                    var json = r.ReadToEnd();
                    tmpData.SyndicateDeadly = JsonConvert.DeserializeObject<Syndicate.RootObject>(json, settings);
                }
            }
            catch
            {
                MSGForm frmMSG = new MSGForm();
                frmMSG.lbMsg.Text = "Can't read Syndicate Information.\r\nPlease check your addon installation and Try again." +
                    "\r\n\r\n(신디케이트 정보를 읽을 수 없습니다.\r\n설치 파일을 확인 후 다시 시도해주세요.)";
                DialogResult dr = frmMSG.ShowDialog();

                // Force Terminate Launcher.
                if (dr == DialogResult.OK)
                    Application.Exit();
                else
                    Application.Exit();
            }

            deadlyOverlayData = tmpData;
            if(deadlyOverlayData.SyndicateDeadly.SyndiData.Count<=0)
            {
                MSGForm frmMSG = new MSGForm();
                frmMSG.lbMsg.Text = "Unknown error occured while reading Syndicate Information.\r\nPlease exit and Try again." +
                    "\r\n\r\n신디케이트 정보를 읽는 중 오류가 발생했습니다.\r\n프로그램을 종료 후 다시 실행해주세요.";
                DialogResult dr = frmMSG.ShowDialog();

                // Force Terminate Launcher.
                if (dr == DialogResult.OK)
                    Application.Exit();
                else
                    Application.Exit();
            }
        }

        private void Get_deadlyInformationData()
        {
            var tmpData = new DeadlyInformation();

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            #region ⨌⨌ Get Deadly JSON Data ⨌⨌
            try
            {
                using (var r = new StreamReader(Application.StartupPath + "\\AtlasDrop\\ZoneInform.json"))
                {
                    var json = r.ReadToEnd();
                    tmpData.InformationDeadly = JsonConvert.DeserializeObject<DeadlyAtlas.RootObject>(json, settings);

                    xuiFlatProgressBar2.Value = 5;
                    labelAddonStatus.Text = String.Format("Addon Data ({0})", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
                }

                using (var r = new StreamReader(Application.StartupPath + "\\AtlasDrop\\Maps.json"))
                {
                    var json = r.ReadToEnd();
                    tmpData.InformationMaps = JsonConvert.DeserializeObject<DeadlyAtlas.RootObjectMap>(json, settings);

                    xuiFlatProgressBar2.Value = 6;
                    labelAddonStatus.Text = String.Format("Addon Data ({0})", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
                }

                using (var r = new StreamReader(Application.StartupPath + "\\AtlasDrop\\Currency.json"))
                {
                    var json = r.ReadToEnd();
                    tmpData.InformationCurrency = JsonConvert.DeserializeObject<DeadlyAtlas.RootObjectCurruncy>(json, settings);

                    xuiFlatProgressBar2.Value = 7;
                    labelAddonStatus.Text = String.Format("Addon Data ({0})", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
                }

                using (var r = new StreamReader(Application.StartupPath + "\\AtlasDrop\\DivinationCards.json"))
                {
                    var json = r.ReadToEnd();
                    tmpData.InformationDivinationCard = JsonConvert.DeserializeObject<DeadlyAtlas.RootObjectDivinationCard>(json, settings);

                    xuiFlatProgressBar2.Value = 8;
                    labelAddonStatus.Text = String.Format("Addon Data ({0})", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
                }

                using (var r = new StreamReader(Application.StartupPath + "\\AtlasDrop\\Delve.json"))
                {
                    var json = r.ReadToEnd();
                    tmpData.InformationDelve = JsonConvert.DeserializeObject<DeadlyAtlas.RootObjectDelve>(json, settings);

                    xuiFlatProgressBar2.Value = 9;
                    labelAddonStatus.Text = String.Format("Addon Data ({0})", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
                }

                using (var r = new StreamReader(Application.StartupPath + "\\AtlasDrop\\Scarabs.json"))
                {
                    var json = r.ReadToEnd();
                    tmpData.InformationScarab = JsonConvert.DeserializeObject<DeadlyAtlas.RootObjectScarab>(json, settings);

                    xuiFlatProgressBar2.Value = 10;
                    labelAddonStatus.Text = String.Format("Addon Data ({0})", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
                }

                using (var r = new StreamReader(Application.StartupPath + "\\AtlasDrop\\MapFragments.json"))
                {
                    var json = r.ReadToEnd();
                    tmpData.InformationMapFragment = JsonConvert.DeserializeObject<DeadlyAtlas.RootObjectMapFragment>(json, settings);

                    xuiFlatProgressBar2.Value = 11;
                    labelAddonStatus.Text = String.Format("Addon Data ({0})", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
                }

                using (var r = new StreamReader(Application.StartupPath + "\\AtlasDrop\\Prophecies.json"))
                {
                    var json = r.ReadToEnd();
                    tmpData.InformationProphecy = JsonConvert.DeserializeObject<DeadlyAtlas.RootObjectProphecy>(json, settings);

                    xuiFlatProgressBar2.Value = 12;
                    labelAddonStatus.Text = String.Format("Addon Data ({0})", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
                }

                using (var r = new StreamReader(Application.StartupPath + "\\AtlasDrop\\UniqueMaps.json"))
                {
                    var json = r.ReadToEnd();
                    tmpData.InformationUniqueMap = JsonConvert.DeserializeObject<DeadlyAtlas.RootObjectUniqueMap>(json, settings);

                    xuiFlatProgressBar2.Value = 13;
                    labelAddonStatus.Text = String.Format("Addon Data ({0})", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
                }

                using (var r = new StreamReader(Application.StartupPath + "\\AtlasDrop\\Uniques.json"))
                {
                    var json = r.ReadToEnd();
                    tmpData.InformationUniqueItem = JsonConvert.DeserializeObject<DeadlyAtlas.RootObjectUnique>(json, settings);

                    xuiFlatProgressBar2.Value = 14;
                    labelAddonStatus.Text = String.Format("Addon Data ({0})", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
                }

                // NOTIFICATION MESSAGE
                using (var r = new StreamReader(Application.StartupPath + "\\DeadlyInform\\NotificationMSG.json"))
                {
                    var json = r.ReadToEnd();
                    tmpData.InformationMSG = JsonConvert.DeserializeObject<DeadlyAtlas.RootObjectNotifyMSG>(json, settings);

                    xuiFlatProgressBar2.Value = 15;
                    labelAddonStatus.Text = String.Format("Addon Data ({0})", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
                }

                // MAP ALERT MESSAGE SETTINGS
                using (var r = new StreamReader(Application.StartupPath + "\\DeadlyInform\\MapAlertSettingMSG.json"))
                {
                    var json = r.ReadToEnd();
                    tmpData.MapAlertMSG = JsonConvert.DeserializeObject<DeadlyAtlas.RootObjectMapAlertMSG>(json, settings);

                    xuiFlatProgressBar2.Value = 16;
                    labelAddonStatus.Text = String.Format("Addon Data ({0})", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
                }

                // OIL PASSIVES : OilPassiveJsonData
                using (var r = new StreamReader(Application.StartupPath + "\\DeadlyInform\\OilsPassivesCollection.json"))
                {
                    var json = r.ReadToEnd();
                    tmpData.OilPassiveJsonData = JsonConvert.DeserializeObject<DeadlyAtlas.RootObjectOilsPassive>(json, settings);

                    xuiFlatProgressBar2.Value = 17;
                    labelAddonStatus.Text = String.Format("Addon Data ({0})", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
                }
            }
            catch
            {
                MSGForm frmMSG = new MSGForm();
                frmMSG.lbMsg.Text = "Can't read Deadly Mapping Information.\r\nPlease check your addon installation and Try again." +
                    "\r\n\r\n(Deadly 매핑 정보를 읽을 수 없습니다.\r\n설치 파일을 확인 후 다시 시도해주세요.)";
                DialogResult dr = frmMSG.ShowDialog();

                // Force Terminate Launcher.
                if (dr == DialogResult.OK)
                    Application.Exit();
                else
                    Application.Exit();
            }
            #endregion

            deadlyInformationData = tmpData;
            try
            {
                if (deadlyInformationData.InformationCurrency.Currency.Count <= 0 ||
                  deadlyInformationData.InformationDeadly.AtlasData.Count <= 0 ||
                  deadlyInformationData.InformationDelve.Delve.Count <= 0 ||
                  deadlyInformationData.InformationDivinationCard.DivinationCards.Count <= 0 ||
                  deadlyInformationData.InformationMapFragment.MapFragments.Count <= 0 ||
                  deadlyInformationData.InformationMaps.Maps.Count <= 0 ||
                  deadlyInformationData.InformationProphecy.Prophecies.Count <= 0 ||
                  deadlyInformationData.InformationScarab.Scarabs.Count <= 0 ||
                  deadlyInformationData.InformationUniqueItem.Uniques.Count <= 0 ||
                  deadlyInformationData.InformationUniqueMap.UniqueMaps.Count <= 0 ||
                  deadlyInformationData.InformationMSG.NotifyMSG.Count <= 0 ||
                  deadlyInformationData.MapAlertMSG.MapAlertMSG.Count <= 0)
                {
                    MSGForm frmMSG = new MSGForm();
                    frmMSG.lbMsg.Text = "Can't read Deadly Mapping Information~.\r\nPlease check your addon installation and Try again~." +
                    "\r\n\r\n(Deadly 매핑 정보를 읽을 수 없습니다~.\r\n설치 파일을 확인 후 다시 시도해주세요~.)";
                    DialogResult dr = frmMSG.ShowDialog();

                    // Force Terminate Launcher.
                    if (dr == DialogResult.OK)
                        Application.Exit();
                    else
                        Application.Exit();
                }
            }
            catch
            {
                MSGForm frmMSG = new MSGForm();
                frmMSG.lbMsg.Text = "Can't read Deadly Mapping Information~!.\r\nPlease check your addon installation and Try again~!." +
                    "\r\n\r\n(Deadly 매핑 정보를 읽을 수 없습니다~!.\r\n설치 파일을 확인 후 다시 시도해주세요~!.)";
                DialogResult dr = frmMSG.ShowDialog();

                // Force Terminate Launcher.
                if (dr == DialogResult.OK)
                    Application.Exit();
                else
                    Application.Exit();
            }

            foreach (var item in deadlyInformationData.InformationMSG.NotifyMSG)
            {
                if (item.Id == "THX")
                    g_strnotiDONE = item.Msg;
                else if (item.Id == "WAIT")
                    g_strnotiWAIT = item.Msg;
                else if (item.Id == "WILLING")
                    g_strnotiRESEND = item.Msg;
                else if (item.Id == "SOLD")
                    g_strnotiSOLD = item.Msg;
            }

            foreach (var item in deadlyInformationData.MapAlertMSG.MapAlertMSG)
            {
                if(item.Id == "RED")
                {
                    foreach (var itemMsg in item.Msg)
                    {
                        g_strArrREDAlert.Add(itemMsg.ToString());
                    }
                }
                else if(item.Id == "GREEN")
                {
                    foreach (var itemMsg in item.Msg)
                    {
                        g_strArrGREENAlert.Add(itemMsg);
                    }
                }
            }
        }

        public void Get_matchingENGKORData()
        {
            var tmpData = new DeadlyENGKORMatching();

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            try
            {
                using (var r = new StreamReader(Application.StartupPath + "\\DeadlyInform\\ENGmatchKOR.json"))
                {
                    var json = r.ReadToEnd();
                    tmpData.engkorMatching = JsonConvert.DeserializeObject<ConvertKOR.RootObject>(json, settings);
                }
            }
            catch
            {
                MSGForm frmMSG = new MSGForm();
                frmMSG.lbMsg.Text = "Can't read Translation Information..\r\nPlease check your addon installation and Try again." +
                    "\r\n\r\n(번역 정보를 읽을 수 없습니다.\r\n설치 파일을 확인 후 다시 시도해주세요.)";
                DialogResult dr = frmMSG.ShowDialog();

                // Force Terminate Launcher.
                if (dr == DialogResult.OK)
                    Application.Exit();
                else
                    Application.Exit();
            }

            matchingENGKORData = tmpData;
            if(matchingENGKORData.engkorMatching.EnkrData.Count<=0)
            {
                MSGForm frmMSG = new MSGForm();
                frmMSG.lbMsg.Text = "Unknown error occured while reading Translation Information.\r\nPlease exit addon and Launch again." +
                    "\r\n\r\n(번역 정보를 읽는 중 오류가 발생했습니다.\r\n설치 파일을 확인 후 다시 시도해주세요.)";
                DialogResult dr = frmMSG.ShowDialog();

                // Force Terminate Launcher.
                if (dr == DialogResult.OK)
                    Application.Exit();
                else
                    Application.Exit();
            }
        }

        private void PictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            nMoving = 1;
            nMovePosX = e.X;
            nMovePosY = e.Y;
        }

        private void PictureBox3_MouseMove(object sender, MouseEventArgs e)
        {
            if (nMoving == 1)
            {
                this.SetDesktopLocation(MousePosition.X - nMovePosX, MousePosition.Y - nMovePosY);
            }
        }

        private void PictureBox3_MouseUp(object sender, MouseEventArgs e)
        {
            nMoving = 0;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void BtnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void LauncherForm_Resize(object sender, EventArgs e)
        {
            if(FormWindowState.Minimized == this.WindowState)
            {
                notifyIconDeadlyTrade.Visible = true;
                notifyIconDeadlyTrade.ShowBalloonTip(500);
                this.Hide();
                ShowInTaskbar = false;
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIconDeadlyTrade.Visible = false;
            }
        }

        #region ⨌⨌ NotifyIcon : Tray ⨌⨌
        private void NotifyIconDeadlyTrade_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
        }

        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }
        #endregion

        private void BtnStartAddon_Click(object sender, EventArgs e)
        {
            if (!g_bCanLaunchAddon)
            {
                MSGForm frmMSG = new MSGForm();
                frmMSG.lbMsg.Text = "Waiting for 'POE' Started\r\n(게임이 실행 되기를 기다리고 있습니다.)";
                frmMSG.ShowDialog();
                return;
            }

            if (g_bAddonLaunched)
            {
                WindowState = FormWindowState.Minimized;
                Hide();
            }

            if(!g_bShowLocalChat)
            {
                MSGForm frmMSG = new MSGForm();
                frmMSG.lbMsg.Text = "Addon use LOCAL CHAT Information.\r\nPlease turn on LOCAL CHAT." +
                    "\r\n\r\n(지역 채팅 정보가 필요합니다.\r\n지역 채팅을 켜주세요.)";
                frmMSG.ShowDialog();
            }

            Start_ControlForm();
        }              

        #region ⨌⨌ FormClosed : Dispose All ⨌⨌
        private void LauncherForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmNinja != null) frmNinja.Close();
            if (frmMainControl != null) frmMainControl.Close();
            if (commonClass != null) commonClass = null;

            if (ninjaData != null) ninjaData = null;
            if (deadlyOverlayData != null) deadlyOverlayData = null;
        }
        #endregion

        private void btnCleaner_Click(object sender, EventArgs e)
        {
            OptimizerForm frmOptimizer = new OptimizerForm();
            frmOptimizer.ShowDialog();
        }

        private void IsExistPOESettingBackup()
        {
            btnBackup.Enabled = true;
            if(File.Exists(Application.StartupPath + "\\POESetting\\production_Config.ini"))
            {
                btnRestore.Enabled = true;
            }
            else
            {
                btnRestore.Enabled = false;
            }
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            if (File.Exists(Application.StartupPath + "\\POESetting\\production_Config.ini"))
            {
                MSGForm frmMSG = new MSGForm();
                frmMSG.btmConfirm.Visible = false;
                frmMSG.btnENG.Text = "NO";
                frmMSG.btnENG.Visible = true;
                frmMSG.btnKOR.Text = "YES";
                frmMSG.btnKOR.Visible = true;
                frmMSG.lbMsg.Text = "Already exist POE setting file in backup folder. Want to overwrite?" +
                    "\r\n\r\n(백업된 POE 설정파일이 있습니다. 덮어쓰시겠습니까?)";
                DialogResult dr = frmMSG.ShowDialog();
                if (dr == DialogResult.Yes)
                {
                    BackupPOESetting();
                }
            }
            else
                BackupPOESetting();
        }

        private void BackupPOESetting()
        {
            try
            {
                String strPathPOEConifg = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                strPathPOEConifg = strPathPOEConifg + "\\My Games\\Path of Exile\\production_Config.ini";
                File.Copy(strPathPOEConifg, Application.StartupPath + "\\POESetting\\production_Config.ini");
            }
            catch (Exception ex)
            {
                log.Error($"File Copy. {MethodBase.GetCurrentMethod().Name}", ex);
                MessageBox.Show("Backup : Failed.");
            }
            finally
            {
                MessageBox.Show("Backup : Success.");
                btnRestore.Enabled = true;
            }
        }

        private void RestorePOESetting()
        {
            try
            {
                String strPathPOEConifg = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                strPathPOEConifg = strPathPOEConifg + "\\My Games\\Path of Exile\\production_Config.ini";
                File.Copy(Application.StartupPath + "\\POESetting\\production_Config.ini", strPathPOEConifg);
            }
            catch (Exception ex)
            {
                log.Error($"File Copy. {MethodBase.GetCurrentMethod().Name}", ex);
                MessageBox.Show("Restore : Failed.");
            }
            finally
            {
                MessageBox.Show("Restore :Success.");
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (File.Exists(Application.StartupPath + "\\POESetting\\production_Config.ini"))
            {
                MSGForm frmMSG = new MSGForm();
                frmMSG.btmConfirm.Visible = false;
                frmMSG.btnENG.Text = "NO";
                frmMSG.btnENG.Visible = true;
                frmMSG.btnKOR.Text = "YES";
                frmMSG.btnKOR.Visible = true;
                frmMSG.lbMsg.Text = "Really want to restore POE setting file to My Games folder from backups?" +
                    "\r\n\r\n(백업해둔 POE 설정 파일을 My Games 폴더로 복원하시겠습니까?)";
                DialogResult dr = frmMSG.ShowDialog();
                if (dr == DialogResult.Yes)
                {
                    RestorePOESetting();
                }
            }
            else
                MessageBox.Show("There is not exist backup file.");
        }

        private void checkUseWheelStash_CheckedChanged(object sender, EventArgs e)
        {
            if (checkUseWheelStash.Checked)
                g_strYNMouseWheelStashTab = "Y";
            else
                g_strYNMouseWheelStashTab = "N";

            log.Info("CTRL+MOUSEWHEEL Checked : " + g_strYNMouseWheelStashTab);
            // Save to INI
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
            parser.AddSetting("MISC", "MOUSESTASHTAB", g_strYNMouseWheelStashTab);
            parser.SaveSettings();
        }

        private void btnToonation_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://toon.at/donate/deadly_trade");
        }

        private void btnPatron_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.patreon.com/bePatron?u=25155273");
        }

        private void btnPaypalSub_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.jumpleasure.me/deadlytrade/?page_id=455");
        }

        private void btnPaypalSub_Click_1(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.jumpleasure.me/deadlytrade/?page_id=455");
        }
    }
}
