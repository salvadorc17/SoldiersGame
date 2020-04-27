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

namespace SoldierTactics.Game
{
    public class Level
    {
        public int ID;
        public string Name;
        public bool Debug;
        public List<Sprite> Floors, Walls, Water;
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

                 ID = id;
                 Name = name;
                 XmlSerializer ax = new XmlSerializer(typeof(Map));



            using (Stream file = TitleContainer.OpenStream("Content/Levels/" + name + ".xml"))
            {
                Map = (Map)ax.Deserialize(file);

                if (Map.WadMap != "")
                {

                    ImageManager.LoadWad(1, Map.WadMap);


                    Floors = GenerateFloors();

                    Walls = GenerateWalls();



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

                        case EntityType.Player:

                            Player = new Game.Player(entity.ID, entity.Name, "Player1", entity.X, entity.Y, content);

                            break;

                        case EntityType.Object:

                            ObjectType otype = ObjectType.Static;

                                Objects.Add(new GameObject(entity.ID, entity.Name, entity.X, entity.Y, otype, 1, content));

                            break;

                        case EntityType.Vehicle:


                            Vehicles.Add(new Vehicle(entity.ID, entity.Name, entity.X, entity.Y, content));

                            break;

                        case EntityType.Enemy:


                            //Enemies.Add(new Enemy(entity.ID, entity.Name, entity.X, entity.Y, entity.Route, entity.RType, content));

                            break;

                    }
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

        public List<Sprite> GenerateFloors()
        {
            List<Sprite> TerrainSprites = new List<Sprite>();


            if (Map.Terrain.Floors.Count > 0)
                for (int i = 0; i < Map.Terrain.Floors.Count; i++)
                {

                    Sprite spr = GetSprite(0, i);


                    TerrainSprites.Add(spr);


                }

            return TerrainSprites;


        }

        public List<Sprite> GenerateWalls()
        {
            List<Sprite> TerrainSprites = new List<Sprite>();


            if (Map.Terrain.Walls.Count > 0)
                for (int i = 0; i <  Map.Terrain.Walls.Count; i++)
                {

                    Sprite spr = GetSprite(0, i);


                    TerrainSprites.Add(spr);


                }

            return TerrainSprites;


        }

        public Sprite GetSprite(int type, int id)
        {
            Sprite sprite = new Sprite();

            //Floor sprite
            if (type == 0)
            {

                sprite = new Sprite(
                ImageManager.ImageFromWADArchive(0, Map.Terrain.Floors[id].Value));


            }
            //Wall sprite
            else if (type == 1)
            {

                sprite = new Sprite(
                ImageManager.ImageFromWADArchive(0, Map.Terrain.Walls[id].Value));


            }

            return sprite;

        }


        public void Draw(SpriteBatch SpriteBatch)
        {

            Global.SpriteBatch = SpriteBatch;

            

            if (Map != null && Map.Terrain.Floors.Count > 0)
                for (int i = 0; i < Map.Terrain.Floors.Count; i++)
                {

                    int value = ImageManager.WADImages[0].Files;

                    if (Floors[i].Image != null)
                    SpriteBatch.Draw(Floors[i].Image, new Rectangle(Map.Terrain.Floors[i].X, Map.Terrain.Floors[i].Y, 
                        Floors[i].Width, Floors[i].Height), Color.White);


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
