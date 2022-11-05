namespace Organization.Models.Organization.Accounts;

public interface IUserAggregator
{
    /// <summary>
    /// Метод добавляем указанный аккаунт в хранилище
    /// </summary>
    /// <param name="account"></param>
    void AddAccount(IAccount account);
    
    /// <summary>
    /// Метод удаляем указанный аккаунт из хранилища
    /// </summary>
    /// <returns>true в случае успушного удаление, иначе - false</returns>
    bool RemoveAccount(IAccount account);
    
    /// <summary>
    /// Метод удаляет аккаунт с указанным уникальным идентификатором из хранилища 
    /// </summary>
    /// <param name="uid">Уникальный идентификатор аккаунта</param>
    /// <returns>true в случае успушного удаление, иначе - false</returns>
    bool RemoveAccount(byte[] uid);
    
    /// <summary>
    /// Метод проверяет, содержитсся ли в хранилище указанный пользователь 
    /// </summary>
    bool Contains(IAccount account);

    /// <summary>
    /// Метод проверяет, содержитсся ли в хранилище пользователь с указанным уникальным идентификатором
    /// </summary>
    /// <param name="uid">Уникальный идентификатор проверяемого пользователя</param>
    bool Contains(byte[] uid);
}