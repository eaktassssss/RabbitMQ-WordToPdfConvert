using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WordToPdfConvert.Web.Publisher.Models
{
    public class Message
    {
        public byte[] File { get; set; }
        public string Email { get; set; }
        public string FileName { get; set; }
    }
}
