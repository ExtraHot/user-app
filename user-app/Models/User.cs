namespace user_app.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("login")]
    public string Login { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("password")]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    [Column("created_date")]
    public DateTime CreatedDate { get; set; }

    [Required]
    [Column("user_group_id")]
    public Guid UserGroupId { get; set; }
    
    [ForeignKey("UserGroupId")]
    public virtual UserGroup UserGroup { get; set; }

    [Required]
    [Column("user_state_id")]
    public Guid UserStateId { get; set; }

    [ForeignKey("UserStateId")]
    public virtual UserState UserState { get; set; }
}