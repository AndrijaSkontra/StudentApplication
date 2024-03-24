using backend.ViewModel.IngredientDTO;

namespace backend.ViewModel.OrderDTO;

public class GetOrderDto
{
    public int Id { get; set; }
    public string Desc { get; set; }
    public float Price { get; set; }
    public DateTime Time { get; set; }
    public List<PancakeForIngredientDto> Pancakes { get; set; }
}