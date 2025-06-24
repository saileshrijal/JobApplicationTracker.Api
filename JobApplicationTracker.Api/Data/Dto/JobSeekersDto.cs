namespace JobApplicationTracker.Api.Data.Dto;

public class JobSeekersDto
{

    public int JobSeekerId { get; set; }
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Location { get; set; }
    public DateTime DateOfBirth { get; set; } // Nullable to allow for optional date of birth
    public string ProfilePicture { get; set; } // URL or path to profile picture
    public string Resume { get; set; } // URL or path to resume file
    public string Bio { get; set; } // Short biography or summary
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

