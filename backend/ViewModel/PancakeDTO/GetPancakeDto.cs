namespace backend.ViewModel.PancakeDTO;

public class GetPancakeDto
{
    public int Id { get; set; }

    // [PancakeValidator]
    public List<IngredientForPancakeDto> Ingredients { get; set; }

    public float Price { get; set; }
}