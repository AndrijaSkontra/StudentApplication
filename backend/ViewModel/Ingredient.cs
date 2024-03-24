namespace backend.ViewModel;

public class Ingredient
{
    public int Id { get; set; }
    public IngredientType IngredientType { get; set; }
    public bool IsHealthy { get; set; }
    public required string Name { get; set; }
    public float Price { get; set; }
    public required List<Pancake> Pancakes { get; set; }
}