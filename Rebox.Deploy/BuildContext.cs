using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebox.Deploy
{
    public static class BuildContext
    {
        public static PackageConfiguration ApplicationConfiguration { get; set; }

        public static Dictionary<string, string> Content { get; set; }
    }
}
