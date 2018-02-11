namespace CLImporter
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Serialization;

    public class CLPalette
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

        [XmlArray("colors")]
        [XmlArrayItem("hex")]
        public List<string> HexColors { get; set; }

        [XmlElement("url")]
        public string Url { get; set; }

        [XmlElement("imageUrl")]
        public string ImageUrl { get; set; }
    }
}