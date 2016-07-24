using edu.stanford.nlp.ie.crf;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.process;
using edu.stanford.nlp.tagger.maxent;
using IE_lib.Models;
using java.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace IE_lib
{
    /// <summary>
    /// Class responsible for text processing functions.
    /// </summary>
    public class Preprocessor
    {
        /// <summary>
        /// The current article.
        /// </summary>
        private Article articleCurrent;
        /// <summary>
        /// The current article after tokenization.
        /// </summary>
        private List<Token> listLatestTokenizedArticle;
        /// <summary>
        /// The list of who candidates for the current article.
        /// </summary>
        private List<Candidate> listWhoCandidates;
        /// <summary>
        /// The list of when candidates for the current article.
        /// </summary>
        private List<Candidate> listWhenCandidates;
        /// <summary>
        /// The list of where candidates for the current article.
        /// </summary>
        private List<Candidate> listWhereCandidates;
        /// <summary>
        /// The list of what candidates for the current article.
        /// </summary>
        private List<List<Token>> listWhatCandidates;
        /// <summary>
        /// The list of why candidates for the current article.
        /// </summary>
        private List<List<Token>> listWhyCandidates;
        /// <summary>
        /// The NER classifier.
        /// </summary>
        private CRFClassifier nerClassifier;
        /// <summary>
        /// The POS tagger.
        /// </summary>
        private MaxentTagger posTagger;

        /// <summary>
        /// The NER model path.
        /// </summary>
        private readonly String nerModelPath = @"..\..\..\NER\filipino.ser.gz";
        /// <summary>
        /// The POS tagger model path.
        /// </summary>
        private readonly String posModelPath = @"..\..\..\POST\filipino.tagger";

        /// <summary>
        /// Initializes a new instance of the <see cref="Preprocessor" /> class.
        /// </summary>
        public Preprocessor()
        {
            listLatestTokenizedArticle = new List<Token>();
            listWhoCandidates = new List<Candidate>();
            listWhenCandidates = new List<Candidate>();
            listWhereCandidates = new List<Candidate>();
            listWhatCandidates = new List<List<Token>>();
            listWhyCandidates = new List<List<Token>>();
            nerClassifier = CRFClassifier.getClassifierNoExceptions(nerModelPath);
            posTagger = new MaxentTagger(posModelPath);
        }

        #region Setters
        /// <summary>
        /// Sets the current article.
        /// </summary>
        /// <param name="pArticle">The p article.</param>
        public void setCurrentArticle(Article pArticle)
        {
            articleCurrent = pArticle;
        }
        #endregion

        #region Getters
        /// <summary>
        /// Gets the current article.
        /// </summary>
        /// <returns>The current article.</returns>
        public Article getCurrentArticle()
        {
            return articleCurrent;
        }

        /// <summary>
        /// Gets the latest tokenized article.
        /// </summary>
        /// <returns>The latest tokenized article.</returns>
        public List<Token> getLatestTokenizedArticle()
        {
            return listLatestTokenizedArticle;
        }

        /// <summary>
        /// Gets the who candidates.
        /// </summary>
        /// <returns>The who candidates of the current article.</returns>
        public List<Candidate> getWhoCandidates()
        {
            return listWhoCandidates;
        }

        /// <summary>
        /// Gets the when candidates.
        /// </summary>
        /// <returns>The when candidates of the current article.</returns>
        public List<Candidate> getWhenCandidates()
        {
            return listWhenCandidates;
        }

        /// <summary>
        /// Gets the where candidates.
        /// </summary>
        /// <returns>The where candidates of the current article.</returns>
        public List<Candidate> getWhereCandidates()
        {
            return listWhereCandidates;
        }

        /// <summary>
        /// Gets the what candidates.
        /// </summary>
        /// <returns>The what candidates of the current article.</returns>
        public List<List<Token>> getWhatCandidates()
        {
            return listWhatCandidates;
        }

        /// <summary>
        /// Gets the why candidates.
        /// </summary>
        /// <returns>The why candidates of the current article.</returns>
        public List<List<Token>> getWhyCandidates()
        {
            return listWhyCandidates;
        }
        #endregion

        /// <summary>
        /// Performs text processing on the current article.
        /// </summary>
        /// <returns>The tokenized version of the current article.</returns>
        public List<Token> preprocess()
        {
            if (articleCurrent == null)
            {
                return null;
            }

            listLatestTokenizedArticle = new List<Token>();

            performTokenizationAndSS();
            performNER();
            performPOST();
            performWS();
            performCandidateSelection();

            foreach (var token in listLatestTokenizedArticle)
            {
                //System.Console.WriteLine("Value: " + token.Value);
                //System.Console.WriteLine("Sentence: " + token.Sentence);
                //System.Console.WriteLine("Position: " + token.Position);
                //System.Console.WriteLine("NER: " + token.NamedEntity);
                //System.Console.WriteLine("POS: " + token.PartOfSpeech);
                //System.Console.WriteLine("WS: " + token.Frequency);
                //System.Console.WriteLine("=====\n");
            }

            return listLatestTokenizedArticle;
        }

        /// <summary>
        /// Performs the candidate selection.
        /// </summary>
        private void performCandidateSelection()
        {
            CandidateSelector selector = new CandidateSelector();
            listWhoCandidates = selector.performWhoCandidateSelection(listLatestTokenizedArticle, articleCurrent.Title);
            listWhenCandidates = selector.performWhenCandidateSelection(listLatestTokenizedArticle, articleCurrent.Title);
            listWhereCandidates = selector.performWhereCandidateSelection(listLatestTokenizedArticle, articleCurrent.Title);
            listWhatCandidates = selector.performWhatCandidateSelection(listLatestTokenizedArticle, articleCurrent.Title);
            listWhyCandidates = selector.performWhyCandidateSelection(listLatestTokenizedArticle, articleCurrent.Title);
        }

        #region Article Preprocessing Functions
        /// <summary>
        /// Performs the tokenization and sentence segmentation on the current article's body.
        /// </summary>
        private void performTokenizationAndSS()
        {
            listLatestTokenizedArticle = performTokenizationAndSS(articleCurrent.Body);
        }

        /// <summary>
        /// Performs the tokenization and sentence segmentation on the <c>String</c> parameter.
        /// </summary>
        /// <param name="toBeTokenized">String to be tokenized.</param>
        /// <returns>Tokenized version of passed string.</returns>
        public List<Token> performTokenizationAndSS(String toBeTokenized)
        {
            List<Token> tokenizedString = new List<Token>();
            var sentences = MaxentTagger.tokenizeText(new java.io.StringReader(toBeTokenized)).toArray();
            int sentenceCounter = 1;
            int positionCounter = 1;
            String[] abbreviationList = new String[] {
            "Dr", //Names
            "Dra",
            "Gng",
            "G",
            "Gg",
            "Bb",
            "Esq",
            "Jr",
            "Mr",
            "Mrs",
            "Ms",
            "Messrs",
            "Mmes",
            "Msgr",
            "Prof",
            "Rev",
            "Pres",
            "Sec",
            "Sr",
            "Fr",
            "St",
            "Hon",
            "Ave", //Streets
            "Aly",
            "Gen", //Military Rank
            "1Lt",
            "2Lt",
            "Cpt",
            "Maj",
            "Capt",
            "1stLt",
            "2ndLt",
            "Adm",
            "W01",
            "CW2",
            "CW3",
            "CW4",
            "CW5",
            "Col",
            "LtCol",
            "BG",
            "MG",
            "Sgt",
            "SSgt",
            "LCpl",
            "SgtMaj",
            "1stSgt",
            "1Sgt",
            "Pvt"
            };
            foreach (java.util.ArrayList sentence in sentences)
            {
                String wordFinal = "";
                foreach (var word in sentence)
                {
                    var newToken = new Token(word.ToString(), positionCounter);
                    newToken.Sentence = sentenceCounter;
                    tokenizedString.Add(newToken);
                    positionCounter++;
                    if (!newToken.Value.Equals("."))
                        wordFinal = word.ToString();
                }
                Boolean flag = true;
                foreach (String word in abbreviationList)
                {
                    if (wordFinal.Equals(word))
                        flag = false;
                }
                if (flag)
                    sentenceCounter++;
            }
            return tokenizedString;
        }

        /// <summary>
        /// Performs NER tagging of tokens.
        /// </summary>
        private void performNER()
        {
            java.util.List tokens;
            List<string> values = new List<string>();
            object[] nerValues;

            foreach (Token token in listLatestTokenizedArticle)
            {
                values.Add(token.Value);
            }

            tokens = Sentence.toCoreLabelList(values.ToArray());

            nerValues = nerClassifier.classifySentence(tokens).toArray();

            for (int i = 0; i < listLatestTokenizedArticle.Count; i++)
            {
                listLatestTokenizedArticle[i].NamedEntity = ((CoreLabel)nerValues[i]).get(typeof(CoreAnnotations.AnswerAnnotation)).ToString();
            }
        }

        /// <summary>
        /// Performs POS tagging of tokens.
        /// </summary>
        private void performPOST()
        {
            //Get all tokens and segregate them into lists based on sentence number
            List<List<Token>> segregatedTokenLists = listLatestTokenizedArticle
                .GroupBy(token => token.Sentence)
                .Select(tokenGroup => tokenGroup.ToList())
                .ToList();

            //Convert the lists into a "CoreLabelList" and store in a Dictionary
            //Dictionary Key: Sentence Number
            //Dictionary Value: CoreLabelList
            Dictionary<int, java.util.List> tokenizedSentenceLists = new Dictionary<int, java.util.List>();
            foreach (List<Token> tokenList in segregatedTokenLists)
            {
                if (tokenList.Count > 0)
                {
                    var tokenToStringArray = tokenList.Select(token => token.Value).ToArray();
                    tokenizedSentenceLists[tokenList[0].Sentence] = Sentence.toCoreLabelList(tokenToStringArray);
                }
            }

            //Tag each sentence
            foreach (KeyValuePair<int, java.util.List> entry in tokenizedSentenceLists)
            {
                var taggedSentence = posTagger.tagSentence(entry.Value).toArray();
                foreach (var word in taggedSentence)
                {
                    var splitWord = word.ToString().Split('/');
                    if (splitWord.Length >= 2)
                    {
                        foreach (var token in listLatestTokenizedArticle)
                        {
                            if ((token.PartOfSpeech == null || token.PartOfSpeech.Length <= 0) &&
                                token.Value.Trim() == splitWord[0].Trim() &&
                                token.Sentence == entry.Key)
                            {
                                token.PartOfSpeech = splitWord[1];
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Performs Word Scoring of Tokens.
        /// </summary>
        private void performWS()
        {
            Dictionary<string, int> frequencies = new Dictionary<string, int>();

            foreach (Token token in listLatestTokenizedArticle)
            {
                if (frequencies.ContainsKey(token.Value))
                {
                    frequencies[token.Value]++;
                }
                else
                {
                    frequencies[token.Value] = 1;
                }
            }

            foreach (Token token in listLatestTokenizedArticle)
            {
                token.Frequency = frequencies[token.Value];
            }
        }
        #endregion
    }
}
