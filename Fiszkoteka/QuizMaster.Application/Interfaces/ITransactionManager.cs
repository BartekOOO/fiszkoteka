using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Application.Interfaces
{
    public interface ITransactionManager
    {
        Task ExecuteInTransactionAsync(
            Func<CancellationToken, Task> action,
            CancellationToken cancellationToken = default);

        Task<T> ExecuteInTransactionAsync<T>(
            Func<CancellationToken, Task<T>> action,
            CancellationToken cancellationToken = default);
    }
}
