using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
   public class RoleModel
    {
        public ICollection<UserModel> Users { get; set; }
        public string Name { get; set; }
        
    }
}
