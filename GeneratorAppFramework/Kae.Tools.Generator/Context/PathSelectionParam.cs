// Copyright (c) Knowledge & Experience. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
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

        public override object GetValue()
        {
            return (Path: Path, IsFolder: IsFolder);
        }

        public override void SetValue(object value)
        {
            var settingValue = ((string Path, bool IsFolder))value;
            Path = settingValue.Path;
            IsFolder = settingValue.IsFolder;
        }
    }
}
