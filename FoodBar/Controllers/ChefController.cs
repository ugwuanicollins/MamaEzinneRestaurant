using Core.DATABASE;
using Core.Models;
using Core.ViewModels;
using FoodBar.Models;
using Logic.IHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FoodBar.Controllers
{

    public class ChefController : BaseController
    {
        private readonly IWorkHelper _workHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;


        public ChefController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IWorkHelper workHelper, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _workHelper = workHelper;
            _context = context;

        }
        public IActionResult Index()
        {
            IEnumerable<WorkDetails> model = _context.WorkDetails.Where(x => x.Id != 0 && !x.IsDeleted);
            return View(model);
        }

        public IActionResult ChefsDetails()
        {
            return View();
        }


        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(WorkersViewModel model )
        {
            var user = User.Identity.Name.ToString();
            if (model != null)
            {
                if(model.Name == string.Empty)
                {
                    SetMessage("Name is Required", Message.Category.Error);
                    return View(model);
                }
                if (model.Salary == double.MinValue)
                {
                    SetMessage("Salary is Required", Message.Category.Error);
                    return View(model);
                }
                if (model.Email == string.Empty)
                {
                    SetMessage("Email is Required", Message.Category.Error);
                    return View(model);
                }
                var createWork = _workHelper.CreateWorks(model, user);
                
            }
            return RedirectToAction("Index", "Chef");
        }


        //Edit-GET
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var work = _context.WorkDetails.Find(id);
            if (work == null)
            {
                return NotFound();
            }
            return View(work);
        }


        //POST- EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(WorkDetails work)
        {
            if (work != null)
            {
                var updateWork = _workHelper.EditWork(work);
                if (updateWork)
                {
                    SetMessage("Work Is Updated Successfully", Message.Category.Information);
                    return RedirectToAction("Index");
                }
            }
            SetMessage("Unable to update work", Message.Category.Error);
            return View(work);
        }


        //Delete-GET
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var model = _context.WorkDetails.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        //POST- DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(WorkDetails work)
        {
            if (work != null)
            {
                var removeWork = _workHelper.DeleteWork(work);
                if (removeWork)
                {
                    SetMessage("Work is deleted Successfully", Message.Category.Information);
                    return RedirectToAction("Index");
                }
            }
            SetMessage("Unable to delete work", Message.Category.Error);
            return View(work);
        }
    }
}

