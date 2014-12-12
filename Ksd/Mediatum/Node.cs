using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.IO;

namespace Ksd.Mediatum
{
    /**
     <summary>  Represents a node in MediaTUM. </summary>
    
     <remarks>  Dr. Torsten Thurow, TU München, 22.07.2014. </remarks>
     */
    [Serializable()]
    public class Node
    {
        /**
         <summary>  Gets the MediaTUM server. </summary>
        
         <value>    The MediaTUM server. </value>
         */
        public Server Server { get; private set; }
        
        /**
         <summary>  Gets the node id. </summary>
        
         <value>    The node id. </value>
         */
        public UInt32 ID { get; private set; }

        /**
         <summary>  Gets the node name. </summary>
        
         <value>    The node name. </value>
         */
        public string Name { get; set; }

        /**
         <summary>  Gets the node type. </summary>
        
         <value>    The node type. </value>
         */
        public string Type { get; private set; }

        /**
         <summary>  Parse a node list. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 11.12.2014. </remarks>
        
         <param name="server">              The MediaTUM server. </param>
         <param name="result">              The original XML result that conatins the node list. </param>
         <param name="response">            The XML node that contains the response with the node list. </param>
         <param name="xmlNamespaceManager"> Manager for XML namespace. </param>
         <param name="prefix">              The right XML prefix. </param>
         <param name="nodeInsert">          A action for all readed nodes. </param>
         */
        internal static void ParseNodeList(Server server, string result, XmlNode response, XmlNamespaceManager xmlNamespaceManager, string prefix, Action<Node> nodeInsert)
        {
            XmlNode xmlNodeList = response[prefix + "nodelist"];
            int numberOfNodes = Convert.ToInt32(xmlNodeList.Attributes["countall"].Value);

            for (int nodeIndex = 0; nodeIndex < numberOfNodes; nodeIndex++)
            {
                XmlNode nextXmlNode = xmlNodeList.ChildNodes[nodeIndex];
                UInt32 newId = Convert.ToUInt32(nextXmlNode.Attributes["id"].Value);

                Node node;
                if (!server.nodeTable.TryGetValue(newId, out node))
                {
                    string typeOfNewNode = nextXmlNode.Attributes["type"].Value;
                    node = server.CreateNode(typeOfNewNode, (XmlElement)nextXmlNode, result, prefix, xmlNamespaceManager);
                }

                nodeInsert(node);
            }
        }

        private List<Node> parents;

        /**
         <summary>  Gets the parent nodes. </summary>
        
         <value>    The parent node. </value>
         */
        public IEnumerable<Node> Parents
        { 
            get
            {
                if (this.parents != null)
                    return this.parents;

                this.parents = new List<Node>();
                Uri uri;
                try
                {
                    string result = this.Server.Export(this.ID, "parents", out uri);
                    string cleanedResult = Node.CleanXml(result);

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cleanedResult);
                    XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(doc.NameTable);

                    XmlNode xmlNode = doc["response"];
                    ParseNodeList(this.Server, result, xmlNode, xmlNamespaceManager, "", (node) => { this.parents.Add(node); });
                }
                catch (System.Net.WebException ex)
                {

                }

                return this.parents;
            }
        }

        private List<Node> children;

        /**
         <summary>  Gets the children nodes. </summary>
        
         <value>    The children nodes. </value>
         */
        public IEnumerable<Node> Children
        {
            get
            {
                if (this.children != null)
                    return this.children;

                this.children = new List<Node>();
                Uri uri;
                try
                {
                    string result = this.Server.Export(this.ID, "children", out uri);
                    string cleanedResult = Node.CleanXml(result);

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(cleanedResult);
                    XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(doc.NameTable);

                    XmlNode xmlNode = doc["response"];
                    ParseNodeList(this.Server, result, xmlNode, xmlNamespaceManager, "", (node) => { this.children.Add(node); });
                }
                catch (System.Net.WebException ex)
                {

                }

                return this.children;
            }
        }

        /**
         <summary>  Gets the read roles. </summary>
        
         <value>    The read roles. </value>
         */
        public string Read { get; private set; }

        /**
         <summary>  Gets the write roles. </summary>
        
         <value>    The write roles. </value>
         */
        public string Write { get; private set; }

        public string Data { get; private set; }

        /**
         <summary>  Gets the original XML input for this Node. </summary>
        
         <value>    The original XML input for this Node. </value>
         */
        public string Xml { get; private set; }

        /**
         <summary>  Gets all attributes of the node. </summary>
        
         <value>    The attributes of the node. </value>
         */
        public IDictionary<string, NodeAttribute> Attributes { get; internal set; }

        /**
         <summary>  Gets all masks of the node. </summary>
        
         <value>    The masks of the node. </value>
         */
        public IDictionary<string, NodeMask> Masks { get; internal set; }

        private IList<NodeFile> files;

        /**
         <summary>  Gets all files of the node. </summary>
        
         <value>    The files of the node. </value>
         */
        public IEnumerable<NodeFile> Files { get { return this.files; } }

