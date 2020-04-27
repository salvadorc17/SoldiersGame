using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;


namespace SoldiersGame
{
    public class Terrain
    {
        [XmlArray("floors")]
        [XmlArrayItem("floor")]
        public List<Floor> Floors;

        [XmlArray("walls")]
        [XmlArrayItem("wall")]
        public List<Wall> Walls;

        [XmlArray("waters")]
        [XmlArrayItem("water")]
        public List<Water> Water;


        public Terrain()
        {

            Floors = new List<Floor>();
            Walls = new List<Wall>();
            Water = new List<Water>();

        }


    }


    public class Floor
    {

        [XmlAttribute("x")]
        public short X { get; set; }

        [XmlAttribute("y")]
        public short Y { get; set; }

        [XmlAttribute("z")]
        public int Z { get; set; }

        [XmlAttribute("value")]
        public int Value { get; set; }

    }

    public class Wall
    {

        [XmlAttribute("x")]
        public short X { get; set; }

        [XmlAttribute("y")]
        public short Y { get; set; }

        [XmlAttribute("z")]
        public int Z { get; set; }

        [XmlAttribute("value")]
        public int Value { get; set; }
    }

    public class Water
    {


        [XmlAttribute("x")]
        public short X { get; set; }

        [XmlAttribute("y")]
        public short Y { get; set; }

        [XmlAttribute("z")]
        public int Z { get; set; }

        [XmlAttribute("value")]
        public int Value { get; set; }
    }
}
