using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Organization.Controllers.Service;

public static class Handlers
{
    /// <summary>
    /// Обработчик ситуации "DbContext is null"
    /// </summary>
    /// <param name="dbContextType">Тип наследника класса DbContext</param>
    /// <param name="message">Пояснение ситуации</param>
    /// <returns>ExceptionResult объект с указанием типа контекста и сообщением</returns>
    public static IActionResult HandleNullDbContext(Type? dbContextType = null, string? message = null)
    {
        string defaultMessage = dbContextType is not null
            ? $"Не удалось создать экземпляр типа {dbContextType}"
            : $"Не удлось создать экземпляр наследника типа {nameof(DbContext)}";
        
        return new ExceptionResult(new Exception(message ?? defaultMessage), true);
    }
    
    /// <summary>
    /// Обработка ситуации "В базе данных не найден элеемнт с указанным уникальным идентификатором"
    /// </summary>
    /// <param name="elementName">Название элемента</param>
    /// <param name="id">Уникальный идентификатор элемента в базе данных</param>
    /// <returns>NotFound объект с указанием названия элемента и его Id</returns>
    public static IActionResult HandleElementNotFound<TId>(string elementName, TId id)  =>
        ResponseGenerator.NotFound($"{elementName} with Id = {id} was not found");
    
    /// <summary>
    /// Обработка ситуации "В базе данных было найдено несколько элеемнтов с указанным уникальным идентификатором"
    /// </summary>
    /// <param name="elementName">Название элемента</param>
    /// <param name="id">Уникальный идентификатор элемента</param>
    /// <returns>ExceptionResult объект с указанием названия элементов и их Id</returns>
    public static IActionResult HandleFoundMultipleElementsWithSameId(string elementName, ulong id) =>
        new ExceptionResult(new Exception($"Multiple elements with name {elementName}s and Id = {id} were found"), true);
}