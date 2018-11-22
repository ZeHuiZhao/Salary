using Salary_MVC.Data;
using Salary_MVC.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Salary_MVC.Models;
using Salary.Common;
using Salary_MVC.Models;
using Salary_MVC.Common;
using Aspose.Cells;
using Salary_MVC.Enum;

namespace Salary_MVC.Services
{
    public class MonthlySalaryService : Service<DataModel.GZ_MonthlySalaryMaster>
    {

        public SMSService _sms = new SMSService();
        public UserService _user = new UserService();

        public DataModel.GZ_MonthlySalaryMaster GetMasterByMonth(DateTime month)
        {
            string monthstr = month.ToString("yyyy-MM");
            var rt = this.DbContext.GZ_MonthlySalaryMaster.Where(x => x.Month == monthstr).FirstOrDefault();
            return rt;
        }

        /// <summary>
        /// 生成指定月份的工资
        /// </summary>
        /// <param name="targetMonth">目标月份</param>
        /// <returns></returns>
        public int GenerateSalary(DateTime targetMonth)
        {
            var rt = this.Validate(targetMonth);
            var isok = rt.Attendence.IsOk && rt.Bonus.IsOk && rt.Employee.IsOk && rt.EmployeeSalary.IsOk && rt.HouseMoney.IsOk && rt.ShortSalary.IsOk && rt.SocialMoney.IsOk;
            if (!isok)
                throw new ArgumentException("状态未就绪，无法合成工资");
            //计算工资的业务逻辑
            var monthstr = targetMonth.ToString("yyyy-MM");
            var master = this.DbContext.GZ_MonthlySalaryMaster.Where(x => x.Month == monthstr).FirstOrDefault();
            if (master != null)
                throw new ArgumentException(string.Format("【{0}】月份的工资已经生成", monthstr));
            master = new GZ_MonthlySalaryMaster() { Id = Guid.NewGuid(), Month = monthstr, Status = GZ_MonthlySalaryMaster.MonthlySalaryStatus.待发起审核 };
            DateTime minDay = new DateTime(targetMonth.Year, targetMonth.Month, 1).AddSeconds(1);
            DateTime maxDay = minDay.AddMonths(1).AddDays(-1);
            var lstEmployee = this.DbContext.GZ_Employee.Where(x => (x.StatusJob == 1 || x.QuitDate >= minDay)
            && x.JoinDate <= maxDay.Date && x.CalcSalary == GZ_Employee.CalcSalaryEnum.Auto).ToList();
            //lstEmployee= lstEmployee.Where(x => x.Name.StartsWith("陈静")).ToList();
            //lstEmployee = lstEmployee.Where(x=>x.Name=="赵泽辉" || x.Name=="邓常青").ToList();
            var dictEmployee = lstEmployee.ToDictionary(x => x.Id, x => x);
            var lstSalaryDetail = lstEmployee.Select(x => new DataModel.GZ_MonthlySalaryDetail
            {
                DepartmentId = x.DepartmentId,
                EmployeeId = x.Id,
                FinancailUnitId = x.FinacialUnitId ?? Guid.Empty,
                Id = Guid.NewGuid(),
                SubjectId = master.Id,
                CompanyId = x.CorpId,
                AwardMoney = 0,
                PercentageMoney = 0,
                MakeupMoney = 0,
                PayableOther = 0,
                PunishMoney = 0,
                CreditMoney = 0,
                ReduceOther = 0
            }).ToList();
            int youxiaoweishu = 2;
            //处理调薪记录
            var dictBaseSalary = this.DbContext.GZ_EmployeeSalary.Where(x => x.EffectedDate <= maxDay.Date && x.Status == Enum.ApproveStatus.财务同意).ToList()
               .GroupBy(x => x.EmployeeId).ToDictionary(gp => gp.Key, gp => gp.ToList());
            lstSalaryDetail.ForEach(x => x.BaseSalary = Math.Round(HandlerBaseSalary(dictBaseSalary, dictEmployee[x.EmployeeId], minDay, maxDay), youxiaoweishu));
            //处理津贴
            var dictBonus = this.DbContext.GZ_Bonus.Where(x => x.StartDate <= maxDay.Date && x.EndDate >= minDay.Date && x.Status == Enum.ApproveStatus.财务同意).ToList()
                .GroupBy(x => x.EmployeeId).ToDictionary(gp => gp.Key, gp => gp.ToList());
            lstSalaryDetail.ForEach(x => x.BonusSalary = Math.Round(HandlerBonus(dictBonus, dictEmployee[x.EmployeeId], minDay, maxDay), youxiaoweishu));
            //处理工资总额
            lstSalaryDetail.ForEach(x => x.TotalSalary = x.BaseSalary + x.BonusSalary);
            //处理应发基本工资
            var dictAttendance = this.DbContext.GZ_Attendance.Where(x => x.Month == monthstr).ToList()
                .ToDictionary(x => x.EmployeeId, x => x);
            lstSalaryDetail.ForEach(x =>
            {
                var finalDays = dictAttendance.ContainsKey(x.EmployeeId) ? dictAttendance[x.EmployeeId].FinalDays : default(decimal);
                x.PayableSalary = Math.Round(finalDays * x.TotalSalary / maxDay.Day, youxiaoweishu);
                x.SalaryDays = finalDays;
            });
            //奖金和赔偿
            var dictAwardAndPunish = this.DbContext.GZ_ShortSalary.Where(x => x.EffectedDate == minDay.Date && x.Status == Enum.ApproveStatus.财务同意).ToList()
                .GroupBy(x => x.EmployeeId).ToDictionary(gp => gp.Key, gp => gp.GroupBy(a => a.Category).ToDictionary(gp2 => gp2.Key, gp2 => gp2.ToList()));
            lstSalaryDetail.ForEach(x =>
            {
                if (dictAwardAndPunish.ContainsKey(x.EmployeeId) && dictAwardAndPunish[x.EmployeeId].ContainsKey(GZ_ShortSalary.ShortSalaryCategoryEnum.奖励))
                    x.AwardMoney = dictAwardAndPunish[x.EmployeeId][GZ_ShortSalary.ShortSalaryCategoryEnum.奖励].Sum(a => a.Money);
                if (dictAwardAndPunish.ContainsKey(x.EmployeeId) && dictAwardAndPunish[x.EmployeeId].ContainsKey(GZ_ShortSalary.ShortSalaryCategoryEnum.赔偿))
                    x.PunishMoney = dictAwardAndPunish[x.EmployeeId][GZ_ShortSalary.ShortSalaryCategoryEnum.赔偿].Sum(a => a.Money);
            });
            //处理公积金
            var dictHouseMoney = this.DbContext.GZ_HouseMoneyMaster.Where(x => x.Month == monthstr && x.Status == Enum.ApproveStatus.财务同意)
                .Join(this.DbContext.GZ_HouseMoneyDetail, x => x.Id, y => y.SubjectId, (x, y) => y).ToList()
                .ToDictionary(x => x.IDCard, x => x);
            lstSalaryDetail.ForEach(x =>
            {
                x.HouseMoney = dictHouseMoney.ContainsKey(dictEmployee[x.EmployeeId].IDCard ?? string.Empty) ? dictHouseMoney[dictEmployee[x.EmployeeId].IDCard ?? string.Empty].Total / 2 : default(decimal);
            });
            //处理社保
            var lstSZ = this.DbContext.GZ_SocialMoneyMasterSZ.Where(x => x.Month == monthstr && x.Status == Enum.ApproveStatus.财务同意)
                .Join(this.DbContext.GZ_SocialMoneyDetailSZ, x => x.Id, y => y.MasterId, (x, y) => y).ToList()
                .Select(x => new { x.IDCard, x.TotalPesonal }).ToList();
            var lstGZ = this.DbContext.GZ_SocialMoneyMasterGZ.Where(x => x.Month == monthstr && x.Status == Enum.ApproveStatus.财务同意)
                .Join(this.DbContext.GZ_SocialMoneyDetailGZ, x => x.Id, y => y.MasterId, (x, y) => y).ToList()
                .Select(x => new { x.IDCard, x.TotalPesonal });
            lstSZ.AddRange(lstGZ);
            var dictSocialMoney = lstSZ.ToDictionary(x => x.IDCard, x => x.TotalPesonal);
            lstSalaryDetail.ForEach(x => x.SocialMoney = dictSocialMoney.ContainsKey(dictEmployee[x.EmployeeId].IDCard ?? string.Empty) ? dictSocialMoney[dictEmployee[x.EmployeeId].IDCard ?? string.Empty] : default(decimal));
            this.DbContext.GZ_MonthlySalaryMaster.Add(master);
            this.DbContext.GZ_MonthlySalaryDetail.AddRange(lstSalaryDetail);
            return this.DbContext.SaveChanges();
        }

