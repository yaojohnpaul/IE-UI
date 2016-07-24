using IE_lib.Models;
using java.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using weka.classifiers;
using weka.classifiers.meta;
using weka.core;
using weka.filters;
using weka.filters.unsupervised.attribute;

namespace IE_lib
{
    /// <summary>
    /// Class used for extracting the 5Ws from the list of candidates for each article.
    /// </summary>
    public class Identifier
    {
        /// <summary>
        /// The current article being processed.
        /// </summary>
        private List<Token> articleCurrent;
        /// <summary>
        /// The title of the current article. Must be set together with <see cref="articleCurrent"/>.
        /// </summary>
        private String titleCurrent;
        /// <summary>
        /// The current article grouped into sentences.
        /// </summary>
        private List<List<Token>> segregatedArticleCurrent;
        /// <summary>
        /// The list of current who candidates.
        /// </summary>
        private List<Candidate> listWhoCandidates;
        /// <summary>
        /// The list of current when candidates.
        /// </summary>
        private List<Candidate> listWhenCandidates;
        /// <summary>
        /// The list of current where candidates.
        /// </summary>
        private List<Candidate> listWhereCandidates;
        /// <summary>
        /// The list of current why candidates after further processing in the <see cref="labelWhy"/> function.
        /// </summary>
        private List<Candidate> listSecondaryWhyCandidates;
        /// <summary>
        /// The list of current what candidates.
        /// </summary>
        private List<List<Token>> listWhatCandidates;
        /// <summary>
        /// The list of current why candidates.
        /// </summary>
        private List<List<Token>> listWhyCandidates;
        /// <summary>
        /// The list of final who annotations of the current article.
        /// </summary>
        private List<String> listWho;
        /// <summary>
        /// The list of final when annotations of the current article.
        /// </summary>
        private List<String> listWhen;
        /// <summary>
        /// The list of final where annotations of the current article.
        /// </summary>
        private List<String> listWhere;
        /// <summary>
        /// The string containing the final what annotation of the current article.
        /// </summary>
        private String strWhat;
        /// <summary>
        /// The string containing the final why annotation of the current article.
        /// </summary>
        private String strWhy;
        /// <summary>
        /// The fast vector for POS Tags to be used by WEKA classes.
        /// </summary>
        private FastVector fvPOS;
        /// <summary>
        /// The who WEKA classifier.
        /// </summary>
        Classifier whoClassifier;
        /// <summary>
        /// The when WEKA classifier.
        /// </summary>
        Classifier whenClassifier;
        /// <summary>
        /// The where WEKA classifier.
        /// </summary>
        Classifier whereClassifier;
        /// <summary>
        /// The why WEKA classifier.
        /// </summary>
        Classifier whyClassifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="Identifier"/> class.
        /// </summary>
        public Identifier()
        {
            listWhoCandidates = new List<Candidate>();
            listWhenCandidates = new List<Candidate>();
            listWhereCandidates = new List<Candidate>();
            listWhatCandidates = new List<List<Token>>();
            listWhyCandidates = new List<List<Token>>();
            listSecondaryWhyCandidates = new List<Candidate>();

            fvPOS = new FastVector(Token.PartOfSpeechTags.Length);
            foreach (String POS in Token.PartOfSpeechTags)
            {
                fvPOS.addElement(POS);
            }

            whoClassifier = (Classifier)SerializationHelper.read(@"..\..\..\Identifier\who.model");
            whenClassifier = (Classifier)SerializationHelper.read(@"..\..\..\Identifier\when.model");
            whereClassifier = (Classifier)SerializationHelper.read(@"..\..\..\Identifier\where.model");
            whyClassifier = (Classifier)SerializationHelper.read(@"..\..\..\Identifier\why.model");

            initializeAnnotations();
        }

        /// <summary>
        /// Initializes the annotations as empty.
        /// </summary>
        private void initializeAnnotations()
        {
            listWho = new List<String>();
            listWhen = new List<String>();
            listWhere = new List<String>();
            strWhat = "";
            strWhy = "";
        }

        #region Setters
        /// <summary>
        /// Sets the current article.
        /// </summary>
        /// <param name="pArticle">The current article.</param>
        public void setCurrentArticle(List<Token> pArticle)
        {
            articleCurrent = pArticle;
            segregatedArticleCurrent = articleCurrent
                        .GroupBy(token => token.Sentence)
                        .Select(tokenGroup => tokenGroup.ToList())
                        .ToList();
        }

        /// <summary>
        /// Sets the who candidates.
        /// </summary>
        /// <param name="pCandidates">The current who candidates.</param>
        public void setWhoCandidates(List<Candidate> pCandidates)
        {
            listWhoCandidates = pCandidates;
        }

        /// <summary>
        /// Sets the when candidates.
        /// </summary>
        /// <param name="pCandidates">The current when candidates.</param>
        public void setWhenCandidates(List<Candidate> pCandidates)
        {
            listWhenCandidates = pCandidates;
        }

