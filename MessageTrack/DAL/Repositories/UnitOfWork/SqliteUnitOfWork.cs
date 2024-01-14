using System.Data;
using System.Data.SQLite;
using MessageTrack.DAL.Interfaces.Repositories;
using MessageTrack.DAL.Interfaces.UnitOfWork;
using MessageTrack.DAL.Repositories.SqliteRepositories;


namespace MessageTrack.DAL.Repositories.UnitOfWork
{
    public class SqliteUnitOfWork : IUnitOfWork
    {
        private bool _disposedValue = false;
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public SqliteUnitOfWork(string pathToDb)
        {
            var builder = new SQLiteConnectionStringBuilder();
            builder.DataSource = pathToDb;
            _connection = new SQLiteConnection(builder.ConnectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        private IExternalRecipientRepository _externalRecipientRepository;
        public IExternalRecipientRepository ExternalRecipientRepository => 
            _externalRecipientRepository ?? (_externalRecipientRepository = new ExternalRecipientRepository(_transaction));

        private IOutboxMessageRepository _outboxMessageRepository;

        public IOutboxMessageRepository OutboxMessageRepository => 
            _outboxMessageRepository ?? (_outboxMessageRepository = new OutboxMessageRepository(_transaction));


        /// <summary>
        /// Метод делает commit, если не возникло проблем
        /// и делает rollback если проблемы возникли.
        /// </summary>
        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction();
                ResetRepositories();
            }
        }

        /// <summary>
        /// Откатывает транзацию
        /// </summary>
        public void Rollback()
        {
            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = _connection.BeginTransaction();
            ResetRepositories();
        }

        /// <summary>
        /// Удаляет connection к бд
        /// </summary>
        public virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if (_connection != null)
                    {
                        _connection.Close();
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _disposedValue = true;
            }
        }

        /// <summary>
        /// Удаляет connection к бд
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// сбрасывает репозитории, которые используются в uow
        /// </summary>
        private void ResetRepositories()
        {
            _externalRecipientRepository = null;
            _outboxMessageRepository = null;
        }
    }

}
