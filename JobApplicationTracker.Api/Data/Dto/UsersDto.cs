namespace JobApplicationTracker.Api.Data.Dto;

public class UsersDto
{
    public int UserId { get; set; }
    public int Email { get; set; }
    public string PasswordHash { get; set; }
    public int UserTypeId { get; set; }
    public bool IsAdmin { get; set; }
    public string CreatedAt { get; set; }
    public int UpdatedAt { get; set; }
    public bool IsActive { get; set; }

}

