using System.ComponentModel.DataAnnotations;
namespace WeddingPlanner.Models;

public class UserWeddingSignup
{
    [Key]
    public int UserWeddingSignupId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;


    public int UserId { get; set; }
    public User? User { get; set; }
    public int WeddingId { get; set; }
    public Wedding? Wedding { get; set; }
}