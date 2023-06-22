using Core.DATABASE;
using Core.Models;
using Core.ViewModels;
using Logic.IHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Security.Cryptography;
using NuGet.Packaging.Signing;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Logic.Helpers
{
    public class FoodHelper : IFoodHelper
    {
        private readonly AppDbContext _context;
        private readonly IUserHelper _userHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly IHostingEnvironment _hostingEnvironment;

        public FoodHelper(AppDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserHelper userHelper, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _userHelper = userHelper;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<FoodViewModel> AdminAddFood(FoodViewModel foodViewModel, string user)
        {
            try
            {
                var processedImageUrl = ProcessFoodImage(foodViewModel);
                var userId = _userHelper.FindByUserName(user);
                if (foodViewModel != null && user != null)
                {
                    var foodView = new Food();

                    foodView.UserId = userId.Id;
                    foodView.Name = foodViewModel.Name;
                    foodView.Price = foodViewModel.Price;
                    foodView.Image = processedImageUrl;
                    foodView.Description = foodViewModel.Description;
                    foodView.DateCreated = DateTime.Now;
                    foodView.IsDeleted = false;
                    foodView.IsActive = true;


                    _context.Foods.Add(foodView);
                    _context.SaveChanges();
                    return foodViewModel;
                }
                return foodViewModel;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
        public string ProcessFoodImage(FoodViewModel foodViewModel)
        {
            try
            {
                var uniqueFileName = string.Empty;
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "pic");
                string pathString = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pic");
                if (!Directory.Exists(pathString))
                {
                    Directory.CreateDirectory(pathString);
                }
                uniqueFileName = Guid.NewGuid().ToString() + "_" + foodViewModel.ImageUrl.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    foodViewModel.ImageUrl.CopyTo(fileStream);
                }
                return "/pic/" + uniqueFileName;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        public async Task<Food> GetFoodById(int foodId)
        {
            try
            {
                if (foodId > 0)
                {
                    var food = _context.Foods.Where(g => g.Id == foodId && g.IsActive && !g.IsDeleted).FirstOrDefault();
                    if (food != null)
                    {
                        return food;
                    }
                    return null;
                }
                return null;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }


        public List<Food> GetFoodDropdown()
        {
            try
            {
                var food = new Food()
                {
                    Id = 0,
                    Name = "__Select__",

                };
                var foods = _context.Foods.Where(x => x.Id != 0 && !x.IsDeleted).ToList();
                if (foods.Any())
                {
                    foods.Insert(0, food);
                    return foods;
                }
                return null;
            }
            catch (Exception exp)
            {

                throw exp;
            }
        }


        public async Task<SalesRecord> SaveSalesRecord(int foodId, double price, DateTime recordDate,string user, double total, int quantity)
        {
            if(foodId != 0)
            {
                var food = _context.Foods.Where(x => x.Id == foodId && !x.IsDeleted).FirstOrDefault();
                var salesRecord = new SalesRecord()
                {
                    FoodId = foodId,
                    Price = price,
                    RecordDate = recordDate,
                    Name = food?.Name,
                    UserId = user,
                    Total = total,
                    Quantity = quantity,
                    GrandTotal = total * quantity,
                    IsActive = true,
                    IsDeleted = false,
                };
                _context.SalesRecords.Add(salesRecord);
                _context.SaveChanges();
                return salesRecord;
            }
            return null;
        }
             
             
        }


    }






