using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Xml;

namespace Ksd.Mediatum
{
    /**
     <summary>  Connector to the MediaTUM server. </summary>
    
     <remarks>  Dr. Torsten Thurow, TU München, 22.07.2014. </remarks>
     */
    [Serializable()]
    public class Server
    {
        /**
         <summary>  Gets or sets the network name or address of the server. </summary>
        
         <value>    The network name or address of the server. </value>
         */
        public String ServerName { get; set; }

        /**
         <summary>  Gets or sets the sign path. </summary>
        
         <value>    The sign path. </value>
         */
        public String SignPath { get; set; }

        /**
          <summary>  Gets or sets the upload path. </summary>
        
          <value>    The upload path. </value>
          */
        public String UploadPath { get; set; }

        /**
         <summary>  Gets or sets the update path. </summary>
        
         <value>    The update path. </value>
         */
        public String UpdatePath { get; set; }

        /**
         <summary>  Gets or sets the export path. </summary>
        
         <value>    The export path. </value>
         */
        public String ExportPath { get; set; }

        /**
         <summary>  Gets or sets the file path. </summary>
        
         <value>    The export path. </value>
         */
        public String FilePath { get; set; }

        /**
         <summary>  Gets or sets the OAuth profile of the user. </summary>
        
         <value>    The OAuth profile of the user. </value>
         */
        public OAuth User { get; set; }

        /**
         <summary>  Constructor. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 22.07.2014. </remarks>
        
         <param name="user">        The OAuth profile of the user. </param>
         <param name="serverName">  (Optional) The network name or address of the server. </param>
         */
        public Server(OAuth user, string serverName = "mediatum.ub.tum.de")
        {
            this.User = user;
            this.ServerName = serverName;
            this.SignPath = "services/upload/calcsign";
            this.UploadPath = "services/upload/new";
            this.UpdatePath = "services/update";
            this.ExportPath = "services/export";
            this.FilePath = "file";
        }

        /**
         <summary>  Exports. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 22.07.2014. </remarks>
        
         <param name="nodeId">  The ID of the node to read. </param>
         <param name="postfix"> The postfix of the request. </param>
         <param name="uri">     [out] The URI of the request. </param>
        
         <returns>  The result string of the request. </returns>
         */
        public string Export(UInt32 nodeId, string postfix, out Uri uri)
        {
            // http://mediatum.ub.tum.de/services/export

            string prefix = this.ExportPath + "/node/" + nodeId.ToString();
            if (postfix != "")
                prefix += '/' + postfix;
            
            String uriString = "https://" + this.ServerName + '/' + prefix;
            uri = new Uri(uriString);

            System.Collections.Specialized.NameValueCollection parameters = new System.Collections.Specialized.NameValueCollection();
            this.User.GetMd5Hash(prefix, parameters);
            String callString = uriString + "/?" + OAuth.GetSortedParameterString(parameters);

            WebClient wc = new WebClient();
            string result = wc.DownloadString(callString);
            WebHeaderCollection col = wc.ResponseHeaders;

            return result;
        }
/*
        string GetOAuthSignFromServer(string key, )
        {
//    values = {'key': params['key'], 'url': "%s?metadata=%s&name=%s&parent=%s&type=%s&user=%s" % (uploadurl, metadata, nodename, params['parentnode'], schemaname, params['user'])}
//    response = requests.get("%s%s" % (servername, signurl), params=values)

            System.Collections.Specialized.NameValueCollection parameters = new System.Collections.Specialized.NameValueCollection
            {
                { "key", key },
                { "url", type },
                { "name", name },
                { "metadata", Uri.EscapeUriString(metadata) },
//                { "metadata", metadata },
                { "data", base64String },
            };
        }
        */
        /**
         <summary>  Uploads a file to an node . </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 16.07.2014. </remarks>
        
         <param name="parent">      The id of parent node where new artifact should be added. </param>
         <param name="type">        The nodetype of new node. User full quantifier objectype/schema. </param>
         <param name="name">        The name of node, will be stored in the node table, field name. </param>
         <param name="metadata">    The metadatafields to given file. Use the json format e.g. {"nodename":"name", ...}. </param>
         <param name="data">        The file in binary form. </param>
        
         <returns>  A Task&lt;string&gt; </returns>
         */
        internal UInt32 Upload(UInt32 parent, string type, string name, string metadata, byte[] data)
        {
            // http://mediatum.ub.tum.de/services/upload
            
            string base64String = System.Convert.ToBase64String(data, 0, data.Length);
            string uri = "https://" + this.ServerName + '/' + this.UploadPath;
            
            System.Collections.Specialized.NameValueCollection parameters = new System.Collections.Specialized.NameValueCollection
            {
                { "parent", parent.ToString() },
                { "type", type },
                { "name", name },
                { "metadata", Uri.EscapeUriString(metadata) },
//                { "metadata", metadata },
                { "data", base64String },
            };

            //this.User.GetMd5Hash(this.UploadPath, parameters);

            WebClient wc = new WebClient();
            byte[] result = wc.UploadValues(uri, parameters);
            string s = Encoding.UTF8.GetString(result, 0, result.Length);
            WebHeaderCollection col = wc.ResponseHeaders;

            string idString = col["NodeID"];

            return Convert.ToUInt32(idString);
        }

