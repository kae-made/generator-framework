﻿using System;
using System.Collections.Generic;

namespace Kae.Tools.Generator
{
    public interface IGenerator
    {
        public string Version { get; set; }

        public IList<Context.ContextParam> ContextParams { get; }
        public void Generate();
    }
}
