using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Core.ViewModels;

namespace Logic.IHelpers
{
    public interface IFoodHelper
    {
        Task<FoodViewModel> AdminAddFood(FoodViewModel foodViewModel, string user);
        Task<Food> GetFoodById(int foodId);
        //bool DeleteFood(FoodViewModel foodViewModelId);
        List<Food> GetFoodDropdown();
        Task<SalesRecord> SaveSalesRecord(int foodId, double price, DateTime recordDate, string user, double total, int quantity);


    }
}
