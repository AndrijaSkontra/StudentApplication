using backend.Validation;

namespace backend.ViewModel;

public class Pancake
{
    public int Id { get; set; }
    public float Price { get; set; }
    public required List<Ingredient> Ingredients { get; set; }
    public Order? Order { get; set; }
    public int? OrderId { get; set; }
}