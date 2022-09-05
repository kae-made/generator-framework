﻿// Copyright (c) Knowledge & Experience. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Kae.Tools.Generator.Coloring.DomainWeaving;
using Kae.Tools.Generator.Coloring.Generator;
using System;
using System.Collections.Generic;

namespace Kae.Tools.Generator
{
    public interface IGenerator
    {
        public string Version { get; set; }
        public string DomainName { get; }

        public ColoringRepository Coloring { get; set; }

        public ColoringManager ColoringManagerForDomainWeaving { get; }

        public utility.GenFolder GenFolder { get; }

        public GeneratorContext GetContext();

        public void ResolveContext();
        public void LoadMetaModel();
        public void LoadDomainModels();

        /// <summary>
        /// deprecated method
        /// </summary>
        public void Generate();

        /// <summary>
        /// Generate environment for contents that will be generated by GenerateContent() methods.
        /// For example, project folder and project file for the case of C#.
        /// </summary>
        public bool GenerateEnvironment();


        public void GenerateContents();
    }
}
