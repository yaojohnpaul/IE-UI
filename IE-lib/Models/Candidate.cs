using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IE_lib.Models
{
    /// <summary>
    /// A model class for a 5W candidate which extends the <see cref="IE_lib.Models.Token"/> class.
    /// </summary>
    /// <seealso cref="IE_lib.Models.Token" />
    public class Candidate : Token
    {
        /// <summary>
        /// Gets or sets the length of the candidate.
        /// </summary>
        /// <value>
        /// The length of the candidate.
        /// </value>
        public int Length { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Candidate"/> class.
        /// </summary>
        /// <param name="value">The value of the candidate.</param>
        /// <param name="pos">The position of the candidate.</param>
        /// <param name="length">The length of the candidate.</param>
        public Candidate(String value, int pos, int length) : base(value, pos)
        {
            this.Length = length;
        }
    }
}
