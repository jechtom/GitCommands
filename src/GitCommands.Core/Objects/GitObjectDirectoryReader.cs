using GitCommands.Core.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GitCommands.Core.Objects
{
    public class GitObjectDirectoryReader
    {
        private GitRepository repo;
        private Regex objectDirRegex = new Regex("^[a-f0-9]{2}$", RegexOptions.IgnoreCase);
        private Regex objectFileRegex = new Regex("^[a-f0-9]{38}$", RegexOptions.IgnoreCase);

        public GitObjectDirectoryReader(GitRepository repo)
        {
            this.repo = repo;
        }

        public IEnumerable<HashWithPath> ReadObjects()
        {
            foreach(var path in Directory.EnumerateDirectories(repo.PathGitObjects))
            {
                string dirName = Path.GetFileName(path);
                if (!objectDirRegex.IsMatch(dirName))
                    continue;

                foreach (var pathFile in Directory.EnumerateFiles(path))
                {
                    string fileName = Path.GetFileName(pathFile);
                    if (!objectFileRegex.IsMatch(fileName))
                        continue;

                    Hash hash = Hash.Parse(dirName + fileName); // combine to full hash

                    yield return new HashWithPath()
                    {
                        Hash = hash,
                        Path = pathFile
                    };
                }
            }
        }
    }
}
