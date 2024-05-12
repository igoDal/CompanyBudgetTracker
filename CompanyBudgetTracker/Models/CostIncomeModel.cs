using System.ComponentModel.DataAnnotations;

namespace CompanyBudgetTracker.Models;

public class CostIncomeModel
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    public string Type { get; set; }
    
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public decimal Amount { get; set; }
    
    [Required]
    public DateTime Date { get; set; }
    
    [Required]
    public byte[]? Attachment { get; set; }
    public string? AttachmentName { get; set; }
    public string? AttachmentContentType { get; set; }
    public bool Settled { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string UserId { get; set; }

}