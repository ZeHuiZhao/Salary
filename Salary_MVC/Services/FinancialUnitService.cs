using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Salary_MVC.DataModel;
using Salary_MVC.Models;

namespace Salary_MVC.Services
{
    public class FinancialUnitService : Data.Service<DataModel.GZ_FinancialUnit>
    {
        /// <summary>
        /// 获取所有的财务核算单位
        /// </summary>
        /// <returns></returns>
        public List<DataModel.GZ_FinancialUnit> GetEntity()
        {
            var lst= this.DbContext.GZ_FinancialUnit.OrderBy(x => x.Name).OrderBy(x=>x.Name).ToList();
            return lst;
        }

        /// <summary>
        /// 获取正常状态的所有财务核算单位
        /// </summary>
        /// <returns></returns>
        public List<DataModel.GZ_FinancialUnit> GetEntityWithKeyValue()
        {
            var lst = this.DbContext.GZ_FinancialUnit.Where(x=>x.Status== GZ_FinancialUnit.FinancialUnit.正常).OrderBy(x => x.Name).ToList();
            return lst;
        }

        public int Add(FinanceUnitAdd parameter)
        {
            var unit= this.DbContext.GZ_FinancialUnit.Where(x => x.Name == parameter.Name).FirstOrDefault();
            if (unit != null)
                throw new ArgumentException(string.Format("已存在名称为【{0}】的财务核算单位", parameter.Name));
            unit = new GZ_FinancialUnit() {
                Id = Guid.NewGuid(),
                CreateDate = DateTime.Now,
                CreateUser = this.UserInfo.Id,
                LastUpdateDate = DateTime.Now,
                LastUpdateUser = this.UserInfo.Id,
                Name = parameter.Name,
                Status = GZ_FinancialUnit.FinancialUnit.正常
            };
            this.DbContext.GZ_FinancialUnit.Add(unit);
            return this.DbContext.SaveChanges();
        }

        public object GetEntityById(Guid id)
        {
            var unit= this.DbContext.GZ_FinancialUnit.Where(x => x.Id == id).FirstOrDefault();
            return unit;
        }

        public int Edit(FinanceUnitEdit parameter)
        {
            var unit= this.DbContext.GZ_FinancialUnit.AsNoTracking().Where(x => x.Id == parameter.Id).FirstOrDefault();
            if (unit == null)
                throw new ArgumentException("未找到要指定Id的财务核算单位");
            if (unit.Status != GZ_FinancialUnit.FinancialUnit.正常)
                throw new ArgumentException("不能对【{0}】状态的财务核算单位进行修改",unit.Status.ToString());
            //修改记录表
            List<GZ_UpdateHistory> lstHistory = new List<GZ_UpdateHistory>();
            DataModel.UpdateHistoryActivator<GZ_FinancialUnit> activator = new UpdateHistoryActivator<GZ_FinancialUnit>(Cookies.User);
            lstHistory.Add(activator.Create(unit, x => x.Name, parameter.Name));
            //lstHistory.Add(DataModel.UpdateHistoryActivator<GZ_EmployeeSalary>.Create(master, x => x.EffectedDate, parameter.EffectedDate));
            ////lstHistory.Add(DataModel.UpdateHistoryActivator<GZ_EmployeeSalary>.Create(master, x => x.Category, parameter.Category));
            //lstHistory.Add(DataModel.UpdateHistoryActivator<GZ_EmployeeSalary>.Create(master, x => x.Comment, parameter.Comment));
            //lstHistory.Add(DataModel.UpdateHistoryActivator<GZ_Attachment>.Create(attachment, x => x.FilePath, parameter.FilePath));
            lstHistory = lstHistory.Where(x => x.OldValue != x.NewValue).ToList();
            this.DbContext.GZ_UpdateHistory.AddRange(lstHistory);
            unit.Name = parameter.Name;
            unit.LastUpdateDate = DateTime.Now;
            unit.LastUpdateUser = this.UserInfo.Id;
            var unitWrapper= this.DbContext.Entry(unit);
            unitWrapper.State = System.Data.Entity.EntityState.Unchanged;
            unitWrapper.Property(x => x.Name).IsModified = true;
            unitWrapper.Property(x => x.LastUpdateDate).IsModified = true;
            unitWrapper.Property(x => x.LastUpdateUser).IsModified = true;
            return this.DbContext.SaveChanges();
        }

        /// <summary>
        /// 删除财务核算单位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteById(Guid id)
        {
            if (this.DbContext.GZ_Employee.Where(x => x.FinacialUnitId == id).Count() > 0)
                throw new ArgumentException("这个财务核算单位已经被员工信息使用，不能删除");
            if (this.DbContext.GZ_ShortSalary.Where(x => x.FinancailUnitId == id).Count() > 0)
                throw new ArgumentException("这个财务核算单位已经被奖惩记录使用，不能删除");
            if (this.DbContext.GZ_EmployeeSalary.Where(x => x.FinancailUnitId == id).Count() > 0)
                throw new ArgumentException("这个财务核算单位已经被调薪记录使用，不能删除");
            if (this.DbContext.GZ_Bonus.Where(x => x.FinancailUnitId == id).Count() > 0)
                throw new ArgumentException("这个财务核算单位已经被津贴使用，不能删除");
            if (this.DbContext.GZ_MonthlySalaryDetail.Where(x => x.FinancailUnitId == id).Count() > 0)
                throw new ArgumentException("这个财务核算单位已经被综合工资明细使用，不能删除");
            DataModel.GZ_FinancialUnit unit = new GZ_FinancialUnit() { Id=id};
            var wrapper= this.DbContext.Entry(unit);
            wrapper.State = System.Data.Entity.EntityState.Deleted;
            return this.DbContext.SaveChanges();
            
        }

        internal object GetFinanceUnitList()
        {
            return Entities.Where(o => o.Status == 0).Select(o => new { o.Id, o.Name }).ToList();
        }

        /// <summary>
        /// 作废财务核算单位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int ForbidById(Guid id)
        {
            DataModel.GZ_FinancialUnit unit = new GZ_FinancialUnit() { Id = id, Status = GZ_FinancialUnit.FinancialUnit.作废 };
            var wrapper= this.DbContext.Entry(unit);
            wrapper.State = System.Data.Entity.EntityState.Unchanged;
            wrapper.Property(x => x.Status).IsModified = true;
            this.DbContext.Configuration.ValidateOnSaveEnabled = false;
            return this.DbContext.SaveChanges();
        }
    }
}