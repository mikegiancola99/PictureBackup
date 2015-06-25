using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PictureHelper
{
    class App
    {
        private int fNumPixCopied;
        private int fNumDupePix;
        private List<String> fPixExt;

        public App()
        {
            fNumPixCopied = 0;
            fNumDupePix = 0;
            fPixExt = new List<string>();
            fPixExt.Add("*.jpg");
            fPixExt.Add("*.jpeg");
        }

        ~App()
        {
            Logger.Log("Number of Pictures copied over: " + Convert.ToString(fNumPixCopied));
            Logger.Log("Number of duplicate Pictures: " + Convert.ToString(fNumDupePix));
        }

        public void RebuildDatabase()
        { 
        }

        public void CheckForUpdates()
        {
            foreach (String curExt in fPixExt)
            {
                ScanDirForPix(Constants.BaseSrcDir, curExt);
            }
            
            ScanDirForDir(Constants.BaseSrcDir);
            ScanForEmptyDirs(Constants.BaseSrcDir);
        }

        private void ScanDirForDir(String inDir)
        {
            try
            {
                var dirList = Directory.EnumerateDirectories(inDir);
                foreach (String directory in dirList)
                {
                    foreach (String curExt in fPixExt)
                    {
                        ScanDirForPix(Constants.BaseSrcDir, curExt);
                    }
                    ScanDirForDir(directory);
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception Raised in ScanDirForDir - " + ex.Message + " " + ex.Source);
                Logger.Log(ex.StackTrace);
            }
        }

        private void ScanDirForPix(String inDir, String inExt)
        {
            try
            {
                var pictureFiles = Directory.EnumerateFiles(inDir, inExt);
                foreach (String currentFile in pictureFiles)
                {
                    PictureFile picFile = new PictureFile();
                    picFile.FullSrcPath = currentFile;
                    picFile.Filename = currentFile.Substring(inDir.Length + 1);
                    SavePic(picFile);
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception Raised in ScanDirForPix - " + ex.Message + " " + ex.Source);
                Logger.Log(ex.StackTrace);
            }
        }

        private void SavePic(PictureFile inFile)
        {
            try
            {
                inFile.CreateDestDir();

                DBHelper database = new DBHelper();
                inFile.DestDirDBId = database.FindDir(inFile.DestDir);
                if (inFile.DestDirDBId == -1)
                {
                    database.AddDir(inFile.DestDir);
                    inFile.DestDirDBId = database.FindDir(inFile.DestDir);
                }

                if (inFile.DestDirDBId != -1)
                {
                    PictureFile dupeFile = database.FindPicByMD5(inFile.MD5Hash);
                    if (dupeFile == null)
                    {
                        Logger.LogCopy("Copying from: " + inFile.FullSrcPath + " to " + inFile.DestDir + inFile.Filename);
                        File.Copy(inFile.FullSrcPath, inFile.DestDir + "\\" + inFile.Filename, true);
                        File.Delete(inFile.FullSrcPath);
                        database.AddPicture(inFile);
                        fNumPixCopied++;
                    }
                    else
                    {
                        Logger.LogDupe(dupeFile, inFile);
                        fNumDupePix++;
                        if (dupeFile.MD5Hash == inFile.MD5Hash)
                            File.Delete(inFile.FullSrcPath);
                    }
                }
                else
                {
                    Logger.Log("Unable to find or add destination dir " + inFile.DestDir);
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception Raised in SavePic - " + ex.Message + " " + ex.Source);
                Logger.Log(ex.StackTrace);
            }
        }

        private void ScanForEmptyDirs(String inDir)
        {
            try
            {
                foreach (var directory in Directory.GetDirectories(inDir))
                {
                    ScanForEmptyDirs(directory);
                    if (Directory.GetFiles(directory).Length == 0)
                    {
                        if (Directory.GetDirectories(directory).Length == 0)
                            Directory.Delete(directory, false);
                        else
                            Logger.Log("Directory has directories");
                    }
                    else
                    {
                        foreach (var gFile in Directory.GetFiles(directory))
                            Logger.Log("File is: " + gFile);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception Raised in ScanForEmptyDirs - " + ex.Message + " " + ex.Source);
                Logger.Log(ex.StackTrace);
            }
        }
    }
}
