using GitCommands.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitCommands.Core.Objects
{
    public struct TreeRecord
    {
        public int Mode;
        public string Name;
        public Hash Reference;

        public bool IsFile { get { return Mode == 100644; } }
        public bool IsDirectory { get { return Mode == 40000; } }

        public string ModeStringDebug
        {
            get
            {
                if (IsFile)
                    return "[file]";

                if (IsDirectory)
                    return "[dir]";

                return "[unknown]";
            }
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} REF: {3}", Mode, ModeStringDebug, Name, Reference);
        }
    }
}
