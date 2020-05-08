using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace SoldierTactics.Game
{


   public class Entity
    {
        [XmlAttribute("id")]
        public int ID { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("x")]
        public int X { get; set; }

        [XmlAttribute("y")]
        public int Y { get; set; }

        [XmlAttribute("z")]
        public int Z { get; set; }

        [XmlAttribute("dir")]
        public int Dir { get; set; }

        public Entity()
        {
            X = 0;
            Y = 0;
            Z = 0;
            Dir = 0;

        }

        public EntityType GetEntityType()
        {

            EntityType type = EntityType.None;


            if (Name.Contains("Aleman"))
                type = EntityType.Enemy;


            return type;


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

    public enum EntityType
    {
        None = 0,
        Player = 1,
        Enemy = 2,
        Object = 3,
        Vehicle = 4

    }

}
