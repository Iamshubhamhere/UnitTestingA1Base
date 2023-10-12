using Microsoft.AspNetCore.Identity;
using UnitTestingA1Base.Data;
using UnitTestingA1Base.Models;

namespace RecipeUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        private BusinessLogicLayer _initializeBusinessLogic()
        {
            return new BusinessLogicLayer(new AppStorage());
        }

        #region GetRecipeByIngredient 
        [TestMethod]
        public void GetRecipesByIngredient_ValidId_ReturnsRecipesWithIngredient()
        {
            // arrange
            BusinessLogicLayer bll = _initializeBusinessLogic();
            int ingredientId = 6;
            int recipeCount = 2;

            // act
            HashSet<Recipe> recipes = bll.GetRecipesByIngredient(ingredientId, null);

            Assert.AreEqual(recipeCount, recipes.Count);
        }

        [TestMethod]
        public void GetRecipesByIngredient_ValidName_ReturnsRecipesWithIngredient()
        {
            // arrange
            BusinessLogicLayer bll = _initializeBusinessLogic();
            string ingredientName = "Salmon";
            int recipeCount = 2; 

            // act
            HashSet<Recipe> recipes = bll.GetRecipesByIngredient(null, ingredientName);

            // assert
            Assert.AreEqual(recipeCount, recipes.Count);

        }

        [TestMethod]
        public void GetRecipesByIngredient_InvalidId_ReturnsNull()
        {
            // arrange
            BusinessLogicLayer bll = _initializeBusinessLogic();
            int invalidIngredientId = -1; 

            // act
            HashSet<Recipe> recipes = bll.GetRecipesByIngredient(invalidIngredientId, null);

            // assert
            Assert.IsNull(recipes);
        }

        [TestMethod]
        public void GetRecipesByIngredient_InvalidName_ReturnsNull()
        {
            //arrange 
            BusinessLogicLayer bll = _initializeBusinessLogic();

            string ingredientName = "Random Name";
           

            //act 
            HashSet<Recipe> recipes = bll.GetRecipesByIngredient(null, ingredientName);

            //assert
            Assert.IsNull(recipes);   
        }

        #endregion
        #region GetRecipeByDiet
        [TestMethod]
        public void GetRecipeByDiet_ValidId_ReturnsRecipesWithDiet()
        {
            // arrange
            BusinessLogicLayer bll = _initializeBusinessLogic();
            int DietId = 5;
            int recipeCount = 2;

            // act
            HashSet<Recipe> recipes = bll.GetRecipeByDiet(DietId, null);

            Assert.AreEqual(recipeCount, recipes.Count);
        }

     [TestMethod]
        public void GetRecipeByDiet_ValidName_ReturnsRecipesWithDiet()
        {
            // arrange
            BusinessLogicLayer bll = _initializeBusinessLogic();
            string DietName = "Lactose-Free";
            int recipeCount = 2;

            // act
            HashSet<Recipe> recipes = bll.GetRecipeByDiet(null, DietName);

            // assert
            Assert.AreEqual(recipeCount, recipes.Count);

        }
       
      [TestMethod]
      public void GetRecipeByDiet_InvalidId_ReturnsNull()
      {
          // arrange
          BusinessLogicLayer bll = _initializeBusinessLogic();
          int invalidDietId = -1;

          // act
          HashSet<Recipe> recipes = bll.GetRecipeByDiet(invalidDietId, null);

          // assert
          Assert.IsNull(recipes);
      }
        
     [TestMethod]
     public void GetRecipeByDiet_InvalidName_ReturnsNull()
     {
         //arrange 
         BusinessLogicLayer bll = _initializeBusinessLogic();

         string InvalidDietName = "Random Name";


         //act 
         HashSet<Recipe> recipes = bll.GetRecipeByDiet(null, InvalidDietName);

         //assert
         Assert.IsNull(recipes);
     }
        #endregion
        [TestMethod]
        public void GetAllRecipes_ValidId_ReturnsRecipesWithDiet()
        {
            // arrange
            BusinessLogicLayer bll = _initializeBusinessLogic();
            int recipeId = 5;
            int recipeCount = 1;

            // act
            HashSet<Recipe> recipes = bll.GetAllRecipes(recipeId, null);

            Assert.AreEqual(recipeCount, recipes.Count);
        }

        [TestMethod]
        public void GetAllRecipes_ValidName_ReturnsRecipesWithDiet()
        {
            // arrange
            BusinessLogicLayer bll = _initializeBusinessLogic();
            string recipeName = "Chocolate Cake";
            int recipeCount = 1;

            // act
            HashSet<Recipe> recipes = bll.GetAllRecipes(null, recipeName);

            // assert
            Assert.AreEqual(recipeCount, recipes.Count);

        }

        [TestMethod]
        public void DeleteIngredient_ByValidId_DeletesIngredient()
        {
            // Arrange
            AppStorage appStorage = new AppStorage();
            BusinessLogicLayer bll = new BusinessLogicLayer(appStorage);

            // Act
            bll.DeleteIngredients(1, null);

            // Assert
            Assert.AreEqual(9, appStorage.Ingredients.Count);
        }

        [TestMethod]
        public void DeleteIngredient_ByValidName_DeletesIngredient()
        {
            // Arrange
            AppStorage appStorage = new AppStorage();
            BusinessLogicLayer bll = new BusinessLogicLayer(appStorage);


            // Act
            bll.DeleteIngredients(null, "Tomatoes");

            // Assert
            Assert.AreEqual(9, appStorage.Ingredients.Count);
        }
        [TestMethod]
        public void DeleteRecipe_ByValidId_DeletesRecipe()
        {
            // Arrange
            AppStorage appStorage = new AppStorage();
            BusinessLogicLayer bll = new BusinessLogicLayer(appStorage);

            Recipe existingRecipe = appStorage.Recipes.First(r => r.Name.Contains("Grilled Salmon"));

            // Act
            bll.DeleteRecipe(existingRecipe.Id, null);

            // Assert
            Assert.AreEqual(11, appStorage.Recipes.Count);
        }

        [TestMethod]
        public void DeleteRecipe_ByValidName_DeletesRecipe()
        {
            // Arrange
            AppStorage appStorage = new AppStorage();
            BusinessLogicLayer bll = new BusinessLogicLayer(appStorage);

            Recipe existingRecipe = appStorage.Recipes.First(r => r.Name.Contains("Grilled Salmon"));


            // Act
            bll.DeleteRecipe(null, existingRecipe.Name);

            // Assert
            Assert.AreEqual(11, appStorage.Recipes.Count);
        }
    }
}
