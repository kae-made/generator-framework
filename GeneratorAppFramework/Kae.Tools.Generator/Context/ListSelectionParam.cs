// Copyright (c) Knowledge & Experience. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kae.Tools.Generator.Context
{
    public  class ListSelectionParam:ContextParam
    {
        public List<string> Candidates { get; } = new List<string>();
        public string SelectedValue { get; set; }
        public ListSelectionParam(string paramName, string [] candidates ) : base(paramName)
        {
            this.Candidates.CopyTo(candidates, 0);
        }

        public override object GetValue()
        {
            return SelectedValue;
        }

        public override void SetValue(object value)
        {
            var existing = Candidates.Where(c =>c == (string)value).FirstOrDefault();
            if (existing != null)
            {
                SelectedValue = (string)value;
            }
        }
    }
}
