using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleMessenger.Data.Models
{
    [Table("Messages")]
    public class Message
    {
        [Column("Id", TypeName = "bigint")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("ReceivedDate", TypeName = "datetime2")]
        [Required]
        public DateTime ReceivedDate { get; set; }

        [Column("Subject", TypeName = "nvarchar(300)")]
        [Required]
        public string Subject { get; set; }

        [Column("Body", TypeName = "nvarchar(max)")]
        [Required]
        public string Body { get; set; }

        [Column("SenderId", TypeName = "nvarchar(450)")]
        [Required]
        public string SenderId { get; set; }

        [ForeignKey("SenderId")]
        public User Sender { get; set; }

        [InverseProperty("IncomingMessages")]
        public ICollection<User> Recipients { get; set; }
    }
}
