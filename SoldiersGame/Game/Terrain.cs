using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using SoldierTactics.Engine;

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

        public Sprite GetSprite(int type, int id)
        {
            Sprite sprite = new Sprite();

            //Floor sprite
            if (type == 0)
            {

                sprite = new Sprite(
                ImageManager.ImageFromWADArchive(0, Floors[id].Value));


            }
            //Wall sprite
            else if (type == 1)
            {

               sprite = new Sprite(
               ImageManager.ImageFromWADArchive(0, Walls[id].Value));
            

            }

            return sprite;

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
