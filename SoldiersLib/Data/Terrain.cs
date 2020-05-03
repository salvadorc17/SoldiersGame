using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;


namespace SoldiersGame
{
    public class Terrain
    {
        [XmlArray("tiles")]
        [XmlArrayItem("tile")]
        public List<Tile> Tiles;



        public Terrain()
        {
            Tiles = new List<Tile>();

        }


    }


    public class Tile
    {

        [XmlAttribute("x")]
        public short X { get; set; }

        [XmlAttribute("y")]
        public short Y { get; set; }

        [XmlAttribute("z")]
        public short Z { get; set; }

        [XmlAttribute("value")]
        public int Value { get; set; }

        [XmlAttribute("type")]
        public int Type { get; set; }

    }

    public enum TileType
    {
        None = 0,
        Floor = 1,
        Wall = 2,
        Water = 3



    }

}
