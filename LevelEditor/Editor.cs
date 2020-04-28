﻿
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
        private List<Bitmap> MapImages;

        private List<string> Files, Paths;

        private WAD WADFile;

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

            if (Map.Terrain.Floors.Count > 0 && MapImages.Count > 0)
                for (int i = 0; i < Map.Terrain.Floors.Count; i++)
                {
                    for (int z = 0; z < 10; z++)
                        if (Map.Terrain.Floors[i].Z == z)
                        {
                            drect = new Rectangle(Map.Terrain.Floors[i].X, Map.Terrain.Floors[i].Y,
                                MapImages[i].Width, MapImages[i].Height);
                            graphics.DrawImage(MapImages[i], drect);
                        }
                }

           if ( Map.Terrain.Floors.Count > 0)
            graphics.DrawRectangle(Pens.Red, new Rectangle(Map.Terrain.Floors[TerrainId].X,
                Map.Terrain.Floors[TerrainId].Y, MapImages[TerrainId].Width, MapImages[TerrainId].Height));

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

            string wad = listBox1.SelectedItem.ToString();

            string wimg = listBox2.SelectedItem.ToString();

            int img = listBox2.SelectedIndex;

            if (WADFile != null && Map != null)
            {

                if (wad.Contains("FASE"))
                {
                   

                    TerrainSelected = img;

                    Image bmp = pictureBox1.BackgroundImage;

                    pictureBox3.BackgroundImage = bmp;

                    label12.Text = label7.Text;

                    Map.Terrain.Floors.Add(new Floor {

                        X = (short)numericUpDown1.Value,
                        Y = (short)numericUpDown2.Value,
                        Z = (short)numericUpDown3.Value,
                        Value = img

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
                TerrainId = id;

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int id = listBox3.SelectedIndex;


            if (WADFile != null && Map != null)
            {

                if (Map.Terrain.Floors.Count > 0 && id >= 0)
                {

                    Map.Terrain.Floors[id].X = (short)numericUpDown1.Value;

                }

            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            int id = listBox3.SelectedIndex;


            if (WADFile != null && Map != null)
            {

                if (Map.Terrain.Floors.Count > 0 && id >= 0)
                {

                    Map.Terrain.Floors[id].Z = (short)numericUpDown3.Value;

                }

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

                if (Map.Terrain.Floors.Count > 0 && id >= 0)
                {

                    Map.Terrain.Floors[id].Y = (short)numericUpDown2.Value;

                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            int id = listBox3.SelectedIndex;


            if (WADFile != null && Map != null)
            {

                if (Map.Terrain.Floors.Count > 0 && listBox3.Items.Count > 0)
                {

                    Map.Terrain.Floors.RemoveAt(id);

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

                WADFile = new WAD(file);

                foreach (WADImage img in WADFile.Images)
                    listBox2.Items.Add(img.Name);

                WadMap = WADFile.Name;
            }

        }
    }
}
