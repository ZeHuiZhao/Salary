using Qcloud.Sms;
using Salary.Common;
using Salary_MVC.Common;
using Salary_MVC.Data;
using Salary_MVC.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static Salary_MVC.DataModel.GZ_SMS;

namespace Salary_MVC.Services
{
    public class SMSService : Service<GZ_SMS>
    {
        static Dictionary<TemplateIdEnum, string> dictTemplateTxt = new Dictionary<TemplateIdEnum, string>() {
            { TemplateIdEnum.社保发起审核,"中力薪酬管家，HR已提交{1}月的{2}审核申请，请登陆薪酬管家进行审核。" },
            { TemplateIdEnum.社保审核结果,"中力薪酬管家，财务{1}{2}月的{3}审核申请，审核意见：{4}" },
            { TemplateIdEnum.基本工资发起审核,"中力薪酬管家，HR已提交员工{1}的{2}审核申请，请登陆薪酬管家进行审核。" },
            { TemplateIdEnum.基本工资审核结果,"中力薪酬管家，财务{1}{2}的{3}审核申请，审核意见：{4}" },
            { TemplateIdEnum.考勤确认,"中力薪酬管家，请确认{1}月的考勤，审核请进入 {2}" },
            { TemplateIdEnum.员工信息发起审核,"中力薪酬管家，HR申请对员工{1}的基本信息进行{2}，审核请进入 {3}" },
            { TemplateIdEnum.员工信息审核结果,"中力薪酬管家，财务{1}对员工{2}的基本信息{3}申请，审核意见：{4}" },
            { TemplateIdEnum.验证码,"中力薪酬管家，您好，您的验证码为{1}，请在10分钟内输入。" },
            { TemplateIdEnum.综合工资待审核,"中力薪酬管家，{1}已提交{2}的审核，请进入薪酬管家进行审核" },//薪酬管家，李彩铃已提交2018-08月工资的审核申请，请打开薪酬管家进行审核
            { TemplateIdEnum.综合工资审核结果,"中力薪酬管家，{1}已处理{2}的审核申请，结果为：{3}，审核意见：{4}" },//中力薪酬管家，赵泽辉已同意2018-08月工资的审核申请，审核意见：无
            { TemplateIdEnum.董办待审核,"中力薪酬管家，您好，公司{1}月份工资已生成，审核请进入 http://salary.zhongliko.com/H5/Login/CEO/{2}/{3}" },//中力薪酬管家，赵泽辉已同意2018-08月工资的审核申请，审核意见：无
        };
        public void SendSms(List<string> phone, TemplateIdEnum templateId, List<string> parameter)
        {
            var lstTmp= parameter.Select(x=>x).ToList();
            lstTmp.Insert(0, string.Empty);
           // if (!phone.Any()) throw new ArgumentException("电话号码不能为空！");
            var model = phone.Select(o => new GZ_SMS
            {
                CreateDate = DateTime.Now,
                CreateUser = this.UserInfo.Id,
                Id = Guid.NewGuid(),
                Mobile = o.Substring(0,11),
                FullContent = string.Format(dictTemplateTxt[templateId],lstTmp.ToArray()),
                RequestBody = string.Join(",", parameter),
                Status = GZ_SMS.SMSStatusEnum.Ready,
                TemplateId = templateId
            });
            DbContext.GZ_SMS.AddRange(model);
        }

        public void SendSms(List<string> phone, TemplateIdEnum templateId, List<string> parameter,Guid createUserId)
        {
            var lstTmp = parameter.Select(x => x).ToList();
            lstTmp.Insert(0, string.Empty);
            // if (!phone.Any()) throw new ArgumentException("电话号码不能为空！");
            var model = phone.Select(o => new GZ_SMS
            {
                CreateDate = DateTime.Now,
                CreateUser = createUserId,
                Id = Guid.NewGuid(),
                Mobile = o.Substring(0, 11),
                FullContent = string.Format(dictTemplateTxt[templateId], lstTmp.ToArray()),
                RequestBody = string.Join(",", parameter),
                Status = GZ_SMS.SMSStatusEnum.Ready,
                TemplateId = templateId
            });
            DbContext.GZ_SMS.AddRange(model);
        }

        public void SendSms(string phone, TemplateIdEnum templateId, List<string> parameter)
        {
            var lstTmp = parameter.Select(x => x).ToList();
            lstTmp.Insert(0, string.Empty);
            // if (!phone.Any()) throw new ArgumentException("电话号码不能为空！");
            var model = new GZ_SMS
            {
                CreateDate = DateTime.Now,
                CreateUser = this.UserInfo.Id,
                Id = Guid.NewGuid(),
                Mobile = phone.Substring(0, 11),
                FullContent = string.Format(dictTemplateTxt[templateId], lstTmp.ToArray()),
                RequestBody = string.Join(",", parameter),
                Status = GZ_SMS.SMSStatusEnum.Ready,
                TemplateId = templateId
            };
            DbContext.GZ_SMS.Add(model);
        }

        /// <summary>
        /// 立即发送短信
        /// </summary>
        /// <returns></returns>
        public SmsSingleSenderResult SendSmsByNow(string phone, TemplateIdEnum templateId, List<string> parameter)
        {
            SmsSingleSenderResult singleResult;
            SmsSingleSender singleSender = new SmsSingleSender(Config.sdkappid, Config.appkey);
            singleResult = singleSender.SendWithParam("86", phone, (int)templateId, parameter, "", "", "");
            return singleResult;//.errmsg.ToUpper() == "OK";
        }

        internal object SendSmsByUnsend()
        {
            var list = Entities.Where(o=>o.Status!=GZ_SMS.SMSStatusEnum.Success).Where(o=>o.SendTimes<=5).ToList();
            if (list == null || list.Count == 0) return new { success = 0, error = 0 };
            //var phoneList = list.Select(o => o.Mobile).ToList();
            // SmsMultiSender msender = new SmsMultiSender(Config.sdkappid, Config.appkey);
            // var sresult = msender.SendWithParam("86", phoneList, templateId, new[] { "5678" }, smsSign, "", "");
            int success = 0;
            int error = 0;
            foreach (var item in list)
            {
                var b = SendSmsByNow(item.Mobile, item.TemplateId, ServiceHelper.GetParams(item.RequestBody.Split(',')));
                if (b.errmsg.ToUpper()=="OK")
                {
                    item.Status = SMSStatusEnum.Success;
                    success++;
                }
                else
                {
                    item.Status = SMSStatusEnum.Fault;
                    error++;
                }
                item.SendTimes++;
                item.ApiResult = b.ToString();
                var entry = DbContext.Entry<GZ_SMS>(item);
                entry.State = System.Data.Entity.EntityState.Modified;
                DbContext.SaveChanges();
            }
            return new { success = success, error = error };
        }
    }
}
