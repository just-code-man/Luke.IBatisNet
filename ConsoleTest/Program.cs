using Luke.IBatisNet.DataMapper;
using Luke.IBatisNet.DataMapper.Configuration;
using System.Data;
using System.Threading.Channels;

namespace ConsoleTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DomSqlMapBuilder builder = new DomSqlMapBuilder();
            ISqlMapper mapper = builder.Configure("SqlMap.config");

            DataTable dt = mapper.QueryForDataTable("TestSql", "xxxx");

            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    Console.WriteLine(row[col.ColumnName].ToString());
                }
            }

            Console.ReadLine();
        }
    }
}
