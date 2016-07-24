using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IE_lib.Models
{
    /// <summary>
    /// A model class for an individual article.
    /// </summary>
    public class Article
    {
        /// <summary>
        /// Gets or sets the article's title.
        /// </summary>
        /// <value>
        /// The article's title.
        /// </value>
        public String Title { get; set; }

        /// <summary>
        /// Gets or sets the article's author.
        /// </summary>
        /// <value>
        /// The article's author.
        /// </value>
        public String Author { get; set; }

        /// <summary>
        /// Gets or sets the article's date.
        /// </summary>
        /// <value>
        /// The article's date.
        /// </value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the article's link.
        /// </summary>
        /// <value>
        /// The article's link.
        /// </value>
        public String Link { get; set; }

        /// <summary>
        /// Gets or sets the article's body.
        /// </summary>
        /// <value>
        /// The article's body.
        /// </value>
        public String Body { get; set; }
    }
}
