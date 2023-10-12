using SendGrid.Helpers.Errors.Model;
using System.Globalization;
using UnitTestingA1Base.Models;

namespace UnitTestingA1Base.Data
{
    public class BusinessLogicLayer
    {
        private AppStorage _appStorage;

        public BusinessLogicLayer(AppStorage appStorage) {
            _appStorage = appStorage;
        }
        public HashSet<Recipe> GetRecipesByIngredient(int? id, string? name)
        {
            try {
                Ingredient ingredient;
                HashSet<Recipe> recipes = new HashSet<Recipe>();
                if (id != null)
                {
                    ingredient = _appStorage.Ingredients.FirstOrDefault(i => i.Id == id);

                    if (ingredient == null)
                    {
                        return null;
                    }
                    HashSet<RecipeIngredient> recipeIngredients = _appStorage.RecipeIngredients.Where(rI => rI.IngredientId == ingredient.Id).ToHashSet();

                    recipes = _appStorage.Recipes.Where(r => recipeIngredients.Any(ri => ri.RecipeId == r.Id)).ToHashSet();
                    return recipes;
                }
                else if (name != null)
                {
                    ingredient = _appStorage.Ingredients.FirstOrDefault(i => i.Name.Contains(name));

                    if (ingredient == null)
                    {
                        return null;
                    }
                    HashSet<RecipeIngredient> recipeIngredients = _appStorage.RecipeIngredients.Where(rI => rI.IngredientId == ingredient.Id).ToHashSet();

                    recipes = _appStorage.Recipes.Where(r => recipeIngredients.Any(ri => ri.RecipeId == r.Id)).ToHashSet();
                    return recipes;
                }
            }
            catch (Exception ex) {
                throw new ArgumentNullException("Either ID or Name must be provided.");
            }
           
          
            return null;
        }

        public HashSet<Recipe> GetRecipeByDiet(int? id, string? name)
        {
            try
            {
                HashSet<Recipe> recipes = new HashSet<Recipe>();
                if(id == null)
                {
                    return null;
                }

                if (id != null || !string.IsNullOrEmpty(name))
                {
                    DietaryRestriction dietaryRestriction;

                    if (id != null)
                    {
                        dietaryRestriction = _appStorage.DietaryRestrictions.FirstOrDefault(dr => dr.Id == id);
                        if(dietaryRestriction  == null) {
                            return null;
                        }
                    }
                    else
                    {
                        dietaryRestriction = _appStorage.DietaryRestrictions.FirstOrDefault(dr => dr.Name.Contains(name));
                        if (dietaryRestriction == null)
                        {
                            return null;
                        }
                    }

                    
                    if (dietaryRestriction != null)
                    {
                        HashSet<int> ingredientRestrictions = _appStorage.IngredientRestrictions
                            .Where(ir => ir.DietaryRestrictionId == dietaryRestriction.Id)
                            .Select(ir => ir.IngredientId)
                            .ToHashSet();

                        if (ingredientRestrictions.Count > 0)
                        {
                            HashSet<int> recipeIds = _appStorage.RecipeIngredients
                                .Where(ri => ingredientRestrictions.Contains(ri.IngredientId))
                                .Select(ri => ri.RecipeId)
                                .ToHashSet();

                            if (recipeIds.Count > 0)
                            {
                                recipes = _appStorage.Recipes
                                    .Where(r => recipeIds.Contains(r.Id))
                                    .ToHashSet();
                            }
                        }
                    }

                    return recipes;
                }
                else
                {
                    return null; 
                }
            }
            catch (Exception ex)
            {
               
                throw new ForbiddenException("Not Found", ex) ;
            }
        }


        public HashSet<Recipe> GetAllRecipes(int? id, string? name)
        {
            try
            {
                HashSet<Recipe> recipes = new HashSet<Recipe>();

                if (id != null )
                {
                    recipes = _appStorage.Recipes
                        .Where(r => r.Id == id).ToHashSet(); 
                       
                    if (recipes == null)
                    {
                        return null;
                    }
                    return recipes;
                }
                else if(name != null) 
                {
                    recipes = _appStorage.Recipes.Where(r=> r.Name.Contains(name)).ToHashSet();
                    if (recipes == null)
                    {
                        return null;
                    }
                    return recipes;
                }
                else
                {
                    return null;
                }
            }catch(Exception ex)
            {
                throw new ArgumentNullException(nameof(id), ex);
            }
            return null;
        }

