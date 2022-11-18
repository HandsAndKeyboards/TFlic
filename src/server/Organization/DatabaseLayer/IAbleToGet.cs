namespace Organization.Models.DatabaseLayer;

public interface IAbleToGet<out T> where T : class
{
    /// <summary>
    /// Получение данных об объекте с указанным уникальным идентификатором
    /// </summary>
    /// <param name="id">Уникальный идентификатор объекта</param>
    /// <returns>Объект или null, если объект не был найден</returns>
    T? Get(long id);

    /// <summary>
    /// Получение списка всех объектов 
    /// </summary>
    IEnumerable<T> GetAll();
}