        /// <summary>
        /// Sets the where candidates.
        /// </summary>
        /// <param name="pCandidates">The current where candidates.</param>
        public void setWhereCandidates(List<Candidate> pCandidates)
        {
            listWhereCandidates = pCandidates;
        }

        /// <summary>
        /// Sets the what candidates.
        /// </summary>
        /// <param name="pCandidates">The current what candidates.</param>
        public void setWhatCandidates(List<List<Token>> pCandidates)
        {
            listWhatCandidates = pCandidates;
        }

        /// <summary>
        /// Sets the why candidates.
        /// </summary>
        /// <param name="pCandidates">The current why candidates.</param>
        public void setWhyCandidates(List<List<Token>> pCandidates)
        {
            listWhyCandidates = pCandidates;
        }

        /// <summary>
        /// Sets the title.
        /// </summary>
        /// <param name="pTitle">The current article's title.</param>
        public void setTitle(String pTitle)
        {
            titleCurrent = pTitle;
        }
        #endregion

        #region Getters
        /// <summary>
        /// Gets the current article.
        /// </summary>
        /// <returns>The current article.</returns>
        public List<Token> getCurrentArticle()
        {
            return articleCurrent;
        }

        /// <summary>
        /// Gets the who.
        /// </summary>
        /// <returns>The current who annotations.</returns>
        public List<String> getWho()
        {
            return listWho;
        }

        /// <summary>
        /// Gets the when.
        /// </summary>
        /// <returns>The current when annotations.</returns>
        public List<String> getWhen()
        {
            return listWhen;
        }

        /// <summary>
        /// Gets the where.
        /// </summary>
        /// <returns>The current where annotations.</returns>
        public List<String> getWhere()
        {
            return listWhere;
        }

        /// <summary>
        /// Gets the what.
        /// </summary>
        /// <returns>The current what annotation.</returns>
        public String getWhat()
        {
            return strWhat;
        }

        /// <summary>
        /// Gets the why.
        /// </summary>
        /// <returns>The current why annotation.</returns>
        public String getWhy()
        {
            return strWhy;
        }
        #endregion

        /// <summary>
        /// Calls on all functions to extract and identify the final 5Ws for the article.
        /// </summary>
        public void labelAnnotations()
        {
            initializeAnnotations();
            labelWho();
            labelWhen();
            labelWhere();
            labelWhat();
            labelWhy();
        }

        #region Labelling Functions
        /// <summary>
        /// Extracts the who.
        /// </summary>
        private void labelWho()
        {
            Instances whoInstances = createWhoInstances();
            
            foreach (Instance instance in whoInstances)
            {
                double[] classProbability = whoClassifier.distributionForInstance(instance);
                if (classProbability[0] >= classProbability[1])
                {
                    listWho.Add(instance.stringValue(0));
                }
            }
        }

        /// <summary>
        /// Extracts the when.
        /// </summary>
        private void labelWhen()
        {
            Instances whenInstances = createWhenInstances();

            foreach (Instance instance in whenInstances)
            {
                double[] classProbability = whenClassifier.distributionForInstance(instance);
                if (classProbability[0] >= classProbability[1])
                {
                    listWhen.Add(instance.stringValue(0));
                }
            }
        }

        /// <summary>
        /// Extracts the where.
        /// </summary>
        private void labelWhere()
        {
            Instances whereInstances = createWhereInstances();

            foreach (Instance instance in whereInstances)
            {
                double[] classProbability = whereClassifier.distributionForInstance(instance);
                if (classProbability[0] >= classProbability[1])
                {
                    listWhere.Add(instance.stringValue(0));
                }
            }
        }

        /// <summary>
        /// Extracts the what.
        /// </summary>
        private void labelWhat()
        {
            double WEIGHT_PER_WHO = 0.3;
            double WEIGHT_PER_WHEN = 0.2;
            double WEIGHT_PER_WHERE = 0.2;
            double WEIGHT_PER_SENTENCE = 0.2;
            double WEIGHT_PER_W_IN_TITLE = 0.1;

            List<double> candidateWeights = new List<double>();
            double highestWeight = -1;

            String[][] markers = new String[][] {
                new String[] { "kaya", "START" },
                new String[] { "para", "END" },
                new String[] { "dahil", "END" },
                new String[] { "upang", "END" },
                new String[] { "makaraang", "END" },
            };

            if (listWhatCandidates.Count > 0)
            {
                foreach (List<Token> candidate in listWhatCandidates)
                {
                    String tempWhat = "";
                    double tempWeight = 0;
                    String[] match;
                    bool hasMarker = false;

                    tempWhat = String.Join(" ", candidate.Select(token => token.Value).ToArray());
                    tempWhat = tempWhat.Replace("-LRB- ", "(");
                    tempWhat = tempWhat.Replace(" -RRB-", ")");
                    tempWhat = tempWhat.Replace(" . ", ".");
                    tempWhat = tempWhat.Replace(" .", ".");
                    tempWhat = tempWhat.Replace(" ,", ",");
                    tempWhat = tempWhat.Replace(" !", "!");

                    tempWeight += listWho.Where(tempWhat.Contains).Count() * WEIGHT_PER_WHO;
                    tempWeight += listWhen.Where(tempWhat.Contains).Count() * WEIGHT_PER_WHEN;
                    tempWeight += listWhere.Where(tempWhat.Contains).Count() * WEIGHT_PER_WHERE;
                    tempWeight += 1 - WEIGHT_PER_SENTENCE * candidate[0].Sentence;

                    tempWeight += listWho.Where(titleCurrent.Contains).Count() * WEIGHT_PER_W_IN_TITLE;
                    tempWeight += listWhen.Where(titleCurrent.Contains).Count() * WEIGHT_PER_W_IN_TITLE;
                    tempWeight += listWhere.Where(titleCurrent.Contains).Count() * WEIGHT_PER_W_IN_TITLE;

                    candidateWeights.Add(tempWeight);

                    match = markers.FirstOrDefault(s => tempWhat.Contains(s[0]));

                    if (match != null)
                    {
                        tempWhat = (match[1].Equals("START")) ?
                            tempWhat.Substring(tempWhat.IndexOf(match[0]) + match[0].Count() + 1) :
                            tempWhat.Substring(0, tempWhat.IndexOf(match[0]));
                        hasMarker = true;
                    }

                    if (tempWeight > highestWeight)
                    {
                        strWhat = tempWhat;
                        highestWeight = tempWeight;
                    }
                }
            }
        }

