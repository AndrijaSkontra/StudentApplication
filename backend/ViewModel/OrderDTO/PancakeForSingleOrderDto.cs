namespace backend.ViewModel.OrderDTO;

public class PancakeForSingleOrderDto
{
    public int Id { get; set; }
    
    public float Price { get; set; }

    public float Discount { get; set; } = 0;
}