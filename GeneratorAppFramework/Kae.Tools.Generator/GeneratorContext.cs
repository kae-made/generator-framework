// Copyright (c) Knowledge & Experience. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Kae.Tools.Generator.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kae.Tools.Generator
{
    public abstract class GeneratorContext
    {
        public string GenerateToFolder { get; set; }

        protected IList<ContextParam> Options { get; } = new List<ContextParam>();


    }
}
