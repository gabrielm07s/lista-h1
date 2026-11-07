using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Uniaraxa.Domain.Entities;

public class Teacher
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress]
    [JsonPropertyName("eMail")]
    public string Email { get; set; } = string.Empty;

    [Required, Url]
    public string UrlLattes { get; set; } = string.Empty;

    public DateOnly DataAdmissao { get; set; }
}
