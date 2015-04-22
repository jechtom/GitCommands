using GitCommands.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GitCommands.Core.Objects
{
    public class GitObjectCommit : GitObjectBase
    {
        Regex authorWithTime = new Regex("^(?<name>.+) (?<time1>[0-9]+) (?<time2>[+-][0-9]{4})$");

        public override void Load(GitObjectHeader header, System.IO.Stream stream)
        {
            base.Load(header, stream);


            // Commit format:
            //tree 47cc632c7b9fbfd7474e66ef71bfad25bb0e5855
            //parent 6f9234a935b1febd0267627558fe0b5dcd6cc318
            //author John Doe <john@doe.com> 1397723047 +0200
            //committer Foo Bar <foo@bar.com> 1397723047 +0200
            //
            //Comment after two end of lines
            //can be multiline.


            var data = ReadDataFromStream(stream);
            var content = this.Encoding.GetString(data);

            // split comment (after header)
            int contentHeadEnd = content.IndexOf("\n\n");
            if (contentHeadEnd > -1)
            {
                this.Comment = content.Substring(contentHeadEnd + 2);
            }
            else
            {
                // no comment
                this.Comment = null;
                contentHeadEnd = content.Length;
            }
            
            // split parameters
            content = content.Substring(0, contentHeadEnd);
            var headers = content.Split('\n').Select(ln =>
            {
                int keyEnd = ln.IndexOf(' ');
                return new
                {
                    Key = ln.Substring(0, keyEnd),
                    Value = ln.Substring(keyEnd + 1)
                };
            }).ToArray();

            // parse
            this.Tree = Hash.Parse(headers.Single(h => h.Key == "tree").Value);
            this.ParentCommits = headers.Where(h => h.Key == "parent").Select(h => Hash.Parse(h.Value)).ToArray();

            string authorString = headers.Single(h => h.Key == "author").Value;
            this.Author = ParseAuthorWithTime(authorString);

            string committerString = headers.Single(h => h.Key == "committer").Value;
            this.Commited = ParseAuthorWithTime(committerString);
        }

        private AuthorWithTime ParseAuthorWithTime(string str)
        {
            //Foo Bar <foo@bar.com> 1397723047 +0200
            var match = authorWithTime.Match(str);
            if (!match.Success)
                throw new InvalidOperationException("Cannot parse author and time from: " + str);

            var result = new AuthorWithTime();
            
            // name
            var nameStr = match.Groups["name"].Value;
            result.Author = Common.Author.Parse(nameStr);

            // pase time
            var time1 = long.Parse(match.Groups["time1"].Value); // 1397723047
            DateTime timeBase = new DateTime(1970, 1, 1).AddSeconds(time1);
            var time2 = match.Groups["time2"].Value; // +0200
            TimeSpan offset = TimeSpan.FromMinutes(int.Parse(time2.Substring(1,2)) * 60 + int.Parse(time2.Substring(3,2)));
            if(time2[0] == '-')
                offset = TimeSpan.Zero - offset;

            result.Time = new DateTimeOffset(timeBase, offset);

            return result;
        }

        public string Comment { get; set; }

        public Hash Tree { get; set; }

        public Hash[] ParentCommits { get; set; }

        public AuthorWithTime Author { get; set; }

        public AuthorWithTime Commited { get; set; }
    }
}
