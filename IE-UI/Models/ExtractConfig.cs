using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IE_UI.Models
{
    /// <summary>
    /// Class for holding information about the source and destination paths for extraction.
    /// </summary>
    public class ExtractConfig
    {
        /// <summary>
        /// Gets or sets the source file path.
        /// </summary>
        /// <value>
        /// The source file path.
        /// </value>
        public string SourceFilePath { get; set; }

        /// <summary>
        /// Gets or sets the destination file path.
        /// </summary>
        /// <value>
        /// The destination file path.
        /// </value>
        public string DestinationFilePath { get; set; }
    }
}
