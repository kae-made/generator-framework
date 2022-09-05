﻿// Copyright (c) Knowledge & Experience. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Kae.Tools.Generator;
using Kae.Tools.Generator.Context;
using Kae.Tools.Generator.utility;
using Kae.Utility.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSample
{
    internal class SampleGenerator : GeneratorBase
    {
        public static string CPKeyOriginalOption = "original";
        public static string CPKeyBooleanOption = "boolean";
        int OriginalOptionValue = 0;
        bool BooleanOptionValue = false;

        public SampleGenerator(string version, Logger logger) : base(logger, version)
        {
            // add options
            GetContext().AddOption(new NumberParam(CPKeyOriginalOption) { Tips = "Original Option" });
            GetContext().AddOption(new BooleanParam(CPKeyBooleanOption));
        }

        protected override void CreateAdditionalContext()
        {
            Console.WriteLine("1. add specific contexts into  ContextParams.");
        }

        protected override bool AdditionalWorkForResloveContext()
        {
            Console.WriteLine("2. resolve specific contexts in ContextParams.");
            OriginalOptionValue = (int)GetContext().GetOptionValue(CPKeyOriginalOption);
            BooleanOptionValue = (bool)GetContext().GetOptionValue(CPKeyBooleanOption);
            return true;
        }

        protected override bool AdditionalWorkForMetaModel()
        {
            Console.WriteLine("3. load specific meta model or do something.");
            return true;
        }

        protected override bool AdditionalWorkForDomainModels()
        {
            Console.WriteLine("4. load specific domain models or do something.");
            return true;
        }

        protected override bool GenerateEnvironmentYourOwn()
        {
            Console.WriteLine("5. generate environment for contents that will be generated by GenerateContents method.");
            return true;
        }

        protected override void GenerateContentsYourOwn()
        {
            Console.WriteLine("6. generate contents in generated environment");
        }
    }
}
