using ChatApp.Data;
using ChatApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace ChatApp.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ILogger<ChatController> _logger;
        private readonly ApplicationDbContext _context;

        public ChatController(ILogger<ChatController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<IdentityUser> users = _context.Users.AsEnumerable();

            ViewBag.Users = users.Select(u => new SelectListItem { Text = u.UserName, Value = u.Id});
            return View();
        }
    }
}