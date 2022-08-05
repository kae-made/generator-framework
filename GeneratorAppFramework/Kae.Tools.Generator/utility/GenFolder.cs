// Copyright (c) Knowledge & Experience. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Kae.Tools.Generator.utility
{
    public class GenFolder
    {
        public enum WriteMode
        {
            Overwrite,
            None,
            Backup
        }
        public string BaseFolder { get; set; }

        [Obsolete("Please use other signature")]
        public async Task WriteContentAsync(string folderName, string fileName, string content,bool store = true)
        {
            var mode = WriteMode.None;
            if (store) mode = WriteMode.Overwrite;
            await WriteContentAsync(folderName, fileName, content, mode);
        }

        public async Task WriteContentAsync(string folderName, string fileName, string content, WriteMode writeMode = WriteMode.Overwrite)
        {
            WriteMode store = WriteMode.Overwrite;
            var absolutePath = Path.Join(BaseFolder, folderName, fileName);
            if (File.Exists(absolutePath))
            {
                store = writeMode;
                if (store == WriteMode.Overwrite)
                {
                    File.Delete(absolutePath);
                }
                else if (store == WriteMode.Backup)
                {
                    string backupExt = "bak";
                    int backNo = 0;
                    var dirInfo = new DirectoryInfo(folderName);
                    foreach (var file in dirInfo.GetFiles())
                    {
                        if (file.Name.StartsWith(fileName))
                        {
                            var frags = file.Name.Split(new char[] { '.' });
                            if (frags.Length == 3)
                            {
                                if (backNo == 0)
                                {
                                    backNo = 1;
                                }
                                string bNo = frags[2].Substring(backupExt.Length);
                                if (bNo.Length > 0)
                                {
                                    int bIndex = int.Parse(bNo);
                                    if (backNo <= bIndex)
                                    {
                                        backNo = bIndex +1;
                                    }
                                }
                            }
                        }
                    }
                    string backupFileName = $"{fileName}.{backupExt}";
                    if (backNo > 0)
                    {
                        backupFileName = $"{backupFileName}{backNo}";
                    }
                    File.Move(absolutePath, Path.Join(folderName, backupFileName));
                }
            }

            if (store== WriteMode.Overwrite||store== WriteMode.Backup)
            {
                using (var writer = File.CreateText(absolutePath))
                {
                    await writer.WriteAsync(content);
                    await writer.FlushAsync();
                }
            }
        }
        public void CreateFolder(string folderName)
        {
            var absolutePath = Path.Combine(BaseFolder, folderName);
            if (!Directory.Exists(absolutePath))
            {
                Directory.CreateDirectory(absolutePath);
            }
        }
    }
}
