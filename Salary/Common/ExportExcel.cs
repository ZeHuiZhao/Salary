using Aspose.Cells;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;

namespace Salary.Common
{
    public enum ExportTypes
    {
        Simple,
        SetTitle,
        Custom
    }

    public class ExportExcel
    {
        public class ExcelSheet
        {
            public class FieldInformation
            {
                public string Title;

                public string FieldName;

                public string Format;

                public int Width;

                public FieldInformation(string mtitle, string field_name, string mformat)
                {
                    this.Title = "";
                    this.FieldName = "";
                    this.Format = "";
                    this.Width = 0;
                    this.Title = mtitle;
                    this.FieldName = field_name;
                    this.Format = mformat;
                }
            }

            public DataView Mdv;

            public ExportTypes ExportType;

            public int HeaderSize;

            public int TitleSize;

            public int ContentSize;

            public string FontName;

            public int StartRow;

            public string ReportTitle;

            public Hashtable HatHeight;

            public Hashtable HatSpecail;

            public Hashtable HatColumnFormula;

            public Dictionary<string, bool> ht_text_wrapped;

            public List<string> list_meger;

            public Dictionary<string, string> ht_hight_filed_value;

            public ArrayList ArrFields;

            public Hashtable HatFields;

            public string TitleString;

            public int MaxWidth;

            private bool DisableAuto;

            private int NormalWidth;

            private int NormalHeight;

            public bool ShowHeaderFooter;

            public bool underLine;

            private bool mRepeatTitle;

            public bool RepeatTitle
            {
                get
                {
                    return this.mRepeatTitle;
                }
            }

            public ExcelSheet(string mReportTitle, DataView dv, ExportTypes mExportType)
            {
                this.HeaderSize = 12;
                this.TitleSize = 9;
                this.ContentSize = 9;
                this.FontName = "Arial";
                this.StartRow = 4;
                this.ReportTitle = "";
                this.HatHeight = new Hashtable();
                this.HatSpecail = new Hashtable();
                this.HatColumnFormula = new Hashtable();
                this.ht_text_wrapped = new Dictionary<string, bool>();
                this.list_meger = new List<string>();
                this.ht_hight_filed_value = new Dictionary<string, string>();
                this.ArrFields = new ArrayList();
                this.HatFields = new Hashtable();
                this.MaxWidth = 0;
                this.DisableAuto = false;
                this.NormalWidth = 500;
                this.NormalHeight = 500;
                this.ShowHeaderFooter = true;
                this.underLine = false;
                this.mRepeatTitle = false;
                this.Mdv = dv;
                this.ExportType = mExportType;
                this.ReportTitle = mReportTitle;
                bool flag = mExportType == ExportTypes.Simple;
                if (flag)
                {
                    this.SetTitle();
                }
            }

            public ExcelSheet(string mReportTitle, DataView dv, ExportTypes mExportType, bool mShowHearderFooter, ref bool iSunderLine)
            {
                this.HeaderSize = 12;
                this.TitleSize = 9;
                this.ContentSize = 9;
                this.FontName = "Arial";
                this.StartRow = 4;
                this.ReportTitle = "";
                this.HatHeight = new Hashtable();
                this.HatSpecail = new Hashtable();
                this.HatColumnFormula = new Hashtable();
                this.ht_text_wrapped = new Dictionary<string, bool>();
                this.list_meger = new List<string>();
                this.ht_hight_filed_value = new Dictionary<string, string>();
                this.ArrFields = new ArrayList();
                this.HatFields = new Hashtable();
                this.MaxWidth = 0;
                this.DisableAuto = false;
                this.NormalWidth = 500;
                this.NormalHeight = 500;
                this.ShowHeaderFooter = true;
                this.underLine = false;
                this.mRepeatTitle = false;
                this.Mdv = dv;
                this.ExportType = mExportType;
                this.ReportTitle = mReportTitle;
                this.ShowHeaderFooter = mShowHearderFooter;
                this.underLine = iSunderLine;
                bool flag = mExportType == ExportTypes.Simple;
                if (flag)
                {
                    this.SetTitle();
                }
            }

            public void Add(string Field, string Title)
            {
                this.Add(Field, Title, "");
            }

