using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Salary_MVC.Code;
using System.ComponentModel;
using Salary_MVC.DataModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace Salary_MVC.Controllers
{
    public class CodeController : Controller
    {
        // GET: Code
        public ActionResult Index()
        {
            string ClassName = Request["ClassName"];
            //string ClassName1 = Request["ClassName1"];

            List<FieldItem> list_field = GetFieldData();
            if (!string.IsNullOrEmpty(ClassName))
            {
                list_field = list_field.Where(o => o.ClassName.Contains(ClassName)).ToList();
            }
            ViewData["list_field"] = list_field;
            ViewData["ClassName"] = ClassName;
            return View();
        }

        public class d
        {
            public string ClassName { get; set; }
            public string FieldName { get; set; }
        }

        public List<FieldItem> GetFieldData()
        {
            List<FieldItem> list = new List<FieldItem>();
            //获取程序集所有的类
            var arr_type = AppDomain.CurrentDomain.GetAssemblies();

            //选择符合BaseEntity的类
            var att_target_type = arr_type.SelectMany(a => a.GetTypes().Where(t => typeof(BaseEntity).IsAssignableFrom(t))).ToArray();

            foreach (Type one_type in att_target_type)
            {
                var class_attr = one_type.GetCustomAttributes(typeof(DescriptionAttribute), false);

                //获取类描述
                string classDesc = string.Empty;
                foreach (DescriptionAttribute item_attr in class_attr)
                {
                    classDesc = item_attr.Description;
                }

                //获取当前类的所有属性
                var arr_prop = one_type.GetProperties();
                foreach (PropertyInfo item_prop in arr_prop)
                {

                    var model = new FieldItem() { ClassName = one_type.Name, FieldType = item_prop.PropertyType.Name, FieldName = item_prop.Name, ClassDescription = classDesc };

                    //获取属性特性的描述
                    foreach (DescriptionAttribute item_prop_attr in item_prop.GetCustomAttributes(typeof(DescriptionAttribute), false))
                    {
                        model.FieldDescription = item_prop_attr.Description;
                    }

                    //获取属性是否为泛型
                    var propType = item_prop.PropertyType;
                    if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        model.FieldType = propType.GenericTypeArguments[0].Name;
                    }

                    //获取属性的自定义特性的描述(控件的key值)
                    foreach (ViewDescriptionAttribute item_prop_attr in item_prop.GetCustomAttributes(typeof(ViewDescriptionAttribute), false))
                    {
                        model.ViewKey = item_prop_attr.Description;
                    }

                    //获取属性的必须填写的特性
                    foreach (RequiredAttribute item_prop_attr in item_prop.GetCustomAttributes(typeof(RequiredAttribute), false))
                    {
                        model.IsRequied = item_prop_attr.AllowEmptyStrings.ToString();
                    }

                    //获取字符串长度的特性
                    foreach (StringLengthAttribute item_prop_attr in item_prop.GetCustomAttributes(typeof(StringLengthAttribute), false))
                    {
                        model.StringLength = item_prop_attr.MaximumLength.ToString();
                    }

                    list.Add(model);
                }
            }
            return list;
        }


        public ActionResult AddCode(List<d> add_dto, List<d> update_dto, List<d> delete_dto, List<d> query_dto)
        {
            List<FieldItem> list_field = GetFieldData();


            string ClassName = string.Empty;
            string ClassDescription = string.Empty;

            List<FieldItem> list_field_selected_add = new List<FieldItem>();
            foreach (d obj_d in add_dto)
            {
                FieldItem item = list_field.Where(o => o.ClassName == obj_d.ClassName && o.FieldName == obj_d.FieldName).FirstOrDefault();
                if (string.IsNullOrEmpty(ClassName))
                {
                    ClassName = item.ClassName;
                }

                if (string.IsNullOrEmpty(ClassDescription))
                {
                    ClassDescription = item.ClassDescription;
                }
                list_field_selected_add.Add(item);
            }


            List<FieldItem> list_field_selected_update = new List<FieldItem>();
            foreach (d obj_d in update_dto)
            {
                FieldItem item = list_field.Where(o => o.ClassName == obj_d.ClassName && o.FieldName == obj_d.FieldName).FirstOrDefault();
                if (string.IsNullOrEmpty(ClassName))
                {
                    ClassName = item.ClassName;
                }

                if (string.IsNullOrEmpty(ClassDescription))
                {
                    ClassDescription = item.ClassDescription;
                }
                list_field_selected_update.Add(item);
            }

            List<FieldItem> list_field_selected_delete = new List<FieldItem>();
            foreach (d obj_d in delete_dto)
            {
                FieldItem item = list_field.Where(o => o.ClassName == obj_d.ClassName && o.FieldName == obj_d.FieldName).FirstOrDefault();
                if (string.IsNullOrEmpty(ClassName))
                {
                    ClassName = item.ClassName;
                }

                if (string.IsNullOrEmpty(ClassDescription))
                {
                    ClassDescription = item.ClassDescription;
                }
                list_field_selected_delete.Add(item);
            }

            List<FieldItem> list_field_selected_query = new List<FieldItem>();
            foreach (d obj_d in query_dto)
            {
                FieldItem item = list_field.Where(o => o.ClassName == obj_d.ClassName && o.FieldName == obj_d.FieldName).FirstOrDefault();
                if (string.IsNullOrEmpty(ClassName))
                {
                    ClassName = item.ClassName;
                }

                if (string.IsNullOrEmpty(ClassDescription))
                {
                    ClassDescription = item.ClassDescription;
                }
                list_field_selected_query.Add(item);
            }

            CodeTemplate t_dto = new DtoTemplate(ClassName, ClassDescription);
            CodeTemplate t_service = new ServiceTemplate(ClassName, ClassDescription);
            CodeTemplate t_html = new HtmlTemplate(ClassName, ClassDescription);
            CodeTemplate t_js = new JSTemplate(ClassName, ClassDescription);

            string str_code = t_service.CreateAdd(list_field_selected_add).ToString();
            string str_code1 = t_service.CreateUpdate(list_field_selected_update).ToString();
            string str_code2 = t_service.CreateDelete(list_field_selected_delete).ToString();
            string str_code3 = t_service.CreateQuery(list_field_selected_query).ToString();

            return Content(str_code+ str_code1+ str_code2+ str_code3);
        }
    }

    public abstract class CodeTemplate : AddTemplate, EditTemplate, DeleteTemplate, QueryTemplate
    {
        public abstract StringBuilder CreateAdd(List<FieldItem> list_column);
        public abstract StringBuilder CreateUpdate(List<FieldItem> list_column);
        public abstract StringBuilder CreateDelete(List<FieldItem> list_column);
        public abstract StringBuilder CreateQuery(List<FieldItem> list_column);

        public string GetActionDesc(string str_action)
        {
            Dictionary<string, string> ht = new Dictionary<string, string>();
            ht.Add("Add", "新增");
            ht.Add("Update", "修改");
            ht.Add("Delete", "删除");
            ht.Add("Query", "查询");
            return ht[str_action];
        }

        public CodeTemplate(string ClassName, string ClassDescription)
        {
            this.ClassName = ClassName;
            this.ClassDescription = ClassDescription;
        }

        public string ClassName { get; set; }
        public string ClassDescription { get; set; }


        /// <summary>
        /// 空格
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        public string Space(int Number)
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < Number; i++)
            {
                str.Append(" ");
            }
            return str.ToString();
        }

        /// <summary>
        /// 换行
        /// </summary>
        public string NewLine
        {
            get
            {
                return System.Environment.NewLine;
            }
        }

        /// <summary>
        /// 根命名空间
        /// </summary>
        public string RootNameSpace
        {
            get
            {
                return "Salary_MVC";
            }
        }

        /// <summary>
        /// DTO命名空间
        /// </summary>
        public string DtoNameSpace
        {
            get
            {
                return "Models";
            }
        }

        /// <summary>
        /// Service命名空间
        /// </summary>
        public string ServiceNameSpace
        {
            get
            {
                return "Service";
            }
        }

        /// <summary>
        /// 左边大括号
        /// </summary>
        public string LeftBrace
        {
            get
            {
                return "{";
            }
        }

        /// <summary>
        ///右边大括号
        /// </summary>
        public string RightBrace
        {
            get
            {
                return "}";
            }
        }

        public string public_class
        {
            get
            {
                return "public class ";
            }
        }

        public string LeftLine(int Number)
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < Number; i++)
            {
                str.Append(@"/");
            }
            return str.ToString();
        }

        public string RemarkPropert(string Description)
        {
            StringBuilder str = new StringBuilder();
            str.Append(NewLine);
            str.Append(LeftLine(3) + "<summary>");
            str.Append(NewLine);
            str.Append(LeftLine(3) + Description);
            str.Append(NewLine);
            str.Append(LeftLine(3) + "</summary>");
            str.Append(NewLine);
            return str.ToString();
        }

        /// <summary>
        /// 生成一个属性
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public StringBuilder CreateProperty(FieldItem item)
        {
            StringBuilder str_code = new StringBuilder();
            str_code.Append(RemarkPropert(item.FieldDescription));
            str_code.Append("public" + Space(1) + item.FieldType + Space(1) + item.FieldName + LeftBrace + Space(1) + "get;" + Space(1) + "set;" + Space(1) + RightBrace);
            str_code.Append(NewLine);
            return str_code;
        }

        public string EFSave
        {
            get
            {
                return "return this.DbContext.SaveChanges();";
            }
        }

    }

    interface AddTemplate
    {
        StringBuilder CreateAdd(List<FieldItem> list_column);
    }

    interface EditTemplate
    {
        StringBuilder CreateUpdate(List<FieldItem> list_column);
    }

    interface DeleteTemplate
    {
        StringBuilder CreateDelete(List<FieldItem> list_column);
    }

    interface QueryTemplate
    {
        StringBuilder CreateQuery(List<FieldItem> list_column);
    }

    public class DtoTemplate : CodeTemplate
    {

        public List<string> list_namespace = new List<string>();

        public DtoTemplate(string ClassName, string ClassDescription) : base(ClassName, ClassDescription)
        {
            list_namespace.Add("System");
            list_namespace.Add("System.Collections.Generic");
            list_namespace.Add("System.ComponentModel.DataAnnotations");
            list_namespace.Add("System.Linq");
            list_namespace.Add("System.Web");
        }

        private StringBuilder RenderNameSpace()
        {
            StringBuilder str_code = new System.Text.StringBuilder();
            list_namespace.ForEach(o => str_code.Append("using" + Space(1) + o + ";" + NewLine));
            return str_code;
        }

        private StringBuilder Create(List<FieldItem> list_column, string Action)
        {
            StringBuilder str_code = new System.Text.StringBuilder();
            str_code.Append(RenderNameSpace());//输出命名空间
            str_code.Append(NewLine);

            str_code.Append("namespace " + RootNameSpace + "." + DtoNameSpace);
            str_code.Append(NewLine);
            str_code.Append(LeftBrace);
            str_code.Append(NewLine);

            //类代码 开始
            str_code.Append(RemarkPropert(GetActionDesc(Action) + ClassDescription) + "Dto");
            str_code.Append(public_class + ClassName + Action + "Dto");
            str_code.Append(NewLine);
            str_code.Append(LeftBrace);
            str_code.Append(NewLine);

            //生成属性
            list_column.ForEach(o => str_code.Append(CreateProperty(o)));

            str_code.Append(NewLine);
            str_code.Append(RightBrace);
            //类代码 结束

            str_code.Append(RightBrace);
            return str_code;
        }


        public override StringBuilder CreateAdd(List<FieldItem> list_column)
        {
            return Create(list_column, "Add");
        }

        public override StringBuilder CreateUpdate(List<FieldItem> list_column)
        {
            return Create(list_column, "Edit");
        }

        public override StringBuilder CreateDelete(List<FieldItem> list_column)
        {
            return Create(list_column, "Delete");
        }

        public override StringBuilder CreateQuery(List<FieldItem> list_column)
        {
            return Create(list_column, "Query");
        }
    }

    public class ServiceTemplate : CodeTemplate
    {
        public List<string> list_namespace = new List<string>();
        public ServiceTemplate(string ClassName, string ClassDescription) : base(ClassName, ClassDescription)
        {
            list_namespace.Add("System");
            list_namespace.Add("System.Collections.Generic");
            list_namespace.Add("System.ComponentModel.DataAnnotations");
            list_namespace.Add("System.Linq");
            list_namespace.Add("System.Web");
            list_namespace.Add("Models");
        }

        private StringBuilder RenderNameSpace()
        {
            StringBuilder str_code = new System.Text.StringBuilder();
            list_namespace.ForEach(o => str_code.Append("using" + Space(1) + o + ";" + NewLine));
            return str_code;
        }
        public string RendFunctionHead(string action)
        {
            StringBuilder str_code = new StringBuilder();
            str_code.Append(RemarkPropert(GetActionDesc(action) + ClassDescription));
            str_code.Append(LeftLine(3) + @"<param name=""" + "dto" + @"""></param>");
            str_code.Append(NewLine);
            if (action == "Query")
            {
                str_code.Append(LeftLine(3) + "<returns>"+ ClassDescription + "列表</returns>");
            }
            else
            {
                str_code.Append(LeftLine(3) + "<returns>影响行数</returns>");
            }
            str_code.Append(NewLine);
            return str_code.ToString();
        }

        protected string NewClass(string class_name)
        {
            return class_name + " model = new " + class_name + "();";
        }

        protected string SearchClass(string class_name)
        {
            return "var model= this.DbContext." + class_name + ".Where(o => o.Id == dto.Id).FirstOrDefault();";
        }

        protected string SearchList(string class_name)
        {
            return "var model= this.DbContext." + class_name + ";";
        }

        protected string DeleteClass(string class_name)
        {
            return "this.DbContext." + class_name + ".Remove(model);";
        }

        public StringBuilder Create()
        {
            StringBuilder str_code = new StringBuilder();
            str_code.Append(RenderNameSpace());

            str_code.Append("namespace " + RootNameSpace + "." + ServiceNameSpace);
            str_code.Append(NewLine);
            str_code.Append(LeftBrace);
            str_code.Append(NewLine);
            //类代码开始


            //类代码结束
            str_code.Append(NewLine);
            str_code.Append(RightBrace);
            return str_code;
        }

        public override StringBuilder CreateAdd(List<FieldItem> list_column)
        {

            string action = "Add";

            StringBuilder str_code = new StringBuilder();
            str_code.Append(RendFunctionHead(action));

            str_code.Append("public int" + Space(1) + action + "(" + ClassName + action + "Dto" + Space(1) + "dto" + ")");
            str_code.Append(LeftBrace);
            str_code.Append(NewLine);
            str_code.Append(NewClass(ClassName));
            str_code.Append(NewLine);

            list_column.ForEach(o => str_code.Append("model." + o.FieldName + Space(1) + "=" + Space(1) + "dto." + o.FieldName + ";" + NewLine));

            str_code.Append(NewLine);
            str_code.Append(EFSave);

            str_code.Append(NewLine);
            str_code.Append(RightBrace);

            return str_code;
        }

        public override StringBuilder CreateUpdate(List<FieldItem> list_column)
        {
            string action = "Update";

            StringBuilder str_code = new StringBuilder();
            str_code.Append(RendFunctionHead(action));

            str_code.Append("public int" + Space(1) + action + "(" + ClassName + action + "Dto" + Space(1) + "dto" + ")");
            str_code.Append(LeftBrace);
            str_code.Append(NewLine);
            str_code.Append(SearchClass(ClassName));//查询实体
            str_code.Append(NewLine);

            list_column.ForEach(o => str_code.Append("model." + o.FieldName + Space(1) + "=" + Space(1) + "dto." + o.FieldName + ";" + NewLine));

            str_code.Append(NewLine);
            str_code.Append(EFSave);

            str_code.Append(NewLine);
            str_code.Append(RightBrace);

            return str_code;
        }

        public override StringBuilder CreateDelete(List<FieldItem> list_column)
        {
            string action = "Delete";

            StringBuilder str_code = new StringBuilder();
            str_code.Append(RendFunctionHead(action));

            str_code.Append("public int" + Space(1) + action + "(" + ClassName + action + "Dto" + Space(1) + "dto" + ")");
            str_code.Append(LeftBrace);
            str_code.Append(NewLine);
            str_code.Append(SearchClass(ClassName));//查询实体
            str_code.Append(NewLine);

            str_code.Append(DeleteClass(ClassName));

            str_code.Append(NewLine);
            str_code.Append(EFSave);

            str_code.Append(NewLine);
            str_code.Append(RightBrace);

            return str_code;
        }

        public override StringBuilder CreateQuery(List<FieldItem> list_column)
        {
            string action = "Query";

            StringBuilder str_code = new StringBuilder();
            str_code.Append(RendFunctionHead(action));
            str_code.Append("public object" + Space(1) + action + "(" + ClassName + action + "Dto" + Space(1) + "dto" + ")");
            str_code.Append(NewLine);
            str_code.Append(LeftBrace);
           
            str_code.Append(NewLine);
            str_code.Append(SearchList(ClassName));

            str_code.Append(NewLine);
            list_column.ForEach(o => str_code.Append("model = model.Where(o =>o."+o.FieldName+"==dto." + o.FieldName+");"+ NewLine));
            
            str_code.Append("return model.ToList();");

            str_code.Append(NewLine);
            str_code.Append(RightBrace);

            return str_code;
        }
    }

    public class HtmlTemplate : CodeTemplate
    {
        public HtmlTemplate(string ClassName, string ClassDescription) : base(ClassName, ClassDescription)
        {

        }

        public override StringBuilder CreateAdd(List<FieldItem> list_column)
        {
            return new StringBuilder();
        }

        public override StringBuilder CreateUpdate(List<FieldItem> list_column)
        {
            return new StringBuilder();
        }

        public override StringBuilder CreateDelete(List<FieldItem> list_column)
        {
            return new StringBuilder();
        }

        public override StringBuilder CreateQuery(List<FieldItem> list_column)
        {
            return new StringBuilder();
        }
    }

    public class ControllerTemplate : CodeTemplate
    {
        public ControllerTemplate(string ClassName, string ClassDescription) : base(ClassName, ClassDescription)
        {
   
        }

        public override StringBuilder CreateAdd(List<FieldItem> list_column)
        {
            return new StringBuilder();
        }

        public override StringBuilder CreateUpdate(List<FieldItem> list_column)
        {
            return new StringBuilder();
        }

        public override StringBuilder CreateDelete(List<FieldItem> list_column)
        {
            return new StringBuilder();
        }

        public override StringBuilder CreateQuery(List<FieldItem> list_column)
        {
            return new StringBuilder();
        }
    }

    public class JSTemplate : CodeTemplate
    {
        public JSTemplate(string ClassName, string ClassDescription) : base(ClassName, ClassDescription)
        {

        }

        public override StringBuilder CreateAdd(List<FieldItem> list_column)
        {
            return new StringBuilder();
        }

        public override StringBuilder CreateUpdate(List<FieldItem> list_column)
        {
            return new StringBuilder();
        }

        public override StringBuilder CreateDelete(List<FieldItem> list_column)
        {
            return new StringBuilder();
        }

        public override StringBuilder CreateQuery(List<FieldItem> list_column)
        {
            return new StringBuilder();
        }
    }

}


//事项       位置                                     命名空间
//1DTO       项目名字/Models/类名字/类名字{动作}Dto   项目名字/Models
//2实体      项目名字/DataModel/Entity/类名字         项目名字/DataModel
//Service    项目名字/Service/类名字Service           项目名字/Service
//Controller 项目名字/Controllers/类名字Controller    项目名字/Controllers
//Html       项目名字/Views/类名字/类名字Index
//JS代码     只能复制张贴

//建立一个function的流程,选择实体名字 勾选增删改查列表 勾选字段 点击生成(只能做一次)
//生成代码选择类名字 选择动作 勾选字段 点击生成，一个地方生成出来，可以点击复制按钮直接复制

//模板列表
//HTML 新增弹窗层，编辑弹窗层，列表页面,查询控件，新增按钮，点击新增保证代码，编辑按钮，编辑保证的代码，删除按钮,批量删除
//Controller 直接返回一个函数
//Service 返回一个函数

//{动作}==Query Add Edit Delete List(返回值)