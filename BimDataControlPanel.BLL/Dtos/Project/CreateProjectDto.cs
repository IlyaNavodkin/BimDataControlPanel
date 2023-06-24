namespace BimDataControlPanel.BLL.Dtos.Project;

public class CreateProjectDto
{
    public string Name { get; set; }
    public string RevitVersion { get; set; }
    public bool Complete { get; set; }
}