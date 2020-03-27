using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POExileDirection
{
    public enum LEAGUE_STRING
    {
        [Description("Metamorph")]
        LEAGUE_CURRENT,

        [Description("Standard")]
        LEAGUE_STANDARD,

        [Description("Hardcore Metamorph")]
        LEAGUE_HDCORE_CURRENT,

        [Description("Hardcore")]
        LEAGUE_HDCORE_STANDARD,
    };

    public static class LEAGUE_STRINGExtensions
    {
        public static string ToDescriptionString(this LEAGUE_STRING val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }

    public class DeadlyRegEx
    {
        // ZONE
        public Regex RegExZoneEnteredENG { get; set; } // new Regex(@"You have entered (.*)\.", RegexOptions.IgnoreCase);
        public Regex RegExZoneEnteredKOR { get; set; } // new Regex(@": (.*)에 진입했습니다.", RegexOptions.IgnoreCase);

        // MONSTER
        public Regex RegExMonsterRemainsENG { get; set; } // new Regex(@": (.*) monsters remain."); // : 3 monsters remain.
        public Regex RegExMonsterRemainsKOR { get; set; } // new Regex(@": 몬스터 (.*)개체가 남아있습니다."); // : 몬스터 0개체가 남아있습니다.
        public Regex RegExMonsterRemainsKORMore { get; set; } // : 몬스터가 (.*)개체 이상 남아있습니다.
        public Regex RegExMonsterRemainsENGMore { get; set; } // : More than 50 monsters remain.

        // Joined the area.
        public Regex RegExJoinedAreENG { get; set; } // : Ian_Curtis has joined the area.
        public Regex RegExJoinedAreKOR { get; set; } // Ian_Curtis has joined the area.

        // Chat Scan : Trade Channel
        public Regex RegExChatTradeChannel { get; set; }

        // Trade Message - English
        public Regex RegExENGPriceWithTabName { get; set; }
        public Regex RegExENGPriceNoTabName { get; set; }
        public Regex RegExENGUnPrice { get; set; }
        public Regex RegExENGBulkCurrencies { get; set; }
        public Regex RegExENGCurrency { get; set; }
        public Regex RegExENGMapLiveSite { get; set; }
            // BUYING
            public Regex RegExENGPriceWithTabNameKAKAO { get; set; } // ^@(.*\s)?(.+) Hi, I (.+ to buy your\s+?(.+?)(\s+?listed for\s+?([\d\.]+?)\s+?(.+))?\s+?in\s+?(.+?)\s+?\(stash tab \"(.*)\"; position: left (\d+), top (\d+)\)\s*?(.*))$
            public Regex RegExENGPriceNoTabNameKAKAO { get; set; }
            public Regex RegExENGCurrencyKAKAO { get; set; }   
            public Regex RegExENGMapLiveSiteKAKAO { get; set; }

        // Trade Message - Korean
        public Regex RegExKORPriceWithTabName { get; set; }
        public Regex RegExKORPriceNoTabName { get; set; }
        public Regex RegExKORUnPrice { get; set; }
        public Regex RegExKORBulkCurrencies { get; set; }
        public Regex RegExKORCurrency { get; set; }
        public Regex RegExKORMapLiveSite { get; set; }
            // BUYING
            public Regex RegExKORPriceWithTabNameKAKAO { get; set; }
            public Regex RegExKORUnPriceKAKAO { get; set; }
        

    }

    public class DeadlyTRADE
    {
        public class TradeMSG
        {
            // 알림 패널 정보
            public string id { get; set; }
            public bool expanded { get; set; }

            // 구매, 판매 정보
            public string tradePurpose { get; set; } // 거래 목적 : 구매? 판매?

            // 기본 정보
            public string fullMSG { get; set; } // 전체 메시지
            public string league { get; set; } // 전체 메시지
            public string nickName { get; set; } // 누가
            public string tabName { get; set; } // 어떤 보관함의
            public string xPos { get; set; } // 가로 좌표
            public string yPos { get; set; } // 세로 좌표
            public string itemName { get; set; } // 어떤 아이템을

            // 메시지 보낸 사람이 내는 가격
            public string priceCall { get; set; } // 얼마의 (요청자) :: 나의 170
            public string whichCurrency { get; set; } // 어떤 커런시로 (요청자) :: 나의 Chaos Orb 로

            // 추가 메시지
            public string offerMSG { get; set; } // 추가로 할말과 함께

            // 메시지 받는 사람에게 원하는 커런시
            public string priceYour { get; set; } // 대상자의 얼마를 :: 너의 1
            public string yourCurrency { get; set; } // 대상자의 어떤 커런시를 :: 너의 Exalted Orb 를

        }
    }

    public enum UI_LANG
    {
        UI_KOREAN,
        UI_ENGLISH,
        UI_UNKOWN,
        UI_ERROR,
    };

    public enum OVERLAY_WHAT
    {
        OVER_JUN,
        OVER_ALVA,
        OVER_MAP,
    }

    public struct RECT
    {
        public int left, top, right, bottom;
    }

    public enum fsModifiers
    {
        None = 0x0000,
        Alt = 0x0001,
        Control = 0x0002,
        Shift = 0x0004,
        Window = 0x0008,
    }

    public class ConvertKOR
    {
        public class RootObject
        {
            public List<EnkrDataCollections> EnkrData { get; set; }
        }

        public class EnkrDataCollections
        {
            public string id { get; set; }
            public string kr { get; set; }
            public string en { get; set; }
            public string DDSFile { get; set; }
        }
    }
    
    public class Syndicate
    {
        public class SyndiDataCollections
        {
            public List<string> word { get; set; }
            public string kr { get; set; }
            public string en { get; set; }
            public string tier { get; set; }
            public List<string> choice { get; set; }
            public List<string> T { get; set; }
            public List<string> F { get; set; }
            public List<string> R { get; set; }
            public List<string> I { get; set; }
        }

        public class RootObject
        {
            public List<SyndiDataCollections> SyndiData { get; set; }
        }
    }

    public class DeadlyTradeUpdate
    {
        public class Notice
        {
            public string Version { get; set; }
            public string Title { get; set; }
            public string TitleKR { get; set; }
            public List<string> Contents { get; set; }
            public List<string> ContentsKR { get; set; }
        }

        public class RootObjectNotice
        {
            public List<Notice> Notice { get; set; }
        }
    }

    public class DeadlyAtlas
    {
        public class OilsPassive
        {
            public string OilA { get; set; }
            public string OilB { get; set; }
            public string OilC { get; set; }
            public string PassiveName { get; set; }
            public string KORName { get; set; }
            public string Effect { get; set; }
        }

        public class RootObjectOilsPassive
        {
            public List<OilsPassive> OilsPassive { get; set; }
        }

        public class AtlasDataCollections
        {
            public string en { get; set; }
            public string kr { get; set; }
            public List<string> drop { get; set; }
        }

        public class RootObject
        {
            public List<AtlasDataCollections> AtlasData { get; set; }
        }

        public class MapCollection
        {
            public string Id { get; set; }
            public string Text_en { get; set; }
            public string Text_ko { get; set; }
            public string DDSFile { get; set; }
        }

        public class RootObjectMap
        {
            public List<MapCollection> Maps { get; set; }
        }

        public class CurrencyCollection
        {
            public string Id { get; set; }
            public string Name_en { get; set; }
            public string Name_ko { get; set; }
            public string DDSFile { get; set; }
        }

        public class RootObjectCurruncy
        {
            public List<CurrencyCollection> Currency { get; set; }
        }

        public class DivinationCardCollection
        {
            public string Id { get; set; }
            public string Name_en { get; set; }
            public string Name_ko { get; set; }
            public string DDSFile { get; set; }
        }

        public class RootObjectDivinationCard
        {
            public List<DivinationCardCollection> DivinationCards { get; set; }
        }

        public class DelveCollection
        {
            public string Id { get; set; }
            public string Name_en { get; set; }
            public string Name_ko { get; set; }
            public string DDSFile { get; set; }
        }

        public class RootObjectDelve
        {
            public List<DelveCollection> Delve { get; set; }
        }

        public class ScarabCollection
        {
            public string Id { get; set; }
            public string Name_en { get; set; }
            public string Name_ko { get; set; }
            public string DDSFile { get; set; }
        }

        public class RootObjectScarab
        {
            public List<ScarabCollection> Scarabs { get; set; }
        }

        public class MapFragmentCollection
        {
            public string Id { get; set; }
            public string Name_en { get; set; }
            public string Name_ko { get; set; }
            public string DDSFile { get; set; }
        }

        public class RootObjectMapFragment
        {
            public List<MapFragmentCollection> MapFragments { get; set; }
        }

        public class ProphecyCollection
        {
            public string Id { get; set; }
            public string Name_en { get; set; }
            public string Name_ko { get; set; }
            public string DDSFile { get; set; }
        }

        public class RootObjectProphecy
        {
            public List<ProphecyCollection> Prophecies { get; set; }
        }

        public class UniqueMapCollection
        {
            public string Id { get; set; }
            public string Text_en { get; set; }
            public string Text_ko { get; set; }
            public string DDSFile { get; set; }
        }

        public class RootObjectUniqueMap
        {
            public List<UniqueMapCollection> UniqueMaps { get; set; }
        }

        public class UniqueCollection
        {
            public string Id { get; set; }
            public string Text_en { get; set; }
            public string Text_ko { get; set; }
            public string DDSFile { get; set; }
        }

        public class RootObjectUnique
        {
            public List<UniqueCollection> Uniques { get; set; }
        }

        public class NotifyMSGCollection
        {
            public string Id { get; set; }
            public string Msg { get; set; }
        }

        public class RootObjectNotifyMSG
        {
            public List<NotifyMSGCollection> NotifyMSG { get; set; }
        }

        public class MapAlertMSGCollection
        {
            public string Id { get; set; }
            public List<string> Msg { get; set; }
        }

        public class RootObjectMapAlertMSG
        {
            public List<MapAlertMSGCollection> MapAlertMSG { get; set; }
        }
    }

    /*public class DeadlyHotkeys
    {
        public string fsModString { get; set; }
        public string hotKeyNumber { get; set; }
    }*/

    public class DeadlyHotkeys
    {
        public fsModifiers fsMod { get; set; }
        public Keys hotKeys { get; set; }
    }

    public class Common
    {
        public static Bitmap cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }

        public static Bitmap ScaleImage(Bitmap bmp, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / bmp.Width;
            var ratioY = (double)maxHeight / bmp.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(bmp.Width * ratio);
            var newHeight = (int)(bmp.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(bmp, 0, 0, newWidth, newHeight);

            return newImage;
        }

        Dictionary<string, string> ActZoneNamePart1 = new Dictionary<string, string>();
        Dictionary<string, string> ActZoneNamePart2 = new Dictionary<string, string>();
        Dictionary<string, string> ActZoneNamePart1ENG = new Dictionary<string, string>();
        Dictionary<string, string> ActZoneNamePart2ENG = new Dictionary<string, string>();

        public void InitActZoneDictionary()
        {
            // KOR Part One
            ActZoneNamePart1.Add("I", "라이온아이 초소");
            ActZoneNamePart1.Add("II", "숲 야영지");
            ActZoneNamePart1.Add("III", "사안 야영지");
            ActZoneNamePart1.Add("IV", "하이게이트");
            ActZoneNamePart1.Add("V", "감시탑");
            // KOR Part Two
            ActZoneNamePart2.Add("VI", "라이온아이 초소");
            ActZoneNamePart2.Add("VII", "다리 야영지");
            ActZoneNamePart2.Add("VIII", "사안 야영지");
            ActZoneNamePart2.Add("IX", "하이게이트");
            ActZoneNamePart2.Add("X", "오리아스 부두");

            // ENG Part One
            ActZoneNamePart1ENG.Add("I", "Lioneye's Watch");
            ActZoneNamePart1ENG.Add("II", "The Forest Encampment");
            ActZoneNamePart1ENG.Add("III", "The Sarn Encampment");
            ActZoneNamePart1ENG.Add("IV", "Highgate");
            ActZoneNamePart1ENG.Add("V", "Overseer's Tower");
            // ENG Part Two
            ActZoneNamePart2ENG.Add("VI", "Lioneye's Watch");
            ActZoneNamePart2ENG.Add("VII", "The Bridge Encampment");
            ActZoneNamePart2ENG.Add("VIII", "The Sarn Encampment");
            ActZoneNamePart2ENG.Add("IX", "Highgate");
            ActZoneNamePart2ENG.Add("X", "Oriath  Docks");
        }


        public string GetActROMAbyZoneName(string strZoneName, bool bIsPartTwo)
        {
            bool bFound = false;
            string strRet = "";

            // Check Part One - KOR
            if (bIsPartTwo == false)
            {
                strRet = ActZoneNamePart1.FirstOrDefault(x => x.Value == strZoneName && (bFound = true)).Key;
                if (bFound)
                    return strRet;
            }
            else // Check Part Two - KOR
            {
                strRet = ActZoneNamePart2.FirstOrDefault(x => x.Value == strZoneName && (bFound = true)).Key;
                if (bFound)
                    return strRet;
            }

            // Check Part One - ENG
            if (bIsPartTwo == false)
            {
                strRet = ActZoneNamePart1ENG.FirstOrDefault(x => x.Value == strZoneName && (bFound = true)).Key;
                if (bFound)
                    return strRet;
            }
            else // Check Part Two- ENG
            {
                strRet = ActZoneNamePart2ENG.FirstOrDefault(x => x.Value == strZoneName && (bFound = true)).Key;
                if (bFound)
                    return strRet;
            }

            // Last Check
            if (strZoneName == "오리아스" || strZoneName == "Oriath")
                return "O";
            else if (strZoneName == "템플러의 실험실" || strZoneName == "The Templar Laboratory")
                return "Z";
            else if (strZoneName.Contains(" 은신처") || strZoneName.Contains(" Hideout"))
                return "H";
            else
                return "?";
        }

        public int GetFileCountFromFolder(string strPath, bool bSearchSubDir)
        {
            int nCnt = 0;

            try
            {
                if (bSearchSubDir)
                    nCnt = Directory.GetFiles(strPath, "*", SearchOption.AllDirectories).Length;    // searches the current directory and sub directory
                else
                    nCnt = Directory.GetFiles(strPath, "*", SearchOption.TopDirectoryOnly).Length;  // searches the current directory
            }
            catch
            {

            }

            return nCnt;
        }

        public void DeleteAllFilesInFoder(string strPath)
        {
            // Delete all files in a directory  
            try
            {
                string[] files = Directory.GetFiles(strPath);
                foreach (string file in files)
                    File.Delete(file);
            }
            catch
            {

            }
        }
    }
}
