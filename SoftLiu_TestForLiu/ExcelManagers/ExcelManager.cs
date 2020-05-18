using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_TestForLiu.ExcelManagers
{
    public class ExcelManager
    {
        public static DataTable ReadExcelToDataTable(string path, string sheet)
        {
            //此连接只能操作Excel2007之前(.xls)文件
            //string connstring = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + path + ";" + "Extended Properties=Excel 8.0;";
            string sheetName = sheet + "$";
            //此连接可以操作.xls与.xlsx文件
            string connstring = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + path + ";Extended Properties='Excel 12.0; HDR=NO; IMEX=1'";
            using (OleDbConnection conn = new OleDbConnection(connstring))
            {
                conn.Open();
                DataTable sheetsName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" }); //得到所有sheet的名字
                //string firstSheetName = sheetsName.Rows[0][2].ToString(); //得到第一个sheet的名字
                string sql = string.Format("SELECT * FROM [{0}]", sheetName); //查询字符串
                //string sql = string.Format("SELECT * FROM [{0}] WHERE [日期] is not null", firstSheetName); //查询字符串

                OleDbDataAdapter odda = new OleDbDataAdapter(sql, conn);
                DataSet set = new DataSet();
                odda.Fill(set, sheetName);
                DataTable dt = set.Tables[0];
                //dt.WriteXml(@"C:\Users\hlsun\Desktop\excel.xml");

                //WriteDataTableToExcel(dt, string.Empty, string.Empty);
                return dt;
            }
        }
    }
}
