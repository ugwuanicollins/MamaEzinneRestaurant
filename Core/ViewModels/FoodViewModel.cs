using Core.Enums;
using Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class FoodViewModel
    {
        public int Id { get; set; }
        public string? Image { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
        public string? Description { get; set; }
        public string? UserId { get; set; }
        public IFormFile? ImageUrl { get; set; }
        public List<Food> FoodList { get; set; }
    }
}
