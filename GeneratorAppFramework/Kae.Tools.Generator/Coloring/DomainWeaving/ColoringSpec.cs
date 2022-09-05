using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kae.Tools.Generator.Coloring.DomainWeaving
{
    public class ColoringSpec
    {
        public ColoringForDomainSpec DomainSpecification { get; set; }
        public string Name { get; set; }
        public string Target { get; set; }
        public IDictionary<string, ColoringParameterSpec> Parameters { get; set; }
    }
}