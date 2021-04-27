using System.Collections.Generic;

namespace Database.ModelBase
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Value { get; set; }
        public virtual ICollection<Role> R { get; set; }
    }
}