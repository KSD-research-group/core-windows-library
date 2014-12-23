using System;
using System.Collections.Generic;
using System.Xml;
using System.Net;

namespace Ksd.Mediatum
{
    /**
     <summary>  A node that represent a floor plan. </summary>
    
     <remarks>  Dr. Torsten Thurow, TU München, 23.07.2014. </remarks>
     */
    public class FloorPlanNode: Node
    {
        /**
         <summary>  Constructor. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 23.07.2014. </remarks>
        
         <param name="server">              The server object to connect MediaTUM. </param>
         <param name="xmlNode">             Element describing the XML node. </param>
         <param name="xml">                 The original XML, witch contains the node. </param>
         <param name="prefix">              The XML prefix (namespace). </param>
         <param name="xmlNamespaceManager"> Manager for XML namespace. </param>
         */
        public FloorPlanNode(Server server, XmlElement xmlNode, string xml, string prefix, XmlNamespaceManager xmlNamespaceManager)
            : base(server, xmlNode, xml, prefix, xmlNamespaceManager)
        { }

        /**
         <summary>  Gets the width of the image. </summary>
        
         <value>    The width of the image. </value>
         */
        public int Width 
        {
            get
            {
                return Convert.ToInt32(this.GetAttributeValue("width"));
            }
        }

        /**
         <summary>  Gets the height of the image. </summary>
        
         <value>    The height of the image. </value>
         */
        public int Height
        {
            get
            {
                return Convert.ToInt32(this.GetAttributeValue("height"));
            }
        }

        /**
         <summary>  Gets the width of the original image. </summary>
        
         <value>    The width of the original image. </value>
         */
        public int OrigWidth
        {
            get
            {
                return Convert.ToInt32(this.GetAttributeValue("origwidth"));
            }
        }

        /**
         <summary>  Gets the height of the original image. </summary>
        
         <value>    The height of the original image. </value>
         */
        public int OrigHeight
        {
            get
            {
                return Convert.ToInt32(this.GetAttributeValue("origheight"));
            }
        }

        /**
         <summary>  Gets or sets the architect of the floor plan. </summary>
        
         <value>    The architect of the floor plan. </value>
         */
        public string Architect
        {
            get
            {
                return this.GetAttributeValue("architect");
            }

            set
            {
                this.SetAttributeValue("architect", value);
            }
        }

        /**
         <summary>  Gets or sets the country of the building. </summary>
        
         <value>    The country of the building. </value>
         */
        public string Country
        {
            get
            {
                return this.GetAttributeValue("country");
            }

            set
            {
                this.SetAttributeValue("country", value);
            }
        }

        /**
         <summary>  Gets URI, where GraphML of floorplan can be downloaded. </summary>
        
         <value>    The GraphML URI. </value>
         */
        public Uri GraphMlUri
        {
            get 
            {
                string uriString = this.GetAttributeValue("roomgraph");
                string[] res = uriString.Split(';');
                return new Uri(res[0]);
            }

            set
            {
                this.SetAttributeValue("roomgraph", value.OriginalString);
            }
        }

        /**
         <summary>  Gets GraphML of floorplan. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 23.07.2014. </remarks>
        
         <returns>  The GraphML of floorplan. </returns>
         */
        public string GetGraphMl()
        {
            WebClient wc = new WebClient();
            string result = wc.DownloadString(this.GraphMlUri);
            WebHeaderCollection col = wc.ResponseHeaders;

            return result;
        }

        /**
         <summary>  Saves the GraphML of floorplan local as file. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 23.07.2014. </remarks>
        
         <param name="path">    Full pathname of the file. </param>
         */
        public void SaveGraphMl(string path)
        {
            WebClient wc = new WebClient();
            wc.DownloadFile(this.GraphMlUri, path);
            WebHeaderCollection col = wc.ResponseHeaders;
        }

        /**
         <summary>  Creates a floor plan node. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 28.07.2014. </remarks>
        
         <param name="parent">      The parent node of the new node. </param>
         <param name="name">        The name of the new node. </param>
         <param name="architect">   The architect of the floor plan. </param>
         <param name="country">     The country of the building. </param>
         <param name="graphMlUri">  The URI to the GraphML file of the floor plan. </param>
         <param name="data">        The image of the floor plan in binary format. </param>
        
         <returns>  The new floor plan node. </returns>
         */
        public static Node CreateFloorPlanNode(Node parent, string name, string architect, string country, Uri graphMlUri, byte[] data)
        {
            Dictionary<string, string> map = new Dictionary<string, string>
            {
                {"architect", architect},
                {"country", country},
                {"roomgraph", graphMlUri.OriginalString}
            };

            return parent.Upload("image/project-arc", name, map, data); 
        }
    }
}
