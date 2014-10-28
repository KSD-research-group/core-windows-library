using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Win32;

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
    [Serializable()]
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

        /**
         <summary>  Gets or sets the user name in registry. </summary>
         <remarks>  The registry key is HKEY_CURRENT_USER\\SOFTWARE\\Mediatum\\UserName. </remarks>
        
         <value>    The user name in registry. </value>
         */
        public static String UserNameInRegistry
        {
            get { return (string)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Mediatum", @"UserName", "deployment"); }

            set { Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Mediatum", @"UserName", value); }
        }

        /**
         <summary>  Gets or sets the preshared tag in registry. </summary>
         <remarks>  The registry key is HKEY_CURRENT_USER\\SOFTWARE\\Mediatum\\PresharedTag. </remarks>
        
         <value>    The preshared tag in registry. </value>
         */
        public static String PresharedTagInRegistry
        {
            get { return (string)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Mediatum", @"PresharedTag", "deployment"); }

            set { Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Mediatum", @"PresharedTag", value); }
        }

        /**
         <summary>  Read the user name and preshared tag from the registry. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 22.10.2014. </remarks>
         */
        public void ReadSigningConfigFromRegistry()
        {
            this.UserName = UserNameInRegistry;
            this.PresharedTag = PresharedTagInRegistry;
        }

        /**
         <summary>  Writes the user name and preshared tag to registry. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 22.10.2014. </remarks>
         */
        public void WriteSigningConfigToRegistry()
        {
            UserNameInRegistry = this.UserName;
            PresharedTagInRegistry = this.PresharedTag;
        }
        
        #endregion

        static readonly MD5 md5 = MD5.Create(); ///< The MD5 algorithm used to generate the key for the OAuth authentication mechanism.

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
         <summary>  Default constructor, reads the user name and preshared tag from the registry. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 22.10.2014. </remarks>
         */
        public OAuth ()
        {
            ReadSigningConfigFromRegistry();
        }

        /**
         <summary>  Gets parameter string of a URI. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 16.07.2014. </remarks>
        
         <param name="parameters">  parameters to convert in URI format. </param>
        
         <returns>  The parameter string. </returns>
         */
        public static string GetSortedParameterString(System.Collections.Specialized.NameValueCollection parameters)
        {
            if (parameters == null)
                return null;

            SortedDictionary<string, string> sorted = new SortedDictionary<string, string>();

            for (int index = 0; index < parameters.Count; index++)
            {
                string key = parameters.GetKey(index);
                string[] values = parameters.GetValues(index);
                Array.Sort<string>(values);
                foreach (string value in values)
                    sorted.Add(key, value);
            }

            IEnumerator<KeyValuePair<string, string>> enumerator = sorted.GetEnumerator();
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
        
         <returns>  The MD5 hash for OAuth as string. </returns>
         */
        public static String GetMd5Hash(String input)
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

        /**
         <summary>  Gets a signed URI. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 22.07.2014. </remarks>
        
         <param name="prefix">      The prefix of the URI. </param>
         <param name="parameters">  The parameters of the URI, this function adds the parameters user and sign. </param>
         */
        public string GetSignedUri(String prefix, System.Collections.Specialized.NameValueCollection parameters = null)
        {
            System.Collections.Specialized.NameValueCollection parametersCopy
                = parameters != null ?
                new System.Collections.Specialized.NameValueCollection(parameters) :
                new System.Collections.Specialized.NameValueCollection();

            parametersCopy.Add("user", this.UserName);
            string input = this.PresharedTag + '/' + prefix + '/' + GetSortedParameterString(parametersCopy);
            string sign = GetMd5Hash(input);

            string result = prefix + "/?";

            for (int index = 0; index < parametersCopy.Count; index++)
            {
                string key = parametersCopy.GetKey(index);
                string[] values = parametersCopy.GetValues(index);
                Array.Sort<string>(values);
                foreach (string value in values)
                    result += key + '=' + value + '&';
            }

            result += "sign=" + sign;

            return result;
        }

        /**
         <summary>  Adds signs parameters to 'parameters'. </summary>
        
         <remarks>  Dr. Torsten Thurow, TU München, 23.10.2014. </remarks>
        
         <param name="prefix">      The prefix of the URI. </param>
         <param name="parameters">  [in,out] parameters to add. </param>
         */
        public void AddSignParams(String prefix, ref System.Collections.Specialized.NameValueCollection parameters)
        {
            parameters.Add("user", this.UserName);
            string input = this.PresharedTag + '/' + prefix + '/' + GetSortedParameterString(parameters);
            string sign = GetMd5Hash(input);
            parameters.Add("sign", sign);
        }
    }
}