            public void Add(string Field, string Title, string StringFormat)
            {
                bool flag = this.ExportType == ExportTypes.Custom;
                if (flag)
                {
                    ExportExcel.ExcelSheet.FieldInformation fieldInformation = new ExportExcel.ExcelSheet.FieldInformation(Title, Field, StringFormat);
                    this.ArrFields.Add(fieldInformation);
                    this.HatFields.Add(fieldInformation.FieldName, fieldInformation);
                    return;
                }
                throw new Exception("Only 'Custom' ExportType allow use 'Add' method.");
            }

            public void SetSumColumn(string Field, string StringFormat = "")
            {
                this.HatColumnFormula.Add(Field, StringFormat);
            }

            public void AddSpecialLine(string[] Arr, int RowIndex)
            {
                this.HatSpecail.Add(RowIndex, Arr);
            }

            public void DisableAutoSetWidth(int width, int height)
            {
                this.DisableAuto = true;
                checked
                {
                    this.NormalWidth = (int)Math.Round(Math.Ceiling((double)width / 7.0));
                    this.NormalHeight = height - 3;
                }
            }

            public void DisableAutoSetWidth()
            {
                this.DisableAuto = true;
                this.NormalWidth = checked((int)Math.Round(Math.Ceiling(11.0)));
                this.NormalHeight = 16;
            }

            public void MeasureHeightAndWidth()
            {
                ExportTypes exportType = this.ExportType;
                checked
                {
                    if (exportType == ExportTypes.Simple)
                    {
                        int num = this.Mdv.Table.Columns.Count - 1;
                        for (int i = 0; i <= num; i++)
                        {
                            string columnName = this.Mdv.Table.Columns[i].ColumnName;
                            bool flag = columnName != "bg_color" & columnName != "font_color";
                            if (flag)
                            {
                                bool flag2 = !this.HatFields.ContainsKey(columnName);
                                if (flag2)
                                {
                                    ExportExcel.ExcelSheet.FieldInformation fieldInformation = new ExportExcel.ExcelSheet.FieldInformation(this.Mdv.Table.Columns[i].ColumnName, this.Mdv.Table.Columns[i].ColumnName, "");
                                    this.ArrFields.Add(fieldInformation);
                                    this.HatFields.Add(fieldInformation.FieldName, fieldInformation);
                                }
                            }
                        }
                    }
                    int num2 = this.Mdv.Count - 1;
                    for (int i = 0; i <= num2; i++)
                    {
                        this.HatHeight.Add(i, 18);
                    }
                    bool disableAuto = this.DisableAuto;
                    if (disableAuto)
                    {
                        int num3 = this.ArrFields.Count - 1;
                        for (int i = 0; i <= num3; i++)
                        {
                            ExportExcel.ExcelSheet.FieldInformation fieldInformation2 = (ExportExcel.ExcelSheet.FieldInformation)this.ArrFields[i];
                            bool flag3 = fieldInformation2.Width == 0;
                            if (flag3)
                            {
                                fieldInformation2.Width = this.NormalWidth;
                            }
                        }
                        int num4 = this.Mdv.Count - 1;
                        for (int i = 0; i <= num4; i++)
                        {
                            this.HatHeight[i] = this.NormalHeight;
                        }
                    }
                    else
                    {
                        SizeF sizeF = default(SizeF);
                        PointF origin = new PointF(0f, 0f);
                        StringFormat stringFormat = new StringFormat(StringFormat.GenericTypographic);
                        Bitmap image = new Bitmap(10, 10, PixelFormat.Format32bppRgb);
                        Graphics graphics = Graphics.FromImage(image);
                        System.Drawing.Font font = new System.Drawing.Font(this.FontName, (float)((double)this.TitleSize / 20.0));
                        int num5 = this.ArrFields.Count - 1;
                        for (int i = 0; i <= num5; i++)
                        {
                            ExportExcel.ExcelSheet.FieldInformation fieldInformation3 = (ExportExcel.ExcelSheet.FieldInformation)this.ArrFields[i];
                            string title = fieldInformation3.Title;
                            sizeF = graphics.MeasureString(title, font, origin, stringFormat);
                            int num6 = (int)Math.Round((double)sizeF.Width);
                            int num7 = (int)Math.Round((double)sizeF.Height);
                            font = new System.Drawing.Font(this.FontName, (float)((double)this.ContentSize / 20.0));
                            int num8 = this.Mdv.Count - 1;
                            for (int j = 0; j <= num8; j++)
                            {
                                DataRow row = this.Mdv[j].Row;
                                string text = this.ConvertString(RuntimeHelpers.GetObjectValue(row[fieldInformation3.FieldName]));
                                sizeF = graphics.MeasureString(text, font, origin, stringFormat);
                                bool flag4 = sizeF.Width > (float)num6;
                                if (flag4)
                                {
                                    num6 = (int)Math.Round((double)sizeF.Width);
                                }
                                bool flag5 = sizeF.Height > (float)Convert.ToInt32(this.HatHeight[j]);
                                if (flag5)
                                {
                                    this.HatHeight[j] = sizeF.Height;
                                }
                            }
                            bool flag6 = this.MaxWidth > 0;
                            if (flag6)
                            {
                                bool flag7 = num6 > this.MaxWidth;
                                if (flag7)
                                {
                                    num6 = this.MaxWidth;
                                }
                            }
                            fieldInformation3.Width = 22;
                        }
                    }
                }
            }

