using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace WordToPdfConvert.Consumer.Model
{
    public class Message
    {
        public byte[] File { get; set; }
        public string Email { get; set; }
        public string FileName { get; set; }

    }
}
