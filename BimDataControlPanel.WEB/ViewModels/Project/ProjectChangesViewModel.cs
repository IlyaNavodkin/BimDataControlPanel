using BimDataControlPanel.BLL.Dtos.Project;
using BimDataControlPanel.DAL.Entities;
using BimDataControlPanel.WEB.ViewModels.Pagination;

namespace BimDataControlPanel.WEB.ViewModels.Project;

public class ProjectChangesViewModel
{
    public ProjectDto? Project { get; set; }
    public List<Change>? Changes { get; set; }
    public PageInfo? PageInfo { get; set; }
    public string? SearchString { get; set; }
    public string? SortBy { get; set; }
    public string? SortOrder { get; set; }}