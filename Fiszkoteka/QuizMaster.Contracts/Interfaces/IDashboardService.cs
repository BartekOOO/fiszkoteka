using QuizMaster.Contracts.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Interfaces
{
    public interface IDashboardService
    {
        Task<MainDashboardDto> GetDashboard(
            int userId,
            CancellationToken cancellationToken = default);
    }
}
