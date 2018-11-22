using System.ComponentModel.DataAnnotations;

namespace Salary.Models.User
{
    public class UserStatus
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// 用户状态  0.取消开通 1 .正常 2.停用 3.删除 4.未审核
        /// </summary>
        [Required]
        public int Status { get; set; }
    }
}