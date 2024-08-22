using Microsoft.AspNetCore.Mvc;

namespace Chat_Application.Controllers
{
    public class ChatRoomController : Controller
    {
        public IActionResult PublicChat()
        {
            if (User?.Identity?.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }



        public IActionResult PrivateChat()
        {




            return View();


        }








        public IActionResult Home()
        {
           return View();
        }

    }
}
