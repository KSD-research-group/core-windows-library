using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Ksd.Mediatum.Surfid.Fingerprint
{
    public class Object
    {
        public string Id { get; internal set; }

        public string Type { get; internal set; }

        public Point Center { get; internal set; }

        public IEnumerable<Point> Corners { get; internal set; }

        public IEnumerable<Object> Children { get; internal set; }

        internal Object (dynamic json)
        {
            this.Id = json.id;
            this.Type = json.type;

            dynamic center = json.center;
            this.Center = new Point((int)center.x, (int)center.y);

            List<Point> corners = new List<Point>();
            foreach(dynamic corner in json.corners)
                corners.Add(new Point((int)corner.x, (int)corner.y));

            this.Corners = corners;
        }
    }
}
