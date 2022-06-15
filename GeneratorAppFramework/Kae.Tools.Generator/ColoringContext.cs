using System;
using System.Collections.Generic;
using System.Text;

namespace Kae.Tools.Generator
{
    public class ColoringContext
    {
        public string Key { get; set; }
        private IDictionary<string,string> @params = new Dictionary<string,string>();
        public IDictionary<string, string> Params { get { return @params; } }
    }
}
