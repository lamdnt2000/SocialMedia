using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Joins;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Business.Schedule
{
    public class RegexUtil
    {
        public static string PATTERN_VALID_URL = @"(?:https?:\/\/)?(?:www\.)?((mbasic.facebook|m\.facebook|facebook|fb)\.(?:com|me)|(m\.tiktok|tiktok)\.(?:com)|(m\.youtube|youtube)\.(?:com))\/";
        public static string VALID_FACEBOOK_USER = @"(?:pages\/|profile\.php\?id\=)?(?:[\w\-\.]*\/)*([\w\-\.]*)";
        public static string VALID_TIKTOK_USER = @"^(?:@)((?!.*\.\.)(?!.*\.$)[^\W][\w.]{0,29})$";
        public static string VALID_YOUTUBE_USER = @"^(?:channel\/|\@|c\/)([a-z\-_0-9\.]+)\/?(?:[\?#]?.*)$";


        public static (string,string) RegexPlatformAndUser(string url)
        {
            Regex rg = new Regex(PATTERN_VALID_URL);
            if (url.EndsWith("/"))
            {
                url = url.Substring(0, url.Length - 1);
            }
            Match match = rg.Match(url);
            var result = match.Value;
            url = url.Replace(result, "");
            if (result.Contains("facebook"))
            {
                rg = new Regex(VALID_FACEBOOK_USER, RegexOptions.IgnoreCase);
                match = rg.Match(url);
                return ("F",match.Groups[1].Value);
                
            }
            else if (result.Contains("tiktok"))
            {
                rg = new Regex(VALID_TIKTOK_USER, RegexOptions.IgnoreCase);
                match = rg.Match(url);
                return ("T", match.Groups[1].Value);
            }
            else if (result.Contains("youtube"))
            {
               
                rg = new Regex(VALID_YOUTUBE_USER, RegexOptions.IgnoreCase);
                match = rg.Match(url);
                return ("Y ", url.StartsWith("@")?"@" + match.Groups[1].Value : match.Groups[1].Value);
            }
           
            return (null,null);
        }
    }
}
