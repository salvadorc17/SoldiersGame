using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SoldierTactics
{
    [XmlRoot("spritetable")]
    public class SpriteTable
    {

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlArray("sequences")]
        [XmlArrayItem("sequence")]
        public List<Sequence> Sequences { get; set; }

        public static void Serialize(String path, SpriteTable sprt)
        {

            XmlSerializer serialize = new XmlSerializer(typeof(SpriteTable));

            using (var writer = new StreamWriter(path))
            {
                serialize.Serialize(writer, sprt);
            }

        }

    }


    public class Sequence 
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("type")]
        public int Type { get; set; }

        [XmlElement("speed")]
        public int Speed { get; set; }

        [XmlArray("frames")]
        [XmlArrayItem("frame")]
        public List<Frame> Frames { get; set; }

       

    }

    public class Frame
    {

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("x")]
        public short X { get; set; }

        [XmlAttribute("y")]
        public short Y { get; set; }



    }

}