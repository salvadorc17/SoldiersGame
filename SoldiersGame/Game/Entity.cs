using System;
using System.Collections.Generic;
using System.IO;


namespace SoldierTactics.Game
{


   public class Entity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public EntityType Type { get; set; }
        public bool Route { get; set; }
        public RouteType RType { get; set; }

       public Entity()
        {
            X = 0;
            Y = 0;
            Type = EntityType.None;
            RType = RouteType.None;

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
