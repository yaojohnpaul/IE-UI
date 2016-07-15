using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IE_lib.Models
{
    public class Annotation
    {
        public int Index { get; set; }

        public string Who { get; set; }

        public string Where { get; set; }

        public string When { get; set; }

        public string What { get; set; }

        public string Why { get; set; }

        public string FormattedWhen { get; set; }
    }
}
