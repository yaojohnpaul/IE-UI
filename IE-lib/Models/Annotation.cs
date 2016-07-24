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
    public class Annotation
    {
        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the who.
        /// </summary>
        /// <value>
        /// The who.
        /// </value>
        public string Who { get; set; }

        /// <summary>
        /// Gets or sets the where.
        /// </summary>
        /// <value>
        /// The where.
        /// </value>
        public string Where { get; set; }

        /// <summary>
        /// Gets or sets the when.
        /// </summary>
        /// <value>
        /// The when.
        /// </value>
        public string When { get; set; }

        /// <summary>
        /// Gets or sets the what.
        /// </summary>
        /// <value>
        /// The what.
        /// </value>
        public string What { get; set; }

        /// <summary>
        /// Gets or sets the why.
        /// </summary>
        /// <value>
        /// The why.
        /// </value>
        public string Why { get; set; }

        /// <summary>
        /// Gets or sets the formatted when.
        /// </summary>
        /// <value>
        /// The formatted when.
        /// </value>
        public string FormattedWhen { get; set; }
    }
}
