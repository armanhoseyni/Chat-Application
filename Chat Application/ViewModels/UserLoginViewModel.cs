using System.ComponentModel.DataAnnotations;

namespace OnlineMarket.ViewModels
{
    public class UserLoginViewModel
    {

        [Required(ErrorMessage = "enter your user name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "enter your user password")]

        public string Password { get; set; }


        public bool RemmeberMe { get; set; }   
    }
}
