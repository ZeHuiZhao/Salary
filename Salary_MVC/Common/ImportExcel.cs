using Aspose.Cells;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;

namespace Salary_MVC
{
    public class ImportExcel
    {
        //public static DataSet GetExcelData0(string mStrPath)
        //{
        //    DataSet dataSet = new DataSet();

        //    IEnumerator enumerator = ImportExcel.OpenWorkbook(mStrPath).GetEnumerator();
        //    while (enumerator.MoveNext())
        //    {
        //        Worksheet worksheet = (Worksheet)enumerator.Current;
        //        string name = worksheet.Name;
        //        Cells cells = worksheet.Cells;
        //        DataTable dataTable = new DataTable(name);
        //        int maxDataColumn = cells.MaxDataColumn;
        //        for (int i = 0; i <= maxDataColumn; i++)
        //        {
        //            DataColumn column = new DataColumn(cells[3, i].StringValue);
        //            dataTable.Columns.Add(column);
        //        }
        //        int num = cells.MaxDataRow - 1;
        //        for (int j = 0; j <= num; j++)
        //        {
        //            DataRow dataRow = dataTable.NewRow();
        //            int maxDataColumn2 = cells.MaxDataColumn;
        //            for (int k = 0; k <= maxDataColumn2; k++)
        //            {
        //                dataRow[k] = cells[j + 1 + 3, k].StringValue;
        //            }
        //            dataTable.Rows.Add(dataRow);
        //        }
        //        dataSet.Tables.Add(dataTable);
        //    }
        //    return dataSet;
        //}

        public static DataSet GetExcelData(string mStrPath)
        {
            DataSet dataSet = new DataSet();

            IEnumerator enumerator = ImportExcel.OpenWorkbook(mStrPath).GetEnumerator();
            while (enumerator.MoveNext())
            {
                Worksheet worksheet = (Worksheet)enumerator.Current;
                string name = worksheet.Name;
                Cells cells = worksheet.Cells;
                DataTable dataTable = new DataTable(name);
                int maxDataColumn = cells.MaxDataColumn;
                for (int i = 0; i <= maxDataColumn; i++)
                {
                    DataColumn column = new DataColumn(cells[0, i].StringValue);
                    dataTable.Columns.Add(column);
                }
                int num = cells.MaxDataRow - 1;
                for (int j = 0; j <= num; j++)
                {
                    DataRow dataRow = dataTable.NewRow();
                    int maxDataColumn2 = cells.MaxDataColumn;
                    for (int k = 0; k <= maxDataColumn2; k++)
                    {
                        dataRow[k] = cells[j + 1 + 0, k].StringValue;
                    }
                    dataTable.Rows.Add(dataRow);
                }
                dataSet.Tables.Add(dataTable);
            }
            return dataSet;
        }

        //public static DataSet GetExcelDataNoTitle(string mStrPath)
        //{
        //    DataSet dataSet = new DataSet();


        //    IEnumerator enumerator = ImportExcel.OpenWorkbook(mStrPath).GetEnumerator();
        //    while (enumerator.MoveNext())
        //    {
        //        Worksheet worksheet = (Worksheet)enumerator.Current;
        //        string name = worksheet.Name;
        //        Cells cells = worksheet.Cells;
        //        DataTable dataTable = new DataTable(name);
        //        int maxDataColumn = cells.MaxDataColumn;
        //        for (int i = 0; i <= maxDataColumn; i++)
        //        {
        //            DataColumn column = new DataColumn((i + 1).ToString());
        //            dataTable.Columns.Add(column);
        //        }
        //        int num = cells.MaxDataRow - 1;
        //        for (int j = 0; j <= num; j++)
        //        {
        //            DataRow dataRow = dataTable.NewRow();
        //            int maxDataColumn2 = cells.MaxDataColumn;
        //            for (int k = 0; k <= maxDataColumn2; k++)
        //            {
        //                dataRow[k] = cells[j + 1, k].StringValue;
        //            }
        //            dataTable.Rows.Add(dataRow);
        //        }
        //        dataSet.Tables.Add(dataTable);
        //    }


        //    return dataSet;
        //}

