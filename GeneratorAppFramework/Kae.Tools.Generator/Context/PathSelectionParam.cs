using System;
using System.Collections.Generic;
using System.Text;

namespace Kae.Tools.Generator.Context
{
    public class PathSelectionParam : ContextParam
    {
        public string Path { get; set; }
        public bool IsFolder { get; set; }
        public PathSelectionParam(string paramName) : base(paramName)
        {

        }
    }
}
