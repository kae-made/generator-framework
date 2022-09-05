using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kae.Tools.Generator.Coloring.DomainWeaving
{
    public class ColoringForDomainSpec
    {
        public string DomainName { get; set; }
        public IDictionary<string, ColoringSpec> Specifications { get; set; }
    }

}
