using Core.DATABASE;
using Core.Models;
using FoodBar.Models;
using Logic.IHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodBar.Controllers
{

    public class JobApplicationController : BaseController
    {
        private readonly IWorkHelper _workHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;


        public JobApplicationController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IWorkHelper workHelper, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _workHelper = workHelper;
            _context = context;

        }
        public IActionResult Index()
        {
            IEnumerable<JobApplication> jobs = _context.JobApplication.Where(x => x.Id != 0 && !x.IsDeleted);
            return View(jobs);
        }


        //GET-ADD
        [HttpGet]
        public IActionResult Create(int id)
        {
            var jobData = new JobApplication();
            if (id > 0)
            {
                jobData.WorkDetailsId = id;
                return View(jobData);
            }
            return View();
        }

        //POST-CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(JobApplication job)
        {
            var user = User.Identity.Name.ToString();
            if (job != null)
            {
                if(job.Name == string.Empty)
                {
                    SetMessage("Name is Required", Message.Category.Error);
                    return View(job);
                }
                if (job.Email == string.Empty)
                {
                    SetMessage("Email is Required", Message.Category.Error);
                    return View(job);
                }
                if (job.PhoneNumber == string.Empty )
                {
                    SetMessage("PhoneNumber is Required", Message.Category.Error);
                    return View(job);
                }
                var applyForJob = _workHelper.ApplyForJob(job, user);
                if (applyForJob)
                {
                    return RedirectToAction("Index","JobApplication");
                }
            }
            return View(job);
        }


        //Get-EDIT
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id > 0)
            {
             var job = _workHelper.GetJobApplicant(id);
                if (job != null)
                {
                    return View(job);
                }
            }
            return View();
        }

        //POST-EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(JobApplication job)
        {
            if (job != null)
            {
               var updateJob = _workHelper.PostJobApplicant(job);
                if (updateJob)
                {
                    SetMessage("JobApplication is updated successfully", Message.Category.Information);
                    return RedirectToAction("Index");
                }
            }
            SetMessage("Unable to Apply For job", Message.Category.Error);
            return View(job);
        }


       //Get
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id > 0)
            {
                var job = _context.JobApplication.Find(id);
                if (job != null)
                {
                    return View(job);
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(JobApplication job)
        {
            if (job != null)
            {
                var removeJob = _workHelper.DeleteJob(job);
                if (removeJob)
                {
                    SetMessage("Job is deleted successfully", Message.Category.Information);
                    return RedirectToAction("Index");
                }
            }
            SetMessage("Unable to delete job", Message.Category.Error);
            return View(job);
        }
    }
}
