namespace BimDataControlPanel.DAL.Entities;

public class Project
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? RevitVersion { get; set; }
    public DateTime CreationTime { get; set; }
    public bool Complete { get; set; }

    public List<Change>? Changes { get; set; } = new List<Change>();
    public List<RevitUserInfo>? RevitUserInfos { get; set; } = new List<RevitUserInfo>();
}
