using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Web;
using System.Net.Http;

namespace Ksd.Mediatum
{
    public class Server
    {
        public String ServerName { get; set; }

        public String UploadLink { get; set; }

        public String UpdateLink { get; set; }

        public String ExportLink { get; set; }



        public OAuth User { get; set; }

        public Server(OAuth user, string serverName = "mediatum.ub.tum.de")
        {
            this.User = user;
            this.ServerName = serverName;
            this.UploadLink = "service/upload/new";
            this.UpdateLink = "services/update";
            this.ExportLink = "services/export";
        }

        public async Task<string> ExportAsync(UInt32 id)
        {
            // http://mediatum.ub.tum.de/services/export

            var client = new HttpClient(new HttpClientHandler());
            Uri uri = new Uri("https://" + this.ServerName + '/' + this.ExportLink + "/node/" + id.ToString());

            var response = await client.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        /**
         <summary>  Uploads asynchronous a file to an node . </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 16.07.2014. </remarks>
        
         <param name="parent">      The id of parent node where new artifact should be added. </param>
         <param name="type">        The nodetype of new node. User full quantifier objectype/schema. </param>
         <param name="name">        The name of node, will be stored in the node table, field name. </param>
         <param name="metadata">    The metadatafields to given file. Use the json format e.g. {"nodename":"name", ...}. </param>
         <param name="data">        The file in binary form. </param>
        
         <returns>  A Task&lt;string&gt; </returns>
         */
        public async Task<UInt32> UploadAsync(UInt32 parent, string type, string name, string metadata, byte[] data)
        {
            // http://mediatum.ub.tum.de/services/upload

            string base64String = System.Convert.ToBase64String(data, 0, data.Length);

            SortedDictionary<string, string> parameters = new SortedDictionary<string, string>
            {
                { "parent", parent.ToString() },
                { "type", type },
                { "name", name },
//                { "metadata", Uri.EscapeUriString(metadata) },
                { "metadata", metadata },
                { "data", base64String },
            };

            var values = new FormUrlEncodedContent(parameters);
            var client = new HttpClient(new HttpClientHandler());
            Uri uri = new Uri("https://" + this.ServerName + '/' + this.UploadLink);
            
            var response = await client.PostAsync(uri, values);
            response.EnsureSuccessStatusCode();

            return 0;
        }

        /**
         <summary>  Updates the asynchronous. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 18.07.2014. </remarks>
        
         <param name="parent">      The new ID of the parent node. </param>
         <param name="name">        The name. </param>
         <param name="metadata">    The new key value pairs containing the metadata, either in XML or JSON. </param>
         <param name="data">        The new raw data of the image to upload. </param>
        
         <returns>  A Task&lt;string&gt; </returns>
         */
        public async void UpdateAsync(UInt32 parent, string format, string metadata, byte[] data)
        {
            // http://mediatum.ub.tum.de/services/upload

            string base64String = System.Convert.ToBase64String(data, 0, data.Length);

            SortedDictionary<string, string> parameters = new SortedDictionary<string, string>
            {
                { "parent", parent.ToString() },
                { "format", format },
                { "metadata", Uri.EscapeUriString(metadata) },
                { "data", base64String }
            };

            var values = new FormUrlEncodedContent(parameters);
            var client = new HttpClient(new HttpClientHandler());
            Uri uri = new Uri("https://" + this.ServerName + '/' + this.UpdateLink);

            var response = await client.PostAsync(uri, values);
            response.EnsureSuccessStatusCode();
        }
    }
}
