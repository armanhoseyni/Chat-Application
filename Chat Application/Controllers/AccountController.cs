using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Chat_Application.Areas.Identity.Data;
using System.Net.Mail;
using System.Net;
using OnlineMarket.ViewModels;

namespace Chat_Application.Controllers
{
    public class AccountController : Controller
    {

        UserManager<ApplicationUser> userManager;
        SignInManager<ApplicationUser> signInManager;


        public AccountController(SignInManager<ApplicationUser> _signInManager, UserManager<ApplicationUser> _userManager)

        {
            signInManager = _signInManager;
            userManager = _userManager;
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            TempData["message"] = "شما با موفقیت از سایت خارج شدید";

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult error()
        {
            return View();
        }

        public async Task<IActionResult> RegisterConfirm(UserRegisterViewModel model)
        {
            ApplicationUser user = await userManager.FindByNameAsync(model.UserName);



            user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailConfirmed = false



            };
            if (model.Image != null)
            {

                byte[] b = new byte[model.Image.Length];

                model.Image.OpenReadStream().Read(b);
                user.Image = b;

            }

            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {

                //quda nsmw oqcj rqsf 
                //arman.82.ah@8@gmail.com
                //SMTP==>smto.gmail.com 587/465 ssl


                MailMessage mailMessage = new MailMessage("arman.82.ah@gmail.com", user.Email);
                mailMessage.Subject = "Confirm your Account";
                mailMessage.IsBodyHtml = true;
                string token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                string address = Url.Action("ConfirmEmail", "Account", new { id = user.Id, token = token }, "https");
                mailMessage.Body = $"Hi<b>{user.FirstName}<b>" +

                    $"Please Click this <a href='{address}'>Link </a> to confirm your email";
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential("arman.82.ah@gmail.com", "qudansmwoqcjrqsf");
                smtpClient.Send(mailMessage);
            }
            else
            {
            }
            return RedirectToAction("Register");

        }
        public async Task<IActionResult> ConfirmEmail(string id, string token)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);
            IdentityResult result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");


            }
            else { }

            return RedirectToAction("Register");


        }

        public IActionResult Login() => View();

        public async Task<IActionResult> LoginConfirm(UserLoginViewModel model)

        {

            bool recaptchaisvalid = CheckRecaptchastatus();

            if (recaptchaisvalid == false)
            {
                return RedirectToAction("Login", "Account");
            }
            ApplicationUser user = await userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {

                TempData["message"] = "نام کاربری موجود نمیباشد";
                return RedirectToAction("Login", "Account");


            }

            var result = await signInManager.PasswordSignInAsync(user, model.Password,
                model.RemmeberMe, true);

            if (result.Succeeded)
            {

                TempData["message"] = "شما با  موفقیت وارد سایت شدید";
                return RedirectToAction("PublicChat", "ChatRoom");

            }


            else if (result.IsLockedOut == true)
            {
                TempData["message"] = "نام کاربری شما به مدت چند دقیقه فیر فعال میباشد";

                return RedirectToAction("Login", "Account");

            }


            else
            {
                TempData["message"] = "نام کاربری یا رمز عبور اشتباه میباشد";

                return RedirectToAction("Login", "Account");

            }



            // btnlogin div.recaptcha result.sussed







        }

        private bool CheckRecaptchastatus()
        {

            string secretkey = "6LfHLhUqAAAAAH3i-u64bllk9PlNkypL60hrCi6k";
            var captcha = Request.Form["g-recaptcha-response"];
            //api
            HttpClient httpClient = new HttpClient();
            var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?" +
                 $"secret={secretkey}&response={captcha}").Result;
            if (res.StatusCode != HttpStatusCode.OK)
            {
                return false;

            }

            string json = res.Content.ReadAsStringAsync().Result;

            Recaptcha recaptcha = Newtonsoft.Json.JsonConvert.DeserializeObject<Recaptcha>(json);




            return recaptcha.success;

        }


        class Recaptcha
        {
            public bool success { get; set; }
            public DateTime challenge_ts { get; set; }
            public string hostnaem { get; set; }

        }
    }
}
