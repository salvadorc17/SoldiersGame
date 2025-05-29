using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using SoldierTactics.Game;

namespace SoldierTactics
{
    public class Soldier
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

        [XmlElement("inventory")]
        public Inventory Inventory { get; set; }

        public EntityType GetEntityType()
        {

            EntityType type = EntityType.None;


            if (Name.Contains("Comando"))
                type = EntityType.Player;
            else if (Name.Contains("Artific"))
                type = EntityType.Player;


            return type;


        }

    }



}
