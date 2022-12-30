namespace Organization.Controllers.Service;

public static class DbValueRetriever
{
    /// <summary>
    /// Получение объекта из базы данных по заданному уникальному идентификатору
    /// </summary>
    /// <param name="dbContent">Содержимое базы данных</param>
    /// <param name="id">Id объекта, который необходимо доставить</param>
    /// <param name="idPropertyName">Название свойства, содержащего уникальный идентификатор объекта</param>
    /// <typeparam name="TValue">Тип объекта, который нужно получить из базы данных</typeparam>
    /// <typeparam name="TId">Тип уникального идентификатора объекта</typeparam>
    /// <returns>Объект из базы данных с указанным Id или null, если база данных не содержит такой объект</returns>
    /// <exception cref="DbValueRetrieverException">Генерируется в случае, если TValue не содержит свойство idPropertyName</exception>
    /// <remarks>Метод не поддерживает заполнение свойств-коллекций типа TValue</remarks>>
    public static TValue? Retrieve<TValue, TId>(IEnumerable<TValue> dbContent, TId id, string idPropertyName) 
        where TValue : class
        where TId : IComparable
    {
        var result = dbContent.SingleOrDefault(elem =>
            {
                var idPropertyInfo = elem.GetType().GetProperty(idPropertyName);
                if (idPropertyInfo is null) { throw new DbValueRetrieverException($"Тип {nameof(TValue)} не содержит свойство {idPropertyName}"); }
                
                var idPropertyValue = (TId?) idPropertyInfo.GetValue(elem);
                if (idPropertyValue is null) { return false; }

                return idPropertyValue.CompareTo(id) == 0;
            });

        return result;
    }
}

public class DbValueRetrieverException : Exception
{
    public DbValueRetrieverException(string message) : base(message) { }
}