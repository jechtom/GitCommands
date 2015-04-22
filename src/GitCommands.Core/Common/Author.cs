using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitCommands.Core.Common
{
    public class Author
    {
        public static Author Parse(string text)
        {
            var addr = new System.Net.Mail.MailAddress(text);
            return new Author()
            {
                Name = addr.DisplayName,
                Email = addr.Address
            };
        }

        public override string ToString()
        {
            return new System.Net.Mail.MailAddress(Email, Name).ToString();
        }

        public string Name { get; set; }
        public string Email { get; set; }
    }
}
