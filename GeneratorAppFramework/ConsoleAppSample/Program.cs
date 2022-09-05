// Copyright (c) Knowledge & Experience. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Kae.Tools.Generator.Context;
using Kae.Utility.Logging;
using System;
using System.IO;

namespace ConsoleAppSample
{
    internal class Program
    {
        static string version = "1.0.0";

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var generator = new SampleGenerator(version,ConsoleLogger.CreateLogger());

            // generator.Coloring.Load("colors.yaml").Wait();
            // generator.Coloring.Load("colors.csv").Wait();
            // generator.Coloring.Load("colors.json").Wait();
            // generator.Coloring.Parse("R_SUBSUP", "@subsupgen:mode=extends,strength=weak;");
            // testDescripColors(generator);

            var genContext = generator.GetContext();
            // setup genContext
            string bv = "true";
            genContext.SetOptionValue(SampleGenerator.CPKeyBooleanOption, bool.Parse(bv));
            // metamodel, datatype for metamodel, genFolder, domain models, ...
            generator.ResolveContext();
            generator.LoadMetaModel();
            generator.LoadDomainModels();
            generator.Generate();
        }

        static void testDescripColors(SampleGenerator generator)
        {
            using (var stream = File.OpenRead("descrip-colors.txt"))
            {
                using(var reader = new StreamReader(stream))
                {
                    string descrip = reader.ReadToEnd();
                    generator.Coloring.Parse("R_SUBSUP", descrip);
                }
            }
        }
    }
}