        static string GetOptionalAttribute(XmlNode xmlNode, string name)
        {
            XmlAttribute attribute = xmlNode.Attributes[name];
            if (attribute == null)
                return null;

            return attribute.Value;
        }

        /**
         <summary>  Clean up MediaTUM XML from wrong CDATA sections. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 10.12.2014. </remarks>
        
         <param name="xml"> The original XML input. </param>
        
         <returns>  A string that contains the cleaned up XML. </returns>
         */
        public static string CleanXml(string xml)
        {
            if (xml == "")
                return null;

            string[] resultArray = xml.Split('\n');

            bool ok = true;

            int counter = 0;

            do
            {
                ok = true;
                string newResult = resultArray[0];
                for (int index = 1; index < resultArray.Length; index++)
                    newResult += '\n' + resultArray[index];

                XmlDocument xmlResponse = new XmlDocument();
                try
                {
                    xmlResponse.LoadXml(newResult);
                    return newResult;
                }
                catch (System.Xml.XmlException e)
                {
                    string line = resultArray[e.LineNumber - 1];

                    if (ok)
                        ;// errors.WriteLine(uri.OriginalString);

                    //errors.WriteLine(String.Format("XmlException by line {0}, position {1}, >>>\"{3}\"<<<", e.LineNumber, e.LinePosition, line));
                    //errors.Flush();

                    line = line.Length > 1 ? line.Remove(e.LinePosition - 1, 1) : " ";

                    resultArray[e.LineNumber - 1] = line;
                    ok = false;
                    counter++;
                }

            }
            while (!ok && counter < 20);

            return null;
        }

        /**
         <summary>  Parse a node. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 11.12.2014. </remarks>
        
         <param name="xmlNode">             Element describing the XML node. </param>
         <param name="xmlNamespaceManager"> Manager for XML namespace. </param>
         <param name="prefix">              The right XML prefix. </param>
         */
        void ParseNode(XmlElement xmlNode, XmlNamespaceManager xmlNamespaceManager, string prefix)
        {
            this.Name = xmlNode.Attributes["name"].Value;
            this.ID = Convert.ToUInt32(xmlNode.Attributes["id"].Value);
            this.Type = xmlNode.Attributes["type"].Value;

            this.Read = GetOptionalAttribute(xmlNode, "read");
            this.Write = GetOptionalAttribute(xmlNode, "write");
            this.Data = GetOptionalAttribute(xmlNode, "data");

            this.Attributes = new Dictionary<string, NodeAttribute>();
            this.Masks = new Dictionary<string, NodeMask>();
            this.files = new List<NodeFile>();

            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                switch (node.LocalName)
                {
                    case "attribute":
                        {
                            string attributeName = node.Attributes["name"].Value;
                            string value;

                            if (prefix == "")
                            {
                                XmlNode childNode = node.ChildNodes[0];
                                XmlCDataSection cdataSection = childNode as XmlCDataSection;
                                value = cdataSection.Value;
                            }
                            else
                                value = node.InnerText;

                            this.Attributes.Add(attributeName, new NodeAttribute(value, false));
                            break;
                        }

                    case "file":
                        {
                            NodeFile file = new NodeFile(this, (XmlElement)node);
                            this.files.Add(file);
                            break;
                        }

                    case "mask":
                        {
                            NodeMask mask = new NodeMask(this, (XmlElement)node);
                            this.Masks.Add(mask.Name, mask);
                            break;
                        }
                }
            }

