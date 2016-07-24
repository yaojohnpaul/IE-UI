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
    public class ParsedResults
    {
        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the list display articles.
        /// </summary>
        /// <value>
        /// The list display articles.
        /// </value>
        public List<DisplayArticle> ListDisplayArticles{ get; set; }

        /// <summary>
        /// Gets or sets the index of the who reverse.
        /// </summary>
        /// <value>
        /// The index of the who reverse.
        /// </value>
        public Dictionary<string, List<int>> WhoReverseIndex { get; set; }

        /// <summary>
        /// Gets or sets the index of the when reverse.
        /// </summary>
        /// <value>
        /// The index of the when reverse.
        /// </value>
        public Dictionary<string, List<int>> WhenReverseIndex { get; set; }

        /// <summary>
        /// Gets or sets the index of the where reverse.
        /// </summary>
        /// <value>
        /// The index of the where reverse.
        /// </value>
        public Dictionary<string, List<int>> WhereReverseIndex { get; set; }

        /// <summary>
        /// Gets or sets the index of the what reverse.
        /// </summary>
        /// <value>
        /// The index of the what reverse.
        /// </value>
        public Dictionary<string, List<int>> WhatReverseIndex { get; set; }

        /// <summary>
        /// Gets or sets the index of the why reverse.
        /// </summary>
        /// <value>
        /// The index of the why reverse.
        /// </value>
        public Dictionary<string, List<int>> WhyReverseIndex { get; set; }
    }
}
