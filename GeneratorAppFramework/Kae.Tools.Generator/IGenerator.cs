using System;
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
        public void Generate();
    }
}
