using GitCommands.Core.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitCommands.Core.Objects
{
    public class GitObjectReader
    {
        const char SpaceChar = ' ';
        const char EndChar = '\0';

        public GitObjectContainer ReadObject(HashWithPath objectFile, Func<GitObjectHeader, bool> shouldReadContent)
        {
            using (var fileStream = new FileStream(objectFile.Path, FileMode.Open, FileAccess.Read))
            using (var zLibStream = new global::Ionic.Zlib.ZlibStream(fileStream, Ionic.Zlib.CompressionMode.Decompress))
            {
                var result = new GitObjectContainer();

                // read header
                result.Header = ReadObjectUnpacked(zLibStream);
                result.Hash = objectFile.Hash;
                
                // read data?
                if (shouldReadContent(result.Header))
                {
                    result.Data = ReadData(result.Header, zLibStream);
                }

                return result;
            }
        }

        private GitObjectHeader ReadObjectUnpacked(Stream stream)
        {
            // read ASCII type of object and length
            // example: "tree 8\0" (ended with zero byte)
            int currentByte;
            var currentBuffer = new StringBuilder();
            var result = new GitObjectHeader();
            bool isReadingName = true;
            bool isReadingSize = false;
            bool isEnded = false;
            while((currentByte = stream.ReadByte()) != -1)
            {
                var currentChar = (char)currentByte;
                if (isReadingName && currentChar == SpaceChar) // next - type has been read
                {
                    isReadingSize = true;
                    isReadingName = false;
                    result.TypeString = currentBuffer.ToString();
                    currentBuffer.Clear();
                    continue;
                }
                else if(isReadingSize && currentChar == EndChar) // next - size has been read
                {
                    isReadingSize = false;
                    isEnded = true;
                    result.DataLength = long.Parse(currentBuffer.ToString());
                    currentBuffer.Clear();
                    break;
                }
                currentBuffer.Append(currentChar);
            }

            if (!isEnded)
                throw new InvalidOperationException("Header invalid.");

            // resolve type
            result.Type = ResolveType(result.TypeString);

            return result;
        }

        private GitObjectHeaderType ResolveType(string typeString)
        {
            switch (typeString.ToLower())
            {
                case "tree":
                    return GitObjectHeaderType.Tree;
                case "blob":
                    return GitObjectHeaderType.Blob;
                case "commit":
                    return GitObjectHeaderType.Commit;
                default:
                    return GitObjectHeaderType.Unknown;
            }
        }

        private GitObjectBase ReadData(GitObjectHeader header, Ionic.Zlib.ZlibStream stream)
        {
            GitObjectBase instance;
            switch (header.Type)
            {
                case GitObjectHeaderType.Tree:
                    instance = new GitObjectTree();
                    break;
                case GitObjectHeaderType.Blob:
                    instance = new GitObjectBlob();
                    break;
                case GitObjectHeaderType.Commit:
                    instance = new GitObjectCommit();
                    break;
                case GitObjectHeaderType.Tag:
                    instance = new GitObjectTag();
                    break;
                default:
                    throw new InvalidOperationException("Unkown object in header: " + header);
            }

            instance.Load(header, stream);
            return instance;
        }
    }
}
