using MessageTrack.DAL.Interfaces.Base;
using System.Data;

namespace MessageTrack.DAL.Repositories.Base
{
    /// <summary>
    /// Базовый класс для репозиториев, содержащих общие методы доступа к данным.
    /// </summary>
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected IDbTransaction Transaction { get; private set; }
        protected IDbConnection Connection => Transaction.Connection;

        public RepositoryBase(IDbTransaction transaction)
        {
            Transaction = transaction;
        }

        /// <summary>
        /// Выполняет запрос, если возникло исключение, откатывает транзакцию
        /// </summary>
        protected async Task SafeExecuteAsync(Func<Task> execute)
        {
            try
            {
                await execute();
            }
            catch (Exception ex)
            {
                Transaction?.Rollback();
                Transaction?.Dispose();
                Transaction = Connection?.BeginTransaction();
                Transaction = null;

                throw;
            }
        }
    }

}
