﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaTum1
{
    public partial class Form1 : Form
    {
//        Ksd.Mediatum.OAuth oAuth = new Ksd.Mediatum.OAuth("ga76juy", "b33db3e738ee71a");
        Ksd.Mediatum.OAuth oAuth = new Ksd.Mediatum.OAuth("ga54yoc", "6ababd6e13fe7df");

        public Form1()
        {
            InitializeComponent();
        }

 
        private void buttonCheck_Click(object sender, EventArgs e)
        {
            // Test 1085713 // frei sichtbar
            // Grundrissanalyse 1220070 // verborgen
            // Mobil 1219454
            // BIMServer 1219448
            // ar:searchbox 1085713
            Ksd.Mediatum.Server server = new Ksd.Mediatum.Server(this.oAuth);
            Ksd.Mediatum.Node node = server.GetNode(1225255);
            List<Ksd.Mediatum.Node> children = new List<Ksd.Mediatum.Node>(node.Children);
            List<Ksd.Mediatum.Node> parents = new List<Ksd.Mediatum.Node>(node.Parents);
            foreach (Ksd.Mediatum.NodeFile file in node.Files)
                file.Download();

            OpenFileDialog openFileDialog = new OpenFileDialog();

            // openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            // openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            System.IO.FileStream inFile;
            byte[] binaryData;

            try
            {
                inFile = new System.IO.FileStream(openFileDialog.FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                binaryData = new Byte[inFile.Length];
                long bytesRead = inFile.Read(binaryData, 0, (int)inFile.Length);
                inFile.Close();
            }
            catch (System.Exception ex)
            {
                // Error creating stream or reading from it.
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                return;
            }

	        String name = "arno_test_java";
            UInt32 parent = 1219448;
	        String type = "image/project-arc";
	        String metadata = "{\"nodename\":\"arno_test_node\"}";

            node = server.GetNode(parent);

            Ksd.Mediatum.Node loadedNode = node.Upload(type, name, metadata, binaryData);

            loadedNode.Update(name, metadata);
        }
    }
}

