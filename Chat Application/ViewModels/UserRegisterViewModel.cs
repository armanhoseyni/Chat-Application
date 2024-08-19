using System.ComponentModel.DataAnnotations;

namespace OnlineMarket.ViewModels
{
    public class UserRegisterViewModel
    {
        [Required(ErrorMessage ="enter your user name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "enter your user password")]

        public string Password { get; set; }
        [Compare("Password",ErrorMessage ="not match!!")]
        public string RePassword { get; set; }
        [Required(ErrorMessage = "enter your fisrt name")]

        public string FirstName { get; set; }
        [Required(ErrorMessage = "enter your last name")]

        public string LastName { get; set; }
        
        // hmishe toye viewmodel image b sorat form file byd bshe
        public IFormFile  Image{ get; set; }
    }
}
