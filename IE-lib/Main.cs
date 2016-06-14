using IE_lib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace IE_lib
{
    public class Main
    {
        public static bool Extract(string source, string destination)
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

            #region Parse Source File
            //status = "parsing source file";

            listCurrentArticles = fileParser.parseFile(source);

            if (listCurrentArticles == null)
            {
                return false;
            }
            #endregion

            #region Preprocess Article 
            //status = "preprocessing articles";

            if (listCurrentArticles != null && listCurrentArticles.Count > 0)
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
                }
            }
            else
            {
                return false;
            }
            #endregion

            #region Identify 5W's
            //status = "identifying features";

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
            }

            #endregion

            #region Generate Output
            //status = "generating output";


            String destinationPath = destination;
            String invertedDestinationPath = destination.Insert(destination.Length - 4, "_inverted_index");
            String formatDateDestinationPath = destination.Insert(destination.Length - 4, "_format_date");

            ResultWriter rw = new ResultWriter(destinationPath, invertedDestinationPath, formatDateDestinationPath, listCurrentArticles, listAllWhoAnnotations, listAllWhenAnnotations, listAllWhereAnnotations, listAllWhatAnnotations, listAllWhyAnnotations);
            rw.generateOutput();
            rw.generateOutputFormatDate();
            rw.generateInvertedIndexOutput();

            #endregion 

            //status = "process completed";

            return true;
        }

        public static ParsedResults View(string path)
        {
            FileParser fileParser = new FileParser();

            List<Article> listArticles = fileParser.parseFile(path);
            List<Annotation> listAnnotations = fileParser.parseAnnotations(path);

            ParsedResults results = new ParsedResults();

            results.FilePath = path;
            results.ListDisplayArticles = new List<DisplayArticle>();
            results.WhoReverseIndex = new Dictionary<string, List<int>>();
            results.WhenReverseIndex = new Dictionary<string, List<int>>();
            results.WhereReverseIndex = new Dictionary<string, List<int>>();
            results.WhatReverseIndex = new Dictionary<string, List<int>>();
            results.WhyReverseIndex = new Dictionary<string, List<int>>();

            String formatDateDestinationPath = path.Insert(path.Length - 4, "_inverted_index");

            if (listArticles.Count <= 0 || listAnnotations.Count <= 0)
            {
                return null;
            }

            foreach (int i in Enumerable.Range(0, listAnnotations.Count()))
            {
                listAnnotations[i].Index = i;
                results.ListDisplayArticles.Add(new DisplayArticle()
                {
                    Article = listArticles[i],
                    Annotation = listAnnotations[i]
                });
            }

            if (File.Exists(formatDateDestinationPath))
            {
                XmlDocument doc = new XmlDocument();

                doc.Load(formatDateDestinationPath);

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
