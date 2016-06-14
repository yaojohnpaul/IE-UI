using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IE_lib.Models
{
    public class ParsedResults
    {
        public string FilePath { get; set; }

        public List<DisplayArticle> ListDisplayArticles{ get; set; }

        public Dictionary<string, List<int>> WhoReverseIndex { get; set; }

        public Dictionary<string, List<int>> WhenReverseIndex { get; set; }

        public Dictionary<string, List<int>> WhereReverseIndex { get; set; }

        public Dictionary<string, List<int>> WhatReverseIndex { get; set; }

        public Dictionary<string, List<int>> WhyReverseIndex { get; set; }
    }
}
