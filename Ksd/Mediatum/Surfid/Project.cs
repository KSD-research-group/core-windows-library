using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

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
        
         <param name="server">  The MediaTUM server with Surfid plugin. </param>
         <param name="json">    The dynamic JSON input object. </param>        
         */
        protected ProjectNode()
        {
        }

        /**
         <summary>  Specialised constructor for use only by derived class. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 11.11.2014. </remarks>
        
         <param name="server">  The MediaTUM server with Surfid plugin. </param>
         <param name="json">    The dynamic JSON input object. </param>
         */
        protected override void LoadJson(Server server, dynamic json) 
        {
            //base.LoadJson(server, json);
            this.IsFingerprintManuallyValidated = (json.manuallyValidated == "True" || json.manuallyValidated == "true"); ;
            this.IsFloorplan = json.isFloorplan;
            this.Name = json.name;
            this.ChildrenUri = json.children;
            JObject inconsistencies = json.inconsistencies;
        }

        internal static IEnumerable<ProjectNode> GetProjects(Server server, string value)
        {
            dynamic jsonDe = Newtonsoft.Json.JsonConvert.DeserializeObject(value);
            dynamic projects = jsonDe.projects;

            List<ProjectNode> result = new List<ProjectNode>();

            foreach (dynamic entry in projects)
                ;// result.Add(new ProjectNode(server, entry));
 
            return result;
        }
    }
}
