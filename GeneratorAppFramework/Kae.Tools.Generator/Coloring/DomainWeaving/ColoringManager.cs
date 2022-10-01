using Kae.CIM;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kae.Tools.Generator.Coloring.DomainWeaving
{
    public class ColoringManager
    {
        protected Dictionary<string, ColoringForDomainSpec> coloringForDomainSpecs = new Dictionary<string, ColoringForDomainSpec>();
        protected Dictionary<string, IColoringGenerator> coloringGenerators = new Dictionary<string, IColoringGenerator>();
        protected List<ColoringForInstance> coloringSettings = new List<ColoringForInstance>();

        protected CIModelRepository modelRepository;

        bool available = false;

        public bool Available { get { return available && coloringSettings.Count > 0; } }

        public ColoringManager(CIModelRepository repository)
        {
            modelRepository = repository;
        }

        public bool Resolve(string domainNameOfUser, string setting)
        {
            dynamic settingJson = Newtonsoft.Json.JsonConvert.DeserializeObject(setting);
            dynamic coloringForDomains = settingJson.ColoringsForDomain;
            foreach (dynamic coloring in coloringForDomains)
            {
                string domainName = coloring.domainname;
                string dllFilePath = coloring.dllfilepath;
                var generator = LoadGenerator(dllFilePath);
                coloringGenerators.Add(domainName, generator);
                coloringForDomainSpecs.Add(domainName, generator.GetColoringForDomainSpec());
            }
            dynamic colorings = settingJson.Coloring;
            foreach (dynamic coloring in colorings)
            {
                string domainName = coloring.domainname;
                if (coloringForDomainSpecs.ContainsKey(domainName))
                {
                    string coloringName = coloring.coloringname;
                    if (coloringForDomainSpecs[domainName].Specifications.ContainsKey(coloringName))
                    {
                        var newColor = new ColoringForInstance() { Specification = coloringForDomainSpecs[domainName].Specifications[coloringName], EmbellishParameters = new Dictionary<string, string>() };
                        var targetParams = new Dictionary<string, string>();
                        foreach (var targetParam in coloring.target)
                        {
                            var jPropParam = (JProperty)targetParam;
                            string targetParamName = jPropParam.Name;
                            string targetParamValue = jPropParam.Value.ToString();
                            targetParams.Add(targetParamName, targetParamValue);
                        }
                        foreach (var coloringParam in coloring.parameters)
                        {
                            var jPropParam = (JProperty)coloringParam;
                            string colorParamName = jPropParam.Name;
                            string colorParamValue = jPropParam.Value.ToString();
                            newColor.EmbellishParameters.Add(colorParamName, colorParamValue);
                        }
                        coloringSettings.Add(newColor);
                        if (coloringGenerators.ContainsKey(domainName))
                        {
                            var generator = coloringGenerators[domainName];
                            generator.FixTarget(newColor, modelRepository, domainNameOfUser, targetParams);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            available = true;
            return true;
        }

        public IList<string> GetExternalPackages()
        {
            var packages = new List<string>();

            foreach(var generator in coloringGenerators)
            {
                var ps = generator.Value.GetColoringForDomainSpec().ExternalPackages;
                foreach(var p in ps)
                {
                    var r = packages.Where(i => i == p).FirstOrDefault();
                    if (r == null)
                    {
                        packages.Add(p);
                    }
                }
            }

            return packages;
        }

        protected IColoringGenerator LoadGenerator(string dllFilePath)
        {
            var assembry = Assembly.LoadFrom(dllFilePath);
            var loadedModles = assembry.GetLoadedModules();
            if (loadedModles.Length > 0)
            {
                var module = loadedModles[0];
                string ns = module.Name.Substring(0, module.Name.LastIndexOf(".dll"));
                var typeOfEntry = module.GetType($"{ns}.ColoringGeneratorEntry");
                if (typeOfEntry != null)
                {
                    var methodOfEntry = typeOfEntry.GetMethod("GetGenerator");
                    if (methodOfEntry != null)
                    {
                        return methodOfEntry.Invoke(null, new object[] { }) as IColoringGenerator;
                    }
                }
            }
            return null;
        }

        public bool HasColoring(CIClassDef target, out IList<ColoringForInstance> coloring)
        {
            coloring = new List<ColoringForInstance>();
            foreach (var coloringSetting in coloringSettings)
            {
                var generator = coloringGenerators[coloringSetting.Specification.DomainSpecification.DomainName];
                if (generator.IsTarget(coloringSetting, target))
                {
                    coloring.Add(coloringSetting);
                }
            }

            return coloring.Count > 0 ? true : false;
        }

        public IList<GenerationResult> GenerateForColoring(IList<ColoringForInstance> colorings, string indent, string baseIndent)
        {
            var generations = new List<GenerationResult>();
            foreach (var color in colorings)
            {
                generations.Add(coloringGenerators[color.Specification.DomainSpecification.DomainName].Generate(color, indent, baseIndent));
            }
            return generations;
        }

    }
}
