using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace UnitTestKsd
{
    using Ksd.Mediatum;

    [TestClass]
    public class UnitTestSurfidPlugin
    {
        OAuth oAuth;
        Ksd.Mediatum.Surfid.Server server;

        public UnitTestSurfidPlugin()
        {
            oAuth = new OAuth("ArSurfId", "1234567890abcdef");
            server = new Ksd.Mediatum.Surfid.Server(oAuth, "reference.ksd.ai.ar.tum.de:8081");
            server.TypeTable.Add("image/project-arc", typeof(FloorPlanNode));
        }

        [TestMethod]
        public void TestSurfidImages()
        {
            foreach (Ksd.Mediatum.Surfid.ImageNode imageNode in server.Images)
            {
                Image image = imageNode.Image;
                image = imageNode.Thumb2;
                image = imageNode.Thumb;
                Uri uri = imageNode.FingerprintUri;
                imageNode.GetFingerprint();
            }
        }

        [TestMethod]
        public void TestSurfidProjects()
        {
            foreach (Ksd.Mediatum.Surfid.ProjectNode projectNode in server.Projects)
            {
                Image image = projectNode.Image;
                image = projectNode.Thumb2;
                image = projectNode.Thumb;
                Uri uri = projectNode.FingerprintUri;
                projectNode.GetFingerprint();
                IEnumerable<Ksd.Mediatum.Surfid.ImageNode> children = projectNode.Children;
            }
        }
    }
}