        /// <summary>
        /// Extracts the why.
        /// </summary>
        private void labelWhy()
        {
            double WEIGHT_PER_MARKER = 0.5;
            double WEIGHT_PER_WHAT = 0.5;
            double CARRY_OVER = 0;

            String[][] markers = new String[][] {
                new String[] { " sanhi sa ", "START" },
                new String[] { " sanhi ng ", "START" },
                new String[] { " sapagkat ", "START" },
                new String[] { " palibhasa ay ", "START" },
                new String[] { " palibhasa ", "START" },
                new String[] { " kasi ", "START" },
                new String[] { " mangyari'y ", "START" },
                new String[] { " mangyari ay ", "START" },
                new String[] { " dahil sa ", "START" },
                new String[] { " dahil na rin sa ", "START" },
                new String[] { " dahil ", "START" },
                new String[] { " dahilan sa", "START" },
                new String[] { " dahilan ", "START" },
                new String[] { " para ", "START" },
                new String[] { " upang ", "START" },
                new String[] { " makaraang ", "START" },
                new String[] { " naglalayong ", "START" },
                new String[] { " kaya ", "END" }
            };

            string[] endMarkers = new string[]
            {
                " makaraang ",
                ", ",
            };

            String[] verbMarkers = new String[]
            {
                "pag-usapan",
                "sinabi",
                "pinalalayo",
                "itatatag",
                "sinisi",
                "nakipag-ugnayan",
                "nagsampa",
                "hiniling"
            };

            List<double> candidateWeights = new List<double>();

            if (listWhyCandidates.Count > 0)
            {
                foreach (List<Token> candidate in listWhyCandidates)
                {
                    String tempWhy = "";
                    String copyWhy = "";
                    double tempWeight = 0;
                    String[] match;

                    tempWhy = String.Join(" ", candidate.Select(token => token.Value).ToArray());
                    tempWhy = tempWhy.Replace("-LRB- ", "(");
                    tempWhy = tempWhy.Replace(" -RRB-", ")");
                    tempWhy = tempWhy.Replace(" . ", ".");
                    tempWhy = tempWhy.Replace(" .", ".");
                    tempWhy = tempWhy.Replace(" ,", ",");
                    tempWhy = tempWhy.Replace(" !", "!");

                    copyWhy = tempWhy;

                    if (tempWhy.Contains(strWhat))
                    {
                        tempWeight += WEIGHT_PER_WHAT;
                    }

                    match = markers.FirstOrDefault(s => tempWhy.Contains(s[0]));

                    if (match != null)
                    {
                        tempWhy = (match[1].Equals("START")) ?
                            tempWhy.Substring(tempWhy.IndexOf(match[0]) + match[0].Count()) :
                            tempWhy.Substring(0, tempWhy.IndexOf(match[0]));
                        tempWeight += WEIGHT_PER_MARKER;
                    }

                    tempWeight += CARRY_OVER;
                    CARRY_OVER = 0;

                    if (strWhat.Contains(tempWhy))
                    {
                        tempWeight = 0;
                    }

                    if (strWhat.Equals(tempWhy))
                    {
                        CARRY_OVER = 0.5;
                    }

                    int position = candidate[0].Position + copyWhy.Substring(0, copyWhy.IndexOf(tempWhy)).Split(' ').Count() - 1;
                    int length = tempWhy.Split(' ').Count();

                    Candidate newCandidate = new Candidate(tempWhy, position, length);

                    newCandidate.Sentence = candidate[0].Sentence;
                    newCandidate.Score = tempWeight;
                    newCandidate.NumWho = listWho.Where(tempWhy.Contains).Count();
                    newCandidate.NumWhen = listWhen.Where(tempWhy.Contains).Count();
                    newCandidate.NumWhere = listWhere.Where(tempWhy.Contains).Count();

                    listSecondaryWhyCandidates.Add(newCandidate);
                }
            }

            Instances whyInstances = createWhyInstances();

            foreach (Instance instance in whyInstances)
            {
                double[] classProbability = whyClassifier.distributionForInstance(instance);
                if (classProbability[0] >= classProbability[1])
                {
                    strWhy = instance.stringValue(0);
                    break;
                }
            }

            listSecondaryWhyCandidates = new List<Candidate>();
        }
        #endregion

