using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.IO.Compression;
using GitCommands.Core.Objects;
using GitCommands.Core.Common;
using System.Linq;
using System.Diagnostics;

namespace GitCommands.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string dir = @"E:\git\main";
            var repository = new GitRepository(dir);

            var dirReader = new GitObjectDirectoryReader(repository);
            var objReader = new GitObjectReader();
            var objectsRefs = dirReader.ReadObjects().ToArray();
            var objects = objectsRefs.Select(o => objReader.ReadObject(o, (h) => h.Type != GitObjectHeaderType.Blob)).ToArray();

            foreach (var obj in objects)
            {
                
            }
        }
    }
}
