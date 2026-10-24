using DataManagement.Components.Pages;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using MySqlX.XDevAPI;
using System.Runtime.InteropServices.Marshalling;

namespace DataManagement.db {
    public class DbManager {

        string connectionString = "Server=localhost; Database=DataBase; Uid=root; Pwd=;";
        string query = "INSERT INTO housing (Type, Square, Price, Status, number_of_rooms, count_floor, elevator, repair, description, adress) VALUES (@Type, @Square, @Price, @Status, @number_of_rooms, @count_floor, @elevator, @repair, @description, @adress)";
        public async Task<Tuple<bool, string>> AddRecord(RowRecord record) {
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                try {
                    connection.OpenAsync();
                    using (MySqlCommand command = new MySqlCommand(query, connection)) {
                        command.Parameters.AddWithValue("@Type", record.type);
                        command.Parameters.AddWithValue("@Square", record.square);
                        command.Parameters.AddWithValue("@Price", MySqlDbType.Decimal).Value = record.price;
                        command.Parameters.AddWithValue("@Status", record.status);
                        command.Parameters.AddWithValue("@number_of_rooms", record.numberOfRooms);
                        command.Parameters.AddWithValue("@count_floor", record.countFloor);
                        command.Parameters.AddWithValue("@elevator", record.elevator);
                        command.Parameters.AddWithValue("@repair", record.repair);
                        command.Parameters.AddWithValue("@description", record.description);
                        command.Parameters.AddWithValue("@adress", record.adress);

                        int row = await command.ExecuteNonQueryAsync();
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
