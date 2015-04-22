using GitCommands.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitCommands.Core.Objects
{
    public class GitObjectContainer
    {
        public GitObjectHeader Header { get; set; }
        public GitObjectBase Data { get; set; }

        public override string ToString()
        {
            return string.Format("{0}; Data loaded: {1}", Header, Data != null);
        }

        public Hash Hash { get; set; }
    }
}
