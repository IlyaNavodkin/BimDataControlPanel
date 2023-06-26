namespace BimDataControlPanel.DAL.Entities;

public class RevitUserInfo
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public DateTime LastConnection { get; set; }
    public string? NameUserOs { get; set; }
    public string? RevitVersion { get; set; } 
    public string? DsToolsVersion { get; set; }
    public List<Change>? Changes { get; set; }
    public List<Project>? Projects { get; set; }
}