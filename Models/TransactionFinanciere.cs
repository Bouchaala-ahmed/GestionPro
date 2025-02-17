public class Transactions
{
    public int Id { get; set; }
    public string Description { get; set; }
    public decimal Montant { get; set; }
    public DateTime DateTransaction { get; set; }
    public string Type { get; set; } // Revenu/Dépense
}
