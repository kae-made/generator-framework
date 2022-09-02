using Kae.Tools.Generator;
using Kae.Tools.Generator.Coloring;
using Kae.Tools.Generator.Context;
using Kae.Utility.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TestColoringManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var p = new Program();
            if (p.ResolveArgs(args))
            {
                p.Work();
            }

        }

        IGenerator generator;
        public Program()
        {
            generator = new SampleGenerator(ConsoleLogger.CreateLogger());

        }

        public void Work()
        {
            generator.ResolveContext();
            generator.LoadMetaModel();
            generator.LoadDomainModels();
            generator.GenerateEnvironment();
            generator.GenerateContents();
        }

        public bool ResolveArgs(string[] args)
        {
            var contextParams = generator.ContextParams;
            if (args.Length == 0)
            {
                // ShowCommandline();
                return false;
            }

            var requiredOptions = new List<string>() { "--metamodel", "--base-datatype", "--domainmodel", "--gen-folder" };
            int index = 0;
            while (index < args.Length)
            {
                if (args[index] == "--metamodel")
                {
                    var cp = contextParams.Where(c => c.ParamName == GeneratorBase.CPKeyOOAofOOAModelFilePath).First();
                    requiredOptions.Remove(args[index]);
                    ((PathSelectionParam)cp).Path = args[++index];
                }
                else if (args[index] == "--meta-datatype")
                {
                    var cp = contextParams.Where(c => c.ParamName == GeneratorBase.CPKeyMetaDataTypeDefFilePath).First();
                    ((PathSelectionParam)cp).Path = args[++index];
                }
                else if (args[index] == "--base-datatype")
                {
                    var cp = contextParams.Where(c => c.ParamName == GeneratorBase.CPKeyBaseDataTypeDefFilePaht).First();
                    requiredOptions.Remove(args[index]);
                    ((PathSelectionParam)cp).Path = args[++index];
                }
                else if (args[index] == "--domainmodel")
                {
                    var cp = contextParams.Where(c => c.ParamName == GeneratorBase.CPKeyDomainModelFilePath).First();
                    requiredOptions.Remove(args[index]);
                    ((PathSelectionParam)cp).Path = args[++index];
                    // domainModelFilePath = args[index];
                }
                else if (args[index] == "--gen-folder")
                {
                    var cp = contextParams.Where(c => c.ParamName == GeneratorBase.CPKeyGenFolderPath).First();
                    requiredOptions.Remove(args[index]);
                    ((PathSelectionParam)cp).Path = args[++index];
                }
                else if (args[index] == "--coloring")
                {
                    var cp = contextParams.Where(c => c.ParamName == GeneratorBase.CPKeyColoringFilePath).First();
                    ((PathSelectionParam)cp).Path = args[++index];
                }
                else
                {
                    // ShowCommandline();
                }
                index++;
            }
            if (requiredOptions.Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
