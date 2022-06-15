using Kae.Tools.Generator;
using Kae.Tools.Generator.Context;
using Kae.Tools.Generator.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSample
{
    internal class SampleGenerator : IGenerator
    {
        public string Version { get; set; }

        private IList<ContextParam> contextParams = new List<ContextParam>();
        public IList<ContextParam> ContextParams { get { return contextParams; } }

        public ColoringRepository Coloring { get; set; } = new ColoringRepository();

        private GenFolder genFolder = new GenFolder();
        public GenFolder GenFolder { get { return genFolder; } }

        public void Generate()
        {
            Console.WriteLine("4. generate something and store generated files by using GenFolder...");
        }

        public void LoadDomainModels()
        {
            Console.WriteLine("3. Load domain models...");
        }

        public void LoadMetaModel()
        {
            Console.WriteLine("2. load meta mode...");
        }

        public void ResolveContext()
        {
            Console.WriteLine("1. resolve context...");
            Console.WriteLine("Solve genFolder's BaseFolder");
        }
    }
}
