using System.Data;
using System.Data.Common;
using DfT.DTRO.DAL;

namespace DfT.DTRO.Extensions.Configuration;

public class DtroScriptGenerator
{
    private readonly DtroContext _context;

    public DtroScriptGenerator(DtroContext context)
    {
        _context = context;
    }


    public void RunScript(string sqlQuery)
    {
        try
        {
            Console.WriteLine(DateTime.UtcNow);

            var connection = _context.Database.GetDbConnection();
            DbProviderFactory factory = DbProviderFactories.GetFactory(connection.ConnectionString);

            using (var cmd = factory.CreateCommand())
            {
                cmd.CommandText = sqlQuery;
                cmd.CommandType = CommandType.Text;
                _context.Database.OpenConnection();
                using (var adapter = factory.CreateDataAdapter())
                {
                    adapter.SelectCommand = cmd;

                    var tb = new DataTable();
                    adapter.Fill(tb);
                    Console.WriteLine(tb.TableName);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(DateTime.UtcNow);
            Console.WriteLine($"Error occurred during SQL query execution {sqlQuery}", ex);
            Console.WriteLine(DateTime.UtcNow);
        }
    }
}