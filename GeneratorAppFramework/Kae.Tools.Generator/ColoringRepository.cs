using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;

namespace Kae.Tools.Generator
{
    public class ColoringRepository
    {
        private Dictionary<string, IList<ColoringContext>> colors;
        public IDictionary<string, IList<ColoringContext>> Colors { get { return colors; } }

        public ColoringRepository()
        {
            colors = new Dictionary<string, IList<ColoringContext>>();
        }

        public ColoringContext Parse(string key, string content)
        {
            ColoringContext coloringContext = null;

            using (var reader = new StringReader(content))
            {
                var line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("@") && line.EndsWith(";"))
                    {
                        string colorStatement = line.Substring(1, line.Length - 2);
                        var frags = colorStatement.Split(new char[] { ':' });
                        if (frags.Length == 2)
                        {
                            string ckey = frags[0];
                            coloringContext = new ColoringContext() { Key = ckey };
                            var cfrags = frags[1].Split(new char[] { ',' });
                            foreach (var c in cfrags)
                            {
                                var ecfrags = c.Split(new char[] { '=' });
                                if (ecfrags.Length == 2)
                                {
                                    coloringContext.Params.Add(ecfrags[0], ecfrags[1]);
                                }
                            }

                        }
                        if (!colors.ContainsKey(key))
                        {
                            colors.Add(key, new List<ColoringContext>());
                        }
                        colors[key].Add(coloringContext);
                    }
                }
            }

            return coloringContext;
        }

        public async Task Load(string filePath)
        {
            if (filePath.EndsWith(".yaml"))
            {
                await LoadYaml(filePath);
            }
            else if (filePath.EndsWith(".csv"))
            {
                await LoadCsv(filePath);
            }
            else if (filePath.EndsWith(".json"))
            {
                await LoadJson(filePath);
            }
        }

        public async Task LoadYaml(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                var yaml = new YamlStream();
                yaml.Load(reader);
                var root = (YamlMappingNode)yaml.Documents[0].RootNode;
                foreach(var item in root.Children)
                {
                    string key = ((YamlScalarNode)item.Key).Value;
                    IList<ColoringContext> target = null;
                    if (colors.ContainsKey(key))
                    {
                        target = colors[key];
                    }
                    else
                    {
                        target = new List<ColoringContext>();
                        colors.Add(key, target);
                    }
                    foreach(var citem in ((YamlMappingNode)item.Value).Children)
                    {
                        var cname= ((YamlScalarNode)citem.Key).Value;
                        var newContext = new ColoringContext() { Key = cname };
                        target.Add(newContext);
                        foreach(var param in ((YamlMappingNode)citem.Value).Children)
                        {
                            var pname = ((YamlScalarNode)param.Key).Value;
                            var pvalue = ((YamlScalarNode)param.Value).Value;
                            newContext.Params.Add(pname, pvalue);
                        }

                    }
                }
            }
        }

        public async Task LoadCsv(string filePath)
        {
            using(var stream = File.OpenRead(filePath))
            {
                using (var reader = new StreamReader(stream))
                {
                    IList<ColoringContext> current = null;
                    ColoringContext currentContext = null;
                    string currentKey = null;
                    string currentParamName = null;
                    string line = null;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var frags = line.Split(new char[] { ',' });
                        if (frags.Length != 4 && !(frags.Length == 1 && string.IsNullOrEmpty(frags[0])))
                        {
                            throw new IndexOutOfRangeException("all line should be 4 colums");
                        }
                        int index = 0;
                        while (index < frags.Length)
                        {
                            if (!string.IsNullOrEmpty(frags[index])){
                                if (index == 0)
                                {
                                    currentKey = frags[index];
                                    if (colors.ContainsKey(currentKey))
                                    {
                                        current = colors[currentKey];
                                    }
                                    else
                                    {
                                        current = new List<ColoringContext>();
                                        colors.Add(currentKey, current);
                                    }
                                    currentContext = null;
                                    currentParamName = null;
                                }
                                else if (index == 1)
                                {
                                    currentContext = new ColoringContext() { Key= frags[index] };
                                    current.Add(currentContext);
                                    currentParamName = null;
                                }
                                else if (index == 2)
                                {
                                    currentParamName= frags[index];
                                }
                                else
                                {
                                    currentContext.Params.Add(currentParamName,frags[index]);
                                }
                            }
                            index++;
                        }
                    }
                }
            }
        }

        public async Task LoadJson(string filePath)
        {
            using (var stream = File.OpenRead(filePath))
            {
                using(var reader = new StreamReader(stream))
                {
                    var json = await reader.ReadToEndAsync();
                    var deserialized = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                    foreach(var i in ((Newtonsoft.Json.Linq.JObject)deserialized).Children())
                    {
                        var key = ((Newtonsoft.Json.Linq.JProperty)i).Name;
                        IList<ColoringContext> currentColors = null;
                        if (Colors.ContainsKey(key))
                        {
                            currentColors= Colors[key];
                        }
                        else
                        {
                            currentColors=new List<ColoringContext>();
                            Colors.Add(key, currentColors);
                        }
                        foreach(var c in ((Newtonsoft.Json.Linq.JProperty)i).Children())
                        {
                            foreach(var p in ((Newtonsoft.Json.Linq.JObject)c).Children())
                            {
                                var cname = ((Newtonsoft.Json.Linq.JProperty)p).Name;
                                var currentColor = new ColoringContext() { Key = cname };
                                currentColors.Add(currentColor);
                                foreach(var aitem in ((Newtonsoft.Json.Linq.JProperty)p).Children())
                                {
                                    foreach(var a in ((Newtonsoft.Json.Linq.JObject)aitem).Children())
                                    {
                                        var aname = ((Newtonsoft.Json.Linq.JProperty)a).Name;
                                        foreach(var v in ((Newtonsoft.Json.Linq.JProperty)a).Children())
                                        {
                                            var vname = ((Newtonsoft.Json.Linq.JValue)v).Value.ToString();
                                            currentColor.Params.Add(aname, vname);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
