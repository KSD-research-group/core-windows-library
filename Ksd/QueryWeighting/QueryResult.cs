using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace Ksd.QueryWeighting
{
    [Serializable()]
    public class QueryResult
    {
        /**
         <summary>  Gets the Query Weighting Server. </summary>
        
         <value>    The server. </value>
         */
        public Server Server { get; internal set; }

        /**
         <summary>  Gets the original result XML. </summary>
        
         <value>    The original result XML. </value>
         */
        public string ResultXml { get; internal set; }

        List<Ksd.Mediatum.Node> mediaTumNodes = new List<Mediatum.Node>();

        /**
         <summary>  Gets the MediaTUM nodes. </summary>
        
         <value>    The MediaTUM nodes. </value>
         */
        public IList<Ksd.Mediatum.Node> MediaTumNodes { get { return mediaTumNodes; } }


        void ParseMediaTumPerCity(Uri uri, string result, XmlNode node, int weight, Ksd.Mediatum.Server mediaTumServer, XmlNamespaceManager xmlNamespaceManager)
        {
            XmlNode response = node["res:response"];
            Ksd.Mediatum.Node.ParseNodeList(
                mediaTumServer,
                result,
                response,
                xmlNamespaceManager,
                "res:",
                (mediaTumNode) => { mediaTumNodes.Add(mediaTumNode); });
        }

        void ParseMediaTumPerGps(XmlNode node, int weight)
        {

        }

        void ParseGraphMatcher(XmlNode node, int weight)
        {

        }

        void ParseNeo4J(XmlNode node, int weight)
        {

        }

        internal QueryResult(Uri uri, string xml, Server server)
        {
            this.ResultXml = xml;
            this.Server = server;

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNamespaceManager manager = new XmlNamespaceManager(doc.NameTable);
            manager.AddNamespace("res", "http://xxxxxx Some URL XXXX");

            XmlNode unifiedResponse = doc["res:unifiedResponse"];
            XPathNavigator nav = doc.CreateNavigator();

            foreach (XmlNode response in unifiedResponse.ChildNodes)
            {
                if (response.Name != "res:response")
                    continue;

                int weight = -1;
                foreach (XmlNode responseChild in response.ChildNodes)
                {
                    if (responseChild.Prefix != "res")
                        continue;

                    switch (responseChild.LocalName)
                    {
                        case "weight":
                            weight = Convert.ToInt32(responseChild.InnerText);
                            break;

                        case "graphMatcher":
                            ParseGraphMatcher(responseChild, weight);
                            break;

                        case "mediatumByCity":
                            ParseMediaTumPerCity(uri, xml, responseChild, weight, server.MediaTumServer, manager);
                            break;

                        case "mediatum-gps":
                            ParseMediaTumPerGps(responseChild, weight);
                            break;

                        case "neo4jByRooms":
                            ParseNeo4J(responseChild, weight);
                            break;
                    }
                }
            }
        }
    }
}
