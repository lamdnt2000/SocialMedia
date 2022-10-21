using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Entities
{
    public partial class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public int Status { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int RoleId { get; set; }

        public virtual Member Id1 { get; set; }
        public virtual Admin IdNavigation { get; set; }
        public virtual Role Role { get; set; }
    }
}
