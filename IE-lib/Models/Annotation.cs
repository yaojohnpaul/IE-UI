using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IE_lib.Models
{
    /// <summary>
    /// Model class holding annotation data for an article.
    /// </summary>
    public class Annotation
    {
        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>
        /// The index of the article that this annotation belongs to.
        /// </value>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the who annotation.
        /// </summary>
        /// <value>
        /// The who annotation.
        /// </value>
        public string Who { get; set; }

        /// <summary>
        /// Gets or sets the where annotation.
        /// </summary>
        /// <value>
        /// The where annotation.
        /// </value>
        public string Where { get; set; }

        /// <summary>
        /// Gets or sets the when annotation.
        /// </summary>
        /// <value>
        /// The when annotation.
        /// </value>
        public string When { get; set; }

        /// <summary>
        /// Gets or sets the what annotation.
        /// </summary>
        /// <value>
        /// The what annotation.
        /// </value>
        public string What { get; set; }

        /// <summary>
        /// Gets or sets the why annotation.
        /// </summary>
        /// <value>
        /// The why annotation.
        /// </value>
        public string Why { get; set; }

        /// <summary>
        /// Gets or sets the formatted when annotation.
        /// </summary>
        /// <value>
        /// The formatted when annotation.
        /// </value>
        public string FormattedWhen { get; set; }
    }
}
