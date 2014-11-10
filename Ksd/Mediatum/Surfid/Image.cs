using System;
using System.Collections.Generic;
using System.Net;
using System.Diagnostics;
using System.Drawing;

namespace Ksd.Mediatum.Surfid
{
    /**
     <summary>  An image node of Surfid plugin. </summary>
    
     <remarks>  Dr. Torsten Thurow, TU München, 10.11.2014. </remarks>
     */
    [Serializable()]
    public class ImageNode
    {
        public static TraceSwitch traceSwitch = new TraceSwitch("General", "Entire Application");

        /**
         <summary>  Gets the MediaTUM server with Surfid plugin. </summary>
        
         <value>    The MediaTUM server. </value>
         */
        public Server Server { get; private set; }

        /**
         <summary>  Gets the URI of the image. </summary>
        
         <value>    The image URI. </value>
         */
        public Uri ImageUri { get; private set; }

        /**
         <summary>  Gets the image. </summary>
        
         <value>    The image. </value>
         */
        public Image Image { get { return GetImage(this.ImageUri); } }

        /**
         <summary>  Gets the URI of the Thumb2 image. </summary>
        
         <value>    The Thumb2 image URI. </value>
         */
        public Uri Thumb2Uri { get; private set; }

        /**
         <summary>  Gets the Thumb2 image. </summary>
        
         <value>    The Thumb2 image. </value>
         */
        public Image Thumb2 { get { return GetImage(this.Thumb2Uri); } }

        /**
         <summary>  Gets the URI of the Thumb image. </summary>
        
         <value>    The Thumb image URI. </value>
         */
        public Uri ThumbUri { get; private set; }

        /**
         <summary>  Gets the Thumb image. </summary>
        
         <value>    The Thumb image. </value>
         */
        public Image Thumb { get { return GetImage(this.ThumbUri); } }

        /**
         <summary>  Gets the URI of the fingerprint. </summary>
        
         <value>    The fingerprint URI. </value>
         */
        public Uri FingerprintUri { get; private set; }

        /**
         <summary>  Gets an image. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 10.11.2014. </remarks>
        
         <param name="uri"> URI of the image. </param>
        
         <returns>  The image. </returns>
         */
        protected static Image GetImage(Uri uri)
        {
            System.IO.Stream s = System.Net.HttpWebRequest.Create(uri).GetResponse().GetResponseStream();
            return Image.FromStream(s);
        }

        /**
         <summary>  Specialised constructor for use only by derived class. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 10.11.2014. </remarks>
        
         <param name="server">          The MediaTUM server with Surfid plugin. </param>
         <param name="imageUri">        The image URI. </param>
         <param name="thumb2Uri">       The Thumb2 image URI. </param>
         <param name="thumbUri">        The Thumb image URI. </param>
         <param name="fingerprintUri">  The fingerprint URI. </param>
         */
        protected ImageNode(Server server, Uri imageUri, Uri thumb2Uri, Uri thumbUri, Uri fingerprintUri)
        {
            this.Server = server;
            this.ImageUri = imageUri;
            this.Thumb2Uri = thumb2Uri;
            this.ThumbUri = thumbUri;
            this.FingerprintUri = fingerprintUri;
        }

        internal static IEnumerable<ImageNode> GetImages(Server server, string value)
        {
            dynamic jsonDe = Newtonsoft.Json.JsonConvert.DeserializeObject(value);
            dynamic images = jsonDe.images;

            List<ImageNode> result = new List<ImageNode>();

            foreach (dynamic entry in images)
            {
                Uri image = entry.image;
                Uri thumb2 = entry.thumb2;
                Uri thumb = entry.thumb;
                Uri fingerprint = entry.fingerprint;

                result.Add(new ImageNode(server, image, thumb2, thumb, fingerprint));
            }

            return result;
        }
    }
}
