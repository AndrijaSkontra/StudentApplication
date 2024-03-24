using System.ComponentModel.DataAnnotations;
using backend.ViewModel;

namespace backend.Validation;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
sealed public class PancakeValidator : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        List<Ingredient>? ingredients = value as List<Ingredient>;
        if (ingredients == null)
        {
            return false;
        }
        if (ingredients.Count < 2)
        {
            return false;
        }

        var baseIngredientCounter = 0;
        var stuffingIngredientCounter = 0;
        
        foreach (var ingredient in ingredients)
        {
            if (ingredient.IngredientType == IngredientType.Base)
            {
                baseIngredientCounter++;
            }
            
            if (ingredient.IngredientType == IngredientType.Stuffing)
            {
                stuffingIngredientCounter++;
            }
            
            if (baseIngredientCounter > 1)
            {
                return false;
            }
        }
        
        if (baseIngredientCounter != 1 || stuffingIngredientCounter < 1)
        {
            return false;
        }
        
        return true;
    }
}