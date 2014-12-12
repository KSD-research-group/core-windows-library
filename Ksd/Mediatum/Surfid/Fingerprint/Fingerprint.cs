using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksd.Mediatum.Surfid.Fingerprint
{
    public class Fingerprint
    {
        public IEnumerable<Object> Objects { get; internal set; }

        internal static void GetFingerprint(Server server, string value)
        {
            dynamic jsonDe = Newtonsoft.Json.JsonConvert.DeserializeObject(value);
            
            string status = jsonDe.status;
            bool manuallyValidated = (jsonDe.manuallyValidated == "True" || jsonDe.manuallyValidated == "true");
            string id = jsonDe.id;

            dynamic fingerprint = jsonDe.fingerprint;
            List<Object> objects = new List<Object>();

            foreach (dynamic obj in fingerprint.objects)
                objects.Add(new Object(obj));
        }
    }
}
