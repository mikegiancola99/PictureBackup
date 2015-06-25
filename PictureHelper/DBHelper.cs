using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace PictureHelper
{
    class DBHelper
    {
        private SqlConnection fDBConn;

        public DBHelper()
        {
            fDBConn = null;
        }
        
        ~DBHelper()
        {
        }

        public bool AddPicture(PictureFile inFile)
        {
            bool added = false;
            if (String.IsNullOrEmpty(inFile.MD5Hash) ||
                String.IsNullOrEmpty(inFile.Filename) ||
                inFile.DestDirDBId == -1)
            {
                Logger.Log("Bad precondition check in AddPicture.");
                return added;
            }

            String sqlCmd = "hi";
            try
            {
                ConnectToDB();
                if (fDBConn != null)
                {
                    sqlCmd = "INSERT INTO [content] ([md5], [filename], [directory]) values (";
                    sqlCmd += "'" + inFile.MD5Hash + "'";
                    sqlCmd += ",'" + inFile.Filename + "', ";
                    sqlCmd += Convert.ToString(inFile.DestDirDBId);
                    sqlCmd += ")";

                    SqlCommand myCommand = new SqlCommand(sqlCmd, fDBConn);
                    myCommand.ExecuteNonQuery();
                    added = true;
                }
                else
                    Logger.Log("AddPicture - no database connection");
            }
            catch (Exception ex)
            {
                Logger.Log("Exception raised in AddPicture. " + ex.Message + " " + ex.Source);
                Logger.Log("SQL: " + sqlCmd);
                Logger.Log(ex.StackTrace);
            }
            CloseConnection();
            return added;
        }
        public PictureFile FindPicByMD5(String inMD5)
        {
            PictureFile retFile = null;
            try
            {
                ConnectToDB();
                if (fDBConn != null)
                {
                    String sqlQuery = "SELECT * FROM [content] WHERE ";
                    sqlQuery += "([md5] ='" + inMD5 + "')";

                    SqlCommand myCommand = new SqlCommand(sqlQuery, fDBConn);
                    SqlDataReader aReader = myCommand.ExecuteReader();
                    if (aReader.HasRows)
                    {
                        aReader.Read();
                        retFile = new PictureFile();
                        retFile.Filename = (string)aReader["Filename"];
                        retFile.DatabaseId = (int)aReader["id"];
                        retFile.DestDirDBId = (int)aReader["directory"];
                    }
                    aReader.Close();
                    if (retFile != null)
                    {
                        retFile.DestDir = FindDir(retFile.DestDirDBId);
                        retFile.FullSrcPath = retFile.DestDir + retFile.Filename;
                    }
                }
                else
                    Logger.Log("FindPicByMD5 - no database connection");
            }
            catch (Exception ex)
            {
                Logger.Log("Exception raised in FindPicByMD5. " + ex.Message + " " + ex.Source);
                Logger.Log(ex.StackTrace);
            }
            CloseConnection();
            return retFile;
        }
        public List<PictureFile> GetAllPix()
        {
            List<PictureFile> retList = new List<PictureFile>();
            try
            {
//                Dictionary<int, String> dirList = GetAllDirs();

                ConnectToDB();
                if (fDBConn != null)
                {
                    String sqlQuery = "SELECT * FROM [content] ";

                    SqlCommand myCommand = new SqlCommand(sqlQuery, fDBConn);
                    SqlDataReader aReader = myCommand.ExecuteReader();
                    while (aReader.Read())
                    {
                        PictureFile tmpFile = new PictureFile();
                        tmpFile = new PictureFile();
                        tmpFile.Filename = (string)aReader["Filename"];
                        tmpFile.DatabaseId = (int)aReader["id"];
                        tmpFile.DestDirDBId = (int)aReader["directory"];
                        //string tmpDir;
                        //dirList.TryGetValue(tmpFile.DestDirDBId, out tmpDir);
                        //tmpFile.DestDir = tmpDir;
                        tmpFile.MD5Hash = (string)aReader["md5"];
                        tmpFile.FullSrcPath = tmpFile.DestDir + tmpFile.Filename;
                        retList.Add(tmpFile);
                    }
                    aReader.Close();
                }
                else
                    Logger.Log("GetAllPix - no database connection");
            }
            catch (Exception ex)
            {
                Logger.Log("Exception raised in GetAllPix. " + ex.Message + " " + ex.Source);
                Logger.Log(ex.StackTrace);
            }
            CloseConnection();
            return retList;
        }

        public bool AddDir(String inDir)
        {
            bool added = false;
            try
            {
                ConnectToDB();
                if (fDBConn != null)
                {
                    String sqlCmd = "INSERT INTO [directories] ([path]) values ('" + inDir + "')";
                    SqlCommand myCommand = new SqlCommand(sqlCmd, fDBConn);
                    myCommand.ExecuteNonQuery();
                    added = true;
                }
                else
                    Logger.Log("AddDir - no database connection");
            }
            catch (Exception ex)
            {
                Logger.Log("Exception raised in AddDir. " + ex.Message + " " + ex.Source);
                Logger.Log(ex.StackTrace);
            }
            CloseConnection();
            return added;

        }
        public int FindDir(String inPath)
        {
            int rval = -1;
            try
            {
                ConnectToDB();
                if (fDBConn != null)
                {
                    String sqlQuery = "SELECT * FROM [directories] WHERE ";
                    sqlQuery += "([path] ='" + inPath + "')";
                    SqlCommand myCommand = new SqlCommand(sqlQuery, fDBConn);
                    SqlDataReader aReader = myCommand.ExecuteReader();
                    if (aReader.HasRows)
                    {
                        aReader.Read();
                        rval = (int)aReader["id"];
                    }
                    aReader.Close();
                }
                else
                    Logger.Log("FindDir - no database connection");
            }
            catch (Exception ex )
            {
                Logger.Log("Exception raised in FindDir. " + ex.Message + " " + ex.Source);
                Logger.Log(ex.StackTrace);
            }
            CloseConnection();
            return rval;
        }
        public String FindDir(int inId)
        {
            String rval = null;
            try
            {
                ConnectToDB();
                if (fDBConn != null)
                {
                    String sqlQuery = "SELECT * FROM [directories] WHERE ";
                    sqlQuery += "([id] =" + Convert.ToString(inId) + ")";
                    SqlCommand myCommand = new SqlCommand(sqlQuery, fDBConn);
                    SqlDataReader aReader = myCommand.ExecuteReader();
                    if (aReader.HasRows)
                    {
                        aReader.Read();
                        rval = (String)aReader["path"];
                    }
                    aReader.Close();
                }
                else
                    Logger.Log("FindDir - no database connection");
            }
            catch (Exception ex)
            {
                Logger.Log("Exception raised in FindDir. " + ex.Message + " " + ex.Source);
                Logger.Log(ex.StackTrace);
            }
            CloseConnection();
            return rval.Trim();
        }
        public Dictionary<int, String> GetAllDirs()
        {
            Dictionary<int, String> retList = new Dictionary<int, string>();
            try
            {
                ConnectToDB();
                if (fDBConn != null)
                {
                    String sqlQuery = "SELECT * FROM [directories]";
                    SqlCommand myCommand = new SqlCommand(sqlQuery, fDBConn);
                    SqlDataReader aReader = myCommand.ExecuteReader();
                    while (aReader.Read())
                    {
                        int dirId = (int)aReader["id"];
                        String path = (String)aReader["path"];
                        retList.Add(dirId, path);
                    }
                    aReader.Close();
                }
                else
                    Logger.Log("GetAllDirs - no database connection");
            }
            catch (Exception ex)
            {
                Logger.Log("Exception raised in GetAllDirs. " + ex.Message + " " + ex.Source);
                Logger.Log(ex.StackTrace);
            }
            CloseConnection();

            return retList;
        }
        
        private void ConnectToDB()
        {
            try
            {
                if (fDBConn == null)
                {

                    fDBConn = new SqlConnection("user id=mikeg2;" +
                                                "password=85crxsi;" +
                                                "server=PIX-PC\\SQLEXPRESS;" +
                                                //"server=192.168.1.21\\SQLEXPRESS;" +
                                                //"server=192.168.1.21;" +
                                                "database=pictures; " +
                                                "connection timeout=30");
                    fDBConn.Open();
                }
            }
            catch (Exception ex)
            {
                fDBConn = null;
                Logger.Log("Exception raised in ConnectToDB. " + ex.Message + " " + ex.Source);
                Logger.Log(ex.StackTrace);
                
            }
        }
        private void CloseConnection()
        {
            try
            {
                if (fDBConn != null)
                {
                    fDBConn.Close();
                    fDBConn = null;
                }
            }
            catch(Exception ex)
            {
                Logger.Log("Exception raised in CloseConnection. " + ex.Message + " " + ex.Source);
                Logger.Log(ex.StackTrace);
            }
        }
    }
}
