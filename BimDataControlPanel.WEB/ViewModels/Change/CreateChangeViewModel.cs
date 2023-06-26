using BimDataControlPanel.BLL.Dtos.Change;
using BimDataControlPanel.DAL.Entities;

namespace BimDataControlPanel.WEB.ViewModels.Change;

public class CreateChangeViewModel
{
    public CreateChangeDto ChangeDto { get; set; }
    public List<RevitUserInfo> RevitUserInfos { get; set; }
}