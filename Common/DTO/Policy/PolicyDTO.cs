using System.ComponentModel.DataAnnotations;

public class PolicyDTO
{
    [Key]
    public Guid PolicyId { get; set; }
    public string PolicyName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int PercentageRefund { get; set; }
    public bool IsBackToFarm { get; set; } = false;
    public Guid FarmId { get; set; }
}
