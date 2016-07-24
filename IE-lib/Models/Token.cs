    
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IE_lib.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Token
    {
        /// <summary>
        /// The named entity tags
        /// </summary>
        public static readonly String[] NamedEntityTags = {
            "PER", "LOC", "DATE", "ORG"
        };

        /// <summary>
        /// The part of speech tags
        /// </summary>
        public static readonly String[] PartOfSpeechTags = {
            "PRC", "CDB", "PP", "PPIN", "LM", "NNC", "VBTR", "JJCC", "JJC", "PR", "VBTS", "JJD", "PRF", "PRI", "JJCN", "PRL", "PRO", "PRN", "PRQ", "JJN", "PRP", "PRS", "DT", "NNP", "JJCS", "RBC", "RBB", "RBD", "NNPA", "RBF", "RBI", "RBK", "RBM", "VBH", "RBL", "RBN", "RBQ", "RBP", "VBL", "RBR", "VBN", "RBT", "RBW", "VBS", "VBW", "VBOF", "VB", "DTPP", "RB", "DTC", "VBTF", "NN", "JJ", "PPA", "DTP", "PPD", "PROP", "PPF", "VBRF", "PPM", "PPL", "PPO", "PPR", "PPU", "PPTS", "DTCP", "CCA", "CC", "CD", "CCC", "CCB", "CCD", "PMC", "PME", "CCP", "PMP", "PPBY", "CCR", "CCT", "PMQ", "PMS", "VBAF", "PRSP", "PM"
        };

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public String Value { get; set; }

        /// <summary>
        /// Gets or sets the sentence.
        /// </summary>
        /// <value>
        /// The sentence.
        /// </value>
        public int Sentence { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets the part of speech.
        /// </summary>
        /// <value>
        /// The part of speech.
        /// </value>
        public String PartOfSpeech { get; set; }

        /// <summary>
        /// Gets or sets the named entity.
        /// </summary>
        /// <value>
        /// The named entity.
        /// </value>
        public String NamedEntity { get; set; }

        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        /// <value>
        /// The frequency.
        /// </value>
        public int Frequency { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is who.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is who; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsWho { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is when.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is when; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsWhen { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is where.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is where; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsWhere { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is what.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is what; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsWhat { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is why.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is why; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsWhy { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="position">The position.</param>
        public Token(String value, int position)
        {
            Value = value;
            Position = position;
        }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        public double Score { get; set; }

        /// <summary>
        /// Gets or sets the number who.
        /// </summary>
        /// <value>
        /// The number who.
        /// </value>
        public int NumWho { get; set; }

        /// <summary>
        /// Gets or sets the number when.
        /// </summary>
        /// <value>
        /// The number when.
        /// </value>
        public int NumWhen { get; set; }

        /// <summary>
        /// Gets or sets the number where.
        /// </summary>
        /// <value>
        /// The number where.
        /// </value>
        public int NumWhere { get; set; }
    }
}
