using System.ComponentModel.DataAnnotations;

namespace SimpleMessenger.ViewModels.Messenger
{
    public class MessageViewModel
    {
        [Required(ErrorMessage = "Subject is required")]
        [StringLength(300, MinimumLength = 1, ErrorMessage = "Subject must be between {2} and {1} characters")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Body is required")]
        public string Body { get; set; }

        [Required(ErrorMessage = "At least one recipient is required")]
        [RegularExpression(@"^(\s*[A-za-z0-9\@\+\-\:\._ ]+\s*(?:;\s*[A-za-z0-9\@\+\-\:\._ ]+\s*)*)(;\s*)*$", ErrorMessage = "The string with recipients is incorrect")]
        public string Recipients { get; set; }

        public string ReceivedDate { get; set; }
        public string Sender { get; set; }
    }
}
