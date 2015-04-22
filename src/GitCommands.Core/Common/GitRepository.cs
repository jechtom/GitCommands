using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitCommands.Core.Common
{
    public class GitRepository
    {
        public GitRepository(string path)
        {
            this.PathRoot = path;
            this.PathGit = Path.Combine(PathRoot, ".git");
            this.PathGitObjects = Path.Combine(PathGit, "objects");
        }

        public string PathRoot { get; private set; }
        public string PathGit { get; private set; }
        public string PathGitObjects { get; private set; }
    }
}
