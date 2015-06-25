using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace PictureHelper
{
    class DataFile
    {
        private String fMD5;
        private String fFullSrcPath;
        private String fDestDir;
        private String fFilename;

        public DataFile()
        {
            fMD5 = "";
            fFullSrcPath = "";
            fDestDir = "";
            fFilename = "";
        }

        public String FullSrcPath 
        {
            get
            {
                return fFullSrcPath;
            }
            set
            {
                fFullSrcPath = value;
            }
        }

        public String MD5Hash
        {
            get
            {
                return fMD5;
            }
            set
            {
                fMD5 = value.Trim();
            }
        }

        public String DestDir 
        {
            get { return fDestDir; }
            set
            {
                fDestDir = value;
                fDestDir = fDestDir.Trim();
            }
        }
        public int DestDirDBId { get; set; }
        public String Filename 
        {
            get
            {
                return fFilename;
            }
            set
            {
                fFilename = value;
                fFilename = fFilename.Trim();
            }
        }
        public int DatabaseId { get; set; }

        static public String ComputeMD5(String inFilePath)
        {
            String md5 = "";
            FileStream dataStream = null;
            try
            {
                MD5 md5Algo = MD5.Create();
                dataStream = File.OpenRead(inFilePath);

                StringBuilder sb = new StringBuilder();
                foreach (Byte b in md5Algo.ComputeHash(dataStream))
                    sb.Append(b.ToString("x2").ToLower());
                md5 = sb.ToString();
            }
            catch (Exception e)
            {
                Logger.Log("Exception raised in ComputeMD5. File: " + inFilePath + " exception: " + e.Message + " " + e.StackTrace);
            }

            if (dataStream != null)
            {
                dataStream.Close();
            }
            return md5;
        }
    }
}
