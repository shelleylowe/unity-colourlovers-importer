namespace CLImporter
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Serialization;

    public class CLColor
    {
        [XmlElement("id")]
        public int Id { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("userName")]
        public string UserName { get; set; }

        [XmlElement("numViews")]
        public int NumViews { get; set; }

        [XmlElement("numVotes")]
        public int NumVotes { get; set; }

        [XmlElement("numComments")]
        public int NumComments { get; set; }

        [XmlElement("numHearts")]
        public float NumHearts { get; set; }

        [XmlElement("rank")]
        public int Rank { get; set; }

        [XmlElement("dateCreated")]
        public string DateCreated { get; set; }

        [XmlElement("hex")]
        public string Hex { get; set; }
        
        [XmlElement("rgb")]
        public CLRGB RGB { get; set; }

        [XmlElement("hsv")]
        public CLHSV HSV { get; set; }

        [XmlElement("url")]
        public string Url { get; set; }

        [XmlElement("imageUrl")]
        public string ImageUrl { get; set; }
    }

    public class CLRGB
    {
        [XmlElement("red")]
        public int Red { get; set; }

        [XmlElement("green")]
        public int Green { get; set; }

        [XmlElement("blue")]
        public int Blue { get; set; }
    }

    public class CLHSV
    {
        [XmlElement("hue")]
        public int Hue { get; set; }

        [XmlElement("saturation")]
        public int Saturation { get; set; }

        [XmlElement("value")]
        public int Value { get; set; }
    }
}