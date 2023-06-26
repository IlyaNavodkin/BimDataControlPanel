namespace BimDataControlPanel.BLL.Dtos.RevitUserInfo;

public class CreateRevitUserInfoDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public DateTime LastConnection { get; set; }
    public string? NameUserOs { get; set; }
    public string? RevitVersion { get; set; } 
    public string? DsToolsVersion { get; set; }
}