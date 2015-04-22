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
    public class GitObjectTree : GitObjectBase
    {
        Encoding encoding = Encoding.UTF8; // default (can be different in legacy repositories)
        Regex nameSplitter = new Regex("^(?<mode>[0-9]+) (?<name>.+)$");
        
        public override void Load(GitObjectHeader header, Stream stream)
        {
            base.Load(header, stream);

            var records = new List<TreeRecord>();
            
            byte[] data = ReadDataFromStream(stream);

            // format:
            // [mode] [file/folder name]\0[SHA-1 of referencing blob or tree]
            // [mode] [file/folder name]\0[SHA-1 of referencing blob or tree]
            // ...

            int recordStart = 0;
            const int hashLength = 20; // 20B - SHA1
            for (int i = 0; i < data.Length + 1; i++)
            {
                bool isEnd = i == data.Length; // take one more iteration after array ends
                bool isHashStart = !isEnd && data[i] == 0; // is there 0 value? that is hash placeholder

                if (isHashStart)
                {
                    // parse record mode and name
                    var recordTextPart = encoding.GetString(data, recordStart, i - recordStart);
                    var recordTextMatch = nameSplitter.Match(recordTextPart);
                    if (!recordTextMatch.Success)
                        throw new InvalidOperationException("Cannot parse record: " + recordTextPart);

                    // write
                    var record = new TreeRecord();
                    record.Mode = int.Parse(recordTextMatch.Groups["mode"].Value);
                    record.Name = recordTextMatch.Groups["name"].Value;

                    // read hash
                    i++;
                    record.Reference = new Hash(data, i, hashLength);
                    
                    // add to result
                    records.Add(record);

                    // move
                    i += hashLength;
                    recordStart = i;
                }
            }

            Records = records.ToArray();
        }

        public TreeRecord[] Records { get; private set; }
    }
}
