using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebox.Deploy
{
    public struct PackageConfiguration
    {
        public string Name { get; set; }
        public Guid Guid { get; set; }
        public Version Version { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
    }
}
