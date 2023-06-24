namespace BimDataControlPanel.BLL.Dtos.Change;

public class ChangeDto
{
    public string? Id { get; set; }
    public string? ProjectId { get; set; }
    public string? UserRevitName { get; set; }
    public DateTime ChangeTime { get; set; }
    public string? Description { get; set; }
    public string? ChangeType { get; set; }
}