
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SoldierTactics;
using SoldierTactics.GameFormats;
using SoldiersGame;
using SoldierTactics.Game;

namespace LevelEditor
{
    public partial class Editor : Form
    {

        private Graphics graphics, BBG;
        private Rectangle srect, drect;
        private Bitmap bb;

        private string Folder, WadMap;
        private int TerrainSelected, EntitySelected, TerrainId, EntityId;

        private Map Map;
        private Entity SelEntity;
        private List<Bitmap> MapImages;

        private List<string> Files, Paths;

        private WAD WADFile;

        private SpriteTable SpriteTable;

        private bool Loaded;

        public Editor()
        {
            InitializeComponent();
        }

        private void Editor_Load(object sender, EventArgs e)
        {

            Folder = AppDomain.CurrentDomain.BaseDirectory + "/DATOS";
            graphics = this.pictureBox2.CreateGraphics();
            bb = new Bitmap(640, 480);

            if (Directory.Exists(Folder))
                GenerateNewMap();
            else
                MessageBox.Show("Game folder not found");

            Loaded = false;

        }

        private void GenerateNewMap()
        {
            Map = new Map();
            MapImages = new List<Bitmap>();
            TerrainSelected = 0;
            EntitySelected = 0;
            TerrainId = 0;
            EntityId = 0;


        }

        private void SaveMap()
        {

            Map.Name = "Test";

            Map.Width = 800;
            Map.Height = 600;
            Map.WadMap = WadMap;


            SoldierTactics.Map.Serialize(AppDomain.CurrentDomain.BaseDirectory + "/Content/Levels/" 
                + Map.Name + ".xml", Map);



        }

        private void DrawMap()
        {

            Rectangle drect;

            if (Map.Terrain.Tiles.Count > 0 && MapImages.Count > 0)
                for (int i = 0; i < Map.Terrain.Tiles.Count; i++)
                {
                    for (int z = 0; z < 10; z++)
                        if (Map.Terrain.Tiles[i].Z == z)
                        {
                            drect = new Rectangle(Map.Terrain.Tiles[i].X, Map.Terrain.Tiles[i].Y,
                                MapImages[i].Width, MapImages[i].Height);
                            graphics.DrawImage(MapImages[i], drect);
                        }
                }

           if ( Map.Terrain.Tiles.Count > 0)
            graphics.DrawRectangle(Pens.Red, new Rectangle(Map.Terrain.Tiles[TerrainId].X,
                Map.Terrain.Tiles[TerrainId].Y, MapImages[TerrainId].Width, MapImages[TerrainId].Height));

            // COPY BACKBUFFER TO GRAPHICS OBJECT
            graphics = Graphics.FromImage(bb);

            // DRAW BACKBUFFER TO SCREEN
            BBG = pictureBox2.CreateGraphics();
            BBG.DrawImage(bb, 0, 0, 640, 480);

            graphics.Clear(Color.Black);
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (!Loaded)
            {

                listBox1.Items.Clear();
                listBox2.Items.Clear();

                Files = new List<string>();
                Paths = new List<string>();
                var recursos = Path.Combine(Folder, "RECURSOS");
                if (Directory.Exists(recursos))
                {
                    Paths.AddRange(Directory.EnumerateFiles(recursos, "*.WAD", SearchOption.AllDirectories));

                }

                foreach (string Pth in Paths)
                    Files.Add(Path.GetFileName(Pth));

                listBox1.DataSource = Files;

                Loaded = true;

                timer1.Start();

            }

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Soldiers Game Editor. Created by salvadorc17 2020");
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            int id = listBox2.SelectedIndex;

            string file = "";


            if (id >= 0 && WADFile != null)
            {
                file = listBox2.SelectedItem.ToString();


                foreach (WADImage img in WADFile.Images)
                    if (img.Name == file)
                    {
                        Bitmap bmp = Utils.GetBitmap(img);

                        if (bmp != null)
                        {
                            pictureBox1.BackgroundImage = bmp;

                            label3.Text = img.Width.ToString();

                            label5.Text = img.Height.ToString();

                            label7.Text = id.ToString();

                            label9.Text = img.RawDataSize.ToString();


                        }

                    }

            }

        }

