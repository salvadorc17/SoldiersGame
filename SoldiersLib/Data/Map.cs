using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using SoldiersGame;
using SoldierTactics.Game;


namespace SoldierTactics
{
    [XmlRoot("map")]
    public class Map
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("width")]
        public int Width { get; set; }

        [XmlElement("height")]
        public int Height { get; set; }

        [XmlElement("wad")]
        public string WadMap { get; set; }

        [XmlElement("terrain")]
        public Terrain Terrain { get; set; }

        [XmlArray("objects")]
        [XmlArrayItem("object")]
        public List<Entity> Entities { get; set; }

        public Map()
        {
            Terrain = new Terrain();
            Entities = new List<Entity>();

        }




        public void Serialize(String path, Map map)
        {

            System.Xml.Serialization.XmlSerializer ax = new System.Xml.Serialization.XmlSerializer(typeof(Map));

            Stream file = File.Open(path, FileMode.Open);

            StreamWriter writer = new StreamWriter(file);
            ax.Serialize(writer, map);
            writer.Dispose();

        }

    }

 
}
