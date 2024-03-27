using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.App.Dtos
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expiresIn { get; set; }
    }
}
