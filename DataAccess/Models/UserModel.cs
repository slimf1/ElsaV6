using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccess.Models
{
    public class UserModel
    {
        [Key]
        public string UserID { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Avatar { get; set; }
        public int OnTime { get; set; }
        public int RegDate { get; set; }
        public IEnumerable<BadgeModel> Badges { get; set; }
    }
}
