using System.Threading.Tasks;

namespace Cloud.Emailing
{
    public interface ISendMail
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="to">收件人</param>
        /// <param name="subject">标题</param>
        /// <param name="body">主体内容</param>
        /// <param name="isBodyHtml">主体是否html</param>
        /// <returns></returns>
        Task SendAsync(
            string to,
            string subject,
            string body,
            bool isBodyHtml = false
        );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from">发件人</param>
        /// <param name="to">收件人</param>
        /// <param name="subject">标题</param>
        /// <param name="body">主体内容</param>
        /// <param name="isBodyHtml">主体是否html</param>
        /// <returns></returns>
        Task SendWithFromAsync(
            string from,
            string to,
            string subject,
            string body,
            bool isBodyHtml = false
        );
    }
}
