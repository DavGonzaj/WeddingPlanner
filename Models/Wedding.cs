#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WeddingPlanner.Models;
public class Wedding
{
    [Key]
    public int WeddingId { get; set; }
    [Required(ErrorMessage = "is required")]
    public string WedOne { get; set; }

    [Required(ErrorMessage = "is required")]
    public string WedTwo { get; set; }

    [Required(ErrorMessage = "is required")]
    public DateTime? WedDate { get; set; }

    [Required(ErrorMessage = "is required")]
    [MinLength(5, ErrorMessage = "must be at least 5 characters")]
    public string WedAddress { get; set; }
    public DateTime Created_at { get; set; } = DateTime.Now;
    public DateTime Updated_at { get; set; } = DateTime.Now;

    public int UserId { get; set; }
    public User? Planner { get; set; }
    public List<UserWeddingSignup> Signups { get; set; } = new List<UserWeddingSignup>();
}