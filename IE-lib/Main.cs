using IE_lib.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace IE_lib
{
    /// <summary>
    /// The main class.
    /// </summary>
    public class Main
    {
        /// <summary>
        /// Extracts the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <returns>Boolean that denotes if the extraction succeeded.</returns>
        public static bool Extract(string source, string destination, BackgroundWorker worker, ref string status)
        {
            FileParser fileParser = new FileParser();
            Preprocessor preprocessor = new Preprocessor();

            List<Article> listCurrentArticles;
            List<List<Token>> listTokenizedArticles = new List<List<Token>>();
            List<List<Candidate>> listAllWhoCandidates = new List<List<Candidate>>();
            List<List<Candidate>> listAllWhenCandidates = new List<List<Candidate>>();
            List<List<Candidate>> listAllWhereCandidates = new List<List<Candidate>>();
            List<List<List<Token>>> listAllWhatCandidates = new List<List<List<Token>>>();
            List<List<List<Token>>> listAllWhyCandidates = new List<List<List<Token>>>();
            List<List<String>> listAllWhoAnnotations = new List<List<String>>();
            List<List<String>> listAllWhenAnnotations = new List<List<String>>();
            List<List<String>> listAllWhereAnnotations = new List<List<String>>();
            List<String> listAllWhatAnnotations = new List<String>();
            List<String> listAllWhyAnnotations = new List<String>();

            int totalProgress = 0;
            int currentProgress = 0;
            
            #region Parse Source File
            status = "parsing source file";

            listCurrentArticles = fileParser.parseFile(source);

            totalProgress = listCurrentArticles.Count * 2 + 2;

            worker.ReportProgress(Convert.ToInt16((float)++currentProgress / totalProgress * 100));

            if (listCurrentArticles == null)
            {
                return false;
            }
            #endregion

            #region Preprocess Article 
            status = "preprocessing articles";

            if (listCurrentArticles.Count > 0)
            {
                for (int nI = 0; nI < listCurrentArticles.Count; nI++)
                {
                    preprocessor.setCurrentArticle(listCurrentArticles[nI]);
                    preprocessor.preprocess();

                    listTokenizedArticles.Add(preprocessor.getLatestTokenizedArticle());
                    listAllWhoCandidates.Add(preprocessor.getWhoCandidates());
                    listAllWhenCandidates.Add(preprocessor.getWhenCandidates());
                    listAllWhereCandidates.Add(preprocessor.getWhereCandidates());
                    listAllWhatCandidates.Add(preprocessor.getWhatCandidates());
                    listAllWhyCandidates.Add(preprocessor.getWhyCandidates());

                    worker.ReportProgress(Convert.ToInt16((float)++currentProgress / totalProgress * 100));
                }
            }
            else
            {
                return false;
            }
            #endregion

            #region Identify 5W's
            status = "identifying features";

            Identifier annotationIdentifier = new Identifier();
            for (int nI = 0; nI < listCurrentArticles.Count; nI++)
            {
                annotationIdentifier.setCurrentArticle(listTokenizedArticles[nI]);
                annotationIdentifier.setWhoCandidates(listAllWhoCandidates[nI]);
                annotationIdentifier.setWhenCandidates(listAllWhenCandidates[nI]);
                annotationIdentifier.setWhereCandidates(listAllWhereCandidates[nI]);
                annotationIdentifier.setWhatCandidates(listAllWhatCandidates[nI]);
                annotationIdentifier.setWhyCandidates(listAllWhyCandidates[nI]);
                annotationIdentifier.setTitle(listCurrentArticles[nI].Title);
                annotationIdentifier.labelAnnotations();
                listAllWhoAnnotations.Add(annotationIdentifier.getWho());
                listAllWhenAnnotations.Add(annotationIdentifier.getWhen());
                listAllWhereAnnotations.Add(annotationIdentifier.getWhere());
                listAllWhatAnnotations.Add(annotationIdentifier.getWhat());
                listAllWhyAnnotations.Add(annotationIdentifier.getWhy());

                worker.ReportProgress(Convert.ToInt16((float)++currentProgress / totalProgress * 100));
            }

            #endregion

            #region Generate Output
            status = "generating output";

            String destinationPath = destination;
            String invertedDestinationPath = destination.Insert(destination.Length - 4, "_inverted_index");
            String formatDateDestinationPath = destination.Insert(destination.Length - 4, "_format_date");

            ResultWriter rw = new ResultWriter(destinationPath, invertedDestinationPath, formatDateDestinationPath, listCurrentArticles, listAllWhoAnnotations, listAllWhenAnnotations, listAllWhereAnnotations, listAllWhatAnnotations, listAllWhyAnnotations);
            rw.generateOutput();
            rw.generateOutputFormatDate();
            rw.generateInvertedIndexOutput();

            worker.ReportProgress(Convert.ToInt16((float)++currentProgress / totalProgress * 100));

            #endregion 

            //status = "process completed";

            return true;
        }

        /// <summary>
        /// Views the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>Parsed results to be used for viewing</returns>
        public static ParsedResults View(string path)
        {
            if (path.Contains("_inverted_index"))
            {
                path = path.Replace("_inverted_index", "");
            }

            string invertedIndexDestinationPath = path.Insert(path.Length - 4, "_inverted_index");
            string formattedDateDestinationPath = path.Insert(path.Length - 4, "_format_date");

            FileParser fileParser = new FileParser();

            List<Article> listArticles = fileParser.parseFile(path);
            List<Annotation> listAnnotations = fileParser.parseAnnotations(path);
            List<Annotation> listFormattedDateAnnotations = fileParser.parseAnnotations(formattedDateDestinationPath);

            ParsedResults results = new ParsedResults();

            results.FilePath = path;
            results.ListDisplayArticles = new List<DisplayArticle>();
            results.WhoReverseIndex = new Dictionary<string, List<int>>();
            results.WhenReverseIndex = new Dictionary<string, List<int>>();
            results.WhereReverseIndex = new Dictionary<string, List<int>>();
            results.WhatReverseIndex = new Dictionary<string, List<int>>();
            results.WhyReverseIndex = new Dictionary<string, List<int>>();

            if (listArticles.Count <= 0 || listAnnotations.Count <= 0)
            {
                return null;
            }

            if (File.Exists(formattedDateDestinationPath) && listAnnotations.Count == listFormattedDateAnnotations.Count)
            {
                foreach (int i in Enumerable.Range(0, listAnnotations.Count()))
                {
                    listAnnotations[i].Index = i;
                    listAnnotations[i].FormattedWhen = listFormattedDateAnnotations[i].When;
                    results.ListDisplayArticles.Add(new DisplayArticle()
                    {
                        Article = listArticles[i],
                        Annotation = listAnnotations[i]
                    });
                }
            }
            else
            {
                return null;
            }

            if (File.Exists(invertedIndexDestinationPath))
            {
                XmlDocument doc = new XmlDocument();

                doc.Load(invertedIndexDestinationPath);

                XmlNodeList whoNodes = doc.DocumentElement.SelectNodes("/data/who/entry");
                XmlNodeList whenNodes = doc.DocumentElement.SelectNodes("/data/when/entry");
                XmlNodeList whereNodes = doc.DocumentElement.SelectNodes("/data/where/entry");
                XmlNodeList whatNodes = doc.DocumentElement.SelectNodes("/data/what/entry");
                XmlNodeList whyNodes = doc.DocumentElement.SelectNodes("/data/why/entry");

                foreach (XmlNode entry in whoNodes)
                {
                    List<int> indices = new List<int>();
                    foreach (XmlNode index in entry.SelectNodes("articleIndex"))
                    {
                        indices.Add(Convert.ToInt32(index.InnerText));
                    }
                    results.WhoReverseIndex.Add(entry["text"].InnerText, indices);
                }

                foreach (XmlNode entry in whenNodes)
                {
                    List<int> indices = new List<int>();
                    foreach (XmlNode index in entry.SelectNodes("articleIndex"))
                    {
                        indices.Add(Convert.ToInt32(index.InnerText));
                    }
                    results.WhenReverseIndex.Add(entry.SelectSingleNode("text").InnerText, indices);
                }

                foreach (XmlNode entry in whereNodes)
                {
                    List<int> indices = new List<int>();
                    foreach (XmlNode index in entry.SelectNodes("articleIndex"))
                    {
                        indices.Add(Convert.ToInt32(index.InnerText));
                    }
                    results.WhereReverseIndex.Add(entry.SelectSingleNode("text").InnerText, indices);
                }

                foreach (XmlNode entry in whatNodes)
                {
                    List<int> indices = new List<int>();
                    foreach (XmlNode index in entry.SelectNodes("articleIndex"))
                    {
                        indices.Add(Convert.ToInt32(index.InnerText));
                    }
                    results.WhatReverseIndex.Add(entry.SelectSingleNode("text").InnerText, indices);
                }

                foreach (XmlNode entry in whyNodes)
                {
                    List<int> indices = new List<int>();
                    foreach (XmlNode index in entry.SelectNodes("articleIndex"))
                    {
                        indices.Add(Convert.ToInt32(index.InnerText));
                    }
                    results.WhyReverseIndex.Add(entry.SelectSingleNode("text").InnerText, indices);
                }
            }
            else
            {
                return null;
            }

            return results;
        }
    }
}
