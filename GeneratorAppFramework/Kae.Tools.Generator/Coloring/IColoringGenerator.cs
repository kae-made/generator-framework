using Kae.CIM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kae.Tools.Generator.Coloring
{
    public interface IColoringGenerator
    {
        public ColoringForDomainSpec GetColoringForDomainSpec();

        public void FixTarget(ColoringForInstance coloring, CIModelRepository modelRepository, string domainNameOfUser, IDictionary<string,string> targetIdentities);

        public bool IsTarget(ColoringForInstance coloring, CIClassDef target);
        public GenerationResult Generate(ColoringForInstance coloring, string indent, string baseIndent);
    }

    public class GenerationResult
    {
        public enum Style
        {
            None = 0,
            Overwrite = 1,
            AddBefore = 2,
            AddBeforeAndOverwrite = 3,
            AddAfter = 4,
            OverwriteAndAddAfter = 5,
            AddBeforeAndAddAfter = 6,
            OverwriteAnddAddBoth = 7
        }
        public Style AddStyle { get; set; }
        public string CodeBefore { get; set; }
        public string CodeOverwrite { get; set; }
        public string CodeAfter { get; set; }
    }
}