        /// <summary>
        /// 计算津贴
        /// </summary>
        /// <param name="dictBonus"></param>
        /// <param name="employee"></param>
        /// <param name="minDay"></param>
        /// <param name="maxDay"></param>
        /// <returns></returns>
        private decimal HandlerBonus(Dictionary<Guid, List<GZ_Bonus>> dictBonus, GZ_Employee employee, DateTime minDay, DateTime maxDay)
        {
            if (!dictBonus.ContainsKey(employee.Id))
                return 0;//没有津贴
            var rt = dictBonus[employee.Id].Select(x => HandlerBonus(x, employee, minDay, maxDay)).Sum();//各个津贴汇总
            return rt;
        }

        /// <summary>
        /// 计算津贴
        /// </summary>
        /// <param name="bonus"></param>
        /// <param name="employee"></param>
        /// <param name="minDay"></param>
        /// <param name="maxDay"></param>
        /// <returns></returns>
        private decimal HandlerBonus(GZ_Bonus bonus, GZ_Employee employee, DateTime minDay, DateTime maxDay)
        {
            if (bonus.EndDate.Date == minDay.Date)
                return bonus.Money_pwd / maxDay.Day;
            if (bonus.StartDate.Date == maxDay.Date)
                return bonus.Money_pwd / maxDay.Day;
            var lstKV = new List<SalalryKeyValue>();//津贴固定四个点
            lstKV.Add(new SalalryKeyValue(bonus.StartDate.Date, SalalryKeyValue.Index.Db, bonus.Money_pwd));
            lstKV.Add(new SalalryKeyValue(bonus.EndDate.Date, SalalryKeyValue.Index.Db, 0));
            lstKV.Add(new SalalryKeyValue(minDay, SalalryKeyValue.Index.Month));
            lstKV.Add(new SalalryKeyValue(maxDay, SalalryKeyValue.Index.Month));
            lstKV = lstKV.OrderBy(x => x.DatePoint).ToList();
            for (int i = 1; i < lstKV.Count; i++)
            {//从第2个开始，可能津贴是 12号开始的
                if (lstKV[i].Group == SalalryKeyValue.Index.Month)
                    lstKV[i].Money = lstKV[i - 1].Money;
            }
            var lst = lstKV.Where(x => x.DatePoint >= minDay && x.DatePoint <= maxDay).ToList();
            for (int i = 0; i < lst.Count - 1; i++)
            {
                if (lst[i].Money <= 0)
                    continue;
                var segmentLength = (lst[i + 1].DatePoint.Date - lst[i].DatePoint.Date).Days + 1;//因为有加1s的，这里计算只取Date部分
                var rt = lst[i].Money * segmentLength / maxDay.Day;
                return rt;
            }
            throw new ArgumentException(string.Format("{0}的津贴记录异常，无法计算，津贴Id为{1}", employee.Name, bonus.Id));
        }

        /// <summary>
        /// 计算基本工资
        /// </summary>
        /// <param name="dictEmployeeSalary"></param>
        /// <param name="employee"></param>
        /// <param name="minDay"></param>
        /// <param name="maxDay"></param>
        /// <returns></returns>
        private decimal HandlerBaseSalary(Dictionary<Guid, List<GZ_EmployeeSalary>> dictEmployeeSalary, GZ_Employee employee, DateTime minDay, DateTime maxDay)
        {
            if (!dictEmployeeSalary.ContainsKey(employee.Id))
                throw new ArgumentException(string.Format("员工{0}没有调薪记录", employee.Name));
            List<GZ_EmployeeSalary> lstEmpolyeeSalary = dictEmployeeSalary[employee.Id];
            var lstKeyValue = lstEmpolyeeSalary.Select(x => new SalalryKeyValue(x.EffectedDate.Date, SalalryKeyValue.Index.Db, x.Money)).ToList();
            lstKeyValue.Add(new SalalryKeyValue(minDay, SalalryKeyValue.Index.Month));
            lstKeyValue.Add(new SalalryKeyValue(maxDay, SalalryKeyValue.Index.Month));
            lstKeyValue = lstKeyValue.OrderBy(x => x.DatePoint).ToList();
            if (lstKeyValue[0].Group == SalalryKeyValue.Index.Month)
            {//入职
                if (lstKeyValue[1].Group == SalalryKeyValue.Index.Db)
                    lstKeyValue[0].Money = lstKeyValue[1].Money;
                else
                    throw new ArgumentException(string.Format("{0}的调薪记录异常，无法计算工资", employee.Name));
            }
            for (int i = 1; i < lstKeyValue.Count; i++)
            {
                if (lstKeyValue[i].Group == SalalryKeyValue.Index.Month)
                    lstKeyValue[i].Money = lstKeyValue[i - 1].Money;
            }
            var lst = lstKeyValue.Where(x => x.DatePoint >= minDay && x.DatePoint <= maxDay).ToList();
            var lst2 = lst.Select(x => x).ToList();

            lst.RemoveAt(lst.Count - 1);//移除最后一个
            lst2.RemoveAt(0);
            //正常是算头不算尾，最后一段要算尾
            lst2.Last().DatePoint = lst2.Last().DatePoint.AddDays(1);
            var lstSegment = lst.Zip(lst2, (x, y) => new { x.Money, Lenth = (y.DatePoint.Date - x.DatePoint.Date).Days }).ToList();//因为有加1s的，这里计算只取Date部门
            var fenzi = lstSegment.Sum(x => x.Money * x.Lenth);
            var fenmu = (decimal)lstSegment.Sum(x => x.Lenth);
            var rt = fenzi / fenmu;
            return rt;
        }

        /// <summary>
        /// 合成工资的辅助类
        /// </summary>
        class SalalryKeyValue
        {
            public SalalryKeyValue(DateTime date, Index group, decimal money = 0)
            {
                this.DatePoint = date;
                this.Group = group;
                this.Money = money;
            }
            public DateTime DatePoint { get; set; }
            public decimal Money { get; set; }

