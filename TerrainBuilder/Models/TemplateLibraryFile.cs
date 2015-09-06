using System.Collections.Generic;
using System.Xml.Serialization;

namespace TerrainBuilder.Models
{
    [XmlRoot("Library")]
    public class TemplateLibraryFile
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("Template")]
        public List<TemplateLibraryObject> Objects { get; set; }
    }
}
