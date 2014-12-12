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
        public void TestQueryWeightingStatusCheck()
        {
            server.StatusCheck();
        }


        void TestChildren(Ksd.Mediatum.Node node)
        {
            foreach (Ksd.Mediatum.Node child in node.Children)
                TestChildren(child);
        }

        void TestParent(Ksd.Mediatum.Node node)
        {
            foreach (Ksd.Mediatum.Node parent in node.Parents)
                TestParent(parent);
        }

        [TestMethod]
        public void TestQueryWeightingUnified()
        {
            string[] querys =
            {
                "../../TestData/system/unified-query.xml",
                "../../TestData/system/unified-query-only-city.xml",
                "../../TestData/system/unified-query-only-graph.xml",
                "../../TestData/system/unified-query-only-neo4j.xml"
            };

            foreach (string query in querys)
            {
                QueryResult result = server.Unified(File.ReadAllText(query));

                foreach (Ksd.Mediatum.Node node in result.MediaTumNodes)
                {
                    TestParent(node);
                    TestChildren(node);
                }
            }
        }
    }
}