            public Index Group { get; set; }
            public enum Index
            {
                /// <summary>
                /// 数据库数据
                /// </summary>
                Db,
                /// <summary>
                /// 月份第一天、最后一天的点
                /// </summary>
                Month,
            }
        }
        /// <summary>
        /// 检测是否就绪
        /// 考勤、员工基本信息、津贴、奖惩、调薪
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public GenerateMonthSalaryValidateInfo Validate(DateTime month)
        {
            //返回值 类别（考勤、员工基本信息、社保深圳、社保广州、工资深圳、工资广州、津贴、奖惩、调薪），是否就绪，消息
            string monthstr = month.ToString("yyyy-MM");
            GenerateMonthSalaryValidateInfo vInfo = new GenerateMonthSalaryValidateInfo();
            vInfo.Attendence = ValidateAttendence(month, vInfo.Attendence);
            //员工基本信息，所有在职员工 或者  本月离职员工，状态为 锁定状态
            vInfo.Employee = ValidateEmployee(month, vInfo.Employee);
            //工资，
            var lstHouseMoney = this.DbContext.GZ_HouseMoneyMaster.Where(x => x.Month == monthstr).ToList();
            //深圳公积金
            var houseMoney = lstHouseMoney.FirstOrDefault(x => x.Template == GZ_HouseMoneyMaster.TemplateIndex.深圳);
            if (houseMoney == null)
                vInfo.HouseMoney.AddError(string.Format("没有【{0}】月份的深圳公积金数据", monthstr), "../HouseMoneySZ/Approve");
            if (houseMoney != null && houseMoney.Status != Enum.ApproveStatus.财务同意)
                vInfo.HouseMoney.AddError(string.Format("【{0}】月份的深圳公积金数据没有通过审核", monthstr), "../HouseMoneySZ/Approve");
            //广州公积金
            houseMoney = lstHouseMoney.FirstOrDefault(x => x.Template == GZ_HouseMoneyMaster.TemplateIndex.广州);
            if (houseMoney == null)
                vInfo.HouseMoney.AddError(string.Format("没有【{0}】月份的广州公积金数据", monthstr), "../HouseMoneyGZ/Approve");
            if (houseMoney != null && houseMoney.Status != Enum.ApproveStatus.财务同意)
                vInfo.HouseMoney.AddError(string.Format("【{0}】月份的广州公积金数据没有通过审核", monthstr), "../HouseMoneyGZ/Approve");
            //深圳社保
            var socialMoneySZ = this.DbContext.GZ_SocialMoneyMasterSZ.Where(x => x.Month == monthstr).FirstOrDefault();
            if (socialMoneySZ == null)
                vInfo.SocialMoney.AddError(string.Format("没有【{0}】月份的深圳社保数据", monthstr), "../SocialMoneySZ/Approve");
            if (socialMoneySZ != null && socialMoneySZ.Status != Enum.ApproveStatus.财务同意)
                vInfo.SocialMoney.AddError(string.Format("【{0}】月份的深圳社保数据没有通过审核", monthstr), "../SocialMoneySZ/Approve");
            //广州社保
            var socialMoneyGZ = this.DbContext.GZ_SocialMoneyMasterSZ.Where(x => x.Month == monthstr).FirstOrDefault();
            if (socialMoneyGZ == null)
                vInfo.SocialMoney.AddError(string.Format("没有【{0}】月份的广州社保数据", monthstr), "../SocialMoneyGZ/Approve");
            if (socialMoneyGZ != null && socialMoneyGZ.Status != Enum.ApproveStatus.财务同意)
                vInfo.SocialMoney.AddError(string.Format("【{0}】月份的广州社保数据没有通过审核", monthstr), "../SocialMoneyGZ/Approve");
            //津贴 待财务审核的津贴，不影响本月的工资计算
            var firstday = new DateTime(month.Year, month.Month, 1);
            var lastday = firstday.AddMonths(1).AddDays(-1);
            int max = 3;
            var lstBonus = this.DbContext.GZ_Bonus.Where(x => x.StartDate <= lastday && x.EndDate >= firstday && x.Status == Enum.ApproveStatus.待财务审核)
                .Join(this.DbContext.GZ_Employee, x => x.EmployeeId, y => y.Id, (x, y) => new { x.StartDate, x.EndDate, x.Status, x.Id, y.Name }).ToList();
            if (lstBonus.Count > 0)
                vInfo.Bonus.AddError(string.Format("【{0}】{1}的津贴记录状态为 {2}", string.Join(",", lstBonus.Take(3).Select(x => x.Name))
                    , lstBonus.Count > max ? "等" : string.Empty, lstBonus[0].Status.ToString()));
            //奖惩 待财务审核的奖惩，不影响本月的工资计算
            var lstShortSalary = this.DbContext.GZ_ShortSalary.Where(x => x.EffectedDate == firstday && x.Status == Enum.ApproveStatus.待财务审核)
                .Join(this.DbContext.GZ_Employee, x => x.EmployeeId, y => y.Id, (x, y) => new { x.Id, x.EffectedDate, x.Status, y.Name }).ToList();
            if (lstShortSalary.Count > 0)
                vInfo.ShortSalary.AddError(string.Format("【{0}】{1}的奖惩记录状态为 {2}", string.Join(",", lstShortSalary.Take(3).Select(x => x.Name))
                   , lstShortSalary.Count > max ? "等" : string.Empty, lstShortSalary[0].Status.ToString()));
            //调薪 待财务审核的调薪，不影响本月的工资计算
            var lstEmpolyeeSalary = this.DbContext.GZ_EmployeeSalary.Where(x => x.EffectedDate <= lastday && x.Status == Enum.ApproveStatus.待财务审核)
                .Join(this.DbContext.GZ_Employee, x => x.EmployeeId, y => y.Id, (x, y) => new { x.Id, x.EffectedDate, x.Status, y.Name }).ToList();
            if (lstEmpolyeeSalary.Count > 0)
                vInfo.EmployeeSalary.AddError(string.Format("【{0}】{1}的调薪记录状态为 {2}", string.Join(",", lstEmpolyeeSalary.Take(3).Select(x => x.Name))
                   , lstEmpolyeeSalary.Count > max ? "等" : string.Empty, lstEmpolyeeSalary[0].Status.ToString()));
            return vInfo;
        }

        /// <summary>
        /// 获取指定Id的工资明细数据
        /// </summary>
        /// <param name="detailId"></param>
        /// <returns></returns>
        public Tuple<GZ_MonthlySalaryDetail, GZ_Employee, GZ_Department, GZ_FinancialUnit> GetEntityById(Guid detailId)
        {
            var salaryWrapper = this.DbContext.GZ_MonthlySalaryDetail.Where(x => x.Id == detailId)
                .Join(this.DbContext.GZ_Employee, x => x.EmployeeId, y => y.Id, (x, y) => new { x, y })
                .Join(this.DbContext.GZ_Department, wrapper => wrapper.x.DepartmentId, z => z.Id, (wrapper, z) => new { wrapper.x, wrapper.y, z })
                .Join(this.DbContext.GZ_FinancialUnit, wrapper => wrapper.x.FinancailUnitId, a => a.Id, (wrapper, a) => new { wrapper.x, wrapper.y, wrapper.z, a }).FirstOrDefault();
            if (salaryWrapper == null)
                throw new ArgumentException("工资明细数据不存在，或者相关的员工，部门，财务核算单位数据不完整");
            return Tuple.Create(salaryWrapper.x, salaryWrapper.y, salaryWrapper.z, salaryWrapper.a);

        }

