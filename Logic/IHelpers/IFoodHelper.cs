﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Core.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Logic.IHelpers
{
    public interface IFoodHelper
    {
        Task<FoodViewModel> AdminAddFood(FoodViewModel foodViewModel, string user);
        Task<Food> GetFoodById(int foodId);
        //bool DeleteFood(FoodViewModel foodViewModelId);
        List<Food> GetFoodDropdown();
        bool EditFood(FoodViewModel foodViewModel);
        bool DeleteFood(int id);
        string ProcessFoodImage(IFormFile image);
    }
}
