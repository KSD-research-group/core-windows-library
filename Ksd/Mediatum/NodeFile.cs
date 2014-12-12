using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.IO;

namespace Ksd.Mediatum
{
    /**
     <summary>  A node file. </summary>
    
     <remarks>  Dr. Torsten Thurow, TU München, 28.07.2014. </remarks>
     */
    [Serializable()]
    public class NodeFile
    {
        /**
         <summary>  Gets the type of the file. </summary>
        
         <value>    The type of the file. </value>
         */
        public string Type { get; internal set; }

        /**
         <summary>  Gets the mime type of the file. </summary>
        
         <value>    The mime type of the file. </value>
         */
        public string MimeType { get; internal set; }

        /**
         <summary>  Gets the server internal filename of the file. </summary>
        
         <value>    The filename. </value>
         */
        public string Filename { get; internal set; }

        /**
         <summary>  Gets the parent node of the file. </summary>
        
         <value>    The parent. </value>
         */
        public Node Parent { get; internal set; }

        /**
         <summary>  Internal constructor for XML reader. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 28.07.2014. </remarks>
        
         <param name="parent">  The parent node. </param>
         <param name="xmlNode"> Element describing the XML node. </param>
         */
        internal NodeFile(Node parent, XmlElement xmlNode)
        {
            this.Parent = parent;
            this.Filename = xmlNode.Attributes["filename"].Value;
            this.MimeType = xmlNode.Attributes["mime-type"].Value;
            this.Type = xmlNode.Attributes["type"].Value;
        }

        /**
         <summary>  Downloads the file in binary format. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 28.07.2014. </remarks>
        
         <returns>  The file in binary format. </returns>
         */
        public byte[] Download()
        {
            Uri uri;
            return this.Parent.Server.Download(this.Parent.ID, Filename, out uri);
        }
    }
}
