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
    public class DisplayArticle
    {
        /// <summary>
        /// Gets or sets the article.
        /// </summary>
        /// <value>
        /// The article.
        /// </value>
        public Article Article { get; set; }

        /// <summary>
        /// Gets or sets the annotation.
        /// </summary>
        /// <value>
        /// The annotation.
        /// </value>
        public Annotation Annotation { get; set; }
    }
}
