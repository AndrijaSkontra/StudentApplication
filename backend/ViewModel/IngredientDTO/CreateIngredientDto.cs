namespace backend.ViewModel.IngredientDTO;

public class CreateIngredientDto
{
    public IngredientType IngredientType { get; set; }
    public bool IsHealthy { get; set; }
    public required string Name { get; set; }
    public float Price { get; set; }
}