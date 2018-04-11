using System.Collections.Generic;

namespace ArticoleCalarie.Models
{
    public class EmailModel
    {
        public List<string> To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string ReplyTo { get; set; }
    }
}