        /// <summary>
        /// 员工基本信息，
        /// 所有在职员工 或者  本月离职员工，状态为 锁定状态
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        private GenerateMonthSalaryValidateInfoWrapper ValidateEmployee(DateTime month, GenerateMonthSalaryValidateInfoWrapper rt)
        {//员工基本信息，所有在职员工 或者  本月离职员工，状态为 锁定状态
            DateTime firtday = new DateTime(month.Year, month.Month, 1);
            DateTime lastday = firtday.AddMonths(1).AddDays(-1);
            var lstEmployee = this.DbContext.GZ_Employee.Where(x => x.JoinDate <= lastday && x.CalcSalary == GZ_Employee.CalcSalaryEnum.Auto).ToList();
            if (lstEmployee.Count < 1)
                return rt.AddError("没有员工数据");
            int max = 3;
            var lstLeaveNoDate = lstEmployee.Where(x => x.StatusJob != 1 && !x.QuitDate.HasValue).ToList();
            if (lstLeaveNoDate.Count > 0)
                return rt.AddError(string.Format("【{0}】{1}的状态为非在职，但是没有离职日期", string.Join(",", lstLeaveNoDate.Take(max).Select(x => x.Name)), lstLeaveNoDate.Count > max ? "等" : string.Empty));
            //真正需要计算工资的员工
            lstEmployee = lstEmployee.Where(x => x.StatusJob == 1 || x.QuitDate >= firtday).ToList();
            //检验状态
            var lstEmployeeUnLock = lstEmployee.Where(x => x.Status != (int)Salary_MVC.Enum.EmployeeStatusEnum.Lock).ToList();
            if (lstEmployeeUnLock.Count > 0)
                rt.AddError(string.Format("【{0}】{1}的员工信息状态未锁定", string.Join(",", lstEmployeeUnLock.Take(max).Select(x => x.Name)), lstEmployeeUnLock.Count > max ? "等" : string.Empty));
            //身份证号码
            var lstEmployeeNoIdCard = lstEmployee.Where(x => string.IsNullOrEmpty(x.IDCard)).ToList();
            if (lstEmployeeNoIdCard.Count > 0)
                rt.AddError(string.Format("【{0}】{1}缺少身份证号码", string.Join(",", lstEmployeeNoIdCard.Take(max).Select(x => x.Name)), lstEmployeeNoIdCard.Count > max ? "等" : string.Empty));
            //财务核算单位
            var lstEmployeeNoUnit = lstEmployee.Where(x => (x.FinacialUnitId ?? Guid.Empty) == Guid.Empty).ToList();
            if (lstEmployeeNoUnit.Count > 0)
                rt.AddError(string.Format("【{0}】{1}缺少财务核算单位", string.Join(",", lstEmployeeNoUnit.Take(max).Select(x => x.Name)), lstEmployeeNoUnit.Count > max ? "等" : string.Empty));
            //发薪公司
            var lstSalaryGroup = System.Enum.GetValues(typeof(Salary_MVC.Enum.SalaryGroupEnum)).Cast<int>().ToList();
            var lstEmployeeNoGroup = lstEmployee.Where(x => !lstSalaryGroup.Contains(x.SalaryGroup ?? -1)).ToList();
            if (lstEmployeeNoGroup.Count > 0)
                rt.AddError(string.Format("【{0}】{1}缺少发薪公司", string.Join(",", lstEmployeeNoGroup.Take(max).Select(x => x.Name)), lstEmployeeNoGroup.Count > max ? "等" : string.Empty));
            //银行卡省份-(中力知识科技的员工专用)
            var lstEmployeeNoAerea = lstEmployee.Where(x => x.SalaryGroup == (int)Salary_MVC.Enum.SalaryGroupEnum.ZLZSJJ && string.IsNullOrEmpty(x.BankArea)).ToList();
            if (lstEmployeeNoAerea.Count > 0)
                rt.AddError(string.Format("【{0}】{1}缺少银行卡省份", string.Join(",", lstEmployeeNoAerea.Take(max).Select(x => x.Name)), lstEmployeeNoAerea.Count > max ? "等" : string.Empty));
            return rt;
        }

        public int Edit(MonthSalaryEdit parameter)
        {
            var wrapper = this.DbContext.GZ_MonthlySalaryDetail.Where(x => x.Id == parameter.Id)
                .Join(DbContext.GZ_MonthlySalaryMaster, x => x.SubjectId, y => y.Id, (x, y) => new { Detail = x, Master = y }).FirstOrDefault();
            if (wrapper == null)
                throw new ArgumentException("指定的工资明细记录不存在，或者其所属的工资主记录不存在");
            List<GZ_MonthlySalaryMaster.MonthlySalaryStatus> lstOkStatus = new List<GZ_MonthlySalaryMaster.MonthlySalaryStatus>() {
             GZ_MonthlySalaryMaster.MonthlySalaryStatus.CFO否决,
                GZ_MonthlySalaryMaster.MonthlySalaryStatus.待发起审核,
             GZ_MonthlySalaryMaster.MonthlySalaryStatus.董办否决,
             GZ_MonthlySalaryMaster.MonthlySalaryStatus.财务经理否决};
            if (!lstOkStatus.Contains(wrapper.Master.Status))
                throw new ArgumentException(string.Format("不能对【{0}】状态的数据进行修改", wrapper.Master.Status.ToString()));
            List<DataModel.GZ_UpdateHistory> lstHistory = new List<GZ_UpdateHistory>();
            DataModel.UpdateHistoryActivator<DataModel.GZ_MonthlySalaryDetail> activator = new UpdateHistoryActivator<GZ_MonthlySalaryDetail>(Cookies.User);
            lstHistory.Add(activator.Create(wrapper.Detail, x => x.PercentageMoney, parameter.PercentageMoney));
            lstHistory.Add(activator.Create(wrapper.Detail, x => x.PayableOther, parameter.PayableOther));
            lstHistory.Add(activator.Create(wrapper.Detail, x => x.ReduceOther, parameter.ReduceOther));
            lstHistory.Add(activator.Create(wrapper.Detail, x => x.CreditMoney, parameter.CreditMoney));
            lstHistory.Add(activator.Create(wrapper.Detail, x => x.MakeupMoney, parameter.MakeupMoney));
            lstHistory = lstHistory.Where(x => x.OldValue != x.NewValue).ToList();
            this.DbContext.GZ_UpdateHistory.AddRange(lstHistory);//修改记录表

            var editWrapper = this.DbContext.Entry(wrapper.Detail);
            editWrapper.State = System.Data.Entity.EntityState.Unchanged;
            wrapper.Detail.PercentageMoney = parameter.PercentageMoney;
            wrapper.Detail.PayableOther = parameter.PayableOther;
            wrapper.Detail.ReduceOther = parameter.ReduceOther;
            wrapper.Detail.CreditMoney = parameter.CreditMoney;
            wrapper.Detail.MakeupMoney = parameter.MakeupMoney;
            editWrapper.Property(x => x.PercentageMoney_Pwd).IsModified = true;
            editWrapper.Property(x => x.PayableOther_Pwd).IsModified = true;
            editWrapper.Property(x => x.ReduceOther_Pwd).IsModified = true;
            editWrapper.Property(x => x.CreditMoney_Pwd).IsModified = true;
            editWrapper.Property(x => x.MakeupMoney_Pwd).IsModified = true;
            this.DbContext.Configuration.ValidateOnSaveEnabled = false;
            var rt = this.DbContext.SaveChanges();
            return rt;
        }

        internal object SyncToZlApp(string month)
        {
            if (string.IsNullOrEmpty(month))
                throw new ArgumentException("必须指定综合工资的月份");
            DateTime tmp;
            if (!DateTime.TryParse(month, out tmp))
                throw new ArgumentException("综合工资的月份的格式错误，示例2018-01");
            if (DbContext.UserWages.Where(o => o.TimeMonth == tmp).Count() > 0) throw new ArgumentException("已经操作过，不能重复操作");
            var salaryMaster = DbContext.GZ_MonthlySalaryMaster.Where(o => o.Month == month).FirstOrDefault();
            if (salaryMaster == null) throw new ArgumentException("该月份还没有生成工资");
            if (salaryMaster.Status != GZ_MonthlySalaryMaster.MonthlySalaryStatus.董办同意) throw new ArgumentException("该月份工资董办还没有同意，不能操作工资发放");
            List<GZ_MonthlySalaryDetail> detailList = DbContext.GZ_MonthlySalaryDetail.Where(o => o.SubjectId == salaryMaster.Id).ToList();
            List<UserWages> userWagesList = new List<UserWages>();
            List<GZ_Employee> emList = DbContext.GZ_Employee.ToList();
            List<User> userList = DbContext.User.ToList();
            detailList.ForEach(o =>
            {
                GZ_Employee em = emList.Where(e => e.Id == o.EmployeeId).FirstOrDefault();
                if (em == null) throw new ArgumentException("找不到员工信息");
                User user = userList.Where(u => u.UserGuid == em.OriginalId).FirstOrDefault();
                if (user == null) throw new ArgumentException("找不到考勤员工信息");
                userWagesList.Add(new UserWages()
                {
                    BaseWag = o.BaseSalary_Pwd,
                    CutPayRemark = "",
                    CutPayTitle = o.ReduceTotal_Pwd,
                    Days = o.SalaryDays.ToString(),
                    HousingFund = o.HouseMoney.ToString(),
                    JobSubsidies = o.BonusSalary_Pwd,
                    Other = o.PayableOther.ToString(),
                    Othercutpay = o.ReduceOther.ToString(),
                    PayTrue = o.RealPay_Pwd,
                    PersonalTax = o.TaxMoney_Pwd,
                    SocialSecurity = o.SocialMoney.ToString(),
                    TimeMonth = tmp,
                    TotalAmount = o.PayableTotal_Pwd,
                    UserName = em.Name,
                    UserID = user.ID.ToString()
                });
            });
            DbContext.UserWages.AddRange(userWagesList);
            return DbContext.SaveChanges();
        }

