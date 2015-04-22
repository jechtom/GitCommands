using GitCommands.Core.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitCommands.Core.Objects
{
    public struct GitObjectHeader
    {
        public string TypeString;
        public long DataLength;
        public GitObjectHeaderType Type;

        public override string ToString()
        {
            return string.Format("{0} (\"{1}\"), data: {2}B", Type, TypeString, DataLength);
        }
    }
}
