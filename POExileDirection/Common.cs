using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POExileDirection
{
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
        }
    }

    public class OverlayHotkeys
    {
        public fsModifiers fsMod { get; set; }
        public Keys hotKeys { get; set; }
    }

    public class Common
    {
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
    }
}
