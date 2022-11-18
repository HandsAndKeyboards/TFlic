namespace Organization.Models.DatabaseLayer;

public interface IAbleToInsert<T> where T : class
{
    /// <summary>
    /// Добавление объекта в хранилище
    /// </summary>
    /// <param name="toInsert">Добавляемый объект</param>
    /// <returns>Добавленный в хранилище объект</returns>
    T Insert(T toInsert);
}