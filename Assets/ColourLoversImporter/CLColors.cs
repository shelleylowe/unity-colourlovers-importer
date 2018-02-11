namespace CLImporter
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Serialization;

    [XmlRoot("colors")]
    public class CLColors
    {
        [XmlElement("color")]
        public List<CLColor> Colors = new List<CLColor>();

        [XmlAttribute(AttributeName = "numResults")]
        public int NumResults { get; set; }

        [XmlAttribute(AttributeName = "totalResults")]
        public int TotalResults { get; set; }
    }
}