using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;


namespace Ksd.Mediatum
{
    /**
     <summary>  An OAuth for MediaTUM. </summary>
    
     <remarks>
     Dr. Torsten Thurow, TU München, 16.07.2014.
       Original code from Patrick Bernhard, ported to C# from Dr. Torsten Thurow
       <a href="http://gitlab.ai.ar.tum.de/ksd-research-group/ksd-documentation/wikis/mediaTumBasicAuthentication">here</a>
     </remarks>
     */
    public class OAuth
    {
        #region Signing Configuration

        /**
         <summary>  Gets or sets the name of the user for OAuth requests. </summary>
        
         <value>    The name of the user for OAuth requests. </value>
         */
        public String UserName { get; set; }

        /**
         <summary>  Gets or sets the preshared tag for OAuth requests. </summary>
        
         <value>    The preshared tag for OAuth requests. </value>
         */
        public String PresharedTag { get; set; }
        
        #endregion

        private static readonly MD5 md5 = MD5.Create(); ///< The MD5 algorithm used to generate the key for the OAuth authentication mechanism.

        /**
         <summary>  Constructor. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 16.07.2014. </remarks>
        
         <param name="userName">        The name of the user for OAuth requests. </param>
         <param name="presharedTag">    The preshared tag for OAuth requests. </param>
         */
        public OAuth (String userName, String presharedTag)
        {
            this.UserName = userName;
            this.PresharedTag = presharedTag;
        }

        /**
         <summary>  Gets signed URL. </summary>
        
         <remarks>
         The signing is based on a common signing algorithm on both the server and client side.
         
            Sort the parameters alphabetically ( yxz=foo user=MyUsername abc=baz becomes abc=baz user=MyUsername yxz=foo )
            Concatenate the keys and values to one string like in URL ( => abc=baz&amp;user=MyUsername&amp;yxz=foo)
            Prefix the string with the request path (/service/export/abc=baz&amp;user=MyUsername&amp;yxz=foo)
            Prefix the resulting string with the shared secret (MySharedSecret/service/export/abc=baz&amp;user=MyUsername&amp;yxz=foo)
            Create the MD5 sum of that string ( MD5 = 1676a3873fc5dd035d362a3a01450dcc)
            Use this hash string as parameter sign.
         
         The resulting request would look like: http://server.de/service/export/?yxz=foo&amp;user=MyUsername&amp;abc=baz&amp;
         sign=1676a3873fc5dd035d362a3a01450dcc.
         </remarks>
        
         <param name="prefix">      The prefix. </param>
         <param name="postfix">     The postfix. </param>
         <param name="parameters">  (Optional) Options for controlling the operation. </param>
        
         <returns>  The signed URL. </returns>
        public Uri GetSignedUrl(string postfix, SortedDictionary<string, string> parameters = null)
        {
            if (parameters == null)
                parameters = new SortedDictionary<string, string>();

            parameters.Add("user", UserName);
            String parameterString = GetParameterString(parameters);
            String sig = this.PresharedTag + '/' + postfix + '/' + parameterString;
            String cleanedSig = Uri.EscapeUriString(sig);
            String newSig = GetMd5Hash(cleanedSig);

            parameters.Add("sign", newSig);
            parameterString = GetParameterString(parameters);
            String url = prefix + '/' + postfix + "/?" + parameterString;
            String cleanedUrl = Uri.EscapeUriString(url);

            return new Uri(cleanedUrl);
        }

        public Uri GetUnsignedUrl(string prefix, string postfix, SortedDictionary<string, string> parameters = null)
        {
            String parameterString = GetParameterString(parameters);
            String url = prefix + '/' + postfix;
            if (parameterString != null)
                url += '?' + parameterString;

            return new Uri(url);
        }
         */

        /**
         <summary>  Gets parameter string of a URI. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 16.07.2014. </remarks>
        
         <param name="parameters">  parameters to convert in URI format. </param>
        
         <returns>  The parameter string. </returns>
         */
        private static string GetParameterString(SortedDictionary<string, string> parameters)
        {
            if (parameters == null)
                return null;

            IEnumerator<KeyValuePair<string, string>> enumerator = parameters.GetEnumerator();
            if (!enumerator.MoveNext())
                return null;
            
            KeyValuePair<string, string> pair = enumerator.Current;
            string result = pair.Key + '=' + pair.Value;

            while (enumerator.MoveNext())
            {
                pair = enumerator.Current;
                result += '&' + pair.Key + '=' + pair.Value;
            }

            return result;
        }

        /**
         <summary>  Get a character of a half byte as hexadecimal. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 16.07.2014. </remarks>
        
         <param name="value">   The value (0 ... 15). </param>
        
         <returns>  The character of a half byte as hexadecimal (0 ... f). </returns>
         */
        private static char HalfByte2Hex(int value)
        {
            return Convert.ToChar(value <= 9 ? value + 48 : value + 87);
        }

        /**
         <summary>  Convert a byte to hexadecimal representation. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 16.07.2014. </remarks>
        
         <param name="buffer">  The output buffer for the hexadecimal representation. </param>
         <param name="index">   Zero-based index of the first position into the output buffer. </param>
         <param name="value">   The value (0 ... 255). </param>
         */
        private static void Byte2Hex(char[] buffer, int index, byte value)
        {
            buffer[index] = HalfByte2Hex(value / 16);
            buffer[index + 1] = HalfByte2Hex(value % 16);
        }

        /**
         <summary>  Gets Md5 hash for OAuth. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 16.07.2014. </remarks>
        
         <param name="input">   The input string to hash. </param>
        
         <returns>  The MD5 hash for OAuth. </returns>
         */
        private static String GetMd5Hash(String input)
        {
            // Code von http://dotnet-snippets.de/snippet/gibt-den-md5-hash-eines-stings-als-string-zurueck/18, Stand: 07.04.2014

            // Prüfen ob Daten übergeben wurden.
            if ((input == null) || (input.Length == 0))
                return string.Empty;

            // MD5 Hash aus dem String berechnen: Dazu muss der String in ein Byte[] zerlegt werden. 
            // Danach muss das Resultat wieder zurück in einen String.
            byte[] inputToHash = Encoding.Default.GetBytes(input);
            byte[] result = md5.ComputeHash(inputToHash);

            int length = result.Length;
            char[] resultString = new char[length * 2];
            for (int index = 0; index < length; index++)
                Byte2Hex(resultString, index * 2, result[index]);

            return new String(resultString);
        }
    }
}
