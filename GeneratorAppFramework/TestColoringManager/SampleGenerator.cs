using Kae.CIM.MetaModel.CIMofCIM;
using Kae.Tools.Generator;
using Kae.Tools.Generator.Coloring;
using Kae.Utility.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestColoringManager
{
    public class SampleGenerator : GeneratorBase
    {
        public SampleGenerator(Logger logger):base(logger)
        {

        }
        protected override bool AdditionalWorkForDomainModels()
        {
            return true;
        }

        protected override bool AdditionalWorkForMetaModel()
        {
            return true;
        }

        protected override bool AdditionalWorkForResloveContext()
        {
            return true;
        }

        protected override void CreateAdditionalContext()
        {
            
        }

        static readonly string baseIndent = "    ";
        protected override void GenerateContentsYourOwn()
        {
            var modelRepository = modelResolver.ModelRepository;
            var classDefs = modelRepository.GetCIInstances(DomainName, "O_OBJ");
            foreach(var classDef in classDefs)
            {
                var objDef = (CIMClassO_OBJ)classDef;
                var tfrDefs = objDef.LinkedFromR115();
                foreach(var tfrDef in tfrDefs)
                {
                    var opbDef = tfrDef.LinkedFromR696();
                    if (opbDef != null)
                    {
                        var actDef = opbDef.CIMSuperClassACT_ACT();
                        if (coloringManager != null)
                        {
                            IList<ColoringForInstance> colors;
                            if (coloringManager.HasColoring(actDef, out colors))
                            {
                                var generations = coloringManager.GenerateForColoring(colors, "    ", baseIndent);
                            }
                        }
                    }
                }
            }
        }

        protected override bool GenerateEnvironmentYourOwn()
        {
            return true;
        }
    }
}
