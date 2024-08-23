using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using Chat_Application.Areas.Identity.Data;
using Chat_Application.Models;
using DB;
using System.Security.Cryptography;

namespace Chat_Application.Hubs
{
    
    public class ChatRoom : Hub
    {

        Chat_ApplicationContext db;

        public ChatRoom(Chat_ApplicationContext db)
        {

            this.db = db;
        }
        public async Task SendMessageFromHtmlToAsp(string message, string username,string receipientuser,string name, [FromServices] UserManager<ApplicationUser> userManager)

        {
            //Alll

            if (receipientuser == "")


            {

                receipientuser = "admin@gmail.com";
                ApplicationUser RecepientUserinfo = await userManager.FindByNameAsync(receipientuser);


                ApplicationUser SendertUserinfo = await userManager.FindByNameAsync(username);

                UserMessage userMessage = new UserMessage
                {
                    SenderUser_Id = SendertUserinfo.Id
                  ,
                    RecepientUser_Id = RecepientUserinfo.Id,
                    Message = message,
                    MessageSentTime = DateTime.Now,



                };
                db.Add(userMessage);
                db.SaveChanges();
                var time = DateTime.Now.ToString("HH:mm");

                await Clients.All.SendAsync("SendMessageFromAsptoHtml", username, message, time);



            }
            else
            {
                ApplicationUser RecepientUserinfo = await userManager.FindByNameAsync(receipientuser);

                ApplicationUser SendertUserinfo = await userManager.FindByNameAsync(username);



                UserMessage userMessage = new UserMessage
                {
                    SenderUser_Id = SendertUserinfo.Id
                    ,
                    RecepientUser_Id = RecepientUserinfo.Id,
                    Message = message,
                    MessageSentTime = DateTime.Now,



                };
                db.Add(userMessage);
                db.SaveChanges();
                var time = DateTime.Now.ToString("HH:mm");
                if (RecepientUserinfo.CurrentRoom == SendertUserinfo.Id)
                {
                    await Clients.Clients(new List<string> { RecepientUserinfo.connectionId, SendertUserinfo.connectionId })
                                  .SendAsync("SendMessageFromAsptoHtml", name, message, time);
                }
                else
                {
                    await Clients.Client(SendertUserinfo.connectionId).SendAsync("SendMessageFromAsptoHtml", name, message, time);
                    await Clients.Client(RecepientUserinfo.connectionId).SendAsync("Notification", name, message, time);
                }
            }


        }

    /*    public async Task CreateGuest( [FromServices] UserManager<ApplicationUser> userManager)

        {
            var guest = "";
            while (true) {
                var b = "1234567890";
                Random ran = new Random();
                int length = 6;

                String random = "";

                for (int i = 0; i < length; i++)
                {
                    int a = ran.Next(10);
                    random = random + b.ElementAt(a);
                }

                var g= "Guest" + random;
                ApplicationUser CheckUser = await userManager.FindByNameAsync(guest);
                if (CheckUser == null) {
                    guest = g;
                    break;
                }
            }

            ApplicationUser NewUser = new ApplicationUser
            {
                UserName = guest    
                


            };
            db.Add(NewUser);
            db.SaveChanges();



            ApplicationUser SenderUserinfo = await userManager.FindByNameAsync(guest);


            
        }
*/



        public async Task RegisterUserInSignalR(string username,[FromServices]UserManager<ApplicationUser> userManager)
        {


            string connectionId = Context.ConnectionId;
            ApplicationUser user =await userManager.FindByNameAsync(username);
            user.connectionId = connectionId;
            await userManager.UpdateAsync(user);


            List<string> onlineusers = userManager.Users.ToList().Where(x => x.connectionId != "").Select(x => x.UserName).ToList();

            await Clients.All.SendAsync("RefreshOnlineUsers", onlineusers);


        }

        
        public async Task UploadMessagesPublic(string username, [FromServices] UserManager<ApplicationUser> userManager)
        {

            var rec = "118ad720-0788-4099-9634-fc75136768a8";

           List<string> allmessages = (List<string>)db.UserMessages.ToList().Where(x => x.RecepientUser_Id == rec).Select(x => x.Message).ToList();
            List<string> alldates = db.UserMessages
                           .Where(x => x.RecepientUser_Id == rec)
                           .Select(x => x.MessageSentTime.ToString("HH:mm"))
                           .ToList();



            

            
            
            var messages = db.UserMessages
                .Where(x => x.RecepientUser_Id == rec)
                .ToList();




            var allusers = messages
         .Join(userManager.Users,
          message => message.SenderUser_Id,
          user => user.Id,
          (message, user) => user.UserName)
             .ToList();



            await Clients.All.SendAsync("RefreshMessages", username,allmessages,alldates,allusers);


        }



