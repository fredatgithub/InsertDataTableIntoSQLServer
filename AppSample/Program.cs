using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace AppSample
{
  class Program
  {
    static void Main(string[] args)
    {
      ReadFile(@"\Countries.txt");
    }

    public static void ReadFile(string fileName)
    {
      try
      {
        DataTable table = new DataTable();
        table = GetTable();
        using (StreamReader sr = new StreamReader(Environment.CurrentDirectory + fileName))
        {
          string line;
          int i = 1;
          while ((line = sr.ReadLine()) != null)
          {
            table.Rows.Add(i, line);
            Console.WriteLine(line);
            i++;
          }
        }
        //Insert datatable to sql Server
        Insert(table, "InsertCountries", "@dtCountry");
      }
      catch (Exception exception)
      {
        // Let the user know what went wrong.
        Console.WriteLine("The file could not be read:");
        Console.WriteLine(exception.Message);
      }

      Console.WriteLine("Press any key to exit:");
      Console.ReadKey();
    }

    /// <summary>This example method generates a DataTable.</summary>
    static DataTable GetTable()
    {
      DataTable table = new DataTable();
      table.Columns.Add("idCountry", typeof(short));
      table.Columns.Add("name", typeof(string));
      return table;
    }

    static void Insert(DataTable dtData, string storedProcedureName, string dataTableName)
    {
      SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-MSI;Initial Catalog=Countries;Integrated Security=True");
      SqlCommand cmd = new SqlCommand(storedProcedureName, con);
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.AddWithValue(dataTableName, dtData);
      cmd.Connection = con;
      try
      {
        con.Open();
        cmd.ExecuteNonQuery();
        Console.WriteLine("Records inserted successfully!");
      }
      catch (Exception exception)
      {
        Console.WriteLine($"exception : {exception.Message}");
      }
      finally
      {
        con.Close();
        con.Dispose();
      }
    }
  }
}
