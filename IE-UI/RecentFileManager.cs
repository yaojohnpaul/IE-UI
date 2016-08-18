using IE_UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IE_UI
{
    /// <summary>
    /// Class for managing the recently accessed files.
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
        /// <param name="file">The file.</param>
        public static void AddRecentFile(RecentFile file)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(GetText(file));
                    sw.Close();
                }
                else
                {
                    FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(GetText(file));
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
        /// <param name="file">The file.</param>
        /// <returns>The text</returns>
        private static string GetText(RecentFile file)
        {
            if (file.OperationType == "/assets/images/view.png")
            {
                return String.Format("{0}|{1}|{2}", "VIEW", file.Name, file.SourceFilePath);
            }
            else if (file.OperationType == "/assets/images/extract.png")
            {
                return String.Format("{0}|{1}|{2}|{3}", "EXTRACT", file.Name, file.SourceFilePath, file.DestinationFilePath);
            }

            return "";
        }

        /// <summary>
        /// Gets the recent files list.
        /// </summary>
        /// <returns>The recent files list.</returns>
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
                            OperationType = "/assets/images/extract.png",
                            Name = splitLine[1],
                            SourceFilePath = splitLine[2],
                            DestinationFilePath = splitLine[3]
                        };

                        if(File.Exists(Path.Combine(newRecentFile.SourceFilePath, newRecentFile.Name + ".xml")))
                        {
                            recentFilesList.Add(newRecentFile);
                        }
                    }
                    else if (splitLine[0] == "VIEW" && splitLine.Count() == 3)
                    {
                        RecentFile newRecentFile = new RecentFile()
                        {
                            OperationType = "/assets/images/view.png",
                            Name = splitLine[1],
                            SourceFilePath = splitLine[2]
                        };
                        
                        if (File.Exists(Path.Combine(newRecentFile.SourceFilePath, newRecentFile.Name + ".xml")))
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
