using DataManagement.Components.Pages;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System.Runtime.InteropServices.Marshalling;

namespace DataManagement.db {
    public class DbManeger {
        public struct GeoLoaction {
            double latitude;
            double longitude;
        }
        string connectionString = "Server=localhost; Database=DataBase; Uid=root; Pwd=;";
        string query = "INSERT INTO housing (Type, Square, Price, Status, location, number_of_rooms, count_floor, elevator, repair, description, adress) VALUES (@Type, @Square, @Price, @Status, @location, @number_of_rooms, @count_floor, @elevator, @repair, @description, @adress)";
        public Tuple<bool, string> AddRecord(string type, float square, float price, bool status, GeoLoaction location, int number_of_rooms, int count_floor, bool elevator, string repair, string description, string adress) {
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                try {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection)) {
                        command.Parameters.AddWithValue("@Type", type);
                        command.Parameters.AddWithValue("@Square", square);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@Status", status);
                        command.Parameters.AddWithValue("@location", location);
                        command.Parameters.AddWithValue("@number_of_rooms", number_of_rooms);
                        command.Parameters.AddWithValue("@count_floor", count_floor);
                        command.Parameters.AddWithValue("@elevator", elevator);
                        command.Parameters.AddWithValue("@repair", repair);
                        command.Parameters.AddWithValue("@description", description);
                        command.Parameters.AddWithValue("@adress", adress);

                        int row = command.ExecuteNonQuery();
                        return Tuple.Create(true, $"Добавлено записей {row}");
                    }
                } catch (MySqlException ex) {
                    return Tuple.Create(false, ex.Message);
                } catch (Exception ex2) {
                    return Tuple.Create(false, ex2.Message);
                }
            }
        }
    }
}
