using System;
using System.Collections.Generic;
using System.Text;

namespace Kae.Tools.Generator.Context
{
    public abstract class ContextParam
    {
        public string ParamName { get; set; }
        public string Tips { get; set; }
        public IList<ContextParam> Children { get; set; } = new List<ContextParam>();

        public ContextParam(string paramName)
        {
            this.ParamName = paramName;
        }
    }
}
