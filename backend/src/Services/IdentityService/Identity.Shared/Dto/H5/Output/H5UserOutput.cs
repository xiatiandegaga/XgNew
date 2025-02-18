using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Shared.Dto.H5.Output
{
    public class H5UserLoginTokenOutput
    {
        public string Token { get; set; }

        public UserDto userDto { get; set; }
    }

   

}
