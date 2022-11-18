using System.Data.Common;
using Npgsql;

namespace Organization.Models.DatabaseLayer;

/// <summary>
/// Класс инкапсулирует выполнение основных SQL операций
/// </summary>
public static class SqlUtils
{
    public static DbDataReader Select(string sqlSelectCommand, NpgsqlConnection connection)
    {
        var command = new NpgsqlCommand(sqlSelectCommand, connection);
        return command.ExecuteReader();
    }

    /// <summary>
    /// Метод выполняет запрос на добавление, после - на выборку
    /// </summary>
    /// <param name="sqlInsertCommand">Запрос на добавление</param>
    /// <param name="sqlSelectCommand">Запрос на выборку</param>
    /// <param name="connection">Объект подключения к базе данных</param>
    /// <returns>Результат выборки</returns>
    /// <remarks>
    /// После добавления объекта в базу данных зачастую требуется получить этот объект с выданным базой данных Id
    /// </remarks>
    public static DbDataReader Insert(string sqlInsertCommand, string sqlSelectCommand, NpgsqlConnection connection)
    {
        var insertCommand = new NpgsqlCommand(sqlInsertCommand, connection);
        insertCommand.ExecuteScalar();
        
        var selectCommand = new NpgsqlCommand(sqlSelectCommand, connection);
        return selectCommand.ExecuteReader();
    }

    public static object? Update(string sqlUpdateCommand, NpgsqlConnection connection)
    {
        var updateCommand = new NpgsqlCommand(sqlUpdateCommand, connection);
        return updateCommand.ExecuteScalar();
    }
    
    public static object? Remove(string sqlDeleteCommand, NpgsqlConnection connection)
    {
        var updateCommand = new NpgsqlCommand(sqlDeleteCommand, connection);
        return updateCommand.ExecuteScalar();
    }
}