using Kae.CIM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kae.Tools.Generator.Coloring.DomainWeaving
{
    public class ColoringForInstance
    {
        public ColoringSpec Specification { get; set; }
        public CIClassDef Target { get; set; }
        public IDictionary<string, string> EmbellishParameters { get; set; }
    }
}
