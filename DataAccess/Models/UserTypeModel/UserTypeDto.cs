using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.UserTypeModel
{
    public class UserTypeDto
    {
        
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public bool Valid { get; set; }   
        public string Name { get; set; }
        public string Feature { get; set; }
        public int UserId { get; set; }
    }
}
