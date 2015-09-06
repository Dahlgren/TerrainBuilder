using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace TerrainBuilder.Parsers
{
    public class ImportFileParser
    {
        private string Path;

        public ImportFileParser(string path)
        {
            this.Path = path;
        }

        public List<string> Parse()
        {
            string[] lines = File.ReadAllLines(this.Path);
            List<string> objects = new List<string>();
            Regex objectRegex = new Regex("^\"(.*)\";");

            foreach (var line in lines)
            {
                var match = objectRegex.Match(line);
                if (match.Success)
                {
                    objects.Add(match.Groups[1].Value);
                }
            }

            return objects;
        }
    }
}
