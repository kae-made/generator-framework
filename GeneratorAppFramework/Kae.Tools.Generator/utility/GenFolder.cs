using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Kae.Tools.Generator.utility
{
    public class GenFolder
    {
        public string BaseFolder { get; set; }
        public async Task WriteContentAsync(string folderName, string fileName, string content, bool overwrite = true)
        {
            bool store = true;
            var absolutePath = Path.Join(BaseFolder, folderName, fileName);
            if (File.Exists(absolutePath))
            {
                store = overwrite;
                if (store)
                {
                    File.Delete(absolutePath);
                }
            }

            if (store)
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