        #region Instances Creation
        #region Instance Group Creation
        /// <summary>
        /// Creates the who instances for WEKA model compatibility.
        /// </summary>
        /// <returns></returns>
        private Instances createWhoInstances()
        {
            FastVector fvWho = createWhoFastVector();
            Instances whoInstances = new Instances("WhoInstances", fvWho, listWhoCandidates.Count);
            foreach (Token candidate in listWhoCandidates)
            {
                if (candidate.Value == null) continue;
                Instance whoInstance = createSingleWhoInstance(fvWho, candidate);
                whoInstance.setDataset(whoInstances);
                whoInstances.add(whoInstance);
            }
            whoInstances.setClassIndex(fvWho.size() - 1);
            return whoInstances;
        }

        /// <summary>
        /// Creates the when instances for WEKA model compatibility.
        /// </summary>
        /// <returns></returns>
        private Instances createWhenInstances()
        {
            FastVector fvWhen = createWhenFastVector();
            Instances whenInstances = new Instances("WhenInstances", fvWhen, listWhenCandidates.Count);
            foreach (Token candidate in listWhenCandidates)
            {
                if (candidate.Value == null) continue;
                Instance whenInstance = createSingleWhenInstance(fvWhen, candidate);
                whenInstance.setDataset(whenInstances);
                whenInstances.add(whenInstance);
            }
            whenInstances.setClassIndex(fvWhen.size() - 1);
            return whenInstances;
        }

        /// <summary>
        /// Creates the where instances for WEKA model compatibility.
        /// </summary>
        /// <returns></returns>
        private Instances createWhereInstances()
        {
            FastVector fvWhere = createWhereFastVector();
            Instances whereInstances = new Instances("WhereInstances", fvWhere, listWhereCandidates.Count);
            foreach (Token candidate in listWhereCandidates)
            {
                if (candidate.Value == null) continue;
                Instance whereInstance = createSingleWhereInstance(fvWhere, candidate);
                whereInstance.setDataset(whereInstances);
                whereInstances.add(whereInstance);
            }
            whereInstances.setClassIndex(fvWhere.size() - 1);
            return whereInstances;
        }

        /// <summary>
        /// Creates the why instances for WEKA model compatibility.
        /// </summary>
        /// <returns></returns>
        private Instances createWhyInstances()
        {
            FastVector fvWhy = createWhyFastVector();
            Instances whyInstances = new Instances("WhyInstances", fvWhy, listSecondaryWhyCandidates.Count);
            foreach (Token candidate in listSecondaryWhyCandidates)
            {
                if (candidate.Value == null) continue;
                Instance whyInstance = createSingleWhyInstance(fvWhy, candidate);
                whyInstance.setDataset(whyInstances);
                whyInstances.add(whyInstance);
            }
            whyInstances.setClassIndex(fvWhy.size() - 1);
            return whyInstances;
        }
        #endregion

        /// <summary>
        /// Number of neighbouring words before the who candidate.
        /// </summary>
        private const int whoWordsBefore = 10;
        /// <summary>
        /// Number of neighbouring words after the who candidate.
        /// </summary>
        private const int whoWordsAfter = 10;
        /// <summary>
        /// Number of neighbouring words before the when candidate.
        /// </summary>
        private const int whenWordsBefore = 3;
        /// <summary>
        /// Number of neighbouring words after the when candidate.
        /// </summary>
        private const int whenWordsAfter = 3;
        /// <summary>
        /// Number of neighbouring words before the where candidate.
        /// </summary>
        private const int whereWordsBefore = 10;
        /// <summary>
        /// Number of neighbouring words after the where candidate.
        /// </summary>
        private const int whereWordsAfter = 10;
        /// <summary>
        /// Number of neighbouring words before the why candidate.
        /// </summary>
        private const int whyWordsBefore = 10;
        /// <summary>
        /// Number of neighbouring words after the why candidate.
        /// </summary>
        private const int whyWordsAfter = 10;

