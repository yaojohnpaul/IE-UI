using IE_lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace IE_lib
{
    /// <summary>
    /// Class responsible for writing results to output files.
    /// </summary>
    public class ResultWriter
    {
        /// <summary>
        /// The destination path for the final result.
        /// </summary>
        private String strPath;
        /// <summary>
        /// The destination path for the final result with processed date.
        /// </summary>
        private String formatDateDstPath;
        /// <summary>
        /// The destination path for the inverted index file.
        /// </summary>
        private String invertedIndexDstPath;
        /// <summary>
        /// The list of all articles.
        /// </summary>
        List<Article> listAllArticles;
        /// <summary>
        /// The list of all who annotations.
        /// </summary>
        List<List<String>> listAllWhoAnnotations;
        /// <summary>
        /// The list of all when annotations.
        /// </summary>
        List<List<String>> listAllWhenAnnotations;
        /// <summary>
        /// The list of all where annotations.
        /// </summary>
        List<List<String>> listAllWhereAnnotations;
        /// <summary>
        /// The list of all what annotations.
        /// </summary>
        List<String> listAllWhatAnnotations;
        /// <summary>
        /// The list of all why annotations.
        /// </summary>
        List<String> listAllWhyAnnotations;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultWriter"/> class.
        /// </summary>
        /// <param name="pPath">The destination path of the final result.</param>
        /// <param name="invertedPath">The destination path of the inverted index file.</param>
        /// <param name="formatDatePath">The destination path of the final result with processed date.</param>
        /// <param name="pAllArticles">The list of all articles.</param>
        /// <param name="pAllWhoAnnotations">The list of all who annotations.</param>
        /// <param name="pAllWhenAnnotations">The list of all when annotations.</param>
        /// <param name="pAllWhereAnnotations">The list of all where annotations.</param>
        /// <param name="pAllWhatAnnotations">The list of all what annotations.</param>
        /// <param name="pAllWhyAnnotations">The list of all why annotations.</param>
        public ResultWriter(String pPath,
            String invertedPath,
            String formatDatePath,
            List<Article> pAllArticles,
            List<List<String>> pAllWhoAnnotations,
            List<List<String>> pAllWhenAnnotations,
            List<List<String>> pAllWhereAnnotations,
            List<String> pAllWhatAnnotations,
            List<String> pAllWhyAnnotations)
        {
            strPath = pPath;
            formatDateDstPath = formatDatePath;
            invertedIndexDstPath = invertedPath;
            listAllArticles = pAllArticles;
            listAllWhoAnnotations = pAllWhoAnnotations;
            listAllWhenAnnotations = pAllWhenAnnotations;
            listAllWhereAnnotations = pAllWhereAnnotations;
            listAllWhatAnnotations = pAllWhatAnnotations;
            listAllWhyAnnotations = pAllWhyAnnotations;
        }

        /// <summary>
        /// Generates the final result output file.
        /// </summary>
        public void generateOutput()
        {
            if (listAllWhoAnnotations.Count > listAllArticles.Count() ||
                listAllWhenAnnotations.Count() != listAllWhoAnnotations.Count() ||
                listAllWhereAnnotations.Count() != listAllWhoAnnotations.Count() ||
                listAllWhatAnnotations.Count() != listAllWhoAnnotations.Count() ||
                listAllWhyAnnotations.Count() != listAllWhoAnnotations.Count())
            {
                return;
            }

            XmlTextWriter writer = new XmlTextWriter(strPath, System.Text.Encoding.UTF8);
            writer.WriteStartDocument(true);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 4;
            writer.WriteStartElement("data");
            for (int nI = 0; nI < listAllWhoAnnotations.Count(); nI++)
            {
                addArticleToXML(writer, listAllArticles[nI].Title, listAllArticles[nI].Author, listAllArticles[nI].Date, listAllArticles[nI].Body, listAllArticles[nI].Link,
                    String.Join("; ", listAllWhoAnnotations[nI].ToArray()),
                    String.Join("; ", listAllWhenAnnotations[nI].ToArray()),
                    String.Join("; ", listAllWhereAnnotations[nI].ToArray()),
                    listAllWhatAnnotations[nI], listAllWhyAnnotations[nI], nI);
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }

        /// <summary>
        /// Adds the article to the XML file via the <see cref="System.Xml.XmlTextWriter"/>.
        /// </summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="title">The article's title.</param>
        /// <param name="author">The article's author.</param>
        /// <param name="date">The article's date.</param>
        /// <param name="body">The article's body.</param>
        /// <param name="link">The article's link.</param>
        /// <param name="who">The article's who annotation.</param>
        /// <param name="when">The article's when annotation.</param>
        /// <param name="where">The article's where annotation.</param>
        /// <param name="what">The article's what annotation.</param>
        /// <param name="why">The article's why annotation.</param>
        /// <param name="index">The article's index.</param>
        public void addArticleToXML(XmlTextWriter writer, String title, String author, DateTime date, String body, String link,
            String who, String when, String where, String what, String why, int index)
        {
            writer.WriteStartElement("article");
            writer.WriteStartElement("title");
            writer.WriteString(title);
            writer.WriteEndElement();
            writer.WriteStartElement("author");
            writer.WriteString(author);
            writer.WriteEndElement();
            writer.WriteStartElement("date");
            writer.WriteStartElement("month");
            writer.WriteString(date.Month.ToString());
            writer.WriteEndElement();
            writer.WriteStartElement("day");
            writer.WriteString(date.Day.ToString());
            writer.WriteEndElement();
            writer.WriteStartElement("year");
            writer.WriteString(date.Year.ToString());
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteStartElement("body");
            writer.WriteString(body);
            writer.WriteEndElement();
            writer.WriteStartElement("link");
            writer.WriteString(link);
            writer.WriteEndElement();
            writer.WriteStartElement("who");
            writer.WriteString(who);
            writer.WriteEndElement();
            writer.WriteStartElement("when");
            writer.WriteString(when);
            writer.WriteEndElement();
            writer.WriteStartElement("where");
            writer.WriteString(where);
            writer.WriteEndElement();
            writer.WriteStartElement("what");
            writer.WriteString(what);
            writer.WriteEndElement();
            writer.WriteStartElement("why");
            writer.WriteString(why);
            writer.WriteEndElement();
            writer.WriteStartElement("index");
            writer.WriteString(index.ToString());
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        /// <summary>
        /// Generates the inverted index internal data.
        /// </summary>
        /// <param name="feature">Who, When, Where, What, or Why</param>
        /// <returns>The dictionary for the inverted index of a feature.</returns>
        public Dictionary<string, List<string>> generateInvertedIndex (string feature)
        {
            List<Article> articleList = new List<Article>();

            XmlDocument doc = new XmlDocument();
            doc.Load(formatDateDstPath);

            XmlNodeList articleNodes = doc.DocumentElement.SelectNodes("/data/article");

            string[] anot = null;
            
            List<string> articles = new List<string>();

            Dictionary<string, List<string>> hash = new Dictionary<string, List<string>>();
            
            foreach (XmlNode articleNode in articleNodes)
            {
                anot = articleNode.SelectSingleNode(feature).InnerText.Split(new string[] {";"}, StringSplitOptions.RemoveEmptyEntries);
                
                for (int w = 0; w < anot.Length; w++)
                {
                    anot[w] = anot[w].Trim();
                    if (!hash.ContainsKey(anot[w]))
                    { 
                        List<string> temp = new List<string>();
                        temp.Add(articleNode.SelectSingleNode("index").InnerText);
                        hash.Add(anot[w], temp);
                    }
                    else
                    {
                        hash[anot[w]].Add(articleNode.SelectSingleNode("index").InnerText);
                    }

                }
            }

            return hash;
        }

        /// <summary>
        /// Generates the inverted index output file.
        /// </summary>
        public void generateInvertedIndexOutput()
        {
            XmlTextWriter writer = new XmlTextWriter(invertedIndexDstPath, System.Text.Encoding.UTF8);
            writer.WriteStartDocument(true);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 4;
            writer.WriteStartElement("data");

            writer.WriteStartElement("who");
            addFeatureToXML(writer, "who");
            writer.WriteEndElement();
            writer.WriteStartElement("when");
            addFeatureToXML(writer, "when");
            writer.WriteEndElement();
            writer.WriteStartElement("where");
            addFeatureToXML(writer, "where");
            writer.WriteEndElement();
            writer.WriteStartElement("what");
            addFeatureToXML(writer, "what");
            writer.WriteEndElement();
            writer.WriteStartElement("why");
            addFeatureToXML(writer, "why");
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }

        /// <summary>
        /// Adds the feature to the inverted index XML.
        /// </summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="feature">Who, When, Where, What, or Why</param>
        public void addFeatureToXML(XmlTextWriter writer, string feature)
        {
            Dictionary<string, List<string>> hash = new Dictionary<string, List<string>>();
            hash = generateInvertedIndex(feature);
            foreach (KeyValuePair<string, List<string>> kvp in hash)
            {
                writer.WriteStartElement("entry");
                writer.WriteStartElement("text");
                writer.WriteString(kvp.Key);
                writer.WriteEndElement();
                foreach (string a in kvp.Value)
                {
                    writer.WriteStartElement("articleIndex");
                    writer.WriteString(a);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
        }

        /// <summary>
        /// Processes the when from relative date to absolute date.
        /// </summary>
        /// <param name="date">The date of the article.</param>
        /// <param name="annotation">The annotation of the article.</param>
        /// <param name="writer">The XML writer.</param>
        public void processWhen(DateTime date, string annotation, XmlTextWriter writer)
        {
            //Console.WriteLine("Annotation: " + annotation + "DATE: " + date);
            if (annotation.Contains("kahapon"))
            {
                date = date.AddDays(-1);
                writer.WriteString(date.Date.ToString());
            }
            else if (annotation.Contains("bukas"))
            {
                date = date.AddDays(1);
                writer.WriteString(date.Date.ToString());
            }
            else if (annotation.Contains("ngayon"))
            {
                writer.WriteString(date.Date.ToString());
            }
            else if (annotation.Contains("kamakalawa"))
            {
                date = date.AddDays(-2);
                writer.WriteString(date.Date.ToString());
            }
            else if (annotation.Contains("Linggo"))
            {
                while ((int)date.DayOfWeek != 0)
                {
                   date = date.AddDays(1);
                }
                date = date.AddDays(-7);
                writer.WriteString(date.Date.ToString());
            }
            else if (annotation.Contains("Lunes"))
            {
                while ((int)date.DayOfWeek != 1)
                {
                    date = date.AddDays(1);
                }
                date = date.AddDays(-7);
                writer.WriteString(date.Date.ToString());
            }
            else if (annotation.Contains("Martes"))
            {
                while ((int)date.DayOfWeek != 2)
                {
                    date = date.AddDays(1);
                }
                date = date.AddDays(-7);
                writer.WriteString(date.Date.ToString());
            }
            else if (annotation.Contains("Miyerkules"))
            {
                while ((int)date.DayOfWeek != 3)
                {
                    date = date.AddDays(1);
                }
                date = date.AddDays(-7);
                writer.WriteString(date.Date.ToString());
            }
            else if (annotation.Contains("Huwebes"))
            {
                while ((int)date.DayOfWeek != 4)
                {
                    date = date.AddDays(1);
                }
                date = date.AddDays(-7);
                writer.WriteString(date.Date.ToString());
            }
            else if (annotation.Contains("Biyernes"))
            {
                while ((int)date.DayOfWeek != 5)
                {
                    date = date.AddDays(1);
                }
                date = date.AddDays(-7);
                writer.WriteString(date.Date.ToString());
            }
            else if (annotation.Contains("Sabado"))
            {
                while ((int)date.DayOfWeek != 6)
                {
                    date = date.AddDays(1);
                }
                date = date.AddDays(-7);
                writer.WriteString(date.Date.ToString());
            }
            else if (annotation.Contains("nakaraan"))
            {
                if (annotation.Contains("linggo"))
                {
                    date = date.AddDays(-1);
                    writer.WriteString(date.Date.ToString());
                }
                else
                {
                    writer.WriteString(annotation);
                }

            }
            else
            {
                writer.WriteString(annotation);
            }
            //Console.WriteLine("NEW DATE: " + date);
        }

        /// <summary>
        /// Generates the final result output file with processed dates.
        /// </summary>
        public void generateOutputFormatDate()
        {

            if (listAllWhoAnnotations.Count > listAllArticles.Count() ||
                listAllWhenAnnotations.Count() != listAllWhoAnnotations.Count() ||
                listAllWhereAnnotations.Count() != listAllWhoAnnotations.Count() ||
                listAllWhatAnnotations.Count() != listAllWhoAnnotations.Count() ||
                listAllWhyAnnotations.Count() != listAllWhoAnnotations.Count())
            {
                return;
            }

            XmlTextWriter writer = new XmlTextWriter(formatDateDstPath, System.Text.Encoding.UTF8);
            writer.WriteStartDocument(true);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 4;
            writer.WriteStartElement("data");
            for (int nI = 0; nI < listAllWhoAnnotations.Count(); nI++)
            {
                addArticleToXMLFormatWhen(writer, listAllArticles[nI].Title, listAllArticles[nI].Author, listAllArticles[nI].Date, listAllArticles[nI].Body, listAllArticles[nI].Link,
                    String.Join("; ", listAllWhoAnnotations[nI].ToArray()),
                    String.Join("; ", listAllWhenAnnotations[nI].ToArray()),
                    String.Join("; ", listAllWhereAnnotations[nI].ToArray()),
                    listAllWhatAnnotations[nI], listAllWhyAnnotations[nI], nI);
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }

        /// <summary>
        /// Adds the article to the XML file with a formatted when via the <see cref="System.Xml.XmlTextWriter"/>.
        /// </summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="title">The article's title.</param>
        /// <param name="author">The article's author.</param>
        /// <param name="date">The article's date.</param>
        /// <param name="body">The article's body.</param>
        /// <param name="link">The article's link.</param>
        /// <param name="who">The article's who annotation.</param>
        /// <param name="when">The article's when annotation.</param>
        /// <param name="where">The article's where annotation.</param>
        /// <param name="what">The article's what annotation.</param>
        /// <param name="why">The article's why annotation.</param>
        /// <param name="index">The article's index.</param>
        public void addArticleToXMLFormatWhen(XmlTextWriter writer, String title, String author, DateTime date, String body, String link,
            String who, String when, String where, String what, String why, int index)
        {
            writer.WriteStartElement("article");
            writer.WriteStartElement("title");
            writer.WriteString(title);
            writer.WriteEndElement();
            writer.WriteStartElement("author");
            writer.WriteString(author);
            writer.WriteEndElement();
            writer.WriteStartElement("date");
            writer.WriteStartElement("month");
            writer.WriteString(date.Month.ToString());
            writer.WriteEndElement();
            writer.WriteStartElement("day");
            writer.WriteString(date.Day.ToString());
            writer.WriteEndElement();
            writer.WriteStartElement("year");
            writer.WriteString(date.Year.ToString());
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteStartElement("body");
            writer.WriteString(body);
            writer.WriteEndElement();
            writer.WriteStartElement("link");
            writer.WriteString(link);
            writer.WriteEndElement();
            writer.WriteStartElement("who");
            writer.WriteString(who);
            writer.WriteEndElement();
            writer.WriteStartElement("when");
            processWhen(date, when, writer);
            writer.WriteEndElement();
            writer.WriteStartElement("where");
            writer.WriteString(where);
            writer.WriteEndElement();
            writer.WriteStartElement("what");
            writer.WriteString(what);
            writer.WriteEndElement();
            writer.WriteStartElement("why");
            writer.WriteString(why);
            writer.WriteEndElement();
            writer.WriteStartElement("index");
            writer.WriteString(index.ToString());
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
    }
}
