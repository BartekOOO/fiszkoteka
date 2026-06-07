using QuizMaster.Application.Interfaces;
using QuizMaster.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Infrastructure.Services
{
    public sealed class EfTransactionManager : ITransactionManager
    {
        private readonly QuizMasterDbContext _context;

        public EfTransactionManager(QuizMasterDbContext context)
        {
            _context = context;
        }

        public async Task ExecuteInTransactionAsync(
            Func<CancellationToken, Task> action,
            CancellationToken cancellationToken = default)
        {
            await using var transaction = await _context.Database
                .BeginTransactionAsync(cancellationToken);

            try
            {
                await action(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public async Task<T> ExecuteInTransactionAsync<T>(
            Func<CancellationToken, Task<T>> action,
            CancellationToken cancellationToken = default)
        {
            await using var transaction = await _context.Database
                .BeginTransactionAsync(cancellationToken);

            try
            {
                var result = await action(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return result;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
