﻿// Copyright (c) Knowledge & Experience. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Text;

namespace Kae.Tools.Generator.Coloring.Generator
{
    public class ColoringContext
    {
        public string Key { get; set; }
        private IDictionary<string, string> @params = new Dictionary<string, string>();
        public IDictionary<string, string> Params { get { return @params; } }
    }
}