            public void SetFormat(string Field, string mFormat)
            {
                ((ExportExcel.ExcelSheet.FieldInformation)this.HatFields[Field]).Format = mFormat;
            }

            public void SetColumnWidth(string Field, int width)
            {
                bool flag = this.HatFields == null;
                if (flag)
                {
                    throw new Exception("HatFields");
                }
                bool flag2 = this.HatFields[Field] == null;
                if (flag2)
                {
                    throw new Exception("Me.HatFields.Item(Field)");
                }
                ((ExportExcel.ExcelSheet.FieldInformation)this.HatFields[Field]).Width = checked((int)Math.Round(Math.Ceiling((double)width / 7.0)));
            }

            private void SetTitle()
            {
                checked
                {
                    int num = this.Mdv.Table.Columns.Count - 1;
                    for (int i = 0; i <= num; i++)
                    {
                        ExportExcel.ExcelSheet.FieldInformation fieldInformation = new ExportExcel.ExcelSheet.FieldInformation(this.Mdv.Table.Columns[i].ColumnName, this.Mdv.Table.Columns[i].ColumnName, "");
                        this.ArrFields.Add(fieldInformation);
                        this.HatFields.Add(fieldInformation.FieldName, fieldInformation);
                    }
                }
            }

            public void SetTitle(string[] Arr)
            {
                bool flag = this.ExportType == ExportTypes.SetTitle;
                checked
                {
                    if (flag)
                    {
                        int num = Arr.GetUpperBound(1);
                        for (int i = 0; i <= num; i++)
                        {
                            ExportExcel.ExcelSheet.FieldInformation fieldInformation = new ExportExcel.ExcelSheet.FieldInformation(Arr[i], this.Mdv.Table.Columns[i].ColumnName, "");
                            this.ArrFields.Add(fieldInformation);
                            this.HatFields.Add(fieldInformation.FieldName, fieldInformation);
                        }
                        return;
                    }
                    throw new Exception("Only 'SetTitle' ExportType allow use 'SetTitle' method.");
                }
            }

            public void SetTitle(string SplitChar, string mTitle)
            {
                string[] title = mTitle.Split(SplitChar.ToCharArray());
                this.SetTitle(title);
            }

            public void SetMaxWidth(int max)
            {
                this.MaxWidth = max;
            }

            public void SetRepeatTitle(bool bln)
            {
                this.mRepeatTitle = bln;
            }

            private string ConvertString(object Mobj)
            {
                bool flag = Mobj == DBNull.Value;
                string result;
                if (flag)
                {
                    result = "";
                }
                else
                {
                    bool flag2 = Mobj == null;
                    if (flag2)
                    {
                        result = "";
                    }
                    else
                    {
                        result = Mobj.ToString().Trim();
                    }
                }
                return result;
            }

            public void SetTextWrapped(string Field, bool IsTextWrapped)
            {
                this.ht_text_wrapped[Field] = IsTextWrapped;
            }
        }

        public enum Colors
        {
            Red = 1,
            Blue,
            Black,
            Yellow,
            LightBlue
        }

        private ArrayList Sheets;

        public ExportExcel()
        {
            this.Sheets = new ArrayList();
        }

