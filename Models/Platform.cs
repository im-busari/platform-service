using System.ComponentModel.DataAnnotations;

namespace PlatformService.Models;

// DTO - Data Transfer Objects are our external view of our data
// This is internal representation of our data
// We will use AutoMapper to allows to map between the separate DTOs and this model - we do that via a Profile 
public class Platform
{
    [Key]
    [Required]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Publisher { get; set; }
    
    [Required]
    public string Cost { get; set; }
    
}