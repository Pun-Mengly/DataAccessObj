using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace DataAccessObj
{
    public class DataAccess<T> where T:class
    {
        public static async Task<List<T>> CallToSqlServerAsync(string conStr, string query)
        {
            List<T> result = new();
            SqlConnection con = new();
            SqlDataAdapter da = new();
            DataTable dt = new();
            SqlCommand cmd = new();
            try
            {
                dt = new DataTable();
                con = new SqlConnection(conStr);
                await con.OpenAsync();

                cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                string jsonString = JsonConvert.SerializeObject(dt, Formatting.Indented);
                result = JsonConvert.DeserializeObject<List<T>>(jsonString)!;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                await con.CloseAsync();
                await con.DisposeAsync();
                await cmd.DisposeAsync();
                da.Dispose();
            }
            return result;
        }
    }
}