namespace JobApplicationTracker.Api.Data.Dto;

    public class JobApplicationStatusDto
    {
            public int StatusId { get; set; }
            public string StatusName { get; set; }
            public string Description { get; set; }
            public bool IsActive { get; set; }
    }