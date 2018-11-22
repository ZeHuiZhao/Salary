using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Salary_MVC.DataModel;

namespace Salary_MVC.Services
{
    public class AttachmentService : Data.Service<DataModel.GZ_Attachment>
    {
        public GZ_Attachment GetEnityBySourceId(Guid id)
        {
            return this.DbContext.GZ_Attachment.Where(x => x.SourceId == id).FirstOrDefault();
        }
    }
}