using IE_UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IE_UI
{
    /// <summary>
    /// 
    /// </summary>
    public class RecentFileManager
    {
        /// <summary>
        /// The file name
        /// </summary>
        private static string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "recentfiles.txt");

        /// <summary>
        /// Adds the recent file.
        /// </summary>
        /// <param name="t">The t.</param>
        public static void AddRecentFile(RecentFile t)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(GetText(t));
                    sw.Close();
                }
                else
                {
                    FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(GetText(t));
                    sw.Close();
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        private static string GetText(RecentFile t)
        {
            if (t.OperationType == Char.ConvertFromUtf32(0xE8E5))
            {
                return String.Format("{0}|{1}|{2}", "VIEW", t.Name, t.SourcePath);
            }
            else if (t.OperationType == Char.ConvertFromUtf32(0xE7E6))
            {
                return String.Format("{0}|{1}|{2}|{3}", "EXTRACT", t.Name, t.SourcePath, t.DestinationPath);
            }

            return "";
        }

        /// <summary>
        /// Gets the recent files list.
        /// </summary>
        /// <returns></returns>
        public static List<RecentFile> GetRecentFilesList()
        {
            List<RecentFile> recentFilesList = new List<RecentFile>();

            if (File.Exists(fileName))
            {
                StreamReader sr = new StreamReader(fileName);
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    string[] splitLine = line.Split('|');

                    if (splitLine[0] == "EXTRACT" && splitLine.Count() == 4)
                    {
                        RecentFile newRecentFile = new RecentFile()
                        {
                            OperationType = Char.ConvertFromUtf32(0xE7E6),
                            Name = splitLine[1],
                            SourcePath = splitLine[2],
                            DestinationPath = splitLine[3]
                        };

                        if(File.Exists(Path.Combine(newRecentFile.SourcePath, newRecentFile.Name + ".xml")))
                        {
                            recentFilesList.Add(newRecentFile);
                        }
                    }
                    else if (splitLine[0] == "VIEW" && splitLine.Count() == 3)
                    {
                        RecentFile newRecentFile = new RecentFile()
                        {
                            OperationType = Char.ConvertFromUtf32(0xE8E5),
                            Name = splitLine[1],
                            SourcePath = splitLine[2]
                        };
                        
                        if (File.Exists(Path.Combine(newRecentFile.SourcePath, newRecentFile.Name + ".xml")))
                        {
                            recentFilesList.Add(newRecentFile);
                        }
                    }
                }
                sr.Close();
            }
            
            return recentFilesList;
        }

    }
}