        public byte[] ExportBank(DateTime tmp)
        {
            var master = this.GetMasterByMonth(tmp);
            if (master == null)
                throw new ArgumentException(string.Format("未找到{0}月的工资主记录", tmp.ToString("yyyy-MM")));
            if (master.Status != GZ_MonthlySalaryMaster.MonthlySalaryStatus.CFO同意 && master.Status != GZ_MonthlySalaryMaster.MonthlySalaryStatus.董办同意)
                throw new ArgumentException(string.Format("{0}月的工资记录状态为{1}，不能导出工资表", tmp.ToString("yyyy-MM"), master.Status.ToString()));
            var lst = this.DbContext.GZ_MonthlySalaryDetail.Where(x => x.SubjectId == master.Id)
                .Join(this.DbContext.GZ_Employee, x => x.EmployeeId, y => y.Id, (x, y) => new { Salary = x, Employee = y }).ToList();
            var dictSalaryGroup = lst.GroupBy(x => x.Employee.SalaryGroup.Value).ToDictionary(gp => (Salary_MVC.Enum.SalaryGroupEnum)gp.Key,
                gp => gp.OrderBy(a => a.Employee.Name).Select(a => Tuple.Create(a.Salary, a.Employee)).ToList());
            using (Aspose.Cells.Workbook book = new Aspose.Cells.Workbook(Aspose.Cells.FileFormatType.Xlsx))
            {
                ExportBankZLZSKJ(book, dictSalaryGroup);//中力知识科技
                ExportBankJP(book, dictSalaryGroup);//京鹏基金
                book.Worksheets.RemoveAt("sheet1");
                using (System.IO.MemoryStream msrt = new System.IO.MemoryStream())
                {
                    book.Save(msrt, Aspose.Cells.SaveFormat.Xlsx);
                    return msrt.ToArray();
                }
            }
        }

        private void ExportBankZLZSKJ(Workbook book, Dictionary<SalaryGroupEnum, List<Tuple<GZ_MonthlySalaryDetail, GZ_Employee>>> dictSalaryGroup)
        {
            if (!dictSalaryGroup.ContainsKey(SalaryGroupEnum.ZLZSJJ))
                return;//没有这个发薪公司的记录
            var asposeSet = new Common.AsposeSet<Tuple<GZ_MonthlySalaryDetail, GZ_Employee>>();
            asposeSet.Map("账号", (value, cell) => cell.PutValue(value.Item2.BankCard));
            asposeSet.Map("名称", (value, cell) => cell.PutValue(value.Item2.Name));
            asposeSet.Map("金额", (value, cell) => cell.PutValue(value.Item1.RealPay));
            asposeSet.Map("账号所属省份", (value, cell) => cell.PutValue(value.Item2.BankArea));
            asposeSet.Map("证件类型", (value, cell) => cell.PutValue(string.Empty));
            asposeSet.Map("证件号码", (value, cell) => cell.PutValue(string.Empty));
            asposeSet.Map("用途", (value, cell) => cell.PutValue("工资"));
            asposeSet.Map("附言", (value, cell) => cell.PutValue(string.Empty));
            var sheet = book.Worksheets.Add(SalaryGroupEnum.ZLZSJJ.GetDescription());
            asposeSet.InsertWithTitle(sheet, dictSalaryGroup[SalaryGroupEnum.ZLZSJJ]);
        }

        private void ExportBankJP(Workbook book, Dictionary<SalaryGroupEnum, List<Tuple<GZ_MonthlySalaryDetail, GZ_Employee>>> dictSalaryGroup)
        {
            if (!dictSalaryGroup.ContainsKey(SalaryGroupEnum.JPJJ))
                return;//没有这个发薪公司的记录
            var asposeSet = new Common.AsposeSet<Tuple<GZ_MonthlySalaryDetail, GZ_Employee>>();
            var bankName = "中国建设银行";//这个和发薪公司绑死的，不会轻易变动，否则所有人的银行卡都要变动
            asposeSet.Map("付款方帐号", (value, cell) => cell.PutValue(string.Empty));
            asposeSet.Map("转帐金额", (value, cell) => cell.PutValue(value.Item1.RealPay));
            asposeSet.Map("收帐方帐号", (value, cell) => cell.PutValue(value.Item2.BankCard));
            asposeSet.Map("收帐帐户名称", (value, cell) => cell.PutValue(value.Item2.Name));
            asposeSet.Map("收帐帐户开户机构名称", (value, cell) => cell.PutValue(bankName));
            asposeSet.Map("摘要", (value, cell) => cell.PutValue(string.Empty));
            var sheet = book.Worksheets.Add(SalaryGroupEnum.JPJJ.GetDescription());
            asposeSet.InsertWithTitle(sheet, dictSalaryGroup[SalaryGroupEnum.JPJJ]);
        }

        /// <summary>
        /// 按照部门和财务核算单位维度的汇总数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Salary_MVC.Models.MonthlySalarySumWrapper GetEntityByCEO(Guid id)
        {
            var lstMaster = this.DbContext.GZ_MonthlySalaryMaster.OrderBy(x => x.Month).ToList();
            var master = lstMaster.FirstOrDefault(x => x.Id == id);
            if (master == null)
                throw new ArgumentException("指定的工资主记录不存在");
            var masterlast = lstMaster.TakeWhile(x => x.Id != id).LastOrDefault();
            //上个月的master可能不存在
            masterlast = masterlast ?? new GZ_MonthlySalaryMaster();
            var lstDetail = this.DbContext.GZ_MonthlySalaryDetail.Where(x => x.SubjectId == master.Id || x.SubjectId == masterlast.Id).ToList();
            var dictMonthMoney = lstDetail.GroupBy(x => x.SubjectId).ToDictionary(x => x.Key, x => x.ToList());
            var dictDepartment = this.DbContext.GZ_Department.ToList().ToDictionary(x => x.Id, x => x.Name);
            var dictFinanceUnit = this.DbContext.GZ_FinancialUnit.ToList().ToDictionary(x => x.Id, x => x.Name);
            Dictionary<Guid, decimal> dictMasterLastDepartment = new Dictionary<Guid, decimal>();
            Dictionary<Guid, decimal> dictMasterLastFinanceUnit = new Dictionary<Guid, decimal>();
            if (dictMonthMoney.ContainsKey(masterlast.Id))
            {
                dictMasterLastDepartment = dictMonthMoney[masterlast.Id].GroupBy(x => x.DepartmentId).ToDictionary(gp => gp.Key, gp => gp.Sum(a => a.RealPay));
                dictMasterLastFinanceUnit = dictMonthMoney[masterlast.Id].GroupBy(x => x.FinancailUnitId).ToDictionary(gp => gp.Key, gp => gp.Sum(a => a.RealPay));
            }
            var lstDepartment = dictMonthMoney[master.Id].GroupBy(x => x.DepartmentId).Select((gp, index) => new Salary_MVC.Models.MonthlySalarySum()
            {
                Index = index + 1,
                LastMonthMoney = dictMasterLastDepartment.ContainsKey(gp.Key) ? dictMasterLastDepartment[gp.Key] : default(decimal),
                MonthMoney = gp.Sum(a => a.RealPay),
                Name = dictDepartment.ContainsKey(gp.Key) ? dictDepartment[gp.Key] : string.Empty
            }).ToList();
            var lstFinanceUnit = dictMonthMoney[master.Id].GroupBy(x => x.FinancailUnitId).Select((gp, index) => new Salary_MVC.Models.MonthlySalarySum()
            {
                Index = index + 1,
                LastMonthMoney = dictMasterLastFinanceUnit.ContainsKey(gp.Key) ? dictMasterLastFinanceUnit[gp.Key] : default(decimal),
                MonthMoney = gp.Sum(a => a.RealPay),
                Name = dictFinanceUnit.ContainsKey(gp.Key) ? dictFinanceUnit[gp.Key] : string.Empty
            }).ToList();
            decimal unit = 10000m;
            int youxiaoweishu = 2;//四舍五入小数点位数
            lstDepartment.ForEach(x =>
            {
                x.LastMonthMoney = Math.Round(x.LastMonthMoney / unit, youxiaoweishu);
                x.MonthMoney = System.Math.Round(x.MonthMoney / unit, youxiaoweishu);
            });
            lstFinanceUnit.ForEach(x =>
            {
                x.LastMonthMoney = System.Math.Round(x.LastMonthMoney / unit, youxiaoweishu);
                x.MonthMoney = System.Math.Round(x.MonthMoney / unit, youxiaoweishu);
            });
            Salary_MVC.Models.MonthlySalarySumWrapper rt = new MonthlySalarySumWrapper()
            {
                DepartmentMonty = lstDepartment,
                FinanceMoney = lstFinanceUnit,
                LastTotalMoney = lstDepartment.Sum(x => x.LastMonthMoney),
                Master = master,
                TotalEmployee = dictMonthMoney[master.Id].Count,
                TotalMoney = lstDepartment.Sum(x => x.MonthMoney),
                Status = master.Status.ToString()
            };

            return rt;
        }

