using IE_lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IE_UI.Models
{
    /// <summary>
    /// Class for holding information about a single article appearing in the search list.
    /// </summary>
    public class ListArticle
    {
        /// <summary>
        /// Gets or sets the display article.
        /// </summary>
        /// <value>
        /// The display article.
        /// </value>
        public DisplayArticle DisplayArticle { get; set; }

        /// <summary>
        /// Gets or sets the matched string.
        /// </summary>
        /// <value>
        /// The matched string.
        /// </value>
        public string MatchedString { get; set; }
    }
}
