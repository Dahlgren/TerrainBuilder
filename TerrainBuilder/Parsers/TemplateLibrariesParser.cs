using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TerrainBuilder.Models;

namespace TerrainBuilder.Parsers
{
    public class TemplateLibrariesParser
    {
        private string Path;

        public TemplateLibrariesParser(string path)
        {
            this.Path = path;
        }

        public List<TemplateLibraryFile> Parse()
        {
            var list = new List<TemplateLibraryFile>();
            var files = Directory.GetFiles(this.Path);

            XmlSerializer serializer = new XmlSerializer(typeof(TemplateLibraryFile));

            foreach (var file in files)
            {
                using (FileStream fileStream = new FileStream(file, FileMode.Open))
                {
                    list.Add((TemplateLibraryFile)serializer.Deserialize(fileStream));
                }
            }

            return list;
        }
    }
}
