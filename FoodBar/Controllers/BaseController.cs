using FoodBar.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodBar.Controllers
{
    public class BaseController : Controller
    {
        protected void SetMessage(string message, Message.Category messageType)
        {
            Message msg = new Message(message, (int)messageType);
            TempData["Message"] = msg;
        }
    }
}
