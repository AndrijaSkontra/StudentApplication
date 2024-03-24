namespace backend.ViewModel.PancakeDTO;

public class IngredientForPancakeDto
{
    public int Id { get; set; }
    public IngredientType IngredientType { get; set; }
    public bool IsHealthy { get; set; }
    public required string Name { get; set; }
    public float Price { get; set; }
}