        public static WorksheetCollection OpenWorkbook(string mStrPath)
        {
            try
            {    
                Aspose.Cells.Workbook w = new Workbook(mStrPath);
                return w.Worksheets;
            }
            catch
            {
                return null;
            }
        }

        public static DataSet GetExcelDataNoTitle(string fullFilePath,int startrowIndex)
        {
            System.Data.DataSet dataset = new DataSet();
            var book = OpenWorkbook(fullFilePath);
            foreach (var sheet in book)
            {
                DataTable table = new DataTable();
                for (int i = 0; i <= sheet.Cells.MaxDataColumn; i++)
                {
                    table.Columns.Add(new DataColumn("C"+i));
                }
                for (int i = startrowIndex; i <= sheet.Cells.MaxDataRow; i++)
                {
                    DataRow row = table.NewRow();
                    for (int j = 0; j <= sheet.Cells.MaxDataColumn; j++)
                    {
                        row[j] = sheet.Cells[i, j].StringValue;
                    }
                    table.Rows.Add(row);
                }
                dataset.Tables.Add(table);
            }
            return dataset;
        }
    }


    //public static WorksheetCollection OpenWorkbook1(string mStrPath)
    //{
   
    //}

    //public static string CheckExcelFormat(string mStrFilename, string mStrLanguage = "EN")
    //{
    //    string text = "";
    //    if (Operators.CompareString(mStrLanguage, "EN", false) != 0)
    //    {
    //        if (Operators.CompareString(mStrLanguage, "CN", false) == 0)
    //        {
    //            text = "插入失败！文件格式错误，应该是Excel文件，请选择正确的文件格式。";
    //        }
    //    }
    //    else
    //    {
    //        text = "Import fail, the file format invalid,it should be excel file,please select the right file format.";
    //    }
    //    bool flag = Strings.Right(mStrFilename, 4).Equals(".xls") | Strings.Right(mStrFilename, 5).Equals(".xlsx");
    //    string result;
    //    if (flag)
    //    {
    //        result = "";
    //    }
    //    else
    //    {
    //        result = text;
    //    }
    //    return result;
    //}

    //public static string CheckColumns(string mStrPath, int mIntCount, int mIntBookNumber = 0, string mStrLanguage = "EN")
    //{
    //    string text = "";
    //    if (Operators.CompareString(mStrLanguage, "EN", false) != 0)
    //    {
    //        if (Operators.CompareString(mStrLanguage, "CN", false) == 0)
    //        {
    //            text = "插入失败！表格至少必须有 " + Conversions.ToString(mIntCount) + " 列，请检查。";
    //        }
    //    }
    //    else
    //    {
    //        text = "Import fail, there are at least " + Conversions.ToString(mIntCount) + " columns in the excel file, please check it.";
    //    }
    //    bool flag = ImportExcel.OpenWorkbook(mStrPath).get_Item(mIntBookNumber).get_Cells().get_MaxDataColumn() >= checked(mIntCount - 1);
    //    string result;
    //    if (flag)
    //    {
    //        result = "";
    //    }
    //    else
    //    {
    //        result = text;
    //    }
    //    return result;
    //}

    //public static string CheckRows(string mStrPath, int mIntCount, int mIntBookNumber = 0, string mStrLanguage = "EN")
    //{
    //    string text = "";
    //    if (Operators.CompareString(mStrLanguage, "EN", false) != 0)
    //    {
    //        if (Operators.CompareString(mStrLanguage, "CN", false) == 0)
    //        {
    //            text = "插入失败！表格中不存在需要插入的行，请检查。";
    //        }
    //    }
    //    else
    //    {
    //        text = "Import fail, there are no items need to be import, please check it.";
    //    }
    //    bool flag = ImportExcel.OpenWorkbook(mStrPath).get_Item(mIntBookNumber).get_Cells().get_MaxDataRow() >= checked(mIntCount - 1);
    //    string result;
    //    if (flag)
    //    {
    //        result = "";
    //    }
    //    else
    //    {
    //        result = text;
    //    }
    //    return result;
    //}

    //public static string CheckExcelDataFormat(string mStrPath, Hashtable mHashTypeList, int mIntBookNumber = 0, string mStrLanguage = "EN")
    //{
    //    Worksheet worksheet = ImportExcel.OpenWorkbook(mStrPath).get_Item(mIntBookNumber);
    //    ArrayList arrayList = new ArrayList();
    //    int maxDataColumn = worksheet.get_Cells().get_MaxDataColumn();
    //    checked
    //    {
    //        for (int i = 0; i <= maxDataColumn; i++)
    //        {
    //            arrayList.Add(worksheet.get_Cells().get_Item(0, i).get_StringValue());
    //        }
    //        int num = worksheet.get_Cells().get_MaxDataRow() - 1;
    //        string result;
    //        for (int j = 0; j <= num; j++)
    //        {
    //            int maxDataColumn2 = worksheet.get_Cells().get_MaxDataColumn();
    //            for (int k = 0; k <= maxDataColumn2; k++)
    //            {
    //                string text = ImportExcel.CheckType(Conversions.ToString(mHashTypeList[RuntimeHelpers.GetObjectValue(arrayList[k])]), worksheet.get_Cells().get_Item(j + 1, k).get_StringValue());
    //                bool flag = !text.Equals("");
    //                if (flag)
    //                {
    //                    result = Conversions.ToString(unchecked(Conversions.ToDouble(text + " in Row ") + (double)(checked(j + 2)) + Conversions.ToDouble(",") + (double)(checked(k + 1))));
    //                    return result;
    //                }
    //            }
    //        }
    //        result = "";
    //        return result;
    //    }
    //}

    //public static string CheckType(string type, string value)
    //{
    //    string result;
    //    if (Operators.CompareString(type, "Integer", false) != 0)
    //    {
    //        if (Operators.CompareString(type, "String", false) != 0)
    //        {
    //            if (Operators.CompareString(type, "Decimal", false) != 0)
    //            {
    //                if (Operators.CompareString(type, "Double", false) != 0)
    //                {
    //                    if (Operators.CompareString(type, "Boolean", false) != 0)
    //                    {
    //                        if (Operators.CompareString(type, "Date", false) == 0)
    //                        {
    //                            try
    //                            {
    //                                Convert.ToDateTime(value);
    //                            }
    //                            catch (Exception expr_132)
    //                            {
    //                                ProjectData.SetProjectError(expr_132);
    //                                result = "Date Error";
    //                                ProjectData.ClearProjectError();
    //                                return result;
    //                            }
    //                        }
    //                    }
    //                    else
    //                    {
    //                        try
    //                        {
    //                            comm_fun.convert_to_boolean(value);
    //                        }
    //                        catch (Exception expr_10E)
    //                        {
    //                            ProjectData.SetProjectError(expr_10E);
    //                            result = "Boolean Error";
    //                            ProjectData.ClearProjectError();
    //                            return result;
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    try
    //                    {
    //                        Convert.ToDouble(value);
    //                    }
    //                    catch (Exception expr_EA)
    //                    {
    //                        ProjectData.SetProjectError(expr_EA);
    //                        result = "Double Error";
    //                        ProjectData.ClearProjectError();
    //                        return result;
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                try
    //                {
    //                    Convert.ToDecimal(value);
    //                }
    //                catch (Exception expr_C6)
    //                {
    //                    ProjectData.SetProjectError(expr_C6);
    //                    result = "Decimal Error";
    //                    ProjectData.ClearProjectError();
    //                    return result;
    //                }
    //            }
    //        }
    //        else
    //        {
    //            try
    //            {
    //                Convert.ToString(value);
    //            }
    //            catch (Exception expr_9D)
    //            {
    //                ProjectData.SetProjectError(expr_9D);
    //                result = "String Error";
    //                ProjectData.ClearProjectError();
    //                return result;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        try
    //        {
    //            Convert.ToInt32(value);
    //        }
    //        catch (Exception expr_74)
    //        {
    //            ProjectData.SetProjectError(expr_74);
    //            result = "Integer Error";
    //            ProjectData.ClearProjectError();
    //            return result;
    //        }
    //    }
    //    result = "";
    //    return result;
    //}

}
