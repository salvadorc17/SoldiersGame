using System;
using System.Collections.Generic;
using SoldierTactics.Engine;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoldiersGame;
using SoldierTactics.GameFormats;

namespace SoldierTactics.Game
{
    public class Level
    {
        public int ID, WadNumber, Entities;
        public string Name;
        public bool Debug;
        public List<Sprite> Tiles;
        public Map Map;
        public GameObject Dummy;
        public List<GameObject> Objects;
        public List<Enemy> Enemies;
        public List<Vehicle> Vehicles;
        public Player Player;


        public Vector2 CameraPos;

        private Single Gametime;

        public Level(int id, string name, ContentManager content)
             {
                Entities = 0;
                ID = id;
                 Name = name;
                 XmlSerializer ax = new XmlSerializer(typeof(Map));



            using (Stream file = TitleContainer.OpenStream("Content/Levels/" + name + ".xml"))
            {
                Map = (Map)ax.Deserialize(file);

                if (Map.WadMap != "")
                {

                    WadNumber = ImageManager.WADImages.Count;

                    ImageManager.LoadWad(1, Map.WadMap);


                    Tiles = GenerateTiles();


                }

                Enemies = new List<Enemy>();
                 Objects = new List<GameObject>();
                 Vehicles = new List<Vehicle>();

                foreach (Entity entity in Map.Entities)
                    if (entity != null)
                    {

                        EntityType type = entity.GetEntityType(); 

                        switch (type)
                        {

                        case EntityType.None:

                                //Do nothing

                                break;

                        case EntityType.Object:

                            ObjectType otype = ObjectType.Static;

                                Objects.Add(new GameObject(entity.ID, entity.Name, entity.X, entity.Y, otype, 1, content));

                            break;

                        case EntityType.Vehicle:


                            Vehicles.Add(new Vehicle(entity.ID, entity.Name, entity.X, entity.Y, content));

                            break;

                        case EntityType.Enemy:
                                {

                                    WAD EntWAD = new WAD(entity.Name, false);

                                   if (!ImageManager.WADImages.Contains(EntWAD))
                                        {

                                            WadNumber = ImageManager.WADImages.Count;

                                            ImageManager.LoadWad(2, entity.Name);

                                        }

                                    Enemies.Add(new Enemy(WadNumber, entity.Name, entity.X, entity.Y, content));

                                    Enemies[Entities].SetRoute(true, RouteType.Horizontal);

                                }
                                break;

                        }

                        Entities++;
                 }

                foreach (Soldier soldier in Map.Soldiers)
                {

                    WadNumber = ImageManager.WADImages.Count;

                    ImageManager.LoadWad(2, soldier.Name);
                    Player = new Player(WadNumber, soldier.Name, "Player" + WadNumber, soldier.X, soldier.Y, 45, content);

                }
            }

        }

        public void Update(GameTime gameTime)
        {


            // Process passing time.
            Gametime += 1;
            if (Gametime > 4)
            {

                Gametime = 0;

          

                if (Objects != null)
                for (int i = 0; i <= Objects.Count - 1; i++)
                    if (Objects[i] != null)
                        Objects[i].Update(gameTime);

                if (Enemies != null)
                for (int i = 0; i <= Enemies.Count - 1; i++)
                    if (Enemies[i] != null)
                        Enemies[i].Update(gameTime);

                if (Vehicles != null)
                for (int i = 0; i <= Vehicles.Count - 1; i++)
                    if (Vehicles[i] != null)
                        Vehicles[i].Update(gameTime);

                Input.GetInput();

                if (Player != null)
                {


                    if (Input.IsKeyDown(Keys.Right))
                    {

                        Player.RightDown = true;
                        Player.LeftDown = false;
                        Player.State = 1;


                    }


                    else if (Input.IsKeyDown(Keys.Left))
                    {

                        Player.LeftDown = true;
                        Player.RightDown = false;
                        Player.State = 1;


                    }

                    else
                    {
                        Player.LeftDown = false;
                        Player.RightDown = false;

                        Player.State = 0;

                    }


                    Player.Update(gameTime);

                }
            }

        }

        public List<Sprite> GenerateTiles()
        {
            List<Sprite> TerrainSprites = new List<Sprite>();


            if (Map.Terrain.Tiles.Count > 0)
                for (int i = 0; i < Map.Terrain.Tiles.Count; i++)
                {

                    Sprite spr = GetSprite(0, i);


                    TerrainSprites.Add(spr);


                }

            return TerrainSprites;


        }

        

        public Sprite GetSprite(int type, int id)
        {
            Sprite sprite = new Sprite();

            //Tile sprite
            if (type == 0)
            {

                sprite = new Sprite(
                ImageManager.ImageFromWADArchive(WadNumber, Map.Terrain.Tiles[id].Value));


            }
           

            return sprite;

        }


        public void Draw(SpriteBatch SpriteBatch)
        {

            Global.SpriteBatch = SpriteBatch;

            

            if (Map != null && Map.Terrain.Tiles.Count > 0)
                for (int i = 0; i < Map.Terrain.Tiles.Count; i++)
                {

                    int value = ImageManager.WADImages[0].Files;

                    if (Tiles[i].Image != null)
                    SpriteBatch.Draw(Tiles[i].Image, new Rectangle(Map.Terrain.Tiles[i].X, Map.Terrain.Tiles[i].Y, 
                        Tiles[i].Width, Tiles[i].Height), Color.White);


                }

            if (Objects != null)
                for (int i = 0; i <= Objects.Count - 1; i++)
                    if (Objects[i] != null)
                        Objects[i].Draw();

            if (Enemies != null)
                for (int i = 0; i <= Enemies.Count - 1; i++)
                    if (Enemies[i] != null)
                        Enemies[i].Draw();



            if (Vehicles != null)
                for (int i = 0; i <= Vehicles.Count - 1; i++)
                    if (Vehicles[i] != null)
                        Vehicles[i].Draw();

            if (Player != null)
                Player.Draw();

        }

    }
}
