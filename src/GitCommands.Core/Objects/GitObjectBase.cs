using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitCommands.Core.Objects
{
    public abstract class GitObjectBase
    {
        public GitObjectBase()
        {
            Encoding = Encoding.UTF8; // default (can be different in legacy repositories)
        }

        public Encoding Encoding { get; set; }

        public GitObjectHeader Header { get; private set; }

        public virtual void Load(GitObjectHeader header, Stream stream)
        {
            this.Header = header;
        }

        protected byte[] ReadDataFromStream(Stream input)
        {
            byte[] data = new byte[Header.DataLength];
            int read = 0;
            while(read < data.Length - 1)
            {
                int offset = input.Read(data, read, data.Length - read);
                if (offset <= 0)
                    throw new InvalidOperationException("Unexpected data end.");

                read += offset;
            }

            return data;
        }
    }
}