        public static int GetHeight(string txt, int max_width, string font_name, int font_size)
        {
            SizeF sizeF = default(SizeF);
            System.Drawing.Font font = new System.Drawing.Font(font_name, (float)font_size);
            PointF origin = new PointF(0f, 0f);
            StringFormat stringFormat = new StringFormat(StringFormat.GenericTypographic);
            Bitmap image = new Bitmap(10, 10, PixelFormat.Format32bppRgb);
            Graphics graphics = Graphics.FromImage(image);
            sizeF = graphics.MeasureString(txt, font, origin, stringFormat);
            checked
            {
                int num = (int)Math.Round(Math.Ceiling((double)(sizeF.Width / (float)max_width)));
                return (int)Math.Round((double)sizeF.Height) * num * 20;
            }
        }

        public static int GetWidth(string txt, string font_name, int font_size)
        {
            SizeF sizeF = default(SizeF);
            System.Drawing.Font font = new System.Drawing.Font(font_name, (float)font_size);
            PointF origin = new PointF(0f, 0f);
            StringFormat stringFormat = new StringFormat(StringFormat.GenericTypographic);
            Bitmap image = new Bitmap(10, 10, PixelFormat.Format32bppRgb);
            Graphics graphics = Graphics.FromImage(image);
            return checked((int)Math.Round(Math.Ceiling((double)(graphics.MeasureString(txt, font, origin, stringFormat).Width / 7f))) * 256);
        }

        private bool IsNumber(Type dt)
        {
            return dt == Type.GetType("System.Byte") | dt == Type.GetType("System.Decimal") | dt == Type.GetType("System.Double") | dt == Type.GetType("System.Int16") | dt == Type.GetType("System.Int32") | dt == Type.GetType("System.Int64") | dt == Type.GetType("System.SByte") | dt == Type.GetType("System.Single") | dt == Type.GetType("System.UInt16") | dt == Type.GetType("System.UInt32") | dt == Type.GetType("System.UInt64");
        }

        private bool IsDecimal(Type dt)
        {
            return dt == Type.GetType("System.Decimal") | dt == Type.GetType("System.Double") | dt == Type.GetType("System.Single");
        }

