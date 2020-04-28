
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

namespace LevelEditor
{
    public partial class Editor : Form
    {

        private string Folder;

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


            if (!Directory.Exists(Folder))
                MessageBox.Show("Game folder not found");

            Loaded = false;

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
                    // TODO: The used RLE format cannot be read ad the moment
                    // files.AddRange(Directory.EnumerateFiles(recursos, "*.RLE", SearchOption.AllDirectories));
                }

                foreach (string Pth in Paths)
                    Files.Add(Path.GetFileName(Pth));

                listBox1.DataSource = Files;

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

                            label7.Text = img.ColorPaletteIndex.ToString();

                            label9.Text = img.RawDataSize.ToString();


                        }

                    }

            }

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


            }

        }
    }
}
