namespace BimDataControlPanel.BLL.Dtos.Project;

public class EditProjectDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? RevitVersion { get; set; }
    public bool Complete { get; set; }
}