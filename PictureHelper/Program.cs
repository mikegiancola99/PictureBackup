using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PictureHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.OpenLogs();
            try
            {
                if (args.Length >= 1)
                {
                    if (args[0] == "-check")
                    {
                        Validator checkMe = new Validator();
                        checkMe.ValidateAll();
                    }
                    else if (args[0] == "-rebuild")
                    {
                        DBHelper database = new DBHelper();
                        database.FindDir("c:\\mikeg\\");

                        Validator checkMe = new Validator();
                        Constants.BaseDestDir = args[1];
                        checkMe.RebuildDatabase();
                    }
                    else if (args[0] == "-dest")
                    {
                        if (args.Length > 2)
                        {
                            Constants.BaseDestDir = args[2];
                            Constants.BaseSrcDir = Directory.GetCurrentDirectory();
                            Logger.Log("Starting app");
                            Logger.Log("Using Src directory of: " + Constants.BaseSrcDir);
                            Logger.Log("Using Dest directory of: " + Constants.BaseDestDir);

                            App daApp = new App();
                            daApp.CheckForUpdates();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception Raised in Main - " + ex.Message + " " + ex.Source);
                Logger.Log(ex.StackTrace);
            }
            Logger.CloseLogs();
        }
    }
}
