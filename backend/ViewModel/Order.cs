namespace backend.ViewModel;

public class Order
{
    public int Id { get; set; }
    public string Desc { get; set; }
    public float Price { get; set; }
    public DateTime Time { get; set; }
    public List<Pancake> Pancakes { get; set; }
}