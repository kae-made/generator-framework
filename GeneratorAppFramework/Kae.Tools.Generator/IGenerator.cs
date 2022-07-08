﻿using System;
using System.Collections.Generic;

namespace Kae.Tools.Generator
{
    public interface IGenerator
    {
        public string Version { get; set; }

        public IList<Context.ContextParam> ContextParams { get; }

        public ColoringRepository Coloring { get; set; }

        public utility.GenFolder GenFolder { get; }

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
