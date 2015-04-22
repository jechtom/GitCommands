using GitCommands.Core.Common;
using GitCommands.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitCommands.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string dir = @"E:\todel\testrepo";
            var repository = new GitRepository(dir);

            Console.WriteLine(string.Format("-- Reading {0}", repository.PathRoot));

            var dirReader = new GitObjectDirectoryReader(repository);
            var objReader = new GitObjectReader();
            var objectsRefs = dirReader.ReadObjects().ToArray();
            var objects = objectsRefs.Select(o => objReader.ReadObject(o, (h) => h.Type != GitObjectHeaderType.Blob)).ToArray();

            Console.WriteLine("-------------");
            foreach (var obj in objects)
            {
                Console.WriteLine(string.Format("OBJECT: {0} {1}", obj.Hash, obj.Header.ToString()));
                switch (obj.Header.Type)
                {
                    case GitObjectHeaderType.Tree:
                        var tree = (GitObjectTree)obj.Data;
                        foreach (var treeRecord in tree.Records)
                            Console.WriteLine(string.Format(" - {0}", treeRecord));
                        break;
                    case GitObjectHeaderType.Blob:
                        break;
                    case GitObjectHeaderType.Commit:
                        var commit = (GitObjectCommit)obj.Data;
                        Console.WriteLine(string.Format(" - Tree: {0}", commit.Tree));
                        foreach (var h in commit.ParentCommits)
                            Console.WriteLine(string.Format(" - Parent: {0}", h));
                        Console.WriteLine(string.Format(" - Author: {0}", commit.Author));
                        Console.WriteLine(string.Format(" - Commited: {0}", commit.Commited));
                        Console.WriteLine(string.Format(" - Comment: {0}", commit.Comment));
                        break;
                    case GitObjectHeaderType.Tag:
                        break;
                    default:
                        break;
                }
                Console.WriteLine("-------------");
            }
            Console.WriteLine(string.Format("-- Total {0} objects", objects.Count()));
        }
    }
}