        private void button4_Click(object sender, EventArgs e)
        {

            string wad = "";
            string wimg = "";

            if (listBox1.SelectedItem != null)
             wad = listBox1.SelectedItem.ToString();

            if (listBox2.SelectedItem != null)
             wimg = listBox2.SelectedItem.ToString();

            int img = listBox2.SelectedIndex;

            if (WADFile != null && Map != null)
            {

                if (wad.Contains("FASE"))
                {
                   

                    TerrainSelected = img;

                    Image bmp = pictureBox1.BackgroundImage;

                    pictureBox3.BackgroundImage = bmp;

                    label12.Text = label7.Text;

                    Map.Terrain.Tiles.Add(new Tile {

                        X = (short)numericUpDown1.Value,
                        Y = (short)numericUpDown2.Value,
                        Z = (short)numericUpDown3.Value,
                        Value = img,
                        Type = comboBox1.SelectedIndex + 1

                    });

                    MapImages.Add((Bitmap)bmp);

                    listBox3.Items.Add(wimg);


                    TerrainId = listBox3.Items.Count - 1;


                }

            }


        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

            int id = listBox3.SelectedIndex;

            if (id >= 0)
            {
                TerrainId = id;

                if (Map.Terrain.Tiles.Count > 0 && id >= 0)
                {

                    numericUpDown1.Value = Map.Terrain.Tiles[id].X;

                    numericUpDown2.Value = Map.Terrain.Tiles[id].Y;

                    numericUpDown3.Value = Map.Terrain.Tiles[id].Z;

                    comboBox1.SelectedIndex = Map.Terrain.Tiles[id].Type - 1;

                }


            }


        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int id = listBox3.SelectedIndex;


            if (WADFile != null && Map != null)
            {

                if (Map.Terrain.Tiles.Count > 0 && id >= 0)
                {

                    Map.Terrain.Tiles[id].X = (short)numericUpDown1.Value;

                }

            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            int id = listBox3.SelectedIndex;


            if (WADFile != null && Map != null)
            {

                if (Map.Terrain.Tiles.Count > 0 && id >= 0)
                {

                    Map.Terrain.Tiles[id].Z = (short)numericUpDown3.Value;

                }

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            int id = comboBox1.SelectedIndex;

            if (WADFile != null && Map != null)
            {

                if (Map.Terrain.Tiles.Count > 0 && id >= 0)
                {

                    Map.Terrain.Tiles[id].Value = id + 1;

                }

            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                SpriteTable = new SpriteTable();
                SpriteTable.Name = textBox1.Text;
                SpriteTable.Sequences = new List<Sequence>();
                if (numericUpDown4.Value >= 1)
                {
                    SpriteTable.Sequences.Add(new Sequence());
                    SpriteTable.Sequences[0].Name = "Idle";
                    SpriteTable.Sequences[0].Frames = new List<Frame>();
                    SpriteTable.Sequences[0].Frames.Add(new Frame());
                }
                if (numericUpDown4.Value >= 2)
                {
                    SpriteTable.Sequences.Add(new Sequence());
                    SpriteTable.Sequences[1].Name = "Walk";
                    SpriteTable.Sequences[1].Frames = new List<Frame>();
                    SpriteTable.Sequences[1].Frames.Add(new Frame());
                }

                SoldierTactics.SpriteTable.Serialize(AppDomain.CurrentDomain.BaseDirectory + "/Content/Sprites/"
                   + textBox1.Text + ".xml", SpriteTable);


            }
            else
                MessageBox.Show("No animation name");




        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();


            int id = comboBox2.SelectedIndex;

            //Soldier entity
            if (id == 0)
            {

                comboBox3.Items.Add("Comando");
                comboBox3.Items.Add("Sniper");

            }
            //Enemy entity
            else if (id == 1)
            {

                comboBox3.Items.Add("Aleman");
                comboBox3.Items.Add("Aleman2");

            }

        }

        private void button6_Click(object sender, EventArgs e)
        {

            string wad = "";

            if (listBox1.SelectedItem != null)
                wad = listBox1.SelectedItem.ToString();

            string wimg = "";

            if (listBox2.SelectedItem != null)
                wimg = listBox2.SelectedItem.ToString();

            int img = listBox2.SelectedIndex;

            if (WADFile != null && Map != null)
            {

                if (wad.Contains("ALEMAN"))
                {
                    Image bmp = pictureBox1.BackgroundImage;

                    pictureBox4.BackgroundImage = bmp;

                    label12.Text = label7.Text;


                    Map.Entities.Add(new SoldierTactics.Game.Entity()
                    {
                        
                        Name = "Aleman",

                        X = (int)numericUpDown8.Value,

                        Y = (int)numericUpDown7.Value,

                        Z = (int)numericUpDown10.Value,

                        Dir = (int)numericUpDown9.Value

                    });

                }

            }

        }

        private void button7_Click(object sender, EventArgs e)
        {

            int id = listBox4.SelectedIndex;


            if (WADFile != null && Map != null)
            {

                if (Map.Soldiers.Count > 0 && listBox4.Items.Count > 0)
                {

                    Map.Soldiers.RemoveAt(id);

                    MapImages.RemoveAt(id);

                    listBox4.Items.RemoveAt(id);

                    EntityId = listBox4.Items.Count - 1;

                }

            }
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            int id = listBox3.SelectedIndex;


            if (WADFile != null && Map != null)
            {

                if (Map.Soldiers.Count > 0 && id >= 0)
                {

                    Map.Soldiers[id].X = (short)numericUpDown8.Value;

                }

            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Files.Clear();
            Paths.Clear();
            MapImages.Clear();
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            int id = listBox3.SelectedIndex;


            if (WADFile != null && Map != null)
            {

                if (Map.Soldiers.Count > 0 && id >= 0)
                {

                    Map.Soldiers[id].Y = (short)numericUpDown7.Value;

                }

            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = comboBox3.SelectedIndex;

            if (id > 0)
                SelEntity = Map.Entities.ElementAt(id);
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = listBox4.SelectedIndex;

            if (id >= 0)
            {

                SelEntity = Map.Entities.ElementAt(id);
                comboBox3.SelectedIndex = id;
            }

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (WADFile != null && Map != null)
                SaveMap();
               
            
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            int id = listBox3.SelectedIndex;


            if (WADFile != null && Map != null)
            {

                if (Map.Terrain.Tiles.Count > 0 && id >= 0)
                {

                    Map.Terrain.Tiles[id].Y = (short)numericUpDown2.Value;

                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            int id = listBox3.SelectedIndex;


            if (WADFile != null && Map != null)
            {

                if (Map.Terrain.Tiles.Count > 0 && listBox3.Items.Count > 0)
                {

                    Map.Terrain.Tiles.RemoveAt(id);

                    MapImages.RemoveAt(id);

                    listBox3.Items.RemoveAt(id);

                    TerrainId = listBox3.Items.Count - 1;

                }

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            string wad = listBox1.SelectedItem.ToString();

            int img = listBox2.SelectedIndex;

            if (WADFile != null && Map != null)
            {

                if (wad.Contains("FASE"))
                {




                }

            }



        }

        private void button2_Click(object sender, EventArgs e)
        {
            WADFile = null;
            Map = null;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (WADFile != null && Map != null)
                DrawMap();

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            listBox2.Items.Clear();

            int id = listBox1.SelectedIndex;
            string file = "";

            if (id >= 0 && Paths.Count > 0)
            {

                file = Paths[id];

                WADFile = new WAD(file, true);

                foreach (WADImage img in WADFile.Images)
                    listBox2.Items.Add(img.Name);

                WadMap = WADFile.Name;
            }

        }
    }
}