        #region Single Instance Creation
        /// <summary>
        /// Creates a single who instance for WEKA model compatibility.
        /// </summary>
        /// <param name="fvWho">The fv who.</param>
        /// <param name="candidate">The candidate.</param>
        /// <returns></returns>
        private Instance createSingleWhoInstance(FastVector fvWho, Token candidate)
        {
            //first word-n attribute number
            int wordsBeforeFirstAttributeNumber = 6;
            //first pos-n attribute number
            int posBeforeFirstAttributeNumber = wordsBeforeFirstAttributeNumber + whoWordsBefore + whoWordsAfter;
            //word+1 attribute number
            int wordsAfterFirstAttributeNumber = wordsBeforeFirstAttributeNumber + whoWordsBefore;
            //pos+1 attribute number
            int posAfterFirstAttributeNumber = posBeforeFirstAttributeNumber + whoWordsBefore;

            int totalAttributeCount = wordsBeforeFirstAttributeNumber + whoWordsBefore * 2 + whoWordsAfter * 2 + 1;

            Instance whoCandidate = new DenseInstance(totalAttributeCount);
            whoCandidate.setValue((weka.core.Attribute)fvWho.elementAt(0), candidate.Value);
            whoCandidate.setValue((weka.core.Attribute)fvWho.elementAt(1), candidate.Value.Split(' ').Count());
            whoCandidate.setValue((weka.core.Attribute)fvWho.elementAt(2), candidate.Sentence);
            whoCandidate.setValue((weka.core.Attribute)fvWho.elementAt(3), candidate.Position);
            double sentenceStartProximity = -1;
            foreach (List<Token> tokenList in segregatedArticleCurrent)
            {
                if (tokenList.Count > 0 && tokenList[0].Sentence == candidate.Sentence)
                {
                    sentenceStartProximity = (double)(candidate.Position - tokenList[0].Position) / (double)tokenList.Count;
                    break;
                }
            }
            if (sentenceStartProximity > -1)
            {
                whoCandidate.setValue((weka.core.Attribute)fvWho.elementAt(4), sentenceStartProximity);
            }
            whoCandidate.setValue((weka.core.Attribute)fvWho.elementAt(5), candidate.Frequency);

            for (int i = whoWordsBefore; i > 0; i--)
            {
                if (candidate.Position - i - 1 >= 0)
                {
                    whoCandidate.setValue((weka.core.Attribute)fvWho.elementAt(whoWordsBefore - i + wordsBeforeFirstAttributeNumber), articleCurrent[candidate.Position - i - 1].Value);
                    if (articleCurrent[candidate.Position - i - 1].PartOfSpeech != null)
                    {
                        whoCandidate.setValue((weka.core.Attribute)fvWho.elementAt(whoWordsBefore - i + posBeforeFirstAttributeNumber), articleCurrent[candidate.Position - i - 1].PartOfSpeech);
                    }
                }
            }
            for (int i = 0; i < whoWordsAfter; i++)
            {
                if (candidate.Position + i < articleCurrent.Count)
                {
                    whoCandidate.setValue((weka.core.Attribute)fvWho.elementAt(wordsAfterFirstAttributeNumber + i), articleCurrent[candidate.Position + i].Value);
                    if (articleCurrent[candidate.Position + i].PartOfSpeech != null)
                    {
                        whoCandidate.setValue((weka.core.Attribute)fvWho.elementAt(posAfterFirstAttributeNumber + i), articleCurrent[candidate.Position + i].PartOfSpeech);
                    }
                }
            }
            return whoCandidate;
        }

        /// <summary>
        /// Creates a single when instance for WEKA model compatibility.
        /// </summary>
        /// <param name="fvWhen">The fv when.</param>
        /// <param name="candidate">The candidate.</param>
        /// <returns></returns>
        private Instance createSingleWhenInstance(FastVector fvWhen, Token candidate)
        {
            //first word-n attribute number
            int wordsBeforeFirstAttributeNumber = 4;
            //first pos-n attribute number
            int posBeforeFirstAttributeNumber = wordsBeforeFirstAttributeNumber + whenWordsBefore + whenWordsAfter;
            //word+1 attribute number
            int wordsAfterFirstAttributeNumber = wordsBeforeFirstAttributeNumber + whenWordsBefore;
            //pos+1 attribute number
            int posAfterFirstAttributeNumber = posBeforeFirstAttributeNumber + whenWordsBefore;

            int totalAttributeCount = wordsBeforeFirstAttributeNumber + whenWordsBefore * 2 + whenWordsAfter * 2 + 1;

            Instance whenCandidate = new DenseInstance(totalAttributeCount);
            whenCandidate.setValue((weka.core.Attribute)fvWhen.elementAt(0), candidate.Value);
            whenCandidate.setValue((weka.core.Attribute)fvWhen.elementAt(1), candidate.Value.Split(' ').Count());
            whenCandidate.setValue((weka.core.Attribute)fvWhen.elementAt(2), candidate.Sentence);
            //whenCandidate.setValue((weka.core.Attribute)fvWhen.elementAt(3), candidate.Position);
            //double sentenceStartProximity = -1;
            //foreach (List<Token> tokenList in segregatedArticleCurrent)
            //{
            //    if (tokenList.Count > 0 && tokenList[0].Sentence == candidate.Sentence)
            //    {
            //        sentenceStartProximity = (double)(candidate.Position - tokenList[0].Position) / (double)tokenList.Count;
            //        break;
            //    }
            //}
            //if (sentenceStartProximity > -1)
            //{
            //    whenCandidate.setValue((weka.core.Attribute)fvWhen.elementAt(4), sentenceStartProximity);
            //}
            whenCandidate.setValue((weka.core.Attribute)fvWhen.elementAt(3), candidate.Frequency);
            for (int i = whenWordsBefore; i > 0; i--)
            {
                if (candidate.Position - i - 1 >= 0)
                {
                    whenCandidate.setValue((weka.core.Attribute)fvWhen.elementAt(whenWordsBefore - i + wordsBeforeFirstAttributeNumber), articleCurrent[candidate.Position - i - 1].Value);
                    if (articleCurrent[candidate.Position - i - 1].PartOfSpeech != null)
                    {
                        whenCandidate.setValue((weka.core.Attribute)fvWhen.elementAt(whenWordsBefore - i + posBeforeFirstAttributeNumber), articleCurrent[candidate.Position - i - 1].PartOfSpeech);
                    }
                }
            }
            for (int i = 0; i < whenWordsAfter; i++)
            {
                if (candidate.Position + i < articleCurrent.Count)
                {
                    whenCandidate.setValue((weka.core.Attribute)fvWhen.elementAt(wordsAfterFirstAttributeNumber + i), articleCurrent[candidate.Position + i].Value);
                    if (articleCurrent[candidate.Position + i].PartOfSpeech != null)
                    {
                        whenCandidate.setValue((weka.core.Attribute)fvWhen.elementAt(posAfterFirstAttributeNumber + i), articleCurrent[candidate.Position + i].PartOfSpeech);
                    }
                }
            }
            return whenCandidate;
        }

