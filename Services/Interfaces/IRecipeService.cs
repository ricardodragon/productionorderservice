using System.Collections.Generic;
using System.Threading.Tasks;
using productionorderservice.Model;

namespace productionorderservice.Services.Interfaces
{
    public interface IRecipeService
    {
        Task<List<Recipe>> getRecipes(int startat, int quantity,
            string fieldFilter, string fieldValue, string orderField, string order);
        Task<Recipe> getRecipe(int recipeId);
    }
}