        public List<Tuple<DataModel.GZ_MonthlySalaryDetail, DataModel.GZ_Employee>> GetEntityDetailByCEO(Guid id)
        {
            var lstMaster = this.DbContext.GZ_MonthlySalaryMaster.OrderBy(x => x.Month).ToList();
            var master = lstMaster.FirstOrDefault(x => x.Id == id);
            if (master == null)
                throw new ArgumentException("指定的工资主记录不存在");
            var masterlast = lstMaster.TakeWhile(x => x.Id != id).LastOrDefault();
            //上个月的master可能不存在
            masterlast = masterlast ?? new GZ_MonthlySalaryMaster();
            var lstDetail = this.DbContext.GZ_MonthlySalaryDetail.Where(x => x.SubjectId == master.Id || x.SubjectId == masterlast.Id)
                .Join(this.DbContext.GZ_Employee, x => x.EmployeeId, y => y.Id, (x, y) => new { Salary = x, y.Name }).ToList();
            return lstDetail.Select(x => Tuple.Create(x.Salary, new DataModel.GZ_Employee() { Name = x.Name })).ToList();
        }

        public bool ApproveByCEO(ApproveInput parameter)
        {
            var master = this.DbContext.GZ_MonthlySalaryMaster.AsNoTracking().Where(x => x.Id == parameter.Id).FirstOrDefault();
            if (master == null)
                throw new ArgumentException("未找到要审核的工资数据");
            if (master.Status != GZ_MonthlySalaryMaster.MonthlySalaryStatus.CFO同意)
                throw new ArgumentException(string.Format("不能对{0}状态的工资数据进行审核", master.Status.ToString()));
            //改状态
            this.DbContext.Configuration.ValidateOnSaveEnabled = false;
            master.Status = parameter.Handler == GZ_ApproveLog.ApproveLogCategory.Through ? GZ_MonthlySalaryMaster.MonthlySalaryStatus.董办同意 : GZ_MonthlySalaryMaster.MonthlySalaryStatus.董办否决;
            var masterWrapper = this.DbContext.Entry(master);
            masterWrapper.State = System.Data.Entity.EntityState.Unchanged;
            masterWrapper.Property(x => x.Status).IsModified = true;
            //审核记录
            DataModel.GZ_ApproveLog approveLog = new GZ_ApproveLog()
            {
                Category = parameter.Handler,//这里需要完善成枚举
                Comment = parameter.Comment ?? "无",
                Id = Guid.NewGuid(),
                OperatorId = this.UserInfo.Id,
                OperatorTime = DateTime.Now,
                TargetId = master.Id,
                TargetStatus = (int)master.Status,
                TargetTable = nameof(DataModel.GZ_MonthlySalaryMaster)
            };
            this.DbContext.GZ_ApproveLog.Add(approveLog);
            //短信记录
            //中力薪酬管家，{1}已处理{2}的审核申请，结果为：{3}，审核意见：{4}
            List<string> requestParameter = new List<string>();
            requestParameter.Add(this.UserInfo.Name);
            requestParameter.Add(string.Format("{0}月工资", master.Month));
            requestParameter.Add(parameter.Handler == GZ_ApproveLog.ApproveLogCategory.Through ? "同意" : "否决");
            requestParameter.Add(approveLog.Comment);
            var lstphone = this._user.GetPhoneList(Salary_MVC.Enum.RoleEnum.Finance);
            if (lstphone != null && lstphone.Count > 0)
                this._sms.SendSms(lstphone, GZ_SMS.TemplateIdEnum.综合工资审核结果, requestParameter);
            var rt = this.DbContext.SaveChanges();
            return rt > 0;
        }

        /// <summary>
        /// 校验考勤，
        /// 该月存在数据，该月所有考勤数据的状态为 人力资本总监同意、系统强制同意
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        private GenerateMonthSalaryValidateInfoWrapper ValidateAttendence(DateTime month, GenerateMonthSalaryValidateInfoWrapper rt)
        {
            string monthstr = month.ToString("yyyy-MM");
            var lstKQ = this.DbContext.GZ_Attendance.Where(x => x.Month == monthstr).Select(x => new { x.Name, x.Status, x.Month }).ToList();
            if (lstKQ.Count < 1)
                return rt.AddError(string.Format("没有【{0}】月的考勤数据", monthstr));
            int max = 3;
            lstKQ = lstKQ.Where(x => x.Status != (int)Salary_MVC.Enum.AttendanceStatusEnum.HRPass && x.Status != (int)Salary_MVC.Enum.AttendanceStatusEnum.SystemPass).ToList();
            if (lstKQ.Count > 0)
                rt.AddError(string.Format("【{0}】{1}的考勤数据未通过审核", string.Join(",", lstKQ.Take(max).Select(x => x.Name)), lstKQ.Count > max ? "等" : string.Empty));
            return rt;
        }


        public bool ApproveByFinance(DateTime month)
        {
            string monthstr = month.ToString("yyyy-MM");
            var master = this.DbContext.GZ_MonthlySalaryMaster.AsNoTracking().Where(x => x.Month == monthstr).FirstOrDefault();
            if (master == null)
                throw new ArgumentException("未找到要审核的工资记录");
            if (master.Status != GZ_MonthlySalaryMaster.MonthlySalaryStatus.待发起审核)
                throw new ArgumentException(string.Format("不能对【{0}】状态的工资发起审核", master.Status));
            //审核
            this.DbContext.Configuration.ValidateOnSaveEnabled = false;
            master.Status = GZ_MonthlySalaryMaster.MonthlySalaryStatus.待财务经理审核;
            var masterWrapper = this.DbContext.Entry(master);
            masterWrapper.State = System.Data.Entity.EntityState.Unchanged;
            masterWrapper.Property(x => x.Status).IsModified = true;//只审核修改状态
            DataModel.GZ_ApproveLog approveLog = new GZ_ApproveLog()
            {
                Category = GZ_ApproveLog.ApproveLogCategory.Through,//这里需要完善成枚举
                Comment = string.Empty,
                Id = Guid.NewGuid(),
                OperatorId = this.UserInfo.Id,
                OperatorTime = DateTime.Now,
                TargetId = master.Id,
                TargetStatus = (int)master.Status,
                TargetTable = nameof(DataModel.GZ_MonthlySalaryMaster)
            };
            this.DbContext.GZ_ApproveLog.Add(approveLog);//增加审核记录
            //短信记录
            //{ TemplateIdEnum.综合工资待审核,"中力薪酬管家，{1}已提交{2}的审核申请，请打开{3}进行审核" }
            List<string> requestParameter = new List<string>();
            requestParameter.Add(this.UserInfo.Name);
            requestParameter.Add(string.Format("{0}月工资", monthstr));
            var lstphone = this._user.GetPhoneList(Enum.RoleEnum.FinanceManager);
            if (lstphone != null && lstphone.Count > 0)
                this._sms.SendSms(lstphone, GZ_SMS.TemplateIdEnum.综合工资待审核, requestParameter);
            var rt = this.DbContext.SaveChanges();
            return rt > 0;
        }

