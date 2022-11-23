namespace Organization.Controllers.Service;

public static class DbValueRetriever
{
    /// <summary>
    /// Получение значения из базы данных по заданному Id
    /// </summary>
    /// <typeparam name="TValue">Тип значения, доставляемого из базы данных</typeparam>
    /// <typeparam name="TId">Тип Id</typeparam>
    /// <returns>(IActionResult - тип ошибки, TValue - элемент из базы данных)</returns>
    /// <remarks>Метод не поддерживает заполнение свойств-коллекций типа TValue</remarks>>
    public static (Exception? Error, TValue? Result) RetrieveFromDb<TValue, TId>(IEnumerable<TValue> values, string idPropertyName, TId id) 
        where TValue : class
        where TId : IComparable
    {
        TValue? result = null;
        Exception? err = null;
        try
        {
            result = values.AsEnumerable().Single(elem =>
                {
                    var idPropertyInfo = elem.GetType().GetProperty(idPropertyName);
                    if (idPropertyInfo is null) { throw new DbValueRetrieverException($"Тип {nameof(TValue)} не содержит свойство {idPropertyName}"); }
                    
                    var idPropertyValue = (TId?) idPropertyInfo.GetValue(elem);
                    if (idPropertyValue is null) { return false; }

                    return idPropertyValue.CompareTo(id) == 0;
                }
            );
        }
        catch (InvalidOperationException ex) { err = ex; }
        catch (ArgumentNullException ex) { err = ex; }

        return (err, result);
    }
}

public class DbValueRetrieverException : Exception
{
    public DbValueRetrieverException(string message) : base(message) { }
    public DbValueRetrieverException() { }
}