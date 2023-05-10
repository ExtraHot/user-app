namespace user_app.Dtos;

using System.Text.Json.Serialization;

public class UserDto
{
    [JsonPropertyName("user_id")]
    public Guid UserId { get; set; } 
    
    [JsonPropertyName("login")]
    public string Login { get; set; }
    
    [JsonPropertyName("created_date")]
    public DateTime CreatedDate { get; set; }
    
    [JsonPropertyName("user_group")]
    public string UserGroup { get; set; }
    
    [JsonPropertyName("user_state")]
    public string UserState { get; set; }
}