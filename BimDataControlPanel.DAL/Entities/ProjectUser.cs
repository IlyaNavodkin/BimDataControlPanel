namespace BimDataControlPanel.DAL.Entities;

public class ProjectUser
{
    public string? ProjectId { get; set; }
    public Project? Project { get; set; }
    public string? IdentityUserId { get; set; }
}