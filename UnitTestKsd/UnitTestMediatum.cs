using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace UnitTestKsd
{
    using Ksd.Mediatum;

    [TestClass]
    public class UnitTestMediatum
    {
        OAuth oAuth;
        Ksd.Mediatum.Server server;

        public UnitTestMediatum()
        {
            oAuth = new OAuth();
            server = new Ksd.Mediatum.Server(oAuth, Server.ServerNameInRegistry);
            server.TypeTable.Add("image/project-arc", typeof(FloorPlanNode));
        }

        [TestMethod]
        public void TestParentsAndChildren()
        {
            Node arSearchboxNode = server.GetNode(1085713);
            foreach (Node parent in arSearchboxNode.Parents) ;
            foreach (Node child in arSearchboxNode.Parents) ;
        }

        [TestMethod]
        public void TestFloorPlanNode()
        {
            FloorPlanNode floorNode = (FloorPlanNode)server.GetNode(1229104);
        }

        [TestMethod]
        public void TestUploadAndUpdate()
        {
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(100, 100);
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);

            stream.Position = 0;
            byte[] binaryData = new byte[stream.Length];
            stream.Read(binaryData, 0, (int)stream.Length);

            Node node = server.GetNode(1219448);

            FloorPlanNode loadedNode = (FloorPlanNode)FloorPlanNode.CreateFloorPlanNode(node, "Upload Test", "Christoph Langenhan", "Germany", new Uri("http://reference.ksd.ai.ar.tum.de:8080/AgraphMLDownloadService/DownloadAgraphml?ifcid=1IEeEPbCv1je0WG2vzMPyH&neo4jurl=http://localhost:7474&floorlevel=0.0"), binaryData);
            
            loadedNode.Architect = "Langenhan Christoph";
            loadedNode.Country = "Germany";
            loadedNode.SetAttributeValue("link", "http://heise.de");
            loadedNode.Update();
            
            foreach (NodeFile file in loadedNode.Files)
                file.Download();
        }
    }
}
