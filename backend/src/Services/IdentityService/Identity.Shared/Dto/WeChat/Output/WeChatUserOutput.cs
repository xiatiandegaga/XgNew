using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Shared.Dto.WeChat.Output
{
    public class WeChatUserLoginTokenOutput
    {
        public int IsFirstLogin { get; set; }

        public string Token { get; set; }
    }
}
