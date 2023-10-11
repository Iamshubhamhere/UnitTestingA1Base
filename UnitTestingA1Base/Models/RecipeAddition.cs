namespace UnitTestingA1Base.Models
{
    public class RecipeAddition
    {
        public Recipe Recipe { get; set; }
        public List<Ingredient> Ingredients { get; set;} = new List<Ingredient>();
    }
}
