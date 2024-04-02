namespace backend.ViewModel.OrderDTO;

public class GetSingleOrderDto
{
    public int Id { get; set; }
    
    public string Desc { get; set; }
    
    public float Price { get; set; }
    
    public DateTime Time { get; set; }
    
    public List<PancakeForSingleOrderDto> Pancakes { get; set; }

    public float Discount { get; set; } = 0;
}