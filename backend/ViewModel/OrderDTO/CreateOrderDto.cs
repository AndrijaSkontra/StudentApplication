namespace backend.ViewModel.OrderDTO;

public class CreateOrderDto
{
    public string Desc { get; set; }
    public List<int> PancakesId { get; set; }
}