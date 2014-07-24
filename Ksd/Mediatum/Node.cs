using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.IO;

namespace Ksd.Mediatum
{
    [Serializable()]
    public class NodeFile
    {
        public string Type { get; set; }
        public string MimeType { get; set; }
        public string Filename { get; set; }

        public Node Parent { get; set; }

        public NodeFile()
        { }

        public NodeFile(Node parent, XmlElement xmlNode)
        {
            this.Parent = parent;
            this.Filename = xmlNode.Attributes["filename"].Value;
            this.MimeType = xmlNode.Attributes["mime-type"].Value;
            this.Type = xmlNode.Attributes["type"].Value;
        }

        public byte[] Download()
        {
            Uri uri;
            return this.Parent.Server.Download(this.Parent.ID, Filename, out uri);
        }
    }

    [Serializable()]
    public class NodeMask
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public Node Parent { get; set; }

        public NodeMask()
        { }

        public NodeMask(Node parent, XmlElement xmlNode)
        {
            this.Parent = parent;
            this.Name = xmlNode.Attributes["name"].Value;
            XmlNode childNode = xmlNode.ChildNodes[0];
            XmlCDataSection cdataSection = childNode as XmlCDataSection;
            this.Value = cdataSection.Value;
        }
    }

    /**
     <summary>  Represents a node in MediaTUM. </summary>
    
     <remarks>  Dr. Torsten Thurow, TU München, 22.07.2014. </remarks>
     */
    [Serializable()]
    public class Node
    {
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
                    XmlNode xmlNodeResponse = GetXmlNodeResponse(uri, result);
                    if (xmlNodeResponse != null)
                    {
                        XmlNode xmlNodeList = xmlNodeResponse["nodelist"];
                        int numberOfNodes = Convert.ToInt32(xmlNodeList.Attributes["countall"].Value);

                        for (int parentIndex = 0; parentIndex < numberOfNodes; parentIndex++)
                        {
                            XmlNode nextXmlNode = xmlNodeList.ChildNodes[parentIndex];
                            UInt32 newId = Convert.ToUInt32(nextXmlNode.Attributes["id"].Value);

                            Node parentNode;
                            if (!this.Server.nodeTable.TryGetValue(newId, out parentNode))
                            {
                                string typeOfNewNode = nextXmlNode.Attributes["type"].Value;
                                parentNode = this.Server.CreateNode(typeOfNewNode, (XmlElement)nextXmlNode, result);
                            }
   
                            this.parents.Add(parentNode);
                        }
                    }
                }
                catch (System.Net.WebException ex)
                {

                }

                return this.parents;
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
         <summary>  Gets the MediaTUM server. </summary>
        
         <value>    The MediaTUM server. </value>
         */
        public Server Server { get; private set; }

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
                    XmlNode xmlNodeResponse = GetXmlNodeResponse(uri, result);
                    if (xmlNodeResponse != null)
                    {
                        XmlNode xmlNodeList = xmlNodeResponse["nodelist"];
                        int numberOfNodes = Convert.ToInt32(xmlNodeList.Attributes["countall"].Value);

                        for (int childrenIndex = 0; childrenIndex < numberOfNodes; childrenIndex++)
                        {
                            XmlNode nextXmlNode = xmlNodeList.ChildNodes[childrenIndex];
                            UInt32 newId = Convert.ToUInt32(nextXmlNode.Attributes["id"].Value);

                            Node childNode;
                            if (!this.Server.nodeTable.TryGetValue(newId, out childNode))
                            {
                                string typeOfNewNode = nextXmlNode.Attributes["type"].Value;
                                childNode = this.Server.CreateNode(typeOfNewNode, (XmlElement)nextXmlNode, result);
                            }

                            this.children.Add(childNode);
                        }
                    }
                }
                catch (System.Net.WebException ex)
                {

                }

                return this.children;
            }
        }

        public string Xml { get; private set; }

        public IDictionary<string, string> Attributes = new Dictionary<string, string>();

        public Dictionary<string, NodeMask> Masks = new Dictionary<string, NodeMask>();

        public List<NodeFile> Files = new List<NodeFile>();

        static string GetOptionalAttribute(XmlNode xmlNode, string name)
        {
            XmlAttribute attribute = xmlNode.Attributes[name];
            if (attribute == null)
                return null;

            return attribute.Value;
        }

        static XmlNode GetXmlNodeResponse(Uri uri, string result)
        {
            if (result == "")
                return null;

            string[] resultArray = result.Split('\n');

            bool ok = true;
            XmlDocument xmlResponse;

            int counter = 0;

            do
            {
                ok = true;
                string newResult = resultArray[0];
                for (int index = 1; index < resultArray.Length; index++)
                    newResult += '\n' + resultArray[index];

                xmlResponse = new XmlDocument();
                try
                {
                    xmlResponse.LoadXml(newResult);
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

            return xmlResponse["response"];
        }

        void Parse(XmlElement xmlNode)
        {
            this.Name = xmlNode.Attributes["name"].Value;
            this.ID = Convert.ToUInt32(xmlNode.Attributes["id"].Value);
            this.Type = xmlNode.Attributes["type"].Value;

            this.Read = GetOptionalAttribute(xmlNode, "read");
            this.Write = GetOptionalAttribute(xmlNode, "write");
            this.Data = GetOptionalAttribute(xmlNode, "data");

            foreach (XmlNode childNode in xmlNode)
            {
                switch (childNode.Name)
                {
                    case "attribute":
                        {
                            string name = childNode.Attributes["name"].Value;
                            XmlNode childNode2 = childNode.ChildNodes[0];
                            XmlCDataSection cdataSection = childNode2 as XmlCDataSection;
                            string value = cdataSection.Value;

                            this.Attributes.Add(name, value);
                            break;
                        }
                    case "file":
                        {
                            NodeFile file = new NodeFile(this, (XmlElement)childNode);
                            this.Files.Add(file);
                            break;
                        }
                    case "mask":
                        {
                            NodeMask mask = new NodeMask(this, (XmlElement)childNode);
                            this.Masks.Add(mask.Name, mask);
                            break;
                        }
                    default:
                        {
                            int a = 3;
                            break;
                        }
                }
            }
        }

        internal static Node CreateNode(Server server, UInt32 nodeId)
        {
            Uri uri;
            string result = server.Export(nodeId, "", out uri);
            XmlNode xmlNodeResponse = GetXmlNodeResponse(uri, result);
            XmlElement xmlNode = (XmlElement)xmlNodeResponse["node"];
            string typeOfNewNode = xmlNode.Attributes["type"].Value;

            return server.CreateNode(typeOfNewNode, xmlNode, result);
        }

        public Node(Server server, XmlElement xmlNode, string xml)
        {
            this.Server = server;
            this.Xml = xml;

            Parse(xmlNode);
            this.Server.nodeTable.Add(this.ID, this);
        }

        string Attributes2Json()
        {
            MemoryStream memoryStream = new MemoryStream();
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(IDictionary<string, string>));
            jsonSerializer.WriteObject(memoryStream, this.Attributes);
            memoryStream.Position = 0;
            StreamReader reader = new StreamReader(memoryStream);
            string json = reader.ReadToEnd();

            return json;
        }

        /**
         <summary>  Uploads a file to an node . </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 16.07.2014. </remarks>
        
         <param name="type">        The nodetype of new node. User full quantifier objectype/schema. </param>
         <param name="name">        The name of node, will be stored in the node table, field name. </param>
         <param name="metadata">    The metadatafields to given file. Use the json format e.g. {"nodename":"name", ...}. </param>
         <param name="data">        The file in binary form. </param>
        
         <returns>  The new node of the uploaded file; </returns>
         */
        public Node Upload(string type, string name, string metadata, byte[] data)
        {
            UInt32 newId = this.Server.Upload(this.ID, type, name, metadata, data);
            Node newNode = Node.CreateNode(this.Server, newId);
            if (this.children != null)
                this.children.Add(newNode);

            return newNode;
        }

        /**
         <summary>  Updates an node. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 18.07.2014. </remarks>
        
         <param name="name">        The new name of node, will be stored in the node table, field name. </param>
         <param name="metadata">    The new metadatafields to given file. Use the json format e.g. {"nodename":"name", ...}. </param>
         */
        public void Update()
        {
            string metadata = Attributes2Json();

            this.Server.Update(this.ID, this.Name, metadata);
            Uri uri;
            string result = this.Server.Export(this.ID, "", out uri);
            this.Xml = result;
            XmlNode xmlNodeResponse = GetXmlNodeResponse(uri, result);
            XmlElement xmlNode = (XmlElement)xmlNodeResponse["node"];

            this.Files.Clear();
            this.Attributes.Clear();
            this.Masks.Clear();
            Parse(xmlNode);
        }
    }
}
