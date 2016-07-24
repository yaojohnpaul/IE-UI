    
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IE_lib.Models
{
    /// <summary>
    /// Class for holding information about an individual article's token entity.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// All possible named entity tags.
        /// </summary>
        public static readonly String[] NamedEntityTags = {
            "PER", "LOC", "DATE", "ORG"
        };

        /// <summary>
        /// All possible part-of-speech tags.
        /// </summary>
        public static readonly String[] PartOfSpeechTags = {
            "PRC", "CDB", "PP", "PPIN", "LM", "NNC", "VBTR", "JJCC", "JJC", "PR", "VBTS", "JJD", "PRF", "PRI", "JJCN", "PRL", "PRO", "PRN", "PRQ", "JJN", "PRP", "PRS", "DT", "NNP", "JJCS", "RBC", "RBB", "RBD", "NNPA", "RBF", "RBI", "RBK", "RBM", "VBH", "RBL", "RBN", "RBQ", "RBP", "VBL", "RBR", "VBN", "RBT", "RBW", "VBS", "VBW", "VBOF", "VB", "DTPP", "RB", "DTC", "VBTF", "NN", "JJ", "PPA", "DTP", "PPD", "PROP", "PPF", "VBRF", "PPM", "PPL", "PPO", "PPR", "PPU", "PPTS", "DTCP", "CCA", "CC", "CD", "CCC", "CCB", "CCD", "PMC", "PME", "CCP", "PMP", "PPBY", "CCR", "CCT", "PMQ", "PMS", "VBAF", "PRSP", "PM"
        };

        /// <summary>
        /// Gets or sets the value of the token.
        /// </summary>
        /// <value>
        /// The value of the token.
        /// </value>
        public String Value { get; set; }

        /// <summary>
        /// Gets or sets the sentence number where the token can be found.
        /// </summary>
        /// <value>
        /// The sentence number where the token can be found.
        /// </value>
        public int Sentence { get; set; }

        /// <summary>
        /// Gets or sets the position of the token in the article.
        /// </summary>
        /// <value>
        /// The position of the token in the article.
        /// </value>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets the part of speech of the token.
        /// </summary>
        /// <value>
        /// The part of speech of the token.
        /// </value>
        public String PartOfSpeech { get; set; }

        /// <summary>
        /// Gets or sets the named entity of the token.
        /// </summary>
        /// <value>
        /// The named entity of the token.
        /// </value>
        public String NamedEntity { get; set; }

        /// <summary>
        /// Gets or sets the frequency of the token in the article.
        /// </summary>
        /// <value>
        /// The frequency of the token in the article.
        /// </value>
        public int Frequency { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this token is a correct who.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this token is a correct who; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsWho { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this token is a correct when.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this token is a correct when; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsWhen { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this token is a correct where.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this token is a correct where; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsWhere { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this token is a correct what.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this token isa correct what; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsWhat { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this token is a correct why.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this token is a correct why; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsWhy { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="value">The value of the token.</param>
        /// <param name="position">The position of the token.</param>
        public Token(String value, int position)
        {
            Value = value;
            Position = position;
        }

        /// <summary>
        /// Gets or sets the score of the token (if applicable during extracting).
        /// </summary>
        /// <value>
        /// The score of the token.
        /// </value>
        public double Score { get; set; }

        /// <summary>
        /// Gets or sets the number who present in the token.
        /// </summary>
        /// <value>
        /// The number who present in the token.
        /// </value>
        public int NumWho { get; set; }

        /// <summary>
        /// Gets or sets the number when present in the token.
        /// </summary>
        /// <value>
        /// The number when present in the token.
        /// </value>
        public int NumWhen { get; set; }

        /// <summary>
        /// Gets or sets the number where present in the token.
        /// </summary>
        /// <value>
        /// The number where present in the token.
        /// </value>
        public int NumWhere { get; set; }
    }
}
