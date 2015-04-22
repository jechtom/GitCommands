using GitCommands.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitCommands.Core.Objects
{
    public class AuthorWithTime
    {
        public Author Author { get; set; }

        public DateTimeOffset Time { get; set; }

        public override string ToString()
        {
            return string.Format("{0} at {1}", Author, Time);
        }
    }
}
