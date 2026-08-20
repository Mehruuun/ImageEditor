using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    
    internal class UserInfo
    {
        [Key]
        public int Id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}
