namespace Organization.Models.DatabaseLayer;

public interface IAbleToRemove<out T> where T : class
{
    /// <summary>
    /// Метод удаляет объект с указанным уникальным идентификатором из ассоциированной таблицы базы данных
    /// </summary>
    /// <param name="id">Уникальный идентификатор объекта</param>
    /// <returns>Удаленный объент или null, если он не был найден</returns>
    T? Remove(long id);
}