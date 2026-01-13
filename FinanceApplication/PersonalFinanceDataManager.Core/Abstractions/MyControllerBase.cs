using Microsoft.AspNetCore.Mvc;
using PersonalFinanceDataManager.Domain.Entities;
using System.Security.Claims;

namespace PersonalFinanceDataManager.Core.Abstractions
{
    public abstract class MyControllerBase : ControllerBase
    {
        protected Guid GetUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User ID not found in token.");

            return Guid.Parse(userId);
        }
    }
}
