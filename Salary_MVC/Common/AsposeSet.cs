using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary_MVC.Common
{
    public class AsposeSet<T>
    {
        Dictionary<int, Action<T, Aspose.Cells.Cell>> dictHandlerNoTitle=new Dictionary<int, Action<T, Aspose.Cells.Cell>>();

        Dictionary<string, Action<T, Aspose.Cells.Cell>> dictHandlerWithTitle = new Dictionary<string, Action<T, Aspose.Cells.Cell>>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="colIndex">列索引</param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public AsposeSet<T> Map( Action<T,Aspose.Cells.Cell> handler)
        {
            this.dictHandlerNoTitle.Add(this.dictHandlerNoTitle.Count,handler);
            return this;
        }

        public AsposeSet<T> Map(string title, Action<T, Aspose.Cells.Cell> handler)
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentException("标题不能为null");
            this.dictHandlerWithTitle.Add(title, handler);
            return this;
        }

        public void InsertNoTitle(Aspose.Cells.Worksheet sheet,List<T> data,int startRowIndex=0)
        {
            if (startRowIndex < 0)
                throw new ArgumentException("生成excel的起始行必须大于等于0");
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (dictHandlerNoTitle.Count < 1)
                throw new ArgumentException("请先处理列和属性的映射关系");
            if (sheet == null)
                throw new ArgumentNullException("sheet不能为null");
            int rowIndex = startRowIndex - 1;
            foreach (var value in data)
            {
                rowIndex++;
                foreach (var kv in dictHandlerNoTitle)
                {
                    kv.Value(value, sheet.Cells[rowIndex, kv.Key]);
                }
            }
        }

        public void InsertWithTitle(Aspose.Cells.Worksheet sheet, List<T> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (dictHandlerWithTitle.Count < 1)
                throw new ArgumentException("请先处理列和属性的映射关系");
            if (sheet == null)
                throw new ArgumentNullException("sheet不能为null");
            int rowIndex =0;
            int colIndex = 0;
            //处理标题
            foreach (var kv in dictHandlerWithTitle)
            {
                sheet.Cells[rowIndex, colIndex++].PutValue(kv.Key);
            }

            foreach (var value in data)
            {
                rowIndex++;
                colIndex = 0;
                foreach (var kv in dictHandlerWithTitle)
                {
                    kv.Value(value, sheet.Cells[rowIndex, colIndex++]);
                }
            }
        }

        /// <summary>
        /// 按照标题解析Excel中的数据
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="titleRowIndex"></param>
        /// <returns></returns>
        public ListWrapper<T> ToListByTitle(Aspose.Cells.Worksheet sheet,int titleRowIndex=0)
        {
            if (titleRowIndex < 0)
                throw new ArgumentException("titleRowIndex不能小于0");
            if (dictHandlerWithTitle.Count < 1)
                throw new ArgumentException("请先处理列和属性的映射关系");
            if (sheet == null)
                throw new ArgumentNullException("sheet不能为null");
            var lstTitle= System.Linq.Enumerable.Range(0, sheet.Cells.MaxColumn).Select(x => new { Index = x, sheet.Cells[titleRowIndex, x].StringValue })
                .Where(x=>dictHandlerWithTitle.Keys.Contains(x.StringValue)).ToList();
            //重复的标题
            ListWrapper<T> rst = new ListWrapper<T>(sheet.Cells.MaxRow);
            var lstChongfu= lstTitle.GroupBy(x => x.StringValue).Select(gp => new { gp.Key, Count = gp.Count() }).Where(x => x.Count > 1).ToList();
            if (lstChongfu.Count > 0)
                return rst.AddErrorMessage(string.Format("存在多个标题【{0}】", string.Join("】，【", lstChongfu.Select(x => x.Key))));
            var dictTitle= lstTitle.ToDictionary(x => x.StringValue, x => x.Index);
            var lstQueshao = dictHandlerWithTitle.Keys.Where(x => !dictTitle.ContainsKey(x)).ToList();
            if (lstQueshao.Count > 0)
                return rst.AddErrorMessage(string.Format("缺少标题【{0}】", string.Join("】，【", lstQueshao)));
            for (int rowindex = titleRowIndex + 1; rowindex <= sheet.Cells.MaxRow; rowindex++)
            {
                T value = System.Activator.CreateInstance<T>();
                foreach (var kv in dictHandlerWithTitle)
                {
                    try
                    {
                        kv.Value(value, sheet.Cells[rowindex, dictTitle[kv.Key]]);
                    }
                    catch (Exception ex)
                    {
                        rst.AddErrorMessage(string.Format("第{0}行，{1} 列附近发生错误：{2}",rowindex+1, kv.Key, ex.Message));
                    }
                }
                rst.RT.Add(value);
            }
            return rst;
        }

        /// <summary>
        /// 按照列索引解析数据
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="titleRowIndex"></param>
        /// <returns></returns>
        public ListWrapper<T> ToList(Aspose.Cells.Worksheet sheet, int titleRowIndex = 0)
        {
            if (titleRowIndex < 0)
                throw new ArgumentException("titleRowIndex不能小于0");
            if (this.dictHandlerNoTitle.Count < 1)
                throw new ArgumentException("请先处理列和属性的映射关系");
            if (sheet == null)
                throw new ArgumentNullException("sheet不能为null");
            ListWrapper<T> rst = new ListWrapper<T>(sheet.Cells.MaxRow);
            for (int rowindex = titleRowIndex; rowindex <= sheet.Cells.MaxRow; rowindex++)
            {
                T value = System.Activator.CreateInstance<T>();
                foreach (var kv in dictHandlerNoTitle)
                {
                    try
                    {
                        kv.Value(value, sheet.Cells[rowindex, kv.Key]);
                    }
                    catch (Exception ex)
                    {
                        rst.AddErrorMessage(string.Format("第{0}行，第{1}列附近发生错误：{2}", rowindex + 1, kv.Key+1, ex.Message));
                    }
                }
                rst.RT.Add(value);
            }
            return rst;
        }
    }
    public class ListWrapper<T>
    {
        public ListWrapper(int capacity = 4)
        {
            this.OnError = false;
            this.Message = new List<string>();
            this.RT = new List<T>(capacity);
        }
        public bool OnError { get; set; }

        public List<string> Message { get; set; }

        public List<T> RT { get; set; }

        public ListWrapper<T> AddErrorMessage(string msg)
        {
            this.OnError = true;
            this.Message.Add(msg);
            return this;
        }
    }

}