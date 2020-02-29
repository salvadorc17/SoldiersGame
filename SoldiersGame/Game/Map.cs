using System;
using System.Collections.Generic;
using System.IO;


namespace SoldierTactics.Game
{

    public class Map
    {

        public string Name { get; set; }
        public string Background { get; set; }
        public Entity[] entities;

        public Map()
        {

            entities = new Entity[20];
            for (int x = 0; x <= entities.Length - 1; x++)
            {
                entities[x] = new Entity();

            }


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