        public void AddRecipeWithIngredient(Recipe recipe, List<Ingredient> ingredient)
        {
            Recipe existRecipe = _appStorage.Recipes.FirstOrDefault(r => r.Name == recipe.Name);
            // Check if a Recipe with the same name already exists
            if (existRecipe != null)
            {
                throw new InvalidOperationException("Recipe with the same name already exists.");
            }
           


            recipe.Id = _appStorage.GeneratePrimaryKey(); // Create a new Recipe with a new ID
            _appStorage.Recipes.Add(recipe);



            Dictionary<string, Ingredient> existingIngredientsByName = _appStorage.Ingredients
                .ToDictionary(ingredient => ingredient.Name, ingredient => ingredient);// Creating a dictionary to track existing Ingredients by name

            // Add or retrieve Ingredients
            foreach (Ingredient inputIngredient in ingredient)
            {
                if (!existingIngredientsByName.ContainsKey(inputIngredient.Name))
                {
                    // Ingredient with the same name doesn't exist, add it with a new ID
                    Ingredient newIngredient = new Ingredient
                    {
                        Id = _appStorage.GeneratePrimaryKey(),
                        Name = inputIngredient.Name
                    };

                    _appStorage.Ingredients.Add(newIngredient);
                    existingIngredientsByName.Add(newIngredient.Name, newIngredient);
                }

                // Create RecipeIngredient and add it to the database
                RecipeIngredient recipeIngredient = new RecipeIngredient
                {
                    RecipeId = recipe.Id,
                    IngredientId = existingIngredientsByName[inputIngredient.Name].Id
                };

                _appStorage.RecipeIngredients.Add(recipeIngredient);
            }

            // Add the new Recipe to the database
            _appStorage.Recipes.Add(recipe);
        }
        public void DeleteIngredients(int? id, string? name)
        {
            Ingredient IngredientTobeDeleted = null;
            try {
                if (id != null)
                {
                    IngredientTobeDeleted = _appStorage.Ingredients.FirstOrDefault(x => x.Id == id);

                }
                else if (name != null) {
                    IngredientTobeDeleted = _appStorage.Ingredients.FirstOrDefault(x => x.Name.Contains(name));
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Either id or name must be provided", ex);
            }

             var AssociatedRecipe = _appStorage.RecipeIngredients.Where(r => r.IngredientId == IngredientTobeDeleted.Id).ToList();
            if (AssociatedRecipe.Count > 1) {
                throw new ForbiddenException("Multpli recipe are asscociated with this ingredients , cannot be deleted");

            }
            if (AssociatedRecipe.Count == 1)
            {
                _appStorage.Recipes.Remove(_appStorage.Recipes.FirstOrDefault(r => r.Equals(AssociatedRecipe[0].RecipeId)));
                _appStorage.RecipeIngredients.Remove(_appStorage.RecipeIngredients.FirstOrDefault(r => r.Equals(AssociatedRecipe[0].RecipeId)));

            }
            _appStorage.Ingredients.Remove(IngredientTobeDeleted);
        }
        public void DeleteRecipe(int? id, string? name)
        {
            Recipe recipeToDelete = null;

            try
            {
                if (id != null)
                {
                    recipeToDelete = _appStorage.Recipes.FirstOrDefault(r => r.Id == id);
                }
                else if (!string.IsNullOrWhiteSpace(name))
                {
                    recipeToDelete = _appStorage.Recipes.FirstOrDefault(r => r.Name.Contains(name));
                }

                if (recipeToDelete == null)
                {
                    throw new InvalidOperationException("Recipe not found.");
                }



                // Remove the RecipeIngredient associations first
                List<RecipeIngredient> recipeIngredientsToDelete = _appStorage.RecipeIngredients.Where(ri => ri.RecipeId == recipeToDelete.Id).ToList();
                foreach (RecipeIngredient ri in recipeIngredientsToDelete)
                {
                    _appStorage.RecipeIngredients.Remove(ri);
                }



                // Now, remove the recipe itself
                _appStorage.Recipes.Remove(recipeToDelete);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Either Id and name canbe found" ,ex);

            }
        }
    }
}
