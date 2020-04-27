
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

        private List<string> Files;

        private List<WAD> WADFiles;

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

                Files = new List<string>();
                var recursos = Path.Combine(Folder, "RECURSOS");
                if (Directory.Exists(recursos))
                {
                    Files.AddRange(Directory.EnumerateFiles(recursos, "*.WAD", SearchOption.AllDirectories));
                    // TODO: The used RLE format cannot be read ad the moment
                    // files.AddRange(Directory.EnumerateFiles(recursos, "*.RLE", SearchOption.AllDirectories));
                }

                foreach (string File in Files)
                    WADFiles.Add(new WAD(File));


                listBox1.DataSource = WADFiles;
            }

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Soldiers Game Editor. Created by salvadorc17 2020");
        }


    }
}
