using System.ComponentModel.DataAnnotations.Schema;

namespace Dft.DTRO.Admin.Models.DtroUser;

public class DtroUser
{
    public Guid Id { get; set; }

    public Guid xAppId { get; set; }

    [Range(1, 9999, ErrorMessage = "TraId must be between 1 and 9999.")]
    public int? TraId { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; }

    [StringLength(2, MinimumLength = 2, ErrorMessage = "Prefix must be exactly 2 characters.")]
    public string Prefix { get; set; }

    public UserGroup UserGroup { get; set; }
}