using Countries.Modelos;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Countries.Servicos
{
    public class DataService
    {
        //coneção BD
        private SQLiteConnection connection;
        //Comados para BD
        private SQLiteCommand command;
        //Dialogo com utilizador 
        private DialogService dialogService;

        public DataService()
        {
            dialogService = new DialogService();

            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
            }
            var path = @"Data\Countries.sqlite";

            try
            { 
                //abre Base de dados ou cria se não existir
                connection = new SQLiteConnection("Data Source =" + path);
                connection.Open();

                //Cria tabela dos paises
                string sqlcommand = "Create table if not exists Country (name varchar(100) PRIMARY KEY, alpha2Code Nchar(3), alpha3Code Nchar(3), capital varchar(50), region varchar(50), subregion varchar(50), population int, demonym varchar(50), area varchar(50), gini varchar(50), nativeName varchar(50), numericCode varchar(20), flag varchar(100), cioc varchar(20))";

                command = new SQLiteCommand(sqlcommand, connection);

                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Erro", e.Message);
            }
        }

        public List<Country> GetData()
        {
            List<Country> listofCountries = new List<Country>();

            try
            {
                string sql = "select name, alpha2Code, alpha3Code, capital, region, subregion, population, demonym, area, gini, nativeName, numericCode, flag, cioc from Country";

                command = new SQLiteCommand(sql, connection);

                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    listofCountries.Add
                        (new Country
                        {
                            name = (string)reader["name"],
                            alpha2Code = (string)reader["alpha2Code"],
                            alpha3Code = (string)reader["alpha3Code"],
                            capital = (string)reader["capital"],
                            region = (string)reader["region"],
                            subregion = (string)reader["subregion"],
                            population = Convert.ToInt32(reader["population"]),
                            demonym = (string)reader["demonym"],
                            area = (string)reader["area"],
                            gini = (string)reader["gini"],
                            nativeName = (string)reader["nativeName"],
                            numericCode = (string)reader["numericCode"],
                            flag = (string)reader["flag"],
                            cioc = (string)reader["cioc"]
                        });
                }
                connection.Close();

                return listofCountries;
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Error reading data from country table", e.Message);
                return null;
            }
        }

        public async Task SaveData(List<Country> listofCountries)
        {
            await Task.Run(() =>
            { 
            try
            {
                foreach (var country in listofCountries)
                {
                    string sql = string.Format("insert into Country (name, alpha2Code, alpha3Code, capital, region, subregion, population, demonym, area, gini, nativeName, numericCode, flag, cioc) " +
                        "values (@name, @alpha2Code, @alpha3Code, @capital, @region, @subregion, @population, @demonym, @area, @gini, @nativeName, @numericCode, @flag, @cioc)");

                    command = new SQLiteCommand(sql, connection);
                    command.Parameters.AddWithValue("@name", country.name);

                    if (!string.IsNullOrEmpty(country.alpha2Code))
                    {command.Parameters.AddWithValue("@alpha2Code", country.alpha2Code);}
                    else {command.Parameters.AddWithValue("@alpha2Code", "N/D");}

                    if (!string.IsNullOrEmpty(country.alpha3Code))
                    { command.Parameters.AddWithValue("@alpha3Code", country.alpha3Code); }
                    else { command.Parameters.AddWithValue("@alpha3Code", "N/D"); }

                    if (!string.IsNullOrEmpty(country.capital))
                    { command.Parameters.AddWithValue("@capital", country.capital); }
                    else { command.Parameters.AddWithValue("@capital", "N/D"); }

                    if (!string.IsNullOrEmpty(country.region))
                    { command.Parameters.AddWithValue("@region", country.region); }
                    else { command.Parameters.AddWithValue("@region", "N/D"); }

                    if (!string.IsNullOrEmpty(country.subregion))
                    { command.Parameters.AddWithValue("@subregion", country.subregion); }
                    else { command.Parameters.AddWithValue("@subregion", "N/D"); }

                    command.Parameters.AddWithValue("@population", country.population);

                    if (!string.IsNullOrEmpty(country.demonym))
                    { command.Parameters.AddWithValue("@demonym", country.demonym); }
                    else { command.Parameters.AddWithValue("@demonym", "N/D"); }

                    if (!string.IsNullOrEmpty(country.area))
                    { command.Parameters.AddWithValue("@area", country.area); }
                    else { command.Parameters.AddWithValue("@area", "N/D"); }

                    if (!string.IsNullOrEmpty(country.gini))
                    { command.Parameters.AddWithValue("@gini", country.gini); }
                    else { command.Parameters.AddWithValue("@gini", "N/D"); }

                    if (!string.IsNullOrEmpty(country.nativeName))
                    { command.Parameters.AddWithValue("@nativeName", country.nativeName); }
                    else { command.Parameters.AddWithValue("@nativeName", "N/D"); }

                    if (!string.IsNullOrEmpty(country.numericCode))
                    { command.Parameters.AddWithValue("@numericCode", country.numericCode); }
                    else { command.Parameters.AddWithValue("@numericCode", "N/D"); }

                    if (!string.IsNullOrEmpty(country.flag))
                    { command.Parameters.AddWithValue("@flag", country.flag); }
                    else { command.Parameters.AddWithValue("@flag", "N/D"); }

                    if (!string.IsNullOrEmpty(country.cioc))
                    { command.Parameters.AddWithValue("@cioc", country.cioc); }
                    else { command.Parameters.AddWithValue("@cioc", "N/D"); }

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Error in SaveData", e.Message);
            }
            });
        }

        public async Task DeleteData()
        {
            await Task.Run(() =>
            { 
            try
            {
                string sql = "delete from Country";
                command = new SQLiteCommand(sql, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                dialogService.ShowMessage("Error deleting data from Database", e.Message);
            }
            });
        }
    }
}