        /// <summary>
        /// Creates a single where instance for WEKA model compatibility.
        /// </summary>
        /// <param name="fvWhere">The fv where.</param>
        /// <param name="candidate">The candidate.</param>
        /// <returns></returns>
        private Instance createSingleWhereInstance(FastVector fvWhere, Token candidate)
        {
            //first word-n attribute number
            int wordsBeforeFirstAttributeNumber = 4;
            //first pos-n attribute number
            int posBeforeFirstAttributeNumber = wordsBeforeFirstAttributeNumber + whereWordsBefore + whereWordsAfter;
            //word+1 attribute number
            int wordsAfterFirstAttributeNumber = wordsBeforeFirstAttributeNumber + whereWordsBefore;
            //pos+1 attribute number
            int posAfterFirstAttributeNumber = posBeforeFirstAttributeNumber + whereWordsBefore;

            int totalAttributeCount = wordsBeforeFirstAttributeNumber + whereWordsBefore * 2 + whereWordsAfter * 2 + 1;

            Instance whereCandidate = new DenseInstance(totalAttributeCount);
            whereCandidate.setValue((weka.core.Attribute)fvWhere.elementAt(0), candidate.Value);
            whereCandidate.setValue((weka.core.Attribute)fvWhere.elementAt(1), candidate.Value.Split(' ').Count());
            whereCandidate.setValue((weka.core.Attribute)fvWhere.elementAt(2), candidate.Sentence);
            //whereCandidate.setValue((weka.core.Attribute)fvWhere.elementAt(3), candidate.Position);
            //double sentenceStartProximity = -1;
            //foreach (List<Token> tokenList in segregatedArticleCurrent)
            //{
            //    if (tokenList.Count > 0 && tokenList[0].Sentence == candidate.Sentence)
            //    {
            //        sentenceStartProximity = (double)(candidate.Position - tokenList[0].Position) / (double)tokenList.Count;
            //        break;
            //    }
            //}
            //if (sentenceStartProximity > -1)
            //{
            //    whereCandidate.setValue((weka.core.Attribute)fvWhere.elementAt(4), sentenceStartProximity);
            //}
            whereCandidate.setValue((weka.core.Attribute)fvWhere.elementAt(3), candidate.Frequency);
            for (int i = whereWordsBefore; i > 0; i--)
            {
                if (candidate.Position - i - 1 >= 0)
                {
                    whereCandidate.setValue((weka.core.Attribute)fvWhere.elementAt(whereWordsBefore - i + wordsBeforeFirstAttributeNumber), articleCurrent[candidate.Position - i - 1].Value);
                    if (articleCurrent[candidate.Position - i - 1].PartOfSpeech != null)
                    {
                        whereCandidate.setValue((weka.core.Attribute)fvWhere.elementAt(whereWordsBefore - i + posBeforeFirstAttributeNumber), articleCurrent[candidate.Position - i - 1].PartOfSpeech);
                    }
                }
            }
            for (int i = 0; i < whereWordsAfter; i++)
            {
                if (candidate.Position + i < articleCurrent.Count)
                {
                    whereCandidate.setValue((weka.core.Attribute)fvWhere.elementAt(wordsAfterFirstAttributeNumber + i), articleCurrent[candidate.Position + i].Value);
                    if (articleCurrent[candidate.Position + i].PartOfSpeech != null)
                    {
                        whereCandidate.setValue((weka.core.Attribute)fvWhere.elementAt(posAfterFirstAttributeNumber + i), articleCurrent[candidate.Position + i].PartOfSpeech);
                    }
                }
            }
            return whereCandidate;
        }
        /// <summary>
        /// Creates a single why instance for WEKA model compatibility.
        /// </summary>
        /// <param name="fvWhy">The fv why.</param>
        /// <param name="candidate">The candidate.</param>
        /// <returns></returns>
        private Instance createSingleWhyInstance(FastVector fvWhy, Token candidate)
        {
            //first word-n attribute number
            int wordsBeforeFirstAttributeNumber = 7;
            //first pos-n attribute number
            int posBeforeFirstAttributeNumber = wordsBeforeFirstAttributeNumber + whyWordsBefore + whyWordsAfter;
            //word+1 attribute number
            int wordsAfterFirstAttributeNumber = wordsBeforeFirstAttributeNumber + whyWordsBefore;
            //pos+1 attribute number
            int posAfterFirstAttributeNumber = posBeforeFirstAttributeNumber + whyWordsBefore;

            int totalAttributeCount = wordsBeforeFirstAttributeNumber + whyWordsBefore * 2 + whyWordsAfter * 2 + 1;

            Instance whyCandidate = new DenseInstance(totalAttributeCount);
            whyCandidate.setValue((weka.core.Attribute)fvWhy.elementAt(0), candidate.Value);
            whyCandidate.setValue((weka.core.Attribute)fvWhy.elementAt(1), candidate.Value.Split(' ').Count());
            whyCandidate.setValue((weka.core.Attribute)fvWhy.elementAt(2), candidate.Sentence);
            whyCandidate.setValue((weka.core.Attribute)fvWhy.elementAt(3), candidate.Score);
            whyCandidate.setValue((weka.core.Attribute)fvWhy.elementAt(4), candidate.NumWho);
            whyCandidate.setValue((weka.core.Attribute)fvWhy.elementAt(5), candidate.NumWhen);
            whyCandidate.setValue((weka.core.Attribute)fvWhy.elementAt(6), candidate.NumWhere);
            for (int i = whyWordsBefore; i > 0; i--)
            {
                if (candidate.Position - i - 1 >= 0)
                {
                    whyCandidate.setValue((weka.core.Attribute)fvWhy.elementAt(whyWordsBefore - i + wordsBeforeFirstAttributeNumber), articleCurrent[candidate.Position - i - 1].Value);
                    if (articleCurrent[candidate.Position - i - 1].PartOfSpeech != null)
                    {
                        whyCandidate.setValue((weka.core.Attribute)fvWhy.elementAt(whyWordsBefore - i + posBeforeFirstAttributeNumber), articleCurrent[candidate.Position - i - 1].PartOfSpeech);
                    }
                }
            }
            for (int i = 0; i < whyWordsAfter; i++)
            {
                if (candidate.Position + i < articleCurrent.Count)
                {
                    whyCandidate.setValue((weka.core.Attribute)fvWhy.elementAt(wordsAfterFirstAttributeNumber + i), articleCurrent[candidate.Position + i].Value);
                    if (articleCurrent[candidate.Position + i].PartOfSpeech != null)
                    {
                        whyCandidate.setValue((weka.core.Attribute)fvWhy.elementAt(posAfterFirstAttributeNumber + i), articleCurrent[candidate.Position + i].PartOfSpeech);
                    }
                }
            }
            return whyCandidate;
        }
        #endregion
        #endregion

