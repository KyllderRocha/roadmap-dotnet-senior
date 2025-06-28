using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Domain.Interfaces
{
    public interface ITaskService
    {
        
        /// <summary>
        /// Busca uma tarefa pelo seu ID.
        /// </summary>
        /// <param name="id">O identificador único da tarefa.</param>
        /// <returns>A tarefa encontrada ou null se não existir.</returns>
        Task<UserTask?> GetByIdAsync(Guid id);

        /// <summary>
        /// Busca todas as tarefas de um usuário específico.
        /// </summary>
        /// <param name="userId">O identificador único do usuário.</param>
        /// <returns>Uma lista de tarefas do usuário.</returns>
        Task<IEnumerable<UserTask>> GetAllByUserIdAsync(Guid userId);

        /// <summary>
        /// Adiciona uma nova tarefa ao banco de dados.
        /// </summary>
        /// <param name="task">A entidade da tarefa a ser adicionada.</param>
        Task<UserTask> AddAsync(UserTask task);

        /// <summary>
        /// Marca uma tarefa existente para ser atualizada.
        /// </summary>
        /// <param name="task">A entidade da tarefa com os dados atualizados.</param>
        void Update(UserTask task);

        /// <summary>
        /// Marca uma tarefa existente para ser removida.
        /// </summary>
        /// <param name="task">A entidade da tarefa a ser removida.</param>
        void Delete(UserTask task);
    }
}