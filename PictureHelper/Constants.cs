using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PictureHelper
{
    class Constants
    {
        //public static String BaseSrcDir = "c:\\shared\\";
        //public static String BaseDestDir = "c:\\pictures\\";
        
        public static String BaseSrcDir = "C:\\ShareMe\\test\\src\\";
        public static String BaseDestDir = "C:\\ShareMe\\test\\dest\\";

        public static void LoadConfig(String inConfFile)
        {
            XDocument document = XDocument.Load(inConfFile);
            BaseSrcDir = document.Descendants("SrcDir").First().Value;
            BaseDestDir = document.Descendants("DestDir").First().Value;
        }
    }
}