        /// <summary>
        /// 导出列表数据
        /// </summary>
        /// <param name="tmp"></param>
        /// <param name="templatePath"></param>
        /// <returns></returns>
        public byte[] ExportList(DateTime tmp, string templatePath)
        {
            var master = this.GetMasterByMonth(tmp);
            if (master == null)
                throw new ArgumentException(string.Format("未找到{0}月的工资主记录", tmp.ToString("yyyy-MM")));
            var lstDepartment = this.DbContext.GZ_Department.ToList();
            var dictDepartment = lstDepartment.ToDictionary(x => x.Id, x => x);
            var lstFinanceUnit = this.DbContext.GZ_FinancialUnit.ToList();
            var dictFinanceUnit = lstFinanceUnit.ToDictionary(x => x.Id, x => x);
            var lst = this.DbContext.GZ_MonthlySalaryDetail.Where(x => x.SubjectId == master.Id)
                .Join(this.DbContext.GZ_Employee, x => x.EmployeeId, y => y.Id, (x, y) => new { Salary = x, Employee = y }).ToList();
            var asposeSet = new Common.AsposeSet<Tuple<GZ_MonthlySalaryDetail, GZ_Employee>>();
            asposeSet.Map((value, cell) => cell.PutValue(value.Item2.Name));
            asposeSet.Map((value, cell) => cell.PutValue(dictFinanceUnit.ContainsKey(value.Item1.FinancailUnitId) ? dictFinanceUnit[value.Item1.FinancailUnitId].Name : string.Empty));
            asposeSet.Map((value, cell) => cell.PutValue(dictDepartment.ContainsKey(value.Item1.DepartmentId) ? dictDepartment[value.Item1.DepartmentId].Name : string.Empty));
            asposeSet.Map((value, cell) => cell.PutValue(value.Item1.BaseSalary));
            asposeSet.Map((value, cell) => cell.PutValue(value.Item1.BonusSalary));
            asposeSet.Map((value, cell) => cell.PutValue(value.Item1.TotalSalary));
            asposeSet.Map((value, cell) => cell.PutValue(value.Item1.SalaryDays));
            asposeSet.Map((value, cell) => cell.PutValue(value.Item1.PayableSalary));
            asposeSet.Map((value, cell) => cell.PutValue(value.Item1.AwardMoney));
            asposeSet.Map((value, cell) => cell.PutValue(value.Item1.PercentageMoney));
            asposeSet.Map((value, cell) => cell.PutValue(value.Item1.MakeupMoney));
            asposeSet.Map((value, cell) => cell.PutValue(value.Item1.PayableOther));
            asposeSet.Map((value, cell) => cell.PutValue(value.Item1.PayableTotal));
            asposeSet.Map((value, cell) => cell.PutValue(value.Item1.SocialMoney));
            asposeSet.Map((value, cell) => cell.PutValue(value.Item1.HouseMoney));
            asposeSet.Map((value, cell) => cell.PutValue(value.Item1.TaxAmount));
            asposeSet.Map((value, cell) => cell.PutValue(value.Item1.TaxMoney));
            asposeSet.Map((value, cell) => cell.PutValue(value.Item1.PunishMoney));
            asposeSet.Map((value, cell) => cell.PutValue(value.Item1.CreditMoney));
            asposeSet.Map((value, cell) => cell.PutValue(value.Item1.ReduceOther));
            asposeSet.Map((value, cell) => cell.PutValue(value.Item1.ReduceTotal));
            asposeSet.Map((value, cell) => cell.PutValue(value.Item1.RealPay));
            var lstWrapper = lst.OrderBy(x => x.Salary.DepartmentId).Select(x => Tuple.Create(x.Salary, x.Employee)).ToList();
            using (System.IO.FileStream fs = new System.IO.FileStream(templatePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                Aspose.Cells.Workbook book = new Aspose.Cells.Workbook(fs);
                var sheet = book.Worksheets[0];
                sheet.Cells["A2"].PutValue(string.Format("{0}工资明细表", tmp.ToString("yyyy年MM月")));
                asposeSet.InsertNoTitle(book.Worksheets[0], lstWrapper, 4);
                using (System.IO.MemoryStream msrt = new System.IO.MemoryStream())
                {
                    book.Save(msrt, Aspose.Cells.SaveFormat.Xlsx);
                    return msrt.ToArray();
                }
            }
        }

        public List<GZ_User> GetCeoApproveOperator()
        {
            var roleCEO = this.DbContext.GZ_Role.Where(x => x.Code == (int)Salary_MVC.Enum.RoleEnum.CEO).FirstOrDefault();
            if (roleCEO == null)
                throw new ArgumentException("不存在董办的角色");
            var lstUser = this.DbContext.GZ_UserRole.Where(x => x.RoleId == roleCEO.Id)
                .Join(this.DbContext.GZ_User, x => x.UserId, y => y.Id, (x, y) => y).ToList();
            return lstUser;
        }


        public bool ApproveByFinanceManager(ApproveInput parameter)
        {
            var master = this.DbContext.GZ_MonthlySalaryMaster.AsNoTracking().Where(x => x.Id == parameter.Id).FirstOrDefault();
            if (master == null)
                throw new ArgumentException("未找到要审核的工资数据");
            if (master.Status != GZ_MonthlySalaryMaster.MonthlySalaryStatus.待财务经理审核)
                throw new ArgumentException(string.Format("财务不能对{0}状态的工资数据进行审核", master.Status.ToString()));
            //改状态
            this.DbContext.Configuration.ValidateOnSaveEnabled = false;
            master.Status = parameter.Handler == GZ_ApproveLog.ApproveLogCategory.Through ? GZ_MonthlySalaryMaster.MonthlySalaryStatus.财务经理同意 : GZ_MonthlySalaryMaster.MonthlySalaryStatus.财务经理否决;
            var masterWrapper = this.DbContext.Entry(master);
            masterWrapper.State = System.Data.Entity.EntityState.Unchanged;
            masterWrapper.Property(x => x.Status).IsModified = true;
            //审核记录
            DataModel.GZ_ApproveLog approveLog = new GZ_ApproveLog()
            {
                Category = parameter.Handler,//这里需要完善成枚举
                Comment = parameter.Comment ?? "无",
                Id = Guid.NewGuid(),
                OperatorId = this.UserInfo.Id,
                OperatorTime = DateTime.Now,
                TargetId = master.Id,
                TargetStatus = (int)master.Status,
                TargetTable = nameof(DataModel.GZ_MonthlySalaryMaster)
            };
            this.DbContext.GZ_ApproveLog.Add(approveLog);
            //短信记录
            //{ TemplateIdEnum.综合工资待审核,"中力薪酬管家，{1}已提交{2}的审核申请，请打开{3}进行审核" },//薪酬管家，李彩铃已提交2018-08月工资的审核申请，请打开薪酬管家进行审核
            //{ TemplateIdEnum.综合工资审核结果,"中力薪酬管家，{1}已{2}{3}的审核申请，审核意见：{4}" },//
            List<string> requestParameter = new List<string>();
            requestParameter.Add(this.UserInfo.Name);
            requestParameter.Add(string.Format("{0}月工资", master.Month));
            if (parameter.Handler == GZ_ApproveLog.ApproveLogCategory.NotThrough)
            {
                requestParameter.Add("否决");
                requestParameter.Add(approveLog.Comment);
            }
            var lstphone = this._user.GetPhoneList(parameter.Handler == GZ_ApproveLog.ApproveLogCategory.Through ? Enum.RoleEnum.CFO : Enum.RoleEnum.Finance);
            if (lstphone != null && lstphone.Count > 0)
                this._sms.SendSms(lstphone, parameter.Handler == GZ_ApproveLog.ApproveLogCategory.Through ? GZ_SMS.TemplateIdEnum.综合工资待审核 : GZ_SMS.TemplateIdEnum.综合工资审核结果, requestParameter);
            var rt = this.DbContext.SaveChanges();
            return rt > 0;
        }


        public bool ApproveByCFO(Models.ApproveByCFOInput parameter)
        {
            var master = this.DbContext.GZ_MonthlySalaryMaster.AsNoTracking().Where(x => x.Id == parameter.Id).FirstOrDefault();
            if (master == null)
                throw new ArgumentException("未找到要审核的工资数据");
            if (master.Status != GZ_MonthlySalaryMaster.MonthlySalaryStatus.财务经理同意)
                throw new ArgumentException(string.Format("不能对{0}状态的工资数据进行审核", master.Status.ToString()));
            //改状态
            this.DbContext.Configuration.ValidateOnSaveEnabled = false;
            master.Status = parameter.Handler == GZ_ApproveLog.ApproveLogCategory.Through ? GZ_MonthlySalaryMaster.MonthlySalaryStatus.CFO同意
                : GZ_MonthlySalaryMaster.MonthlySalaryStatus.CFO否决;
            var masterWrapper = this.DbContext.Entry(master);
            masterWrapper.State = System.Data.Entity.EntityState.Unchanged;
            masterWrapper.Property(x => x.Status).IsModified = true;
            //审核记录
            DataModel.GZ_ApproveLog approveLog = new GZ_ApproveLog()
            {
                Category = parameter.Handler,//这里需要完善成枚举
                Comment = parameter.Comment ?? "无",
                Id = Guid.NewGuid(),
                OperatorId = this.UserInfo.Id,
                OperatorTime = DateTime.Now,
                TargetId = master.Id,
                TargetStatus = (int)master.Status,
                TargetTable = nameof(DataModel.GZ_MonthlySalaryMaster)
            };
            this.DbContext.GZ_ApproveLog.Add(approveLog);
            //短信记录
            List<string> requestParameter = new List<string>();
            List<string> lstphone = new List<string>();
            //{ TemplateIdEnum.综合工资审核结果,"中力薪酬管家，{1}已{2}{3}的审核申请，审核意见：{4}" },
            if (parameter.Handler == GZ_ApproveLog.ApproveLogCategory.NotThrough)
            {
                requestParameter.Add(this.UserInfo.Name);
                requestParameter.Add(string.Format("{0}月工资", master.Month));
                requestParameter.Add("否决");
                requestParameter.Add(approveLog.Comment);
                lstphone = this._user.GetPhoneList(Enum.RoleEnum.Finance);
                this._sms.SendSms(lstphone, GZ_SMS.TemplateIdEnum.综合工资审核结果, requestParameter);
            }
            else
            {//每个号码的参数不一样,短信内容不一样
                //{ TemplateIdEnum.董办待审核,"中力薪酬管家，您好，公司{1}月份工资已生成，请手机登录 {2} 进行审批" }
                var lstUser = this.DbContext.GZ_User.Where(x => parameter.Operators.Contains(x.Id)).ToList();
                //var lstParameter = lstUser.Select(x =>Tuple.Create(x, string.Format("{0}/H5/Login/CEO/{1}/{2}", Common.Config.BaseAddress,x.UserName, master.Id.ToString("N"))) ).ToList();
                lstUser.ForEach(x => this._sms.SendSms(new List<string>() { x.UserName }, GZ_SMS.TemplateIdEnum.董办待审核, new List<string>() { master.Month, x.UserName, master.Id.ToString("N") }));
            }
            var rt = this.DbContext.SaveChanges();
            return rt > 0;
        }
        /// <summary>
        /// 获取财务待审核、已审核的工资主记录
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public DataModel.GZ_MonthlySalaryMaster GetEntityByFinanceManager(Models.GetEntityByApprove parameter)
        {
            var query = this.DbContext.GZ_MonthlySalaryMaster.AsQueryable();
            if (parameter.TabIndex == TabEnum.已审核)
                query = query.Where(x => x.Month == parameter.Month && x.Status != GZ_MonthlySalaryMaster.MonthlySalaryStatus.待发起审核 && x.Status != GZ_MonthlySalaryMaster.MonthlySalaryStatus.待财务经理审核);
            else if (parameter.TabIndex == TabEnum.待审核)
                query = query.Where(x => x.Status == GZ_MonthlySalaryMaster.MonthlySalaryStatus.待财务经理审核);
            else
                throw new Common.InputException("只能获取待审核或者已审核的工资主记录数据");
            var master = query.ToList().OrderByDescending(x => x.Month).FirstOrDefault();//如果存在多个月的待审核工资，只展示最新月份的
            return master;
        }

        public DataModel.GZ_MonthlySalaryMaster GetEntityByCFO(Models.GetEntityByApprove parameter)
        {
            var query = this.DbContext.GZ_MonthlySalaryMaster.AsQueryable();
            var lstStatus = new List<GZ_MonthlySalaryMaster.MonthlySalaryStatus>() {  GZ_MonthlySalaryMaster.MonthlySalaryStatus.CFO同意,
                GZ_MonthlySalaryMaster.MonthlySalaryStatus.CFO否决,
                GZ_MonthlySalaryMaster.MonthlySalaryStatus.董办同意,
                GZ_MonthlySalaryMaster.MonthlySalaryStatus.董办否决};
            if (parameter.TabIndex == TabEnum.已审核)
                query = query.Where(x => x.Month == parameter.Month && lstStatus.Contains(x.Status));
            else if (parameter.TabIndex == TabEnum.待审核)
                query = query.Where(x => x.Status == GZ_MonthlySalaryMaster.MonthlySalaryStatus.财务经理同意);
            else
                throw new Common.InputException("只能获取待审核或者已审核的工资主记录数据");
            var master = query.ToList().OrderByDescending(x => x.Month).FirstOrDefault();//如果存在多个月的待审核工资，只展示最新月份的
            return master;
        }


        /// <summary>
        /// 获取指定主表记录相关联的所有明细数据
        /// </summary>
        /// <param name="master"></param>
        /// <returns></returns>
        public List<Tuple<DataModel.GZ_MonthlySalaryDetail, DataModel.GZ_Employee>> GetEntityByMaster(DataModel.GZ_MonthlySalaryMaster master, Models.MonthSalaryQuery parameter)
        {
            var query = this.DbContext.GZ_MonthlySalaryDetail.Where(x => x.SubjectId == master.Id);
            var queryEmployee = this.DbContext.GZ_Employee.AsQueryable();
            if (!string.IsNullOrEmpty(parameter.Name))
                queryEmployee = queryEmployee.Where(x => x.Name.Contains(parameter.Name));
            if ((parameter.CompanyId ?? Guid.Empty) != Guid.Empty)
                query = query.Where(x => x.CompanyId == parameter.CompanyId.Value);
            if ((parameter.DepartmentId ?? Guid.Empty) != Guid.Empty)
                query = query.Where(x => x.DepartmentId == parameter.DepartmentId.Value);
            if ((parameter.FinancailUnitId ?? Guid.Empty) != Guid.Empty)
                query = query.Where(x => x.FinancailUnitId == parameter.FinancailUnitId.Value);
            var lst = query.Join(queryEmployee, x => x.EmployeeId, y => y.Id, (x, y) => new { x, y }).ToList();
            return lst.Select(x => Tuple.Create(x.x, x.y)).ToList();
        }

    }
}