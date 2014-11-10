using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksd.Mediatum.Surfid
{
    /**
     <summary>  A project node of Surfid plugin. </summary>
    
     <remarks>  Dr. Torsten Thurow, TU München, 10.11.2014. </remarks>
     */
    [Serializable()]
    public class ProjectNode: ImageNode
    {
        /**
         <summary>  Is fingerprint manually validated? </summary>
        
         <value>    true if fingerprint is manually validated, false if not. </value>
         */
        public bool IsFingerprintManuallyValidated { get; private set; }

        /**
         <summary>  Is this object floorplan? </summary>
        
         <value>    true if this object is floorplan, false if not. </value>
         */
        public bool IsFloorplan { get; private set; }

        /**
         <summary>  Gets the name of this project. </summary>
        
         <value>    The name of this project. </value>
         */
        public string Name { get; private set; }

        /**
         <summary>  Gets the URI to get the children (images). </summary>
        
         <value>    The URI to get the children (images). </value>
         */
        public Uri ChildrenUri { get; private set; }

        /**
         <summary>  Gets the children (images). </summary>
        
         <value>    The children (images). </value>
         */
        public IEnumerable<ImageNode> Children
        {
            get
            {
                return this.Server.GetImages(this.ChildrenUri);
            }
        }

        /**
         <summary>  Specialised constructor for use only by derived class. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 10.11.2014. </remarks>
        
         <param name="server">              The MediaTUM server with Surfid plugin. </param>
         <param name="imageUri">            URI of the image. </param>
         <param name="thumb2Uri">           URI of the Thumb2 image. </param>
         <param name="thumbUri">            URI of the Thumb image. </param>
         <param name="fingerprintUri">      URI of the fingerprint. </param>
         <param name="manuallyValidated">   true if fingerprint is manually validated, false if not. </param>
         <param name="isFloorplan">         true if this object is floorplan, false if not. </param>
         <param name="name">                The name of this project. </param>
         <param name="childrenUri">         The URI to get the children (images). </param>
         */
        protected ProjectNode(Server server, Uri imageUri, Uri thumb2Uri, Uri thumbUri, Uri fingerprintUri, bool manuallyValidated, bool isFloorplan, string name, Uri childrenUri)
            : base(server, imageUri, thumb2Uri, thumbUri, fingerprintUri)
        {
            this.IsFingerprintManuallyValidated = manuallyValidated;
            this.IsFloorplan = isFloorplan;
            this.Name = name;
            this.ChildrenUri = childrenUri;
        }

        internal static IEnumerable<ProjectNode> GetProjects(Server server, string value)
        {
            dynamic jsonDe = Newtonsoft.Json.JsonConvert.DeserializeObject(value);
            dynamic projects = jsonDe.projects;

            List<ProjectNode> result = new List<ProjectNode>();

            foreach (dynamic entry in projects)
            {
                Uri image = entry.image;
                Uri thumb2 = entry.thumb2;
                Uri thumb = entry.thumb;
                Uri fingerprint = entry.fingerprint;
                bool manuallyValidated = (entry.manuallyValidated == "True" || entry.manuallyValidated == "true");
                bool isFloorplan = entry.isFloorplan;
                string name = entry.name;
                Uri children = entry.children;

                result.Add(new ProjectNode(server, image, thumb2, thumb, fingerprint, manuallyValidated, isFloorplan, name, children));
            }

            return result;
        }
    }
}