        #region Fast Vector Creation
        /// <summary>
        /// Creates the who fast vector as template for creating WEKA-compatible instances.
        /// </summary>
        /// <returns></returns>
        private FastVector createWhoFastVector()
        {
            FastVector fvWho = new FastVector(7 + whoWordsBefore * 2 + whoWordsAfter * 2);
            fvWho.addElement(new weka.core.Attribute("word", (FastVector)null));
            fvWho.addElement(new weka.core.Attribute("wordCount"));
            fvWho.addElement(new weka.core.Attribute("sentence"));
            fvWho.addElement(new weka.core.Attribute("position"));
            fvWho.addElement(new weka.core.Attribute("sentenceStartProximity"));
            fvWho.addElement(new weka.core.Attribute("wordScore"));
            for (int i = whoWordsBefore; i > 0; i--)
            {
                fvWho.addElement(new weka.core.Attribute("word-" + i, (FastVector)null));
            }
            for (int i = 1; i <= whoWordsAfter; i++)
            {
                fvWho.addElement(new weka.core.Attribute("word+" + i, (FastVector)null));
            }
            for (int i = whoWordsBefore; i > 0; i--)
            {
                fvWho.addElement(new weka.core.Attribute("postag-" + i, fvPOS));
            }
            for (int i = 1; i <= whoWordsAfter; i++)
            {
                fvWho.addElement(new weka.core.Attribute("postag+" + i, fvPOS));
            }
            FastVector fvClass = new FastVector(2);
            fvClass.addElement("yes");
            fvClass.addElement("no");
            fvWho.addElement(new weka.core.Attribute("who", fvClass));
            return fvWho;
        }

