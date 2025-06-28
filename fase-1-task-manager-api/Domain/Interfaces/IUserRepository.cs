
using Domain.Entities;

namespace Domain.Interfaces;

public interface IUserRepository
{

    /// <summary>
    /// Busca todos os usuários cadastrados.
    /// </summary>
    /// <returns>Uma lista de usuários.</returns>
    Task<IEnumerable<User>> GetAllAsync();

    /// <summary>
    /// Busca um usuário pelo seu ID.
    /// </summary>
    /// <param name="id">O identificador único do usuário.</param>
    /// <returns>O usuário encontrado ou null se não existir.</returns>
    Task<User?> GetByIdAsync(Guid id);

    /// <summary>
    /// Busca todos os usuários cadastrados.
    /// </summary>
    /// <returns>Uma lista de usuários.</returns>
    Task<User> AddAsync(User user);

    /// <summary>
    /// Adiciona um novo usuário ao banco de dados.
    /// </summary>
    /// <param name="user">A entidade do usuário a ser adicionada.</param>
    void Update(User user);

    /// <summary>
    /// Marca um usuário existente para ser removido.
    /// </summary>
    /// <param name="user">A entidade do usuário a ser removida.</param>
    /// <returns>Uma tarefa representando a operação assíncrona.</returns>
    void Delete(User user);


}