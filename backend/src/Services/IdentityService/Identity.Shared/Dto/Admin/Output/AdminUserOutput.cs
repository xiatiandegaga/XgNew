using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Shared.Dto.Admin.Output
{
    public class AdminResponseLoginUserOutput
    {
        public string Token { get; set; }

        public string NickName { get; set; }

        public string Avatar { get; set; }

        public string RealName { get; set; }
    }


}
