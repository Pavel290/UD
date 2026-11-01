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
        SELECT id, Type, Square, Price, Status, number_of_rooms,
               count_floor, elevator, repair, description, adress
        FROM housing
        ORDER BY datetime DESC";

            var list = new List<RowRecord>();

            await using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            await using var command = new MySqlCommand(selectQuery, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync()) {
                list.Add(new RowRecord {
                    id = reader.GetInt32(0),
                    type = reader.IsDBNull(1) ? null : reader.GetString(1),
                    square = reader.IsDBNull(2) ? null : reader.GetFloat(2),
                    price = reader.IsDBNull(3) ? null : reader.GetDecimal(3),
                    status = reader.IsDBNull(4) ? null : reader.GetString(4),
                    numberOfRooms = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                    countFloor = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                    elevator = reader.IsDBNull(7) ? null : reader.GetBoolean(7),
                    repair = reader.IsDBNull(8) ? null : reader.GetString(8),
                    description = reader.IsDBNull(9) ? null : reader.GetString(9),
                    adress = reader.IsDBNull(10) ? null : reader.GetString(10)
                });
            }

            return list;
        }
        public async Task<List<LeadRecord>> GetLeads() {
            const string selectQuery = @"SELECT id, Name, Number_phone, Description, Status, city FROM leads ORDER BY datetime DESC";
            var list = new List<LeadRecord>();

            await using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            await using var command = new MySqlCommand(selectQuery, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync()) {
                list.Add(new LeadRecord {
                    id = reader.GetInt32(0),
                    name = reader.IsDBNull(1) ? null : reader.GetString(1),
                    number_phone = reader.IsDBNull(2) ? null : reader.GetString(2),
                    description = reader.IsDBNull(3) ? null : reader.GetString(3),
                    status = reader.IsDBNull(4) ? null : reader.GetString(4),
                    city = reader.IsDBNull(5) ? null : reader.GetString(5),
                });
            }

            return list;
        }
        public async Task<LeadRecord?> GetLead(int id) {
            const string query = @"
        SELECT id, Name, Number_phone, Description, Status, city
        FROM leads
        WHERE id = @id
        LIMIT 1";

            await using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            await using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            await using var reader = await command.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return null;

            return new LeadRecord {
                id = reader.GetInt32(0),
                name = reader.IsDBNull(1) ? null : reader.GetString(1),
                number_phone = reader.IsDBNull(2) ? null : reader.GetString(2),
                description = reader.IsDBNull(3) ? null : reader.GetString(3),
                status = reader.IsDBNull(4) ? null : reader.GetString(4),
                city = reader.IsDBNull(5) ? null : reader.GetString(5)
            };
        }

        public async Task<Tuple<bool, string>> UpdateLead(LeadRecord record) {
            const string query = @"
        UPDATE leads SET
            Name = @Name, Number_phone = @Number_phone,
            Description = @Description, Status = @Status, city = @city
        WHERE id = @id";

            using var connection = new MySqlConnection(connectionString);
            try {
                await connection.OpenAsync();
                using var command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@Name", record.name);
                command.Parameters.AddWithValue("@Number_phone", record.number_phone);
                command.Parameters.AddWithValue("@Description", record.description);
                command.Parameters.AddWithValue("@Status", record.status);
                command.Parameters.AddWithValue("@city", record.city);
                command.Parameters.AddWithValue("@id", record.id);

                int rows = await command.ExecuteNonQueryAsync();
                return rows > 0
                    ? Tuple.Create(true, $"Обновлено записей {rows}")
                    : Tuple.Create(false, "Запись не найдена");
            }
            catch (MySqlException ex) {
                return Tuple.Create(false, ex.Message);
            }
            catch (Exception ex) {
                return Tuple.Create(false, ex.Message);
            }
        }
        public async Task<List<User>> GetUsers() {
            const string selectQuery = @"SELECT id, Role, Status, name, number_phone, city FROM users ORDER BY datetime DESC";
            var list = new List<User>();

            await using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            await using var command = new MySqlCommand(selectQuery, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync()) {
                list.Add(new User {
                    id = reader.GetInt32(0),
                    roly = reader.IsDBNull(1) ? null : reader.GetString(1),
                    status = reader.IsDBNull(2) ? null : reader.GetString(2),
                    name = reader.IsDBNull(3) ? null : reader.GetString(3),
                    number_phone = reader.IsDBNull(4) ? null : reader.GetString(4),
                    city = reader.IsDBNull(5) ? null : reader.GetString(5),
                });
            }
            return list;
        }
        public async Task<User?> GetUser(int id) {
            const string query = @"
        SELECT id, Login, Role, Status, name, number_phone, city
        FROM users
        WHERE id = @id
        LIMIT 1";

            await using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            await using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            await using var reader = await command.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return null;

            return new User {
                id = reader.GetInt32(0),
                login = reader.IsDBNull(1) ? null : reader.GetString(1),
                roly = reader.IsDBNull(2) ? null : reader.GetString(2),
                status = reader.IsDBNull(3) ? null : reader.GetString(3),
                name = reader.IsDBNull(4) ? null : reader.GetString(4),
                number_phone = reader.IsDBNull(5) ? null : reader.GetString(5),
                city = reader.IsDBNull(6) ? null : reader.GetString(6)
            };
        }
        public async Task<Tuple<bool, string>> UpdateUser(User record) {
            const string query = @"
        UPDATE users SET
            Role = @Role, Status = @Status, name = @name,
            number_phone = @number_phone, city = @city
        WHERE id = @id";

            using var connection = new MySqlConnection(connectionString);
            try {
                await connection.OpenAsync();
                using var command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@Role", record.roly);
                command.Parameters.AddWithValue("@Status", record.status);
                command.Parameters.AddWithValue("@name", record.name);
                command.Parameters.AddWithValue("@number_phone", record.number_phone);
                command.Parameters.AddWithValue("@city", record.city);
                command.Parameters.AddWithValue("@id", record.id);

                int rows = await command.ExecuteNonQueryAsync();
                return rows > 0
                    ? Tuple.Create(true, $"Обновлено записей {rows}")
                    : Tuple.Create(false, "Запись не найдена");
            }
            catch (MySqlException ex) {
                return Tuple.Create(false, ex.Message);
            }
            catch (Exception ex) {
                return Tuple.Create(false, ex.Message);
            }
        }

        public async Task<RowRecord?> GetHous(int id) {
            const string query = @"SELECT id, Type, Square, Price, Status, number_of_rooms, count_floor, elevator, repair, description, adress FROM housing WHERE id = @id LIMIT 1";

            await using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            await using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            await using var reader = await command.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return null;

            return new RowRecord {
                id = reader.GetInt32(0),
                type = reader.IsDBNull(1) ? null : reader.GetString(1),
                square = reader.IsDBNull(2) ? null : reader.GetFloat(2),
                price = reader.IsDBNull(3) ? null : reader.GetDecimal(3),
                status = reader.IsDBNull(4) ? null : reader.GetString(4),
                numberOfRooms = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                countFloor = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                elevator = reader.IsDBNull(7) ? null : reader.GetBoolean(7),
                repair = reader.IsDBNull(8) ? null : reader.GetString(8),
                description = reader.IsDBNull(9) ? null : reader.GetString(9),
                adress = reader.IsDBNull(10) ? null : reader.GetString(10)
            };
        }

        public async Task<Tuple<bool, string>> UpdateHous(RowRecord record) {
            const string query = @"
        UPDATE housing SET
            Type = @Type, Square = @Square, Price = @Price, Status = @Status,
            number_of_rooms = @number_of_rooms, count_floor = @count_floor,
            elevator = @elevator, repair = @repair,
            description = @description, adress = @adress
        WHERE id = @id";

            using var connection = new MySqlConnection(connectionString);
            try {
                await connection.OpenAsync();
                using var command = new MySqlCommand(query, connection);

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
                command.Parameters.AddWithValue("@id", record.id);

                int rows = await command.ExecuteNonQueryAsync();
                return rows > 0
                    ? Tuple.Create(true, $"Обновлено записей {rows}")
                    : Tuple.Create(false, "Запись не найдена");
            }
            catch (MySqlException ex) {
                return Tuple.Create(false, ex.Message);
            }
            catch (Exception ex) {
                return Tuple.Create(false, ex.Message);
            }
        }

    }
}
