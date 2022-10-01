using Kae.CIM;
using Kae.CIM.MetaModel.CIMofCIM;
using Kae.Tools.Generator.Coloring;
using Kae.Tools.Generator.Coloring.DomainWeaving;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SampleColoringGenerator
{
    public static class ColoringGeneratorEntry
    {
        public static IColoringGenerator GetGenerator() { return new SampleColoringGenerator(); }
    }

    public class SampleColoringGenerator : IColoringGenerator
    {
        protected static readonly string domainName = "iot-device";

        protected ColoringForDomainSpec coloringForDomainSpec;
        List<ColoringForInstance> myColoring = new List<ColoringForInstance>();

        public SampleColoringGenerator()
        {
            coloringForDomainSpec = new ColoringForDomainSpec() { DomainName = domainName, Specifications = new Dictionary<string, ColoringSpec>(), ExternalPackages = new List<string>() };

            var bindToCommandColor = new ColoringSpec()
            {
                Name = "bind-to-command",
                Parameters = new Dictionary<string, ColoringParameterSpec>(),
                Target = "ACT_ACT of O_TFR",
                DomainSpecification = coloringForDomainSpec
            };
            coloringForDomainSpec.Specifications.Add("bind-to-command", bindToCommandColor);
            bindToCommandColor.Parameters.Add("class-key-letter", new ColoringParameterSpec() { Name = "class-key-letter", IsIdentity = true });
            bindToCommandColor.Parameters.Add("operation-name", new ColoringParameterSpec() { Name = "operation-name", IsIdentity = true });
            bindToCommandColor.Parameters.Add("device-type", new ColoringParameterSpec() { Name = "device-type", IsIdentity = false });

        }
        public void FixTarget(ColoringForInstance coloring, CIModelRepository modelRepository, string domainNameOfUser, IDictionary<string, string> targetIdentities)
        {
            var ciObjDef = modelRepository.GetCIInstances(domainNameOfUser, "O_OBJ").Where(selected => (((CIMClassO_OBJ)selected).Attr_Key_Lett == targetIdentities["class-key-letter"])).FirstOrDefault();
            if (ciObjDef != null)
            {
                var objDef = (CIMClassO_OBJ)ciObjDef;
                var tfrDef = objDef.LinkedFromR115().Where(selcted => (selcted.Attr_Name == targetIdentities["operation-name"])).FirstOrDefault();
                if (tfrDef != null)
                {
                    var opbDef = tfrDef.LinkedFromR696();
                    if (opbDef != null)
                    {
                        coloring.Target = opbDef.CIMSuperClassACT_ACT();
                    }
                }
            }
            myColoring.Add(coloring);
        }

        public GenerationResult Generate(ColoringForInstance coloring, string indent, string baseIndent)
        {

            var opbDef = ((CIMClassACT_ACT)coloring.Target).SubClassR698() as CIMClassACT_OPB;
            var tfrDef = opbDef.LinkedToR696();
            var tparmDefs = tfrDef.LinkedFromR117();
            string comment = "";
            foreach(var  tparmDef in tparmDefs)
            {
                if (!string.IsNullOrEmpty(comment))
                {
                    comment += ", ";
                }
                comment += $"parameter:{tparmDef.Attr_Name}";
            }
            var sbBody = new StringBuilder();
            var writerBody = new StringWriter(sbBody);
            //writerBody.WriteLine($"{indent}// send command to device by {comment}...");

            var sbBefore = new StringBuilder();
            var writerBefore = new StringWriter(sbBefore);
            writerBefore.WriteLine($"{indent}// before code to send command...");

            var sbAfter = new StringBuilder();
            var writerAfter = new StringWriter(sbAfter);
            writerAfter.WriteLine($"{indent}// after code to send command...");


            string codeBefore = sbBefore.ToString();
            string codeBody = sbBody.ToString();
            string codeAfter = sbAfter.ToString();

            var addStyle = GenerationResult.Style.None;
            if (!string.IsNullOrEmpty(codeBefore))
            {
                addStyle |= GenerationResult.Style.AddBefore;
            }
            if (!string.IsNullOrEmpty(codeBody))
            {
                addStyle |= GenerationResult.Style.Overwrite;
            }
            if (!string.IsNullOrEmpty(codeAfter))
            {
                addStyle |= GenerationResult.Style.AddAfter;
            }

            return new GenerationResult() { AddStyle = addStyle, CodeOverwrite = codeBody, CodeBefore = codeBefore, CodeAfter = codeAfter };
        }

        public ColoringForDomainSpec GetColoringForDomainSpec()
        {
            return coloringForDomainSpec;
        }

        public bool IsTarget(ColoringForInstance coloring, CIClassDef target)
        {
            if (coloring.Specification.Name== "bind-to-command")
            {
                if (((CIMClassACT_ACT)coloring.Target).Attr_Action_ID == ((CIMClassACT_ACT)target).Attr_Action_ID)
                {
                    return true; 
                }
            }
            return false;
        }
    }
}