            /*
            foreach (XmlNode attributeNode in xmlNode.SelectNodes("attribute", xmlNamespaceManager))
            {
                string attributeName = attributeNode.Attributes["name"].Value;
                XmlNode childNode = attributeNode.ChildNodes[0];
                XmlCDataSection cdataSection = childNode as XmlCDataSection;
                string value = cdataSection.Value;

                this.Attributes.Add(attributeName, new NodeAttribute(value, false));
            }

            foreach (XmlNode fileNode in xmlNode.SelectNodes(prefix + "file", xmlNamespaceManager))
            {
                NodeFile file = new NodeFile(this, (XmlElement)fileNode);
                this.files.Add(file);
            }

            foreach (XmlNode maskNode in xmlNode.SelectNodes(prefix + "mask", xmlNamespaceManager))
            {
                NodeMask mask = new NodeMask(this, (XmlElement)maskNode);
                this.Masks.Add(mask.Name, mask);
            }
             */
        }

        /**
         <summary>  Gets a attribute value. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 28.07.2014. </remarks>
        
         <param name="name">    The name of the attribute. </param>
        
         <returns>  The value of the attribute. </returns>
         */
        public string GetAttributeValue(string name)
        {
            return this.Attributes[name].Value;
        }

        /**
         <summary>  Sets a attribute value. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 28.07.2014. </remarks>
        
         <param name="name">    The name of the attribute. </param>
         <param name="value">   The new value of the attribute. </param>
         */
        public void SetAttributeValue(string name, string value)
        {
            NodeAttribute attribute;
            if (this.Attributes.TryGetValue(name, out attribute))
            {
                attribute.Value = value;
                return;
            }

            this.Attributes.Add(name, new NodeAttribute(value, true));
        }

        /**
         <summary>  Creates a node. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 11.12.2014. </remarks>
        
         <param name="server">  The MediaTUM server. </param>
         <param name="nodeId">  Identifier for the node. </param>
        
         <returns>  The new node. </returns>
         */
        internal static Node CreateNode(Server server, UInt32 nodeId)
        {
            Uri uri;
            string result = server.Export(nodeId, "", out uri);

            string cleanedResult = Node.CleanXml(result);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(cleanedResult);
            XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(doc.NameTable);

            XmlElement xmlNode = (XmlElement)(doc["response"])["node"];
           
            string typeOfNewNode = xmlNode.Attributes["type"].Value;

            return server.CreateNode(typeOfNewNode, xmlNode, result, "", xmlNamespaceManager);
        }

        /**
         <summary>  Specialised constructor for use only by derived classes. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 28.07.2014. </remarks>
        
         <param name="server">              The MediaTUM server. </param>
         <param name="xmlNode">             Element describing the XML node. </param>
         <param name="xml">                 The original XML input for this Node. </param>
         <param name="prefix">              The right XML prefix. </param>
         <param name="xmlNamespaceManager"> Manager for XML namespace. </param>
         */
        public Node(Server server, XmlElement xmlNode, string xml, string prefix, XmlNamespaceManager xmlNamespaceManager)
        {
            this.Server = server;
            this.Xml = xml;

            ParseNode(xmlNode, xmlNamespaceManager, prefix);
            this.Server.nodeTable.Add(this.ID, this);
        }

        /**
         <summary>  Converts a attribute table to JSON format. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 25.07.2014. </remarks>
        
         <param name="table">   The attribute table. </param>
        
         <returns>  The attribute table in JSON format. </returns>
         */
        static string AttributeTable2Json(IDictionary<string, string> table)
        {
            Newtonsoft.Json.JsonSerializer jsonSerializer = new Newtonsoft.Json.JsonSerializer();
            StringWriter textWriter = new StringWriter();
            jsonSerializer.Serialize(textWriter, table);
            return textWriter.ToString();
        }

        /**
         <summary>  Gets all modifyed attributes in JSON format. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 25.07.2014. </remarks>
        
         <returns>  The modifyed attributes in JSON format. </returns>
         */
        string ModifyedAttributes2Json()
        {
            SortedDictionary<string, string> map = new SortedDictionary<string,string>();
            foreach (KeyValuePair<string, NodeAttribute> pair in this.Attributes)
                if (pair.Value.Modifyed)
                    map.Add(pair.Key, pair.Value.Value);
            
            return AttributeTable2Json(map);
        }

        /**
         <summary>  Uploads a file to an node . </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 16.07.2014. </remarks>
        
         <param name="type">        The nodetype of new node. User full quantifier objectype/schema. </param>
         <param name="name">        The name of node, will be stored in the node table, field name. </param>
         <param name="metadata">    The metadatafields to given file. </param>
         <param name="data">        The file in binary form. </param>
        
         <returns>  The new node of the uploaded file; </returns>
         */
        public Node Upload(string type, string name, IDictionary<string, string> metadata, byte[] data)
        {
            UInt32 newId = this.Server.Upload(this.ID, type, name, AttributeTable2Json(metadata), data);
            Node newNode = Node.CreateNode(this.Server, newId);
            if (this.children != null)
                this.children.Add(newNode);

            return newNode;
        }

        /**
         <summary>  Sends all modifications of the node to the server. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 18.07.2014. </remarks>        
         */
        public void Update()
        {
            string metadata = this.ModifyedAttributes2Json();

            this.Server.Update(this.ID, this.Name, metadata);
            
            Uri uri;
            string result = this.Server.Export(this.ID, "", out uri);
            this.Xml = result;

            result = Node.CleanXml(result);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);
            XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(doc.NameTable);

            XmlElement xmlNode = (XmlElement)(doc["response"])["node"];

            ParseNode(xmlNode, xmlNamespaceManager, "");
        }

        /**
         <summary>  Gets the creator of the node. </summary>
        
         <value>    The creator of the node. </value>
         */
        public string Creator
        {
            get
            {
                return this.GetAttributeValue("creator");
            }
        }

        /**
         <summary>  Gets the creation time of the node. </summary>
        
         <value>    The creation time of the node. </value>
         */
        public DateTime CreationTime
        {
            get
            {
                return Convert.ToDateTime(this.GetAttributeValue("creationtime"));
            }
        }

        /**
         <summary>  Gets the update user of the node. </summary>
        
         <value>    The update user of the node. </value>
         */
        public string UpdateUser
        {
            get
            {
                return this.GetAttributeValue("updateuser");
            }
        }

        /**
         <summary>  Gets the last update time of the node. </summary>
        
         <value>    The last update time of the node. </value>
         */
        public DateTime UpdateTime
        {
            get
            {
                return Convert.ToDateTime(this.GetAttributeValue("updatetime"));
            }
        }
    }
}
