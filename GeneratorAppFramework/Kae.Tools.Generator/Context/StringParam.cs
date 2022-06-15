using System;
using System.Collections.Generic;
using System.Text;

namespace Kae.Tools.Generator.Context
{
    public class StringParam : ContextParam
    {
        public string Value { get; set; }
        public StringParam (string paramName) : base(paramName)
        {

        }
    }
}
