using CookieCookbook.DataAccess;
using CookieCookbook.Recipes.Ingredients;

namespace CookieCookbook.Recipes;

public class RecipesRepository : IRecipesRepository
{
    private readonly IStringsRepository _stringsRepository;
    private readonly IIngredientsRegister _ingredientsRegister;
    private const string Separator = ",";

    public RecipesRepository(
        IStringsRepository stringsRepository,
        IIngredientsRegister ingredientsRegister)
    {
        _stringsRepository = stringsRepository;
        _ingredientsRegister = ingredientsRegister;
    }

    public List<Recipe> Read(string filePath)
    {
        List<Recipe> recipes = _stringsRepository.Read(filePath)
                                                 .Select(recipeFromFile => RecipeFromString(recipeFromFile))
                                                 .ToList();
        
        //var recipes = new List<Recipe>();

        //foreach (var recipeFromFile in recipesFromFile)
        //{
        //    var recipe = RecipeFromString(recipeFromFile);
        //    recipes.Add(recipe);
        //}

        return recipes;
    }

    private Recipe RecipeFromString(string recipeFromFile)
    {
        var ingredients = recipeFromFile.Split(Separator)
                                        .Select(textualId => int.Parse(textualId))
                                        .Select(id => _ingredientsRegister.GetById(id));
        return new Recipe(ingredients);

        //var ingredients = new List<Ingredient>();

        //foreach (var textualId in textualIds)
        //{
        //    var id = int.Parse(textualId);
        //    var ingredient = _ingredientsRegister.GetById(id);
        //    ingredients.Add(ingredient);
        //}

        //return new Recipe(ingredients);
    }

    public void Write(string filePath, List<Recipe> allRecipes)
    {

        //var recipesAsStrings = allRecipes.Select(recipe =>
        //                                                     {
        //                                                         var allIds = recipe.Ingredients
        //                                                                     .Select(ingredient => ingredient.Id);
        //                                                         return string.Join(Separator, allIds);
        //                                                      }).ToList();

        var recipesAsStrings = allRecipes.Select(recipe => string.Join(Separator, recipe.Ingredients.
                                                                        Select(ingredient => ingredient.Id)))
                                         .ToList();
        
        //var recipesAsStrings = new List<string>();
        //foreach (var recipe in allRecipes)
        //{
        //    var allIds = recipe.Ingredients.Select(ingredient => ingredient.Id);
        //    //foreach (var ingredient in recipe.Ingredients)
        //    //{
        //    //    allIds.Add(ingredient.Id);
        //    //}
        //    recipesAsStrings.Add(string.Join(Separator, allIds));
        //}

        _stringsRepository.Write(filePath, recipesAsStrings);
    }
}
