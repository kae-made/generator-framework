using System;
using System.Collections.Generic;
using System.Text;

namespace Kae.Tools.Generator.Context
{
    public  class ListSelectionParam:ContextParam
    {
        public IList<string> Candidates { get; } = new List<string>();
        public ListSelectionParam(string paramName, string [] candidates ) : base(paramName)
        {
            this.Candidates.CopyTo(candidates, 0);
        }
    }
}
