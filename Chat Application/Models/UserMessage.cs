using Chat_Application.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat_Application.Models
{
    public class UserMessage
    {

        public int Id { get; set; }

        public string Message { get; set; }

        public DateTime MessageSentTime { get; set; }



        public string? SenderUser_Id { get; set; }

        [ForeignKey("SenderUser_Id")]
        public ApplicationUser SenderUser { get; set; }

        ///////////
        ///
        public string? RecepientUser_Id { get; set; }

        [ForeignKey("RecepientUser_Id")]
        public ApplicationUser? RecepientUser { get; set; }

        public Boolean seened { get; set; }
        








    }
}
