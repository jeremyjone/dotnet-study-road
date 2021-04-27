using System.Collections.Generic;

namespace Database.ModelBase
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Permission> P { get; set; }
    }
}