        private void ExportSheet(ref Workbook EF, ref ExportExcel.ExcelSheet st)
        {
            Worksheet worksheet = EF.Worksheets.Add(st.ReportTitle);
            int num = 0;
            int i = 0;
            bool showHeaderFooter = st.ShowHeaderFooter;
            checked
            {
                if (showHeaderFooter)
                {
                    worksheet.Cells[0, 0].PutValue(st.ReportTitle);
                    worksheet.Cells[0, 0].GetStyle().Font.Size = st.HeaderSize;
                    worksheet.Cells[0, 0].GetStyle().Font.Name = st.FontName;
                    worksheet.Cells[0, 4].PutValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                    worksheet.Cells[0, 4].GetStyle().Font.Size = st.HeaderSize;
                    worksheet.Cells[0, 4].GetStyle().Font.Name = st.FontName;
                    num = st.StartRow - 1;
                }
                else
                {
                    num = 0;
                }
                int num2 = st.ArrFields.Count - 1;
                for (i = 0; i <= num2; i++)
                {
                    ExportExcel.ExcelSheet.FieldInformation fieldInformation = (ExportExcel.ExcelSheet.FieldInformation)st.ArrFields[i];
                    worksheet.Cells.SetColumnWidth(i, (double)fieldInformation.Width);
                    worksheet.Cells[num, i].PutValue(fieldInformation.Title);
                    worksheet.Cells[num, i].GetStyle().Font.Size = st.TitleSize;
                    worksheet.Cells[num, i].GetStyle().Font.Name = st.FontName;
                    worksheet.Cells[num, i].GetStyle().Pattern = Aspose.Cells.BackgroundType.Solid;
                    worksheet.Cells[num, i].GetStyle().ForegroundColor = Color.Yellow;
                    worksheet.Cells[num, i].GetStyle().BackgroundColor = Color.Yellow;

                }
                num++;
                int num3 = num;
                bool flag = st.HatSpecail.Count > 0;
                if (flag)
                {
                    IDictionaryEnumerator enumerator = st.HatSpecail.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        int num4 = Convert.ToInt32(enumerator.Key) - 1;
                        string[] array = (string[])enumerator.Value;
                        int num5 = array.GetUpperBound(1);
                        for (i = 0; i <= num5; i++)
                        {
                            bool flag2 = array[i] != "";
                            if (flag2)
                            {
                                worksheet.Cells[num4, i].PutValue(array[i]);
                                worksheet.Cells[num4, i].GetStyle().Pattern = Aspose.Cells.BackgroundType.Solid;
                                worksheet.Cells[num4, i].GetStyle().ForegroundColor = Color.Yellow;
                                worksheet.Cells[num4, i].GetStyle().BackgroundColor = Color.Yellow;
                                worksheet.Cells[num4, i].GetStyle().HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                            }
                            else
                            {
                                worksheet.Cells[num4, i].GetStyle().Pattern = Aspose.Cells.BackgroundType.Solid;
                                worksheet.Cells[num4, i].GetStyle().ForegroundColor = Color.Yellow;
                                worksheet.Cells[num4, i].GetStyle().BackgroundColor = Color.Yellow;
                                worksheet.Cells[num4, i].GetStyle().HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                            }
                        }
                    }
                }
                int num6 = st.Mdv.Count - 1;
                for (int j = 0; j <= num6; j++)
                {
                    DataRow row = st.Mdv[j].Row;
                    worksheet.Cells.SetRowHeight(num, (double)Convert.ToInt32(st.HatHeight[j]));
                    bool flag3 = st.Mdv.Table.Columns.Contains("bg_color");
                    if (flag3)
                    {
                        string text = this.ConvertString(RuntimeHelpers.GetObjectValue(row["bg_color"]));
                        bool flag4 = text != "";
                        if (flag4)
                        {
                            int num7 = st.ArrFields.Count - 1;
                            for (i = 0; i <= num7; i++)
                            {
                                worksheet.Cells[num, i].GetStyle().BackgroundColor = (this.GetColorType(text));
                            }
                        }
                    }
                    bool flag5 = st.Mdv.Table.Columns.Contains("font_color");
                    if (flag5)
                    {
                        string text2 = this.ConvertString(RuntimeHelpers.GetObjectValue(row["font_color"]));
                        bool flag6 = text2 != "";
                        if (flag6)
                        {
                            int num8 = st.ArrFields.Count - 1;
                            for (i = 0; i <= num8; i++)
                            {
                                worksheet.Cells[num, i].GetStyle().Font.Color = (this.GetColorType(text2));
                            }
                        }
                    }
                    int num9 = st.ArrFields.Count - 1;
                    for (i = 0; i <= num9; i++)
                    {
                        ExportExcel.ExcelSheet.FieldInformation fieldInformation = (ExportExcel.ExcelSheet.FieldInformation)st.ArrFields[i];
                        string fieldName = fieldInformation.FieldName;
                        object objectValue = RuntimeHelpers.GetObjectValue(row[fieldName]);
                        try
                        {
                            Dictionary<string, string>.Enumerator enumerator2 = st.ht_hight_filed_value.GetEnumerator();
                            while (enumerator2.MoveNext())
                            {
                                KeyValuePair<string, string> current = enumerator2.Current;
                                bool flag7 = current.Key.Trim() == fieldName.Trim() & current.Value.Trim() == convert_string(objectValue).Trim();
                                if (flag7)
                                {
                                    worksheet.Cells[num, i].GetStyle().Pattern = Aspose.Cells.BackgroundType.Solid;
                                    worksheet.Cells[num, i].GetStyle().ForegroundColor = Color.Yellow;
                                    worksheet.Cells[num, i].GetStyle().BackgroundColor = Color.Yellow;
                                }
                            }
                        }
                        finally
                        {

                        }
                        worksheet.Cells[num, i].GetStyle().Font.Size = st.ContentSize;
                        worksheet.Cells[num, i].GetStyle().Font.Name = st.FontName;
                        worksheet.Cells[num, i].GetStyle().VerticalAlignment = Aspose.Cells.TextAlignmentType.Center;
                        Type dataType = st.Mdv.Table.Columns[fieldInformation.FieldName].DataType;
                        string format = fieldInformation.Format;
                        bool flag8 = st.ht_text_wrapped.ContainsKey(fieldInformation.FieldName);
                        if (flag8)
                        {
                            worksheet.Cells[num, i].GetStyle().IsTextWrapped = st.ht_text_wrapped[fieldInformation.FieldName];
                        }
                        bool flag9 = format == "";
                        if (flag9)
                        {
                            bool flag10 = this.IsNumber(dataType);
                            if (flag10)
                            {
                                worksheet.Cells[num, i].GetStyle().HorizontalAlignment = Aspose.Cells.TextAlignmentType.Right;
                            }
                            else
                            {
                                worksheet.Cells[num, i].GetStyle().HorizontalAlignment = Aspose.Cells.TextAlignmentType.Left;
                            }
                            worksheet.Cells[num, i].PutValue(RuntimeHelpers.GetObjectValue(objectValue));
                        }
                        else
                        {
                            worksheet.Cells[num, i].GetStyle().Custom = format;
                            worksheet.Cells[num, i].PutValue(objectValue);
                        }
                    }
                    num++;
                    bool flag11 = st.RepeatTitle & j != st.Mdv.Count - 1;
                    if (flag11)
                    {
                        int num10 = st.ArrFields.Count - 1;
                        for (i = 0; i <= num10; i++)
                        {
                            ExportExcel.ExcelSheet.FieldInformation fieldInformation = (ExportExcel.ExcelSheet.FieldInformation)st.ArrFields[i];
                            worksheet.Cells[num, i].PutValue(fieldInformation.Title);
                            worksheet.Cells[num, i].GetStyle().Font.Size = st.TitleSize;
                            worksheet.Cells[num, i].GetStyle().Font.Name = st.FontName;
                            worksheet.Cells[num, i].GetStyle().Pattern = Aspose.Cells.BackgroundType.Solid;
                            worksheet.Cells[num, i].GetStyle().ForegroundColor = Color.LightBlue;
                            worksheet.Cells[num, i].GetStyle().BackgroundColor = Color.LightBlue;
                        }
                        num++;
                    }
                }
                bool flag12 = false;
                int num11 = st.ArrFields.Count - 1;
                for (i = 0; i <= num11; i++)
                {
                    ExportExcel.ExcelSheet.FieldInformation fieldInformation = (ExportExcel.ExcelSheet.FieldInformation)st.ArrFields[i];
                    IDictionaryEnumerator enumerator3 = st.HatColumnFormula.GetEnumerator();
                    while (enumerator3.MoveNext())
                    {
                        bool flag13 = enumerator3.Key.ToString() == fieldInformation.FieldName;
                        if (flag13)
                        {
                            flag12 = true;
                            string formula = string.Concat(new string[]
                            {
                                "=Sum(",
                                this.GetSheetColumnName(i),
                                (num3 + 1).ToString(),
                                ":",
                                this.GetSheetColumnName(i),
                                num.ToString(),
                                ")"
                            });
                            worksheet.Cells[num, i].Formula = formula;
                            worksheet.Cells[num, i].GetStyle().Custom = enumerator3.Value.ToString();
                        }
                    }
                }
                bool flag14 = flag12;
                if (flag14)
                {
                    int num12 = st.ArrFields.Count - 1;
                    for (i = 0; i <= num12; i++)
                    {
                        worksheet.Cells[num, 0].PutValue("总计");
                        worksheet.Cells[num, i].GetStyle().Pattern = Aspose.Cells.BackgroundType.Solid; ;
                        worksheet.Cells[num, i].GetStyle().ForegroundColor = Color.Gray;
                        worksheet.Cells[num, i].GetStyle().BackgroundColor = Color.Gray;
                    }
                    num++;
                }
                bool underLine = st.underLine;
                if (underLine)
                {
                    int num13 = 0;
                    bool showHeaderFooter2 = st.ShowHeaderFooter;
                    if (showHeaderFooter2)
                    {
                        num13 = 3;
                    }
                    int num14 = num13;
                    int num15 = st.Mdv.Count + num13;
                    for (int k = num14; k <= num15; k++)
                    {
                        int num16 = st.ArrFields.Count - 1;
                        for (int l = 0; l <= num16; l++)
                        {

                            worksheet.Cells[k, 1].GetStyle().Borders[Aspose.Cells.BorderType.TopBorder].Color = Color.Black;
                            worksheet.Cells[k, 1].GetStyle().Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                            worksheet.Cells[k, 1].GetStyle().Borders[Aspose.Cells.BorderType.BottomBorder].Color = Color.Black;
                            worksheet.Cells[k, 1].GetStyle().Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                            worksheet.Cells[k, 1].GetStyle().Borders[Aspose.Cells.BorderType.LeftBorder].Color = Color.Black;
                            worksheet.Cells[k, 1].GetStyle().Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                            worksheet.Cells[k, 1].GetStyle().Borders[Aspose.Cells.BorderType.RightBorder].Color = Color.Black;
                            worksheet.Cells[k, 1].GetStyle().Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                        }
                    }
                }
                bool showHeaderFooter3 = st.ShowHeaderFooter;
                if (showHeaderFooter3)
                {
                }
                try
                {
                    List<string>.Enumerator enumerator4 = st.list_meger.GetEnumerator();
                    while (enumerator4.MoveNext())
                    {
                        string current2 = enumerator4.Current;
                        Array array2 = current2.Split(",".ToCharArray());
                        worksheet.Cells.Merge(Convert.ToInt32(array2.GetValue(0)), Convert.ToInt32(array2.GetValue(1)), Convert.ToInt32(array2.GetValue(2)), Convert.ToInt32(array2.GetValue(3)));
                    }
                }
                finally
                {

                }
            }
        }

