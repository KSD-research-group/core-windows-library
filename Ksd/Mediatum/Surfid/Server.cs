using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using Microsoft.Win32;

namespace Ksd.Mediatum.Surfid
{
    /**
     <summary>  A MediaTUM server with Surfid plugin. </summary>
    
     <remarks>  Dr. Torsten Thurow, TU München, 10.11.2014. </remarks>
     */
    [Serializable()]
    public class Server : Ksd.Mediatum.Server
    {
        /**
          <summary>  Gets or sets the surfid path. </summary>
        
          <value>    The surfid path. </value>
          */
        public String SurfidPath { get; set; }

        /**
         <summary>  Constructor. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 03.11.2014. </remarks>
        
         <param name="user">        The OAuth profile of the user. </param>
         <param name="serverName">  (Optional) The network name or address of the server. </param>
         */
        public Server(OAuth user, string serverName)
            : base(user, serverName)
        {
            this.SurfidPath = "/services/surfid";
        }

        internal IEnumerable<ImageNode> GetImages(Uri uri)
        {
            string signedUri = this.User.GetSignedUri(uri);
            WebClient wc = new WebClient();
            System.Diagnostics.Trace.WriteIf(traceSwitch.TraceInfo, String.Format("Get {0}", uri));
            string result = wc.DownloadString(signedUri);
            System.Diagnostics.Trace.WriteIf(traceSwitch.TraceInfo, String.Format("Response is {0}", result));
            WebHeaderCollection col = wc.ResponseHeaders;

            return ImageNode.GetImages(this, result);
        }

        /**
         <summary>  Gets the images of the MediaTUM server. </summary>
        
         <value>    The images. </value>
         */
        public IEnumerable<ImageNode> Images
        {
            get
            {
                // http://mediatum.ub.tum.de/services/surfid/v1/images

                string prefix = this.SurfidPath + "/v1/images";

                String uriString = "http://" + this.ServerName + prefix;

                return GetImages(new Uri(uriString));
            }
        }

        /**
         <summary>  Gets the projects of the MediaTUM server. </summary>
        
         <value>    The projects. </value>
         */
        public IEnumerable<ProjectNode> Projects
        {
            get
            {
                // http://mediatum.ub.tum.de/services/surfid/v1/project

                string prefix = this.SurfidPath + "/v1/project";

                String uriString = "http://" + this.ServerName + prefix;

                //String callString = uriString + "/?" + OAuth.GetSortedParameterString(parameters); // unsigned
                String callString = "http://" + this.ServerName + this.User.GetSignedUri(prefix); // signed

                WebClient wc = new WebClient();
                System.Diagnostics.Trace.WriteIf(traceSwitch.TraceInfo, String.Format("Get {0}", callString));
                string result = wc.DownloadString(callString);
                System.Diagnostics.Trace.WriteIf(traceSwitch.TraceInfo, String.Format("Response is {0}", result));
                WebHeaderCollection col = wc.ResponseHeaders;

                return ProjectNode.GetProjects(this, result);
            }
        }
    }
}
