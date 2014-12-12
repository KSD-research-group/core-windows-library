using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace UnitTestKsd
{
    using Ksd.QueryWeighting;

    [TestClass]
    public class UnitTestQueryWeighting
    {
        Ksd.QueryWeighting.Server server;

        public UnitTestQueryWeighting()
        {
            server = new Ksd.QueryWeighting.Server(Ksd.QueryWeighting.Server.ServerNameInRegistry);
            Ksd.Mediatum.OAuth oAuth = new Ksd.Mediatum.OAuth();
            server.MediaTumServer = new Ksd.Mediatum.Server(oAuth, Ksd.Mediatum.Server.ServerNameInRegistry);
        }

        [TestMethod]
        public void TestStatusCheck()
        {
            server.StatusCheck();
        }

        [TestMethod]
        public void Unified()
        {
            QueryResult result = server.Unified(File.ReadAllText("../../TestData/system/unified-query.xml"));
            foreach(Ksd.Mediatum.Node node in result.MediaTumNodes)
            {
                foreach(Ksd.Mediatum.Node child in node.Children)
                    ;
                
                foreach(Ksd.Mediatum.Node parent in node.Parents)
                    ;
            }

            result = server.Unified(File.ReadAllText("../../TestData/system/unified-query-only-city.xml"));
            result = server.Unified(File.ReadAllText("../../TestData/system/unified-query-only-graph.xml"));
            result = server.Unified(File.ReadAllText("../../TestData/system/unified-query-only-neo4j.xml"));
        }
    }
}
