using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseProject.Models
{
    public class GameUser
    {
        public int UserId { get; set; } = 0; //user number
        public string UserName { get; set; } = "Error Loading"; //user name
        public string UserMail { get; set; } = "Error Loading"; //user mail
        public string UserPassword { get; set; } = "Error Loading"; //user password
        public int Money { get; set; } = 0; //user amount of money
        public string UsingProductName { get; set; } = "GreenTable"; //user current using product name
        public string UserAvatarPic { get; set; } = "DefaultAvatar"; //user avatar picture
        public string UserAvatarEditPic { get; set; } = "DefaultAvatarEdit"; //user avatar edit picture
        public List<string> userTables { get; set; } = new List<string>() //all possible products user can buy/equip
        {
            "GreenTable",
            "RedTable",
            "BlueTable",
            "OrangeTable",
            "PinkTable"
        };
    }
}
