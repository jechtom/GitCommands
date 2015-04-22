using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitCommands.Core.Objects
{
    public enum GitObjectHeaderType
    {
        Unknown,
        Tree,
        Blob,
        Commit,
        Tag
    }
}