        public async Task UploadMessagesPrivate(string recepName,string senderName,  [FromServices] UserManager<ApplicationUser> userManager)
        {
            var senderID = userManager.Users
                .Where(x => x.UserName == senderName)
                .Select(x => x.Id)
                .FirstOrDefault();
            var recepID = userManager.Users
                .Where(x => x.UserName == recepName)
                .Select(x => x.Id)
                .FirstOrDefault();




            ApplicationUser SenderUser = await userManager.FindByNameAsync(senderName);
            SenderUser.CurrentRoom=recepID;

            await userManager.UpdateAsync(SenderUser);


            List<string> allmessages = db.UserMessages
                   .Where(x => (x.RecepientUser_Id == recepID && x.SenderUser_Id == senderID) || (x.RecepientUser_Id == senderID && x.SenderUser_Id == recepID))
                   .Select(x => x.Message)
                   .ToList();

            List<string> alldates = db.UserMessages
                  .Where(x => (x.RecepientUser_Id == recepID && x.SenderUser_Id == senderID) || (x.RecepientUser_Id == senderID && x.SenderUser_Id == recepID))
                  .Select(x => x.MessageSentTime.ToString("HH:mm"))
                  .ToList();






            /*     List<string> alldates = db.UserMessages
                                .Where(x => x.RecepientUser_Id == rece)
                                .Select(x => x.MessageSentTime.ToString("HH:mm"))
                                .ToList();
     */

            /*
                        var messages = db.UserMessages
                            .Where(x =>( x.RecepientUser_Id == rece && x.SenderUser_Id==send || x.RecepientUser_Id == send && x.SenderUser_Id == rece))
                            .ToList();

                        var allusers = messages
                     .Join(userManager.Users, message => message.SenderUser_Id, user => user.Id,(message, user) => user.FirstName)
                         .ToList();

            */

            // Filter messages based on the given conditions
            var filteredMessages = db.UserMessages
                .Where(x => (x.RecepientUser_Id == recepID && x.SenderUser_Id == senderID) || (x.RecepientUser_Id == senderID && x.SenderUser_Id == recepID));

            // Join the filtered messages with userManager.Users and select the FirstName of the sender
            var allusers = filteredMessages
                .Join(userManager.Users,
                      message => message.SenderUser_Id,
                      user => user.Id,
                      (message, user) => user.UserName)
                .ToList();

            //  await Clients.All.SendAsync("RefreshMessages", allmessages, alldates, allusers);

            ApplicationUser SenderUserinfo = await userManager.FindByNameAsync(senderName);


            await Clients.Client(SenderUserinfo.connectionId).SendAsync("RefreshMessages", allmessages, alldates, allusers);
        }






        public async Task ExitOfChat(string username, [FromServices] UserManager<ApplicationUser> userManager)
        {

            ApplicationUser user = await userManager.FindByNameAsync(username);
            user.connectionId = "";
            user.CurrentRoom = null;
            await userManager.UpdateAsync(user);

            await Clients.All.SendAsync("RemoveUsersFromOnlineUsers", user.UserName);


            /*
                        List<string> onlineusers = userManager.Users.ToList().Where(x => x.connectionId != "").Select(x => x.UserName).ToList();

                        await Clients.All.SendAsync("RefreshOnlineUsers", onlineusers);*/
        }

    }
}