using Cloud.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Cloud.Utilities
{
    public static class ExcelUtility
    {
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="dataTable">数据源</param>
        /// <param name="heading">工作簿Worksheet</param>
        /// <param name="showSrNo">//是否显示行编号</param>
        /// <param name="columnsToTake">要导出的列</param>
        /// <returns></returns>
        public static byte[] ExportExcel(DataTable dataTable, string heading = "", bool showSrNo = true, params string[] columnsToTake)
        {
            byte[] result = null;
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(string.Format("{0}Data", heading));
                int startRowFrom = string.IsNullOrEmpty(heading) ? 1 : 3; //开始的行
                                                                          //是否显示行编号
                if (showSrNo)
                {
                    DataColumn dataColumn = dataTable.Columns.Add("行号", typeof(int));
                    dataColumn.SetOrdinal(0);
                    int index = 1;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        item[0] = index;
                        index++;
                    }
                }
                //Add Content Into the Excel File
                workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true, TableStyles.Medium9);
                // autofit width of cells with small content 
                int columnIndex = 1;
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex];
                    int maxLength = columnCells.Max(cell => cell.Value == null ? 0 : cell.Value.ToString().Count());
                    if (maxLength < 150)
                    {
                        workSheet.Column(columnIndex).AutoFit();
                    }
                    columnIndex++;


                    if ((dataTable.Columns[i].DataType).FullName == "System.DateTime" && (dataTable.Columns[i].DataType).Name == "DateTime")
                    {
                        workSheet.Column(i + 1).Style.Numberformat.Format = "yyyy-mm-dd h:mm";
                        workSheet.Column(i + 1).Width = 25;
                    }

                }
                // format header - bold, yellow on black 
                using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFrom, dataTable.Columns.Count])
                {
                    r.Style.Font.Color.SetColor(Color.White);
                    r.Style.Font.Bold = true;
                    r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#1fb5ad"));
                }
                if (dataTable.Rows.Count > 0)
                {
                    // format cells - add borders 
                    using (ExcelRange r = workSheet.Cells[startRowFrom + 1, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
                    {
                        r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        r.Style.Border.Top.Color.SetColor(Color.Black);
                        r.Style.Border.Bottom.Color.SetColor(Color.Black);
                        r.Style.Border.Left.Color.SetColor(Color.Black);
                        r.Style.Border.Right.Color.SetColor(Color.Black);

                        r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;//水平居中
                    }
                }
                // removed ignored columns 
                if (columnsToTake != null)
                {
                    for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
                    {
                        if (i == 0 && showSrNo)
                        {
                            continue;
                        }
                        if (!columnsToTake.Contains(dataTable.Columns[i].ColumnName))
                        {
                            workSheet.DeleteColumn(i + 1);
                        }
                    }
                }
                if (!String.IsNullOrEmpty(heading))
                {
                    workSheet.Cells["A1"].Value = heading;
                    workSheet.Cells["A1"].Style.Font.Size = 20;

                    workSheet.InsertColumn(1, 1);
                    workSheet.InsertRow(1, 1);
                    workSheet.Column(1).Width = 5;
                }
                result = package.GetAsByteArray();
            }
            return result;
        }
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="heading"></param>
        /// <param name="isShowSlNo"></param>
        /// <param name="ColumnsToTake"></param>
        /// <returns></returns>
        public static byte[] DumpExcelWithColumn(DataTable dt, string heading = "", bool isShowSlNo = true, params string[] ColumnsToTake)
        {
            return ExportExcel(dt, heading, isShowSlNo, ColumnsToTake);
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="heading"></param>
        /// <param name="isShowSlNo"></param>
        /// <returns></returns>
        public static byte[] DumpExcel(DataTable dt, string heading = "", bool isShowSlNo = true)
        {
            return ExportExcel(dt, heading, isShowSlNo, null);
        }

        /// <summary>
        /// 从Excel中加载数据（泛型）
        /// </summary>
        /// <typeparam name="T">每行数据的类型</typeparam>
        /// <param name="stream">Excel流</param>
        /// <returns>泛型列表</returns>
        public static IEnumerable<T> LoadFromExcel<T>(Stream stream) where T : new()
        {
            List<T> resultList = new List<T>();
            Dictionary<string, int> dictHeader = new Dictionary<string, int>();
            using var package = new ExcelPackage(stream);
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            int colStart = worksheet.Dimension.Start.Column;  //工作区开始列
            int colEnd = worksheet.Dimension.End.Column;       //工作区结束列
            int rowStart = worksheet.Dimension.Start.Row;       //工作区开始行号
            int rowEnd = worksheet.Dimension.End.Row;       //工作区结束行号                                                          //将每列标题添加到字典中
            for (int i = colStart; i < colEnd + 1; i++)
            {
                dictHeader[worksheet.Cells[rowStart, i].Value.ToString()] = i;
            }
            List<PropertyInfo> propertyInfoList = new List<PropertyInfo>(typeof(T).GetProperties());
            for (int row = rowStart + 1; row < rowEnd + 1; row++)
            {
                T result = new T();
                //为对象T的各属性赋值
                foreach (PropertyInfo p in propertyInfoList)
                {
                    try
                    {
                        ExcelRange cell = worksheet.Cells[row, dictHeader[p.Name]]; //与属性名对应的单元格
                        if (cell.Value == null)
                            continue;
                        try
                        {
                            var colType = p.PropertyType;
                            if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                            {
                                colType = colType.GetGenericArguments()[0];
                            }
                            var getValue = cell.GetType().GetMethod("GetValue").MakeGenericMethod(new Type[] { colType });
                            p.SetValue(result, getValue.Invoke(cell, null));
                            //switch (p.PropertyType.Name.ToLower())
                            //{
                            //    case "string":
                            //        p.SetValue(result, cell.GetValue<String>());
                            //        break;
                            //    case "int16":
                            //p.SetValue(result, cell.GetValue<Int16>());
                            //        break;
                            //    case "int32":
                            //        p.SetValue(result, cell.GetValue<Int32>());
                            //        break;
                            //    case "int64":
                            //        p.SetValue(result, cell.GetValue<Int64>());
                            //        break;
                            //    case "decimal":
                            //        p.SetValue(result, cell.GetValue<Decimal>());
                            //        break;
                            //    case "double":
                            //        p.SetValue(result, cell.GetValue<Double>());
                            //        break;
                            //    case "datetime":
                            //        p.SetValue(result, cell.GetValue<DateTime>());
                            //        break;
                            //    case "boolean":
                            //        p.SetValue(result, cell.GetValue<Boolean>());
                            //        break;
                            //    case "byte":
                            //        p.SetValue(result, cell.GetValue<Byte>());
                            //        break;
                            //    case "char":
                            //        p.SetValue(result, cell.GetValue<Char>());
                            //        break;
                            //    case "single":
                            //        p.SetValue(result, cell.GetValue<Single>());
                            //        break;
                            //    default:
                            //        break;
                            //}
                        }
                        catch (Exception ex)
                        {
                            throw new MyException($"字段【{p.Name}】格式不正确！",0);
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
                resultList.Add(result);
            }
            return resultList;
        }
    }
}
