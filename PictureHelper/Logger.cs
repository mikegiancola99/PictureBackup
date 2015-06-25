using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PictureHelper
{
    class Logger
    {
        private static StreamWriter logFilePtr = null;
        private static StreamWriter dupeFilePtr = null;
        private static StreamWriter copyFilePtr = null;

        public static void OpenLogs()
        {
            if ((logFilePtr != null) || (dupeFilePtr != null) || (copyFilePtr != null))
            {
                return;
            }

            DateTime curTime = DateTime.Now;
            String logDir = Convert.ToString(curTime.Hour) + Convert.ToString(curTime.Minute) + Convert.ToString(curTime.Second);

            Directory.CreateDirectory(logDir);

            logFilePtr = File.AppendText(logDir + "\\log.txt");
            dupeFilePtr = File.AppendText(logDir + "\\dupe.txt");
            copyFilePtr = File.AppendText(logDir + "\\copy.txt");
        }

        public static void CloseLogs()
        {
            if (logFilePtr != null)
                logFilePtr.Close();

            if (dupeFilePtr != null)
                dupeFilePtr.Close();

            if (copyFilePtr != null)
                copyFilePtr.Close();

            dupeFilePtr = null;
            logFilePtr = null;
            copyFilePtr = null;
        }

        public static void Log(String inMsg)
        {
            if (logFilePtr != null)
                logFilePtr.WriteLine(inMsg);
            logFilePtr.Flush();
        }

        public static void LogDupe(PictureFile inFile1, PictureFile inFile2)
        {
            if (dupeFilePtr != null)
            {
                String message = inFile1.DestDir.Trim() + inFile1.Filename.Trim() + " (" + inFile1.MD5Hash + ")" + " matches " + inFile2.FullSrcPath.Trim() + " (" + inFile2.MD5Hash + ")";
                dupeFilePtr.WriteLine(message);
            }
        }

        public static void LogCopy(String inMsg)
        {
            if (copyFilePtr != null)
                copyFilePtr.WriteLine(inMsg);
        }
    }
}
