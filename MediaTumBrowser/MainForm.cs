using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MediaTumBrowser
{
    public partial class MainForm : Form
    {
//        Ksd.Mediatum.OAuth oAuth = new Ksd.Mediatum.OAuth("ga76juy", "b33db3e738ee71a");
        Ksd.Mediatum.OAuth oAuth = new Ksd.Mediatum.OAuth("ga54yoc", "6ababd6e13fe7df");

        public MainForm()
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
            server.typeTable.Add("image/project-arc", typeof(Ksd.Mediatum.FloorPlanNode));

            Uri uri;
            //string scheme = server.MetaData("image/project-arc", out uri);

            //string appdefinitions = server.AppDefinitions("image/project-arc", out uri);

            Ksd.Mediatum.FloorPlanNode floorNode = (Ksd.Mediatum.FloorPlanNode)server.GetNode(1225255);
            string result = floorNode.GetGraphMl();
            List<Ksd.Mediatum.Node> children = new List<Ksd.Mediatum.Node>(floorNode.Children);
            List<Ksd.Mediatum.Node> parents = new List<Ksd.Mediatum.Node>(floorNode.Parents);
            foreach (Ksd.Mediatum.NodeFile file in floorNode.Files)
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
            String metadata = "{\"architect\":\"Christoph Langenhahn\",\"roomgraph\":\"http://reference.ksd.ai.ar.tum.de:8080/AgraphMLDownloadService/DownloadAgraphml?ifcid=1IEeEPbCv1je0WG2vzMPyH&neo4jurl=http://localhost:7474&floorlevel=0.0\"}";

            Ksd.Mediatum.Node node = server.GetNode(parent);

            Ksd.Mediatum.Node loadedNode = node.Upload(type, name, metadata, binaryData);

            metadata = "{\"architect\":\"Langenhahn Christoph\"}";
            loadedNode.Update(name, metadata);
        }
    }
}

