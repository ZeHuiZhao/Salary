using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary.Models.Material
{
    public class MaterialModel
    {
        public int Id { get; set; }
        /// <summary>
        /// 行号
        /// </summary>
        public int RowNum { get; set; }
        /// <summary>
        /// 显示的名字
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 素材标题
        /// </summary>
        public string MaterialTitle { get; set; }
        /// <summary>
        /// 素材封面
        /// </summary>
        public string CoverImg { get; set; }
        /// <summary>
        /// 素材类别名称
        /// </summary>
        public string MaterialTypeName { get; set; }
        
        /// <summary>
        /// 是否可用
        /// </summary>
        public int IsActive { get; set; }

        /// <summary>
        /// 渠道分享
        /// </summary>
        public int ChannelShare { get; set; }

        /// <summary>
        /// 所有分享
        /// </summary>
        public int AllShare { get; set; }

        /// <summary>
        /// 销售员分享
        /// </summary>
        public int SalesShare { get; set; }

        /// <summary>
        /// 渠道浏览
        /// </summary>
        public int ChannelBrowse { get; set; }

        /// <summary>
        /// 所有浏览
        /// </summary>
        public int AllBrowse { get; set; }

        /// <summary>
        /// 销售员浏览
        /// </summary>
        public int SalesBrowse { get; set; }

        /// <summary>
        /// 销售员报名人数
        /// </summary>
        public int SalesEnrollCount { get; set; }

        /// <summary>
        /// 渠道报名人数
        /// </summary>
        public int ChannelEnrollCount { get; set; }
        /// <summary>
        /// 所有报名人数
        /// </summary>
        public int AllEnrollCount { get; set; }
        /// <summary>
        /// 销售员参课数
        /// </summary>
        public int SalesParticipate { get; set; }

        /// <summary>
        /// 渠道参课
        /// </summary>
        public int ChannelParticipate { get; set; }
        /// <summary>
        /// 所有参课数
        /// </summary>
        public int AllParticipate { get; set; }

        /// <summary>
        /// 最后分享时间
        /// </summary>
        public string LastShareTime { get; set; }
        
        public DateTime LastTime { get; set; }
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 删除是否可用 0不可删除 1可删除
        /// </summary>
        public int DeleteIsable { get; set; }
    }
}