        public void AddSheet(ExportExcel.ExcelSheet st)
        {
            this.Sheets.Add(st);
        }

        public void Export(string ExcelFilePath)
        {
            Workbook workbook = new Workbook();
            workbook.RemoveExternalLinks();
            workbook.Worksheets.Clear();
            checked
            {
                int num = this.Sheets.Count - 1;
                for (int i = 0; i <= num; i++)
                {
                    ExportExcel.ExcelSheet excelSheet = (ExportExcel.ExcelSheet)this.Sheets[i];
                    excelSheet.MeasureHeightAndWidth();
                    this.ExportSheet(ref workbook, ref excelSheet);
                }
                workbook.Save(ExcelFilePath,  SaveFormat.Excel97To2003);
            }
        }

        private string ConvertString(object Mobj)
        {
            bool flag = Mobj== DBNull.Value;
            string result;
            if (flag)
            {
                result = "";
            }
            else
            {
                bool flag2 = Mobj == null;
                if (flag2)
                {
                    result = "";
                }
                else
                {
                    result = Mobj.ToString().Trim();
                }
            }
            return result;
        }

        private Color GetColorType(string color)
        {
            Color result= Color.White;
            if (color== "Red")
            {
                if (color == "Blue")
                {
                    if (color == "Black")
                    {
                        if (color == "Yellow")
                        {
                            if (color == "LightBlue")
                            {
                                result = Color.LightBlue;
                            }
                        }
                        else
                        {
                            result = Color.Yellow;
                        }
                    }
                    else
                    {
                        result = Color.Black;
                    }
                }
                else
                {
                    result = Color.Blue;
                }
            }
            else
            {
                result = Color.Red;
            }
            return result;
        }

