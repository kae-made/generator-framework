// Copyright (c) Knowledge & Experience. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Kae.Tools.Generator.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kae.Tools.Generator
{
    public class GeneratorContext
    {
        public IList<ContextParam> Options { get; } = new List<ContextParam>();

        public void AddOption(ContextParam param)
        {
            Options.Add(param);
        }
        public object GetOptionValue(string paramName)
        {
            var option = Options.Where(cp => cp.ParamName == paramName).FirstOrDefault();
            if (option != null)
            {
                return option.GetValue();
            }
            return null;
        }
        public void SetOptionValue(string paramName, object value)
        {
            var option = Options.Where(cp => cp.ParamName == paramName).FirstOrDefault();
            if (option != null)
            {
                option.SetValue(value);
            }
        }
    }
}
