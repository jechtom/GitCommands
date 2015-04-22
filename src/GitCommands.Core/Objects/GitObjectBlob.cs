using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitCommands.Core.Objects
{
    public class GitObjectBlob : GitObjectBase
    {
        public override void Load(GitObjectHeader header, System.IO.Stream stream)
        {
            base.Load(header, stream);
            Data = ReadDataFromStream(stream);
        }

        public byte[] Data { get; private set; }
    }
}
