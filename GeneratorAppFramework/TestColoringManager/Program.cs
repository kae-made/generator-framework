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
            var generatorContext = generator.GetContext();
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
                    requiredOptions.Remove(args[index]);
                    generatorContext.SetOptionValue(GeneratorBase.CPKeyOOAofOOAModelFilePath, (args[++index],false));
//                    var cp = contextParams.Where(c => c.ParamName == GeneratorBase.CPKeyOOAofOOAModelFilePath).First();
  //                  ((PathSelectionParam)cp).Path = args[++index];
                }
                else if (args[index] == "--meta-datatype")
                {
                    generatorContext.SetOptionValue(GeneratorBase.CPKeyMetaDataTypeDefFilePath, (args[++index],false));
////                    var cp = contextParams.Where(c => c.ParamName == GeneratorBase.CPKeyMetaDataTypeDefFilePath).First();
//                    ((PathSelectionParam)cp).Path = args[++index];
                }
                else if (args[index] == "--base-datatype")
                {
                    requiredOptions.Remove(args[index]);
                    generatorContext.SetOptionValue(GeneratorBase.CPKeyBaseDataTypeDefFilePaht, (args[++index], false));
//                    var cp = contextParams.Where(c => c.ParamName == GeneratorBase.CPKeyBaseDataTypeDefFilePaht).First();
//                    ((PathSelectionParam)cp).Path = args[++index];
                }
                else if (args[index] == "--domainmodel")
                {
                    //var cp = contextParams.Where(c => c.ParamName == GeneratorBase.CPKeyDomainModelFilePath).First();
                    requiredOptions.Remove(args[index]);
                    string specifiedString = args[++index];
                    generatorContext.SetOptionValue(GeneratorBase.CPKeyDomainModelFilePath, (specifiedString, !File.Exists(specifiedString)));
//                    ((PathSelectionParam)cp).Path = args[++index];
                    // domainModelFilePath = args[index];
                }
                else if (args[index] == "--gen-folder")
                {
                    requiredOptions.Remove(args[index]);
                    generatorContext.SetOptionValue(GeneratorBase.CPKeyGenFolderPath, (args[++index], true));
//                    var cp = contextParams.Where(c => c.ParamName == GeneratorBase.CPKeyGenFolderPath).First();
//                    ((PathSelectionParam)cp).Path = args[++index];
                }
                else if (args[index] == "--coloring")
                {
                    generatorContext.SetOptionValue(GeneratorBase.CPKeyColoringFilePath, (args[++index], false));
//                    var cp = contextParams.Where(c => c.ParamName == GeneratorBase.CPKeyColoringFilePath).First();
 //                   ((PathSelectionParam)cp).Path = args[++index];
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
