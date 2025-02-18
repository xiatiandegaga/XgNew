using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xg.Cloud.Caching.Cloud.Caching
{
    public static class CacheKeyNamePrefix
    {
        #region 云走廊
        /// <summary>
        /// 视频浏览量key前缀
        /// </summary>
        public const string Video_ScanCount = "Video_ScanCount";
        /// <summary>
        /// 视频目录浏览量key前缀
        /// </summary>
        public const string VideoType_ScanCount = "VideoType_ScanCount";

        /// <summary>
        /// list视频点赞用户key前缀
        /// </summary>
        public const string List_Video_PostLike_User = "List_Video_PostLike_User";
        /// <summary>
        /// 目录点赞次数 key前缀
        /// </summary>
        public const string VideoType_PostLikeCount = "VideoType_PostLikeCount";

        /// <summary>
        /// list用户点赞视频
        /// </summary>
        public const string List_User_PostLike_Video = "List_User_PostLike_Video";

        /// <summary>
        /// list目录下视频点赞量 key前缀
        /// </summary>
        public const string List_VideoType_PostLikeCount_Video = "List_VideoType_PostLikeCount_Video";

        /// <summary>
        /// list视频点赞量 key前缀
        /// </summary>
        public const string List_PostLikeCount_Video = "List_PostLikeCount_Video";


        /// <summary>
        /// list目录下视频浏览量 key前缀
        /// </summary>
        public const string List_VideoType_ScanCount_Video = "List_VideoType_ScanCount_Video";

        /// <summary>
        /// list视频浏览量 key前缀
        /// </summary>
        public const string List_ScanCount_Video = "List_ScanCount_Video";

        /// <summary>
        /// 用户浏览量key前缀
        /// </summary>
        public const string User_ScanCount = "User_ScanCount";
        #endregion


        #region 快闪店
        /// <summary>
        /// 视频浏览量key前缀
        /// </summary>
        public const string Ksd_Video_ScanCount = "Ksd_Video_ScanCount";
        /// <summary>
        /// 快闪店视频浏览量key前缀
        /// </summary>
        public const string Ksd_Shop_ScanCount = "Ksd_Shop_ScanCount";

        /// <summary>
        /// list视频点赞用户key前缀
        /// </summary>
        public const string Ksd_List_Video_PostLike_User = "Ksd_List_Video_PostLike_User";
        /// <summary>
        /// 快闪店点赞次数 key前缀
        /// </summary>
        public const string Ksd_Shop_PostLikeCount = "Ksd_Shop_PostLikeCount";

        /// <summary>
        /// list用户点赞视频
        /// </summary>
        public const string Ksd_List_User_PostLike_Video = "Ksd_List_User_PostLike_Video";

        /// <summary>
        /// list 快闪店下视频点赞量 key前缀
        /// </summary>
        public const string Ksd_List_Shop_PostLikeCount_Video = "Ksd_List_Shop_PostLikeCount_Video";

        /// <summary>
        /// list视频点赞量 key前缀
        /// </summary>
        public const string Ksd_List_PostLikeCount_Video = "Ksd_List_PostLikeCount_Video";


        /// <summary>
        /// list 快闪店下视频浏览量 key前缀
        /// </summary>
        public const string Ksd_List_Shop_ScanCount_Video = "Ksd_List_Shop_ScanCount_Video";

        /// <summary>
        /// list视频浏览量 key前缀
        /// </summary>
        public const string Ksd_List_ScanCount_Video = "Ksd_List_ScanCount_Video";

        /// <summary>
        /// 用户浏览量key前缀
        /// </summary>
        public const string Ksd_User_ScanCount = "Ksd_User_ScanCount";
        #endregion
    }
}
