using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.IO;

namespace Ksd.Mediatum
{
    [Serializable()]
    public class NodeMask
    {
        /**
         <summary>  Gets the name of the mask. </summary>
        
         <value>    The name of the mask. </value>
         */
        public string Name { get; internal set; }

        /**
         <summary>  Gets the value of the mask. </summary>
        
         <value>    The value of the mask. </value>
         */
        public string Value { get; internal set; }

        /**
         <summary>  Gets the parent node. </summary>
        
         <value>    The parent node of the mask. </value>
         */
        public Node Parent { get; internal set; }

        /**
         <summary>  Internal constructor for XML reader. </summary>
        
         <param name="parent">  The parent node of the mask. </param>
         <param name="xmlNode"> Element describing the XML node. </param>
         */
        internal NodeMask(Node parent, XmlElement xmlNode)
        {
            this.Parent = parent;
            this.Name = xmlNode.Attributes["name"].Value;
            XmlNode childNode = xmlNode.ChildNodes[0];
            XmlCDataSection cdataSection = childNode as XmlCDataSection;

            if (cdataSection != null)
                this.Value = cdataSection.Value;
        }
    }
}
