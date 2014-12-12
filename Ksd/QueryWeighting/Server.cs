using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using RestSharp;
using Microsoft.Win32;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;

/**
// namespace: Ksd.QueryWeighting
//
// summary:	.
 */
namespace Ksd.QueryWeighting
{
    /**
     <summary>  Connector to the Query Weighting Service. </summary>
    
     <remarks>  Dr. Torsten Thurow, TU München, 11.12.2014. </remarks>
     */
    [Serializable()]
    public class Server
    {
        public static TraceSwitch traceSwitch = new TraceSwitch("General", "Entire Application");

        int fileNumber;

        public Ksd.Mediatum.Server MediaTumServer;  ///< The MediaTUM server

        /**
         <summary>  Gets or sets the server name in registry. </summary>
        
         <value>    The server name in registry. </value>
         */
        public static String ServerNameInRegistry
        {
            get { return (string)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Mediatum", @"QueryWeightingWebService_URI", @"localhost"); }

            set { Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Mediatum", @"QueryWeightingWebService_URI", value); }
        }

        /**
         <summary>  Gets or sets the network name or address of the server. </summary>
        
         <value>    The network name or address of the server. </value>
         */
        public String ServerName { get; set; }

        /**
         <summary>  Constructor. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 01.12.2014. </remarks>
        
         <param name="serverName">  The network name or address of the server. </param>
         */
        public Server(string serverName)
        {
            this.ServerName = serverName;
        }

        /**
         <summary>  Determines if we can status check. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 11.12.2014. </remarks>
        
         <returns>  true if it succeeds, false if it fails. </returns>
         */
        public bool StatusCheck()
        {
            String callString = "http://" + this.ServerName + "/status";

            WebClient wc = new WebClient();
            System.Diagnostics.Trace.WriteIf(traceSwitch.TraceInfo, String.Format("Get {0}", callString));
            string result = wc.DownloadString(callString);
            System.Diagnostics.Trace.WriteIf(traceSwitch.TraceInfo, String.Format("Response is {0}", result));
            WebHeaderCollection col = wc.ResponseHeaders;

            return result.CompareTo(@"Hi, this is the unified query endpoint at your service, how may I help you?") == 0;
        }

        /**
         <summary>  Unified. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 11.12.2014. </remarks>
        
         <param name="search">  The search string. </param>
        
         <returns>  A Query Result. </returns>
         */
        public QueryResult Unified(string search)
        {
            String callString = "http://" + this.ServerName + "/unified";

            System.Collections.Specialized.NameValueCollection parameters = new System.Collections.Specialized.NameValueCollection
            {
                { "q", search },
            };

            WebClient wc = new WebClient();
            System.Diagnostics.Trace.WriteIf(traceSwitch.TraceInfo, String.Format("Get {0}", callString));
            byte[] result = wc.UploadValues(callString, parameters);
            System.Diagnostics.Trace.WriteIf(traceSwitch.TraceInfo, String.Format("Response is {0}", result));
            WebHeaderCollection col = wc.ResponseHeaders;

            fileNumber++;
            string s = Encoding.UTF8.GetString(result, 0, result.Length);
            File.WriteAllText(fileNumber.ToString() + "Response.xml", s);
            return new QueryResult(new Uri(callString), s, this);
        }
    }
}
