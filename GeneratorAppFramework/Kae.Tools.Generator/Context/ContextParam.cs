// Copyright (c) Knowledge & Experience. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
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

        public abstract object GetValue();
        public abstract void SetValue(object value);
    }
}