        private string GetSheetColumnName(int i)
        {
            bool flag = i > 26;
            checked
            {
                string result;
                if (flag)
                {
                    int num = (int)Math.Round(unchecked((double)i / 26.0 - 1.0));
                    i %= 26;
                    result = Convert.ToChar(65 + num).ToString() + Convert.ToChar(65 + i).ToString();
                }
                else
                {
                    result = Convert.ToChar(65 + i).ToString ();
                }
                return result;
            }
        }

        public string convert_string(object Mobj_str)
        {

            string result;
            if (Mobj_str == DBNull.Value)
            {
                return "";
            }
            else
            {
                if (Mobj_str == null || Mobj_str.ToString().Trim().Equals(""))
                {
                    result = "";
                }
                else
                {
                    result = Change_string(Mobj_str.ToString()).Trim();
                }
            }
            return result;
        }

        private string Change_string(string str_data)
        {
            str_data = str_data.Replace("&nbsp;", "");
            str_data = str_data.Replace("&lt;BR&gt;", "");
            str_data = str_data.Replace("&lt;P&gt;", "");
            str_data = str_data.Replace("&lt;", "");
            str_data = str_data.Replace("&gt;", "");
            str_data = str_data.Replace("<br>", "");
            str_data = str_data.Replace("&quot;", "");
            str_data = str_data.Replace("&amp;", "");
            str_data = str_data.Replace("\t", "");
            str_data = str_data.Replace("\r", "");
            str_data = str_data.Replace("\r\n", "");
            str_data = str_data.Replace("\n", "");
            str_data = str_data.Replace("&", "＆");
            str_data = str_data.Replace("\"", "'");
            str_data = str_data.Replace("&yen", "￥");
            str_data = str_data.Replace("&#160;", " ");
            return str_data;
        }
    }
}
