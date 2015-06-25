using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PictureHelper
{
    class PictureFinder
    {
        private List<String> fPixExt;

        public PictureFinder()
        {
            fPixExt = new List<string>();
            fPixExt.Add("*.jpg");
            fPixExt.Add("*.jpeg");
        }

        public List<PictureFile> FindAllPix(String srcDir)
        {
            List<String> dirList = FindAllDirs(srcDir);
            dirList.Add(srcDir);

            List<PictureFile> retList = new List<PictureFile>();
            foreach (String curDir in dirList)
            {
                foreach (string exten in fPixExt)
                {
                    var picFiles = Directory.EnumerateFiles(curDir, exten);
                    foreach (String pic in picFiles)
                    {
                        PictureFile picFile = new PictureFile();
                        picFile.FullSrcPath = pic;
                        picFile.Filename = pic.Substring(curDir.Length + 1);
                        picFile.DestDir = pic.Substring(0, curDir.Length);
                        picFile.MD5Hash = DataFile.ComputeMD5(picFile.FullSrcPath);
                        retList.Add(picFile);
                    }
                }
            }

            return retList;
        }
        public List<String> FindAllDirs(String baseDir)
        {
            List<String> retList = new List<string>();
            try
            {
                List<String> tmpList = Directory.EnumerateDirectories(baseDir).ToList();
                foreach (String subDir in tmpList)
                {
                    retList.Add(subDir);
                    retList.AddRange(FindAllDirs(subDir));
                }
            }
            catch (Exception)
            { }

            return retList;
        }
    }
}
