﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Kae.Tools.Generator.Context
{
    public class BooleanParam : ContextParam
    {
        bool Value { get; set; }
        public BooleanParam(string paramName) : base(paramName)
        {
            ;
        }
    }
}
