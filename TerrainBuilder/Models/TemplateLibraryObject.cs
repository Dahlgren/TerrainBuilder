using System.Xml.Serialization;

namespace TerrainBuilder.Models
{
    public class TemplateLibraryObject
    {
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("File")]
        public string File { get; set; }
    }
}
