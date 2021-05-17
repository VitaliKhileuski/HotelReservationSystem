using System.Collections.Generic;

namespace Business.Models
{
   public class RoleModel
    {
        public ICollection<UserModel> Users { get; set; }
        public string Name { get; set; }
        
    }
}