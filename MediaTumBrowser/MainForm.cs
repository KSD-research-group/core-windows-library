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
using Microsoft.Win32;


namespace MediaTumBrowser
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.textBoxUserName.Text = Ksd.Mediatum.OAuth.UserNameInRegistry;
            this.textBoxPresharedTag.Text = Ksd.Mediatum.OAuth.PresharedTagInRegistry;
            this.textBoxMediaTumUri.Text = Ksd.Mediatum.Server.ServerNameInRegistry;

            Ksd.Mediatum.Server.traceSwitch.Level = System.Diagnostics.TraceLevel.Info;
        }

        private void buttonCheck_Click(object sender, EventArgs e)
        {
            Ksd.Mediatum.OAuth oAuth = new Ksd.Mediatum.OAuth(this.textBoxUserName.Text, this.textBoxPresharedTag.Text);
            oAuth.WriteSigningConfigToRegistry();
            
            Ksd.Mediatum.Server server = new Ksd.Mediatum.Server(oAuth, this.textBoxMediaTumUri.Text);
            Ksd.Mediatum.Server.ServerNameInRegistry = server.ServerName;
            
            server.TypeTable.Add("image/project-arc", typeof(Ksd.Mediatum.FloorPlanNode));

            Ksd.Mediatum.Node rootNode = server.GetNode(1085713);


            Ksd.Mediatum.FloorPlanNode floorNode = (Ksd.Mediatum.FloorPlanNode)server.GetNode(1229104);

            // Test 1222076
            // Test 1085713 // frei sichtbar
            // Grundrissanalyse 1220070 // verborgen
            // Mobil 1219454
            // BIMServer 1219448
            // ar:searchbox 1085713

            Uri uri;
            //string scheme = server.MetaData("image/project-arc", out uri);

            //string appdefinitions = server.AppDefinitions("image/project-arc", out uri);

//            string result = floorNode.GetGraphMl();
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

            UInt32 parent = 1219448;
            Ksd.Mediatum.Node node = server.GetNode(parent);

            Ksd.Mediatum.FloorPlanNode loadedNode = (Ksd.Mediatum.FloorPlanNode)Ksd.Mediatum.FloorPlanNode.CreateFloorPlanNode(node, "Upload Test", "Christoph Langenhan", "Germany", new Uri("http://reference.ksd.ai.ar.tum.de:8080/AgraphMLDownloadService/DownloadAgraphml?ifcid=1IEeEPbCv1je0WG2vzMPyH&neo4jurl=http://localhost:7474&floorlevel=0.0"), binaryData);
            loadedNode.Architect = "Langenhan Christoph";
            loadedNode.Country = "Germany";
            loadedNode.SetAttributeValue("link", "http://heise.de");
            loadedNode.Update();
            foreach (Ksd.Mediatum.NodeFile file in loadedNode.Files)
                file.Download();
        }
    }
}

