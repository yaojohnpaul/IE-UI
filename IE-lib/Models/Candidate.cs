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
    /// <seealso cref="IE_lib.Models.Token" />
    public class Candidate : Token
    {
        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        public int Length { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Candidate"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="pos">The position.</param>
        /// <param name="length">The length.</param>
        public Candidate(String value, int pos, int length) : base(value, pos)
        {
            this.Length = length;
        }
    }
}
