using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.IO;

namespace Ksd.Mediatum
{
    /**
     <summary>  Attribute for a node. </summary>
    
     <remarks>  Dr. Torsten Thurow, TU München, 28.07.2014. </remarks>
     */
    [Serializable()]
    public class NodeAttribute
    {
        /**
         <summary>  Gets a value indicating whether the attribute value is modifyed on client side. </summary>
        
         <value>    true if the attribute value is modifyed on client side, false if not. </value>
         */
        public bool Modifyed { get; internal set; }

        string value;

        /**
         <summary>  Gets or sets the value of the attribute. </summary>
        
         <value>    The value. </value>
         */
        public string Value 
        {
            get
            {
                return this.value;
            }
            
            set
            {
                if (this.value == value)
                    return;

                this.value = value;
                this.Modifyed = true;
            }
        }

        /**
         <summary>  Constructor. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 28.07.2014. </remarks>
        
         <param name="value">       The value. </param>
         <param name="modifyed">    true if the attribute value is modifyed on client side, false if not. </param>
         */
        public NodeAttribute(string value, bool modifyed)
        {
            this.value = value;
            this.Modifyed = modifyed;
        }
    }
}
