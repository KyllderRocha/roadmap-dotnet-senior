
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces;

public interface IUserReplicaRepository
{
    /// <summary>
    /// Busca um usuário pelo seu ID.
    /// </summary>
    /// <param name="id">O identificador único do usuário.</param>
    /// <returns>O usuário encontrado ou null se não existir.</returns>
    Task<UserReplica?> GetByIdAsync(Guid id);

    /// <summary>
    /// Busca um usuário pelo seu email.
    /// </summary>
    /// <param name="email">O email do usuário.</param>
    /// <returns>O usuário encontrado ou null se não existir.</returns>
    Task<UserReplica?> GetByEmailAsync(string email);

    /// <summary>
    /// Busca todos os usuários cadastrados.
    /// </summary>
    /// <returns>Uma lista de usuários.</returns>
    Task<UserReplica> AddAsync(UserReplica user);

    /// <summary>
    /// Adiciona um novo usuário ao banco de dados.
    /// </summary>
    /// <param name="user">A entidade do usuário a ser adicionada.</param>
    void Update(UserReplica user);

    /// <summary>
    /// Marca um usuário existente para ser removido.
    /// </summary>
    /// <param name="user">A entidade do usuário a ser removida.</param>
    /// <returns>Uma tarefa representando a operação assíncrona.</returns>
    void Delete(UserReplica user);


}