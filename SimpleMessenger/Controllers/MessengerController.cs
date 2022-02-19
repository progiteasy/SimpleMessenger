using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SimpleMessenger.Data.Contexts;
using SimpleMessenger.Data.Models;
using SimpleMessenger.Extensions;
using SimpleMessenger.Services;
using SimpleMessenger.ViewModels.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleMessenger.Controllers
{
    [Route("")]
    [Authorize]
    public class MessengerController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IHubContext<MessageNotifierHub> _messageNotifierHub;

        public MessengerController(AppDbContext dbContext, IHubContext<MessageNotifierHub> messageNotifierHub)
        {
            _dbContext = dbContext;
            _messageNotifierHub = messageNotifierHub;
        }

        [HttpGet]
        public async Task<IActionResult> GetIndexViewAsync()
        {
            var loggedInUser = await _dbContext.Users.Include(user => user.IncomingMessages).ThenInclude(message => message.Sender).
                SingleOrDefaultAsync(user => user.UserName == User.Identity.Name);
            if (loggedInUser == null)
                return Redirect("/account/login");

            var messengerModel = new MessengerViewModel()
            {
                ReceivedMessageModels = loggedInUser.IncomingMessages.Select(message => new MessageViewModel()
                {
                    Subject = message.Subject,
                    Body = message.Body,
                    ReceivedDate = message.ReceivedDate.ToString(),
                    Sender = message.Sender.UserName,
                }).OrderByDescending(messageModel => messageModel.ReceivedDate).ToList(),
                MessageToSendModel = new MessageViewModel()
            };

            if (Request.IsAjax())
                return PartialView("_ReceivedMessagesPartialView", messengerModel);
            
            return View("IndexView", messengerModel);
        }

        [HttpPost("message/send")]
        public async Task<IActionResult> SendMessage(MessengerViewModel messengerModel)
        {
            if (!ModelState.IsValid)
                return View("Messenger", messengerModel);

            var loggedInUser = await _dbContext.Users.SingleOrDefaultAsync(user => user.UserName == User.Identity.Name);
            if (loggedInUser == null)
                return Redirect("/account/login");

            var recipientUsernames = messengerModel.MessageToSendModel.Recipients.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).
                Select(recipientUsername => recipientUsername.Trim()).ToList();
            var recipients = new List<User>();

            foreach (var recipientUsername in recipientUsernames)
            {
                var currentRecipient = await _dbContext.Users.SingleOrDefaultAsync(user => user.UserName == recipientUsername);
                if (currentRecipient != null && !recipients.Contains(currentRecipient))
                    recipients.Add(currentRecipient);
            }

            var message = new Message()
            {
                Subject = messengerModel.MessageToSendModel.Subject,
                Body = messengerModel.MessageToSendModel.Body,
                Sender = loggedInUser,
                Recipients = recipients,
                ReceivedDate = DateTime.Now
            };

            messengerModel.MessageToSendModel.Recipients = null;
            messengerModel.MessageToSendModel.Sender = message.Sender.UserName;
            messengerModel.MessageToSendModel.ReceivedDate = message.ReceivedDate.ToString();

            await _messageNotifierHub.Clients.Users(message.Recipients.Select(recipient => recipient.Id)).
                SendAsync("ShowNotification", new { receivedDate = messengerModel.MessageToSendModel.ReceivedDate, sender = messengerModel.MessageToSendModel.Sender });
            await _dbContext.Messages.AddAsync(message);
            await _dbContext.SaveChangesAsync();

            return Redirect("/");
        }
    }
}
