using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SoldierTactics
{
   public class Inventory
    {
        [XmlArray("weeapons")]
        [XmlArrayItem("weapon")]
        public List<Weapon> Weapons { get; set; }

    }


    public class Weapon
    {
        [XmlAttribute("id")]
        public int ID { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("ammo")]
        public int Ammo { get; set; }


    }

}
