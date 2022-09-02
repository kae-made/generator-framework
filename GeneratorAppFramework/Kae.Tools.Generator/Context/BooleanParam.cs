// Copyright (c) Knowledge & Experience. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Text;

namespace Kae.Tools.Generator.Context
{
    public class BooleanParam : ContextParam
    {
        public bool Value { get; set; }
        public BooleanParam(string paramName) : base(paramName)
        {
            ;
        }
    }
}