        /// <summary>
        /// Creates the when fast vector as template for creating WEKA-compatible instances.
        /// </summary>
        /// <returns></returns>
        private FastVector createWhenFastVector()
        {
            FastVector fvWhen = new FastVector(5 + whenWordsBefore * 2 + whenWordsAfter * 2);
            fvWhen.addElement(new weka.core.Attribute("word", (FastVector)null));
            fvWhen.addElement(new weka.core.Attribute("wordCount"));
            fvWhen.addElement(new weka.core.Attribute("sentence"));
            //fvWhen.addElement(new weka.core.Attribute("position"));
            //fvWhen.addElement(new weka.core.Attribute("sentenceStartProximity"));
            fvWhen.addElement(new weka.core.Attribute("wordScore"));
            for (int i = whenWordsBefore; i > 0; i--)
            {
                fvWhen.addElement(new weka.core.Attribute("word-" + i, (FastVector)null));
            }
            for (int i = 1; i <= whenWordsAfter; i++)
            {
                fvWhen.addElement(new weka.core.Attribute("word+" + i, (FastVector)null));
            }
            for (int i = whenWordsBefore; i > 0; i--)
            {
                fvWhen.addElement(new weka.core.Attribute("postag-" + i, fvPOS));
            }
            for (int i = 1; i <= whenWordsAfter; i++)
            {
                fvWhen.addElement(new weka.core.Attribute("postag+" + i, fvPOS));
            }
            FastVector fvClass = new FastVector(2);
            fvClass.addElement("yes");
            fvClass.addElement("no");
            fvWhen.addElement(new weka.core.Attribute("when", fvClass));
            return fvWhen;
        }

        /// <summary>
        /// Creates the where fast vector as template for creating WEKA-compatible instances.
        /// </summary>
        /// <returns></returns>
        private FastVector createWhereFastVector()
        {
            FastVector fvWhere = new FastVector(5 + whereWordsBefore * 2 + whereWordsAfter * 2);
            fvWhere.addElement(new weka.core.Attribute("word", (FastVector)null));
            fvWhere.addElement(new weka.core.Attribute("wordCount"));
            fvWhere.addElement(new weka.core.Attribute("sentence"));
            //fvWhere.addElement(new weka.core.Attribute("position"));
            //fvWhere.addElement(new weka.core.Attribute("sentenceStartProximity"));
            fvWhere.addElement(new weka.core.Attribute("wordScore"));
            for (int i = whereWordsBefore; i > 0; i--)
            {
                fvWhere.addElement(new weka.core.Attribute("word-" + i, (FastVector)null));
            }
            for (int i = 1; i <= whereWordsAfter; i++)
            {
                fvWhere.addElement(new weka.core.Attribute("word+" + i, (FastVector)null));
            }
            for (int i = whereWordsBefore; i > 0; i--)
            {
                fvWhere.addElement(new weka.core.Attribute("postag-" + i, fvPOS));
            }
            for (int i = 1; i <= whereWordsAfter; i++)
            {
                fvWhere.addElement(new weka.core.Attribute("postag+" + i, fvPOS));
            }
            FastVector fvClass = new FastVector(2);
            fvClass.addElement("yes");
            fvClass.addElement("no");
            fvWhere.addElement(new weka.core.Attribute("where", fvClass));
            return fvWhere;
        }

        /// <summary>
        /// Creates the why fast vector as template for creating WEKA-compatible instances.
        /// </summary>
        /// <returns></returns>
        private FastVector createWhyFastVector()
        {
            FastVector fvWhy = new FastVector(8 + whyWordsBefore * 2 + whyWordsAfter * 2);
            fvWhy.addElement(new weka.core.Attribute("candidate", (FastVector)null));
            fvWhy.addElement(new weka.core.Attribute("wordCount"));
            fvWhy.addElement(new weka.core.Attribute("sentence"));
            fvWhy.addElement(new weka.core.Attribute("candidateScore"));
            fvWhy.addElement(new weka.core.Attribute("numWho"));
            fvWhy.addElement(new weka.core.Attribute("numWhen"));
            fvWhy.addElement(new weka.core.Attribute("numWhere"));
            for (int i = whereWordsBefore; i > 0; i--)
            {
                fvWhy.addElement(new weka.core.Attribute("word-" + i, (FastVector)null));
            }
            for (int i = 1; i <= whereWordsAfter; i++)
            {
                fvWhy.addElement(new weka.core.Attribute("word+" + i, (FastVector)null));
            }
            for (int i = whyWordsBefore; i > 0; i--)
            {
                fvWhy.addElement(new weka.core.Attribute("postag-" + i, fvPOS));
            }
            for (int i = 1; i <= whyWordsAfter; i++)
            {
                fvWhy.addElement(new weka.core.Attribute("postag+" + i, fvPOS));
            }
            FastVector fvClass = new FastVector(2);
            fvClass.addElement("yes");
            fvClass.addElement("no");
            fvWhy.addElement(new weka.core.Attribute("why", fvClass));
            return fvWhy;
        }
        #endregion
    }
}