        internal byte[] Download(UInt32 nodeId, string fileName, out Uri uri)
        {
            // http://mediatum.ub.tum.de/file

            string prefix = this.FilePath + '/' + nodeId.ToString() + '/' + fileName;

            String uriString = "https://" + this.ServerName + '/' + prefix;
            uri = new Uri(uriString);

            System.Collections.Specialized.NameValueCollection parameters = new System.Collections.Specialized.NameValueCollection();
            this.User.GetMd5Hash(prefix, parameters);
            String callString = uriString + "/?" + OAuth.GetSortedParameterString(parameters);

            WebClient wc = new WebClient();
            byte[] result = wc.DownloadData(uriString);
            WebHeaderCollection col = wc.ResponseHeaders;

            return result;
        }

        /**
         <summary>  Updates an node. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 18.07.2014. </remarks>
        
         <param name="nodeId">      The ID of the node to update. </param>
         <param name="name">        The new name of node, will be stored in the node table, field name. </param>
         <param name="metadata">    The new metadatafields to given file. Use the json format e.g. {"nodename":"name", ...}. </param>
         */
        internal void Update(UInt32 nodeId, string name, string metadata)
        {
            // http://mediatum.ub.tum.de/services/update

            string uri = "https://" + this.ServerName + '/' + this.UpdatePath + '/' + nodeId.ToString();

            System.Collections.Specialized.NameValueCollection parameters = new System.Collections.Specialized.NameValueCollection
            {
                { "name", name },
                { "metadata", Uri.EscapeUriString(metadata) },
            };

            //this.User.GetMd5Hash(this.UploadPath, parameters);

            WebClient wc = new WebClient();
            byte[] result = wc.UploadValues(uri, parameters);
            string s = Encoding.UTF8.GetString(result, 0, result.Length);
            WebHeaderCollection col = wc.ResponseHeaders;
        }

        internal Dictionary<UInt32, Node> nodeTable = new Dictionary<uint,Node>();

        /**
         <summary>  Gets a node form MediaTUM. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 22.07.2014. </remarks>
        
         <param name="nodeId">  The ID of the node to read. </param>
        
         <returns>  The node. </returns>
         */
        public Node GetNode(UInt32 nodeId)
        {
            Node node;
            if (!this.nodeTable.TryGetValue(nodeId, out node))
                return Node.CreateNode(this, nodeId);

            return node;
        }

        public Dictionary<string, Type> typeTable = new Dictionary<string, Type>();

        internal Node CreateNode(string typeAsString, XmlElement xmlNode, string xml)
        {
            Type type;
            if (!this.typeTable.TryGetValue(typeAsString, out type))
                type = typeof(Node);

            Object[] args = new Object[] { this, xmlNode, xml };

            return (Node)Activator.CreateInstance(type, args);
        }
    }
}
