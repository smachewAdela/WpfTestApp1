using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTestApp1
{
    public class ExcelUtlity
    {
        public static void CreateExcelDocument(ExcelParameters parameters, Stream stream)
        {
            //..Encoding.GetEncoding(862)
            // latinEncoding  Encoding.GetEncoding(1252)
            // hebrew  Encoding.GetEncoding(1255)
            using (var sWriter = new StreamWriter(stream, Encoding.GetEncoding(1255)))
            {
                //sets font
                WriteFileHeaders(sWriter);
                WriteMainHeaders(parameters, sWriter);
                foreach (var p in parameters.TablesParameters)
                {
                    DataTable table = p.table;

                    WriteTopTitles(sWriter, p);
                    WriteTableColumnsHeaders(sWriter, table);
                    WriteTableValues(sWriter, p, table);
                    WriteBottomTitles(sWriter, p);

                    sWriter.Write("<TR><td></td></TR>");
                    sWriter.Write("<TR><td></td></TR>");
                    sWriter.Write("</Table></font>");
                }
            }
        }

        private static void WriteFileHeaders(StreamWriter sWriter)
        {
            sWriter.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=windows-1255\">");
        }

        private static void WriteTableValues(StreamWriter sWriter, ExcelTableParameters p, DataTable table)
        {
            int rowsCount = 0;
            foreach (DataRow row in table.Rows)
            {
                //write in new row
                var highLight = p.RowsToHighlight.Contains(rowsCount);
                rowsCount++;
                bool lastRow = p.LastRowAsTotalRow && rowsCount == table.Rows.Count;

                sWriter.Write("<TR>");
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    if (lastRow || highLight)
                    {
                        if (lastRow)
                            sWriter.Write(string.Format("<Td style=\"background:#E1E1E1;font-weight:bold;\">{0}</Td>", row[i].ToString()));
                        else
                            sWriter.Write(string.Format("<Td style=\"background:#262626;font-weight:bold; color:white;\">{0}</Td>", row[i].ToString()));
                    }
                    else
                        sWriter.Write(string.Format("<Td>{0}</Td>", row[i].ToString()));
                }
                sWriter.Write("</TR>");
            }
        }

        private static void WriteTableColumnsHeaders(StreamWriter sWriter, DataTable table)
        {
            foreach (DataColumn column in table.Columns) // headers
            {
                sWriter.Write("<Td  style=\"background:#BDD7EE; font-size:16px;\"><B><center>");
                sWriter.Write(column.ColumnName);
                sWriter.Write("</center></B></Td>");
            }
            sWriter.Write("</TR>");
        }

        private static void WriteTopTitles(StreamWriter sWriter, ExcelTableParameters p)
        {
            sWriter.Write("<Table  dir=\"rtl\" bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:14px; font-family:Calibri; background:#ffffff;'>");
            // header rows
            if (p.TopTitles != null && p.TopTitles.Keys.Count > 0)
            {
                foreach (var titleRow in p.TopTitles)
                {
                    if (!titleRow.Value)
                        sWriter.Write(string.Format("<TR  border='1'><Td colspan=\"{1}\" style=\"font-size:18px;background:#458AC9;font-weight:bold; color:white;\">{0}</Td></TR>", titleRow.Key, p.ColumnsCount));
                    else
                        sWriter.Write(string.Format("<TR  border='1'><Td colspan=\"{1}\" style=\"font-size:18px;background:#458AC9;font-weight:bold; color:white;\"><center>{0}</center></Td></TR>", titleRow.Key, p.ColumnsCount));
                }
            }
        }

        private static void WriteBottomTitles(StreamWriter sWriter, ExcelTableParameters p)
        {
            if (p.BottomTitles != null && p.BottomTitles.Keys.Count > 0)
            {
                foreach (var titleRow in p.BottomTitles)
                {
                    if (!titleRow.Value)
                        sWriter.Write(string.Format("<TR  border='1'><Td colspan=\"{1}\" style=\"font-size:18px;\">{0}</Td></TR>", titleRow.Key, p.ColumnsCount));
                    else
                        sWriter.Write(string.Format("<TR  border='1'><Td colspan=\"{1}\" style=\"font-size:18px;\"><center>{0}</center></Td></TR>", titleRow.Key, p.ColumnsCount));
                }
            }
        }

        private static void WriteMainHeaders(ExcelParameters parameters, StreamWriter sWriter)
        {
            sWriter.Write("<font style='font-size:10.0pt; font-family:Calibri;'><BR><BR><BR>");
            if (parameters.MainHeaders.Count > 0)
            {
                sWriter.Write("<Table>");
                foreach (var pHeader in parameters.MainHeaders)
                {
                    sWriter.Write(string.Format("<TR style=\"font-size:18px;background:#BDD7EE;width:450px;\"><td colspan=\"10\"><center>{0}</center></td></TR>", pHeader));
                }
                sWriter.Write("<TR><td colspan=\"10\"></td></TR>");
                sWriter.Write("<TR><td colspan=\"10\"></td></TR>");
                sWriter.Write("</Table>");
            }
        }

    
    }

    public class ExcelParameters
    {
        public List<ExcelTableParameters> TablesParameters { get; set; }
        public string FileName { get; set; }
        public List<string> MainHeaders { get; set; }

        public ExcelParameters()
        {
            TablesParameters = new List<ExcelTableParameters>();
            MainHeaders = new List<string>();
        }

        public ExcelTableParameters GetNewtableParameter()
        {
            var t = new ExcelTableParameters();
            TablesParameters.Add(t);
            return t;
        }
    }

    public class ExcelTableParameters
    {
        public DataTable table { get; set; }
        public Dictionary<string, bool> TopTitles { get; set; }
        public int ColumnsCount
        {
            get
            {
                return table.Columns.Count;
            }
        }

        public Dictionary<string, bool> BottomTitles { get; set; }
        public bool LastRowAsTotalRow { get; set; }
        public List<int> RowsToHighlight { get; set; }

        public ExcelTableParameters()
        {
            table = new DataTable();
            TopTitles = new Dictionary<string, bool>();
            BottomTitles = new Dictionary<string, bool>();
            RowsToHighlight = new List<int>();
        }
    }

}
