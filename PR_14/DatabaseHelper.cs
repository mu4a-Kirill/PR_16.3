using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using PR_14.Models;

namespace PR_14
{
    public class DatabaseHelper
    {
        private string connectionString = @"Data Source=KIR_PC_888\SQLEXPRESS;Initial Catalog=CinemaBD;Integrated Security=True";

        private string BasePath = AppDomain.CurrentDomain.BaseDirectory;


        public Polzovatel ProveritPolzovatela(string login, string parol)
        {
            Polzovatel polzovatel = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Polzovateli WHERE Login = @Login AND Parol = @Parol";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Login", login);
                command.Parameters.AddWithValue("@Parol", parol);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    polzovatel = new Polzovatel
                    {
                        Id = (int)reader["Id"],
                        Login = reader["Login"].ToString(),
                        Parol = reader["Parol"].ToString(),
                        Imya = reader["Imya"].ToString(),
                        Familiya = reader["Familiya"].ToString(),
                        Vozrast = reader["Vozrast"] != DBNull.Value ? (int)reader["Vozrast"] : 0,
                        Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : ""
                    };
                }
            }
            return polzovatel;
        }

        public void RegistrirovatPolzovatela(string login, string parol, string imya, string familiya)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Polzovateli (Login, Parol, Imya, Familiya) VALUES (@Login, @Parol, @Imya, @Familiya)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Login", login);
                command.Parameters.AddWithValue("@Parol", parol);
                command.Parameters.AddWithValue("@Imya", imya);
                command.Parameters.AddWithValue("@Familiya", familiya);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public Seans PoluchitSeansPoId(int seansId)
        {
            Seans seans = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT s.*, f.Nazvanie as FilmNazvanie, z.NomerZala as ZalNomer 
                         FROM Seansi s 
                         JOIN Filmi f ON s.FilmId = f.Id 
                         JOIN Zali z ON s.ZalId = z.Id 
                         WHERE s.Id = @SeansId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SeansId", seansId);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    seans = new Seans
                    {
                        Id = (int)reader["Id"],
                        FilmId = (int)reader["FilmId"],
                        ZalId = (int)reader["ZalId"],
                        DataSeansa = (DateTime)reader["DataSeansa"],
                        Vremya = (TimeSpan)reader["Vremya"],
                        Cena = (decimal)reader["Cena"],
                        FilmNazvanie = reader["FilmNazvanie"].ToString(),
                        ZalNomer = (int)reader["ZalNomer"]
                    };
                }
            }
            return seans;
        }
        public List<Film> PoluchitVseFilmi()
        {
            List<Film> filmi = new List<Film>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Filmi";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string relativePath = reader["Oblozhka"] != DBNull.Value ? reader["Oblozhka"].ToString() : "";
                    string fullPath = "";
                    if (!string.IsNullOrEmpty(relativePath))
                    {
                        if (System.IO.Path.IsPathRooted(relativePath))
                            fullPath = relativePath;
                        else
                            fullPath = System.IO.Path.Combine(BasePath, relativePath);
                    }

                    Film film = new Film
                    {
                        Id = (int)reader["Id"],
                        Nazvanie = reader["Nazvanie"].ToString(),
                        Opisanie = reader["Opisanie"] != DBNull.Value ? reader["Opisanie"].ToString() : "",
                        Reyting = (decimal)reader["Reyting"],
                        VozrastnoyReyting = (int)reader["VozrastnoyReyting"],
                        DataNachala = (DateTime)reader["DataNachala"],
                        Oblozhka = fullPath
                    };
                    filmi.Add(film);
                }
            }
            return filmi;
        }

        public Film PoluchitFilmPoId(int id)
        {
            Film film = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Filmi WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string relativePath = reader["Oblozhka"] != DBNull.Value ? reader["Oblozhka"].ToString() : "";
                    string fullPath = "";
                    if (!string.IsNullOrEmpty(relativePath))
                    {
                        if (System.IO.Path.IsPathRooted(relativePath))
                            fullPath = relativePath;
                        else
                            fullPath = System.IO.Path.Combine(BasePath, relativePath);
                    }

                    film = new Film
                    {
                        Id = (int)reader["Id"],
                        Nazvanie = reader["Nazvanie"].ToString(),
                        Opisanie = reader["Opisanie"] != DBNull.Value ? reader["Opisanie"].ToString() : "",
                        Reyting = (decimal)reader["Reyting"],
                        VozrastnoyReyting = (int)reader["VozrastnoyReyting"],
                        DataNachala = (DateTime)reader["DataNachala"],
                        Oblozhka = fullPath
                    };
                }
            }
            return film;
        }

        public List<Seans> PoluchitSeansiPoFilmu(int filmId)
        {
            List<Seans> seansi = new List<Seans>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT s.*, f.Nazvanie as FilmNazvanie, z.NomerZala as ZalNomer " +
                               "FROM Seansi s " +
                               "JOIN Filmi f ON s.FilmId = f.Id " +
                               "JOIN Zali z ON s.ZalId = z.Id " +
                               "WHERE s.FilmId = @FilmId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FilmId", filmId);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Seans seans = new Seans
                    {
                        Id = (int)reader["Id"],
                        FilmId = (int)reader["FilmId"],
                        ZalId = (int)reader["ZalId"],
                        DataSeansa = (DateTime)reader["DataSeansa"],
                        Vremya = (TimeSpan)reader["Vremya"],
                        Cena = (decimal)reader["Cena"],
                        FilmNazvanie = reader["FilmNazvanie"].ToString(),
                        ZalNomer = (int)reader["ZalNomer"]
                    };
                    seansi.Add(seans);
                }
            }
            return seansi;
        }

        public List<int> PoluchitZanyatieMesta(int seansId)
        {
            List<int> mesta = new List<int>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Mesto FROM Bileti WHERE SeansId = @SeansId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SeansId", seansId);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    mesta.Add((int)reader["Mesto"]);
                }
            }
            return mesta;
        }

        public void KupitBilet(int seansId, int polzovatelId, int mesto)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Bileti (SeansId, PolzovatelId, Mesto) VALUES (@SeansId, @PolzovatelId, @Mesto)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SeansId", seansId);
                command.Parameters.AddWithValue("@PolzovatelId", polzovatelId);
                command.Parameters.AddWithValue("@Mesto", mesto);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<Bilet> PoluchitBiletiPolzovatela(int polzovatelId)
        {
            List<Bilet> bileti = new List<Bilet>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT b.*, f.Nazvanie as FilmNazvanie, z.NomerZala as ZalNomer, s.DataSeansa, s.Vremya, s.Cena " +
                               "FROM Bileti b " +
                               "JOIN Seansi s ON b.SeansId = s.Id " +
                               "JOIN Filmi f ON s.FilmId = f.Id " +
                               "JOIN Zali z ON s.ZalId = z.Id " +
                               "WHERE b.PolzovatelId = @PolzovatelId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PolzovatelId", polzovatelId);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Bilet bilet = new Bilet
                    {
                        Id = (int)reader["Id"],
                        SeansId = (int)reader["SeansId"],
                        PolzovatelId = (int)reader["PolzovatelId"],
                        Mesto = (int)reader["Mesto"],
                        DataPokupki = (DateTime)reader["DataPokupki"],
                        FilmNazvanie = reader["FilmNazvanie"].ToString(),
                        ZalNomer = (int)reader["ZalNomer"],
                        DataSeansa = (DateTime)reader["DataSeansa"],
                        Vremya = (TimeSpan)reader["Vremya"],
                        Cena = (decimal)reader["Cena"]
                    };
                    bileti.Add(bilet);
                }
            }
            return bileti;
        }

        public bool ProveritSushestvovanieLogina(string login)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Polzovateli WHERE Login = @Login";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Login", login);
                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }    

    }
}