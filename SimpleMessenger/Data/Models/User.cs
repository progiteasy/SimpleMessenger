using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleMessenger.Data.Models
{
    [Table("Users")]
    public class User : IdentityUser
    {
        [Column("RegistrationDate", TypeName = "datetime2")]
        [Required]
        public DateTime RegistrationDate { get; set; }

        [InverseProperty("Sender")]
        public ICollection<Message> OutgoingMessages { get; set; }

        [InverseProperty("Recipients")]
        public ICollection<Message> IncomingMessages { get; set; }
    }
}
