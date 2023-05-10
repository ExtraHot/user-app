using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace user_app.Models;

public class UserState
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("user_state_id")]
    public Guid UserStateId { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("code")]
    public StateCode Code { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("description")]
    public string Description { get; set; }

    [JsonIgnore]
    public virtual ICollection<User> Users { get; set; }
    
    public string DisplayCode => Enum.GetName(Code)!;
}