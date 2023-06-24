namespace BimDataControlPanel.BLL.Dtos.Project;

public class ProjectDto
{
    public string? Name { get; set; }
    public string? Id { get; set; }
    public string? RevitVersion { get; set; }
    public DateTime CreationTime { get; set; }
    public bool Complete { get; set; }
    
}