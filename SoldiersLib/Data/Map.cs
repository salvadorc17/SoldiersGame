﻿using System;
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

        [XmlArray("entities")]
        [XmlArrayItem("entity")]
        public List<Entity> Entities { get; set; }

        [XmlArray("soldiers")]
        [XmlArrayItem("soldier")]
        public List<Soldier> Soldiers { get; set; }

        public Map()
        {
            Terrain = new Terrain();
            Entities = new List<Entity>();
            Soldiers = new List<Soldier>();

        }




        public static void Serialize(String path, Map map)
        {

            XmlSerializer serialize = new XmlSerializer(typeof(Map));

            using (var writer = new StreamWriter(path))
            {
                serialize.Serialize(writer, map);
            }

        }

    }

 
}
