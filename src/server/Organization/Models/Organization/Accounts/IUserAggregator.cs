namespace Organization.Models.Organization.Accounts;

public interface IUserAggregator
{
    /// <summary>
    /// Метод добавляем указанный аккаунт в хранилище
    /// </summary>
    /// <returns>true в случае удачного добавление, иначе - false</returns>
    bool AddAccount(IAccount account);
    
    /// <summary>
    /// Метод удаляем указанный аккаунт из хранилища
    /// </summary>
    /// <returns>true в случае успушного удаление, иначе - false</returns>
    bool RemoveAccount(IAccount account);

    /// <summary>
    /// Метод удаляет аккаунт с указанным уникальным идентификатором из хранилища 
    /// </summary>
    /// <param name="id">Уникальный идентификатор удаляемого объекта</param>
    /// <returns>Ссылка на удаляемый объект в случае успушного удаления, иначе - null</returns>
    IAccount? RemoveAccount(long id);
    
    /// <summary>
    /// Метод проверяет, содержитсся ли в хранилище указанный пользователь 
    /// </summary>
    bool Contains(IAccount account);
    
    /// <summary>
    /// Метод проверяет, содержитсся ли в хранилище пользователь с указанным уникальным идентификатором
    /// </summary>
    /// <param name="id">Уникальный идентификатор проверяемого аккаунта</param>
    /// <returns>Ссылка на объект, если он присутствует, иначе - null</returns>
    IAccount? Contains(long id);
}