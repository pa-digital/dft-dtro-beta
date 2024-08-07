using System.ComponentModel.DataAnnotations;

public class SwaCode
{
    [Required(ErrorMessage = "TraId is required.")]
    [Range(-9999, 9999, ErrorMessage = "TraId must be between -9999 and 9999.")]
    public int TraId { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Prefix is required.")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "Prefix must be exactly 2 characters.")]
    public string Prefix { get; set; }

    [Required(ErrorMessage = "IsAdmin is required.")]
    public bool IsAdmin { get; set; }

    [Required(ErrorMessage = "IsActive is required.")]
    public bool IsActive { get; set; }
}