using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IE_lib.Models
{
    /// <summary>
    /// Container class for passing information about a group of extracted data.
    /// </summary>
    public class ParsedResults
    {
        /// <summary>
        /// Gets or sets the file path of the data XML file.
        /// </summary>
        /// <value>
        /// The file path of the data XML file.
        /// </value>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the list of articles included in this extracted batch of data.
        /// </summary>
        /// <value>
        /// The list of articles included in this extracted batch of data.
        /// </value>
        public List<DisplayArticle> ListDisplayArticles{ get; set; }

        /// <summary>
        /// Gets or sets the dictionary of extracted who strings to its corresponding article indices.
        /// </summary>
        /// <value>
        /// The dictionary of extracted who strings to its corresponding article indices.
        /// </value>
        public Dictionary<string, List<int>> WhoReverseIndex { get; set; }

        /// <summary>
        /// Gets or sets the dictionary of extracted when strings to its corresponding article indices.
        /// </summary>
        /// <value>
        /// The dictionary of extracted when strings to its corresponding article indices.
        /// </value>
        public Dictionary<string, List<int>> WhenReverseIndex { get; set; }

        /// <summary>
        /// Gets or sets the dictionary of extracted where strings to its corresponding article indices.
        /// </summary>
        /// <value>
        /// The dictionary of extracted where strings to its corresponding article indices.
        /// </value>
        public Dictionary<string, List<int>> WhereReverseIndex { get; set; }

        /// <summary>
        /// Gets or sets the dictionary of extracted what strings to its corresponding article indices.
        /// </summary>
        /// <value>
        /// The dictionary of extracted what strings to its corresponding article indices.
        /// </value>
        public Dictionary<string, List<int>> WhatReverseIndex { get; set; }

        /// <summary>
        /// Gets or sets the dictionary of extracted why strings to its corresponding article indices.
        /// </summary>
        /// <value>
        /// The dictionary of extracted why strings to its corresponding article indices.
        /// </value>
        public Dictionary<string, List<int>> WhyReverseIndex { get; set; }
    }
}
