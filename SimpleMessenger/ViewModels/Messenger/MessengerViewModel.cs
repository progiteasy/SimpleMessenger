using System.Collections.Generic;

namespace SimpleMessenger.ViewModels.Messenger
{
    public class MessengerViewModel
    {
        public List<MessageViewModel> ReceivedMessageModels { get; set; }
        public MessageViewModel MessageToSendModel { get; set; }
    }
}
