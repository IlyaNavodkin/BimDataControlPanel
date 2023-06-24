using Microsoft.AspNetCore.Identity;

namespace BimDataControlPanel.DAL.Entities;

    public class BimDataUser : IdentityUser
    {
        public string RevitUserNickName2020 { get; set; }
        public string RevitUserNickName2022 { get; set; }
        
        public BimDataUser () { }
    }