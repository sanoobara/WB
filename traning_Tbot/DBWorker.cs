using Microsoft.Data.Sqlite;

namespace traning_Tbot
{
    internal class DBWorker
    {
        private string connectionString;
        public DBWorker(string connectionString)
        {
            this.connectionString = connectionString;
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());

        }

        public async Task AddUser(long Id, string NikName, string Name)
        {
            await using (var connection = new SqliteConnection(this.connectionString))
            {

                connection.OpenAsync();

                string sqlExpression = "INSERT OR IGNORE INTO Users (Id, NikName, Name, Active) VALUES (@Id, @NikName, @Name, @Active)";
                using (var command = new SqliteCommand(sqlExpression, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@NikName", NikName);
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Active", 1);
                    await command.ExecuteNonQueryAsync();
                }

            }
        }

        public async Task<string> AddUserInStat(long Id_user, int status, string date)
        {
            try
            {
                string name = "NONAME";
                using (var connection = new SqliteConnection(this.connectionString))
                {
                    await connection.OpenAsync();

                    string sqlExpression = "INSERT INTO Statistic (Id_user, Status, Date) VALUES (@Id_user, @Status, @Date)";

                    using (var command = new SqliteCommand(sqlExpression, connection))
                    {
                        command.Parameters.AddWithValue("@Id_user", Id_user);
                        command.Parameters.AddWithValue("@Status", status);
                        command.Parameters.AddWithValue("@Date", date);

                        await command.ExecuteNonQueryAsync();
                    }
                    sqlExpression = "Select Nikname From Users WHERE Id = @Id";
                    using (var command = new SqliteCommand(sqlExpression, connection))
                    {
                        command.Parameters.AddWithValue("@Id", Id_user);
                        var reader = await command.ExecuteReaderAsync();
                        while (await reader.ReadAsync()) // построчно считываем данные
                        {
                            name = reader.GetValue(0).ToString();

                        }

                    }

                }
                return $"Юзер {name} добавлен записан в стату";
            }
            catch (Exception exception)
            {

                var ErrorMessage = exception switch
                {
                    SqliteException sqliteException => $"SQLexxeption:\n[{sqliteException.ErrorCode}]\n{sqliteException.Message}",
                    _ => exception.ToString()

                };
                Console.WriteLine(ErrorMessage);
                return ErrorMessage;
            }




        }

        public async Task<string> AddUserInStatMain2(long Id_user, int status, string date)
        {
            try
            {
                await using (var connection = new SqliteConnection(this.connectionString))
                {
                    int count = 0;
                    string sqlExpression = "SELECT count(*) FROM Statistic WHERE Id_user = @id_user and Date = @date";
                    await using (var command = new SqliteCommand(sqlExpression, connection))
                    {
                        command.Parameters.AddWithValue("@Id_user", Id_user);
                        command.Parameters.AddWithValue("@date", date);
                        var reader = await command.ExecuteReaderAsync();
                        while (await reader.ReadAsync()) // построчно считываем данные
                        {
                            count = int.Parse(reader.GetValue(0).ToString());
                        }
                    }
                    if (count == 1)
                    {
                        sqlExpression = "UPDATE Statistic SET Status = @newstatus WHERE Id_user = @Id_user and Date = @date";
                        await using (var command = new SqliteCommand(sqlExpression, connection))
                        {
                            command.Parameters.AddWithValue("@newstatus", status);
                            command.Parameters.AddWithValue("@Id_user", Id_user);
                            command.Parameters.AddWithValue("@date", date);
                            await command.ExecuteNonQueryAsync();
                        }
                        string name = "NONAME";
                        sqlExpression = "Select Nikname From Users WHERE Id = @Id";
                        using (var command = new SqliteCommand(sqlExpression, connection))
                        {
                            command.Parameters.AddWithValue("@Id", Id_user);
                            var reader = await command.ExecuteReaderAsync();
                            while (await reader.ReadAsync()) // построчно считываем данные
                            {
                                name = reader.GetValue(0).ToString();

                            }

                        }
                        return $"Юзер {name} перезаписан в стату";
                    }
                    else { return await AddUserInStat(Id_user, status, date); }


                }


            }
            catch (Exception exception)
            {

                var ErrorMessage = exception switch
                {
                    SqliteException sqliteException => $"SQLexxeption:\n[{sqliteException.ErrorCode}]\n{sqliteException.Message}",
                    _ => exception.ToString()

                };
                Console.WriteLine(ErrorMessage);
                return ErrorMessage;
            }


        }

        public async Task<string> AddUserInStatMain(long Id_user, int status, string date)
        {
            try
            {
                await using (var connection = new SqliteConnection(this.connectionString))
                {
                    await connection.OpenAsync();

                    string sqlExpression = "SELECT count(*) FROM Statistic WHERE Id_user = @Id_user and Date = @date";
                    await using (var command = new SqliteCommand(sqlExpression, connection))
                    {
                        command.Parameters.AddWithValue("@Id_user", Id_user);
                        command.Parameters.AddWithValue("@date", date);
                        var result = await command.ExecuteScalarAsync();
                        int count = Convert.ToInt32(result);

                        if (count == 1)
                        {
                            sqlExpression = "UPDATE Statistic SET Status = @newstatus WHERE Id_user = @Id_user and Date = @date";
                            using (var updateCommand = new SqliteCommand(sqlExpression, connection))
                            {
                                updateCommand.Parameters.AddWithValue("@newstatus", status);
                                updateCommand.Parameters.AddWithValue("@Id_user", Id_user);
                                updateCommand.Parameters.AddWithValue("@date", date);

                                await updateCommand.ExecuteNonQueryAsync();
                            }

                            string name = "NONAME";
                            sqlExpression = "SELECT Nikname FROM Users WHERE Id = @Id";
                            using (var selectCommand = new SqliteCommand(sqlExpression, connection))
                            {
                                selectCommand.Parameters.AddWithValue("@Id", Id_user);
                                var reader = await selectCommand.ExecuteReaderAsync();
                                while (await reader.ReadAsync())
                                {
                                    name = reader.GetString(0);
                                }
                            }

                            return $"Юзер {name} перезаписан в стату";
                        }
                        else
                        {
                            return await AddUserInStat(Id_user, status, date);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                var ErrorMessage = exception switch
                {
                    SqliteException sqliteException => $"SQLexсeption:\n[{sqliteException.ErrorCode}]\n{sqliteException.Message}",
                    _ => exception.ToString()
                };

                Console.WriteLine(ErrorMessage);
                return ErrorMessage;
            }
        }

        public async Task<string> ShowStat()
        {
            string result = "";
            try
            {
                using (var connection = new SqliteConnection(this.connectionString))
                {
                    await connection.OpenAsync();


                    string sqlExpression = "SELECT(SELECT Name FROM Users " +
                    "WHERE Users.Id = Statistic.Id_user) AS Name, status, count(*) " +
                    "from Statistic WHERE status = 101 GROUP BY Id_user, Status";

                    using (var command = new SqliteCommand(sqlExpression, connection))
                    {
                        var reader = await command.ExecuteReaderAsync();
                        if (reader.HasRows) // если есть данные
                        {
                            Dictionary<string, string> valuePairs = new();

                            while (await reader.ReadAsync()) // построчно считываем данные
                            {
                                valuePairs.Add(reader.GetValue(0).ToString(), reader.GetValue(2).ToString());

                            }
                            result += "Стата по мужикам c 18.12:\n";
                            result += "---------------\n";
                            foreach (var item in valuePairs)
                            {
                                result += $"{item.Key}\t{item.Value}\t\n";
                               

                            }
                            result += "---------------\n";
                        }
                    }
                    result += "До 18.12";
                    result += "Артем\t4\r\nСаня\t6\r\nСемен\t4\r\nСтас\t5";
                }
                return result;
            }
            catch (Exception exception)
            {

                var ErrorMessage = exception switch
                {
                    SqliteException sqliteException => $"SQLexxeption:\n[{sqliteException.ErrorCode}]\n{sqliteException.Message}",
                    _ => exception.ToString()

                };
                Console.WriteLine(ErrorMessage);
                return ErrorMessage;
            }


        }




    }
}

