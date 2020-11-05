using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WordToPdfConvert.Web.Publisher.Models
{
    public class Content
    {
        public IFormFile File { get; set; }
        public string Email { get; set; }
    }
}
