namespace CLImporter
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Serialization;

    [XmlRoot("palettes")]
    public class CLPalettes
    {
        [XmlElement("palette")]
        public List<CLPalette> Palettes = new List<CLPalette>();

        [XmlAttribute(AttributeName = "numResults")]
        public int NumResults { get; set; }

        [XmlAttribute(AttributeName = "totalResults")]
        public int TotalResults { get; set; }
    }
}