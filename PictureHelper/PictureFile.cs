using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Media.Imaging;

namespace PictureHelper
{
    class PictureFile : DataFile
    {
        public PictureFile()
        {
        }

        public void CreateDestDir()
        {
            try
            {
                DateTime createTime = File.GetCreationTime(FullSrcPath);
                String year = Convert.ToString(createTime.Year);
                String day = Convert.ToString(createTime.Day);
                String fullPath = Constants.BaseDestDir + "\\" + year + "\\" + GetMonthText(createTime.Month) + "\\" + day + "\\";

                string dateTimeTaken = GetDateTaken(FullSrcPath);
                if (!String.IsNullOrEmpty(dateTimeTaken))
                {
                    DateTime dateTaken = Convert.ToDateTime(dateTimeTaken);
                    if (dateTaken.Year > 1990)
                    {
                        fullPath = Constants.BaseDestDir + "\\" + Convert.ToString(dateTaken.Year) + "\\" + GetMonthText(dateTaken.Month) + "\\" + Convert.ToString(dateTaken.Day) + "\\";
                    }
                    else
                        Logger.Log("Image: " + FullSrcPath + " has year < 1990. Date is: " + Convert.ToString(dateTaken.Year));

                    Directory.CreateDirectory(fullPath);
                    DestDir = fullPath;
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception Raised in CreateDestDir - " + ex.Message + " " + ex.Source);
                Logger.Log(ex.StackTrace);
            }
        }

        private string GetDateTaken(String inFile)
        {
            string date = "";

            try
            {
                FileStream fs = new FileStream(inFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                BitmapSource img = BitmapFrame.Create(fs);
                BitmapMetadata md = (BitmapMetadata)img.Metadata;
                date = md.DateTaken;
                fs.Close();
            }
            catch (Exception ex)
            {
                Logger.Log("Exception Raised in GetDateTaken - File: " + inFile + " " + ex.Message + " " + ex.Source);
                Logger.Log(ex.StackTrace);
            }
            return date;
        }

        private String GetMonthText(int inMonth)
        {
            String rval = "unknown";
            if (inMonth == 1)
                rval = "January";
            else if (inMonth == 2)
                rval = "February";
            else if (inMonth == 3)
                rval = "March";
            else if (inMonth == 4)
                rval = "April";
            else if (inMonth == 5)
                rval = "May";
            else if (inMonth == 6)
                rval = "June";
            else if (inMonth == 7)
                rval = "July";
            else if (inMonth == 8)
                rval = "August";
            else if (inMonth == 9)
                rval = "September";
            else if (inMonth == 10)
                rval = "October";
            else if (inMonth == 11)
                rval = "November";
            else if (inMonth == 12)
                rval = "December";
            return rval;
        }
    }
}
