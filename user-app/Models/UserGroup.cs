using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace user_app.Models;

public class UserGroup
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("user_group_id")]
    public Guid UserGroupId { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("code")]
    public GroupCode Code { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("description")]
    public string Description { get; set; }

    [JsonIgnore]
    public virtual ICollection<User> Users { get; set; }
    
    public string DisplayCode => Enum.GetName(Code)!;
}