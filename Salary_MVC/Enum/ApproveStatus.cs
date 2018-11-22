using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary_MVC.Enum
{
    /// <summary>
    /// 最普通场景的审核
    /// HR发起
    /// 财务审核
    /// 社保、公积金、调薪、津贴、奖惩
    /// </summary>
    public enum ApproveStatus
    {//30待发起审核，31待财务审核，12财务同意，23财务否决
        待发起审核 = 30,
        财务同意 = 12,
        财务否决 = 23,
        待财务审核 = 31
    }
}