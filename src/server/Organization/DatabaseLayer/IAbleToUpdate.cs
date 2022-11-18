namespace Organization.Models.DatabaseLayer;

public interface IAbleToUpdate<in T> where T : class
{
    /// <summary>
    /// Обновление данных об объекте с указанным уникальным идентификатором
    /// </summary>
    /// <param name="id">Уникальный идентификатор объекта</param>
    /// <param name="newData">Новые данные объекта</param>
    /// <returns>true, если данные объекта были успешно обновлены, иначе - false</returns>
    bool Update(long id, T newData);
}