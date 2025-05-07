using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;

namespace Exportar_Pedidos_Culiacán
{
    public class GetFireBirdValue
    {
        public string GetValue(string query)
        {
            FbConnection con = new FbConnection(GlobalSettings.Instance.StringConnection);
            try
            {
                con.Open();
                FbCommand command = new FbCommand(query, con);
                FbDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return reader.GetString(0);
                }
                else
                {
                    return null;
                }

            }
            catch
            {
                MessageBox.Show("Se perdió la conexión :( , contacta a 06 o intenta de nuevo", "¡Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                con.Close();
            }
        }
        public List<List<string>> GetList(string query)
        {
            List<List<string>> list = new List<List<string>>();
            FbConnection con = new FbConnection(GlobalSettings.Instance.StringConnection);
            try
            {
                con.Open();
                FbCommand command = new FbCommand(query, con);
                FbDataReader reader = command.ExecuteReader();
                int contador = 0;
                while (reader.Read())
                {
                    List<string> row = new List<string>();
                    row.Add(contador.ToString());
                    row.Add(reader.GetString(0));
                    row.Add(reader.GetString(1));
                    row.Add(reader.GetString(2));
                    row.Add(reader.GetString(3));
                    row.Add(reader.GetString(4));
                    row.Add(reader.GetString(5));
                    row.Add(reader.GetString(6));
                    list.Add(row);
                    contador++;
                }
                return list;
            }
            catch
            {
                MessageBox.Show("Se perdió la conexión :( , contacta a 06 o intenta de nuevo", "¡Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                con.Close();
            }
        }
    }
}
