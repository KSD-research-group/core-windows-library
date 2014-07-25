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
        
         <param name="server">  The server. </param>
         <param name="xmlNode"> Element describing the XML node. </param>
         <param name="xml">     The XML. </param>
         */
        public FloorPlanNode(Server server, XmlElement xmlNode, string xml)
            :base(server, xmlNode, xml)
        { }

        public int Width 
        {
            get
            {
                return Convert.ToInt32(this.GetAttributeValue("width"));
            }
        }

        public int Height
        {
            get
            {
                return Convert.ToInt32(this.GetAttributeValue("height"));
            }
        }

        public int OrigWidth
        {
            get
            {
                return Convert.ToInt32(this.GetAttributeValue("origwidth"));
            }
        }

        public int OrigHeight
        {
            get
            {
                return Convert.ToInt32(this.GetAttributeValue("origheight"));
            }
        }

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

        public static Node CreateFloorPlanNode(Node parent, string name, string architect, string country, Uri graphMlUri, byte[] data)
        {
            Dictionary<string, string> map = new Dictionary<string, string>
            {
                {"architect", architect},
                {"country", country},
                {"roomgraph", graphMlUri.OriginalString}
            };

            return parent.Upload("image/project-arc", name, Node.AttributeTable2Json(map), data); 
        }
    }
}
