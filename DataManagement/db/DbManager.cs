using DataManagement.Components.Pages;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using MySqlX.XDevAPI;
using System.Runtime.InteropServices.Marshalling;

namespace DataManagement.db {
    public class DbManager {

        string connectionString = "Server=localhost; Database=DataBase; Uid=root; Pwd=;";
        public async Task<Tuple<bool, string>> AddHousinng(RowRecord record) {
            string query = "INSERT INTO housing (Type, Square, Price, Status, number_of_rooms, count_floor, elevator, repair, description, adress) VALUES (@Type, @Square, @Price, @Status, @number_of_rooms, @count_floor, @elevator, @repair, @description, @adress)";
            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                try {
                    await connection.OpenAsync();
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
                }
                catch (MySqlException ex) {
                    return Tuple.Create(false, ex.Message);
                }
                catch (Exception ex2) {
                    return Tuple.Create(false, ex2.Message);
                }
            }
        }
        public async Task<Tuple<bool, string>> AddLead(LeadRecord record) {
            string query = "INSERT INTO leads (Name, Number_phone, Description, Status, city) VALUES (@Name, @Number_phone, @Description, @Status, @city)";

            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                try {
                    await connection.OpenAsync();
                    using (MySqlCommand command = new MySqlCommand(query, connection)) {
                        command.Parameters.AddWithValue("@Name", record.name);
                        command.Parameters.AddWithValue("@Number_phone", record.number_phone);
                        command.Parameters.AddWithValue("@Description", record.description);
                        command.Parameters.AddWithValue("@Status", record.status);
                        command.Parameters.AddWithValue("@city", record.city);

                        int row = await command.ExecuteNonQueryAsync();
                        return Tuple.Create(true, $"Добавлено записей {row}");
                    }
                }
                catch (MySqlException ex) {
                    return Tuple.Create(false, ex.Message);
                }
                catch (Exception ex2) {
                    return Tuple.Create(false, ex2.Message);
                }
            }
        }
        public async Task<Tuple<bool, string>> AddUser(User record) {
            string query = "INSERT INTO users (Login, Hash, Role, Status, name, number_phone, city) VALUES (@Login, @Hash, @Role, @Status, @name, @number_phone, @city)";

            using (MySqlConnection connection = new MySqlConnection(connectionString)) {
                try {
                    await connection.OpenAsync();
                    using (MySqlCommand command = new MySqlCommand(query, connection)) {
                        command.Parameters.AddWithValue("@Login", record.login);
                        command.Parameters.AddWithValue("@Hash",record.hesh );
                        command.Parameters.AddWithValue("@Role", record.roly);
                        command.Parameters.AddWithValue("@Status", record.status);
                        command.Parameters.AddWithValue("@name", record.name);
                        command.Parameters.AddWithValue("@number_phone", record.number_phone);
                        command.Parameters.AddWithValue("@city", record.city);

                        int row = await command.ExecuteNonQueryAsync();
                        return Tuple.Create(true, $"Добавлено записей {row}");
                    }
                }
                catch (MySqlException ex) {
                    return Tuple.Create(false, ex.Message);
                }
                catch (Exception ex2) {
                    return Tuple.Create(false, ex2.Message);
                }
            }
        }
        public async Task<List<RowRecord>> GetHousing() {
            const string selectQuery = @"
        SELECT Type, Square, Price, Status, number_of_rooms,
               count_floor, elevator, repair, description, adress
        FROM housing
        ORDER BY Price DESC";

            var list = new List<RowRecord>();

            await using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            await using var command = new MySqlCommand(selectQuery, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync()) {
                list.Add(new RowRecord {
                    type = reader.IsDBNull(0) ? null : reader.GetString(0),
                    square = reader.IsDBNull(1) ? null : reader.GetFloat(1),
                    price = reader.IsDBNull(2) ? null : reader.GetDecimal(2),
                    status = reader.IsDBNull(3) ? null : reader.GetString(3),
                    numberOfRooms = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                    countFloor = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                    elevator = reader.IsDBNull(6) ? null : reader.GetBoolean(6),
                    repair = reader.IsDBNull(7) ? null : reader.GetString(7),
                    description = reader.IsDBNull(8) ? null : reader.GetString(8),
                    adress = reader.IsDBNull(9) ? null : reader.GetString(9)
                });
            }

            return list;
        }


    }
}
