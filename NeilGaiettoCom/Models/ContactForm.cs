using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NeilGaiettoCom.Models
{
    public class ContactForm
    {
        [Required, DisplayName("Name")]
        public string FullName { get; set; }

        [Required, DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, DisplayName("Message")]
        public string Message { get; set; }
    }
}