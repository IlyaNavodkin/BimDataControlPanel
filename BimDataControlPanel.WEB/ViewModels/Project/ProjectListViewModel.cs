using BimDataControlPanel.WEB.ViewModels.Pagination;
using BimDataControlPanel.DAL.Entities;

namespace BimDataControlPanel.WEB.ViewModels.Project;

public class ProjectListViewModel
{
    public List<DAL.Entities.Project>? Projects { get; set; }
    public PageInfo? PageInfo { get; set; }
    public string? SearchString { get; set; }
}