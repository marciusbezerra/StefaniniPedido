using System;
using System.Collections.Generic;
using System.Text;

namespace StefaniniPedido.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
