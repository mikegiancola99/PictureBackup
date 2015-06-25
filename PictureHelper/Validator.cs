using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureHelper
{
    public class Validator
    {
        public void ValidateAll()
        {
            DBHelper dbGuy = new DBHelper();
            List<PictureFile> pixList = dbGuy.GetAllPix();
            int okayCount = 0;
            int failCount = 0;
            foreach (PictureFile curFile in pixList)
            {
                curFile.FullSrcPath = dbGuy.FindDir(curFile.DestDirDBId) + "\\" + curFile.Filename;
                string computedHash = DataFile.ComputeMD5(curFile.FullSrcPath);
                if (computedHash.CompareTo(curFile.MD5Hash) != 0)
                {
                    Logger.Log("!!!ERROR!!! Hash doesn't match.." + curFile.FullSrcPath + " computed: " + computedHash + " database: " + curFile.MD5Hash);
                    failCount++;
                    PictureFile otherFile = dbGuy.FindPicByMD5(computedHash);
                    if (otherFile != null)
                    {
                        Logger.Log("Found Picture at: " + otherFile.FullSrcPath);
                    }
                }
                else
                {
                    okayCount++;
                }
            }
            Logger.Log("Number okay: " + Convert.ToString(okayCount));
            Logger.Log("Number Failed: " + Convert.ToString(failCount));
        }

        public void RebuildDatabase()
        {
            Logger.Log("Inside RebuildDatabase");

            DBHelper database = new DBHelper();

            PictureFinder finder = new PictureFinder();
            List<PictureFile> allFiles = finder.FindAllPix(Constants.BaseDestDir);
            Logger.Log("Num dirs: " + Convert.ToString(allFiles.Count));

            foreach (PictureFile curFile in allFiles)
            {
                int dirId = database.FindDir(curFile.DestDir);
                if (dirId == -1)
                {
                    database.AddDir(curFile.DestDir);
                    dirId = database.FindDir(curFile.DestDir);
                }

                if (dirId != -1)
                {
                    curFile.DestDirDBId = dirId;
                    database.AddPicture(curFile);
                }
            }
        }
    }
}
