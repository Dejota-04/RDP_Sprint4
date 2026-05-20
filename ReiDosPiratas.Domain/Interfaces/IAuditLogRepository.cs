using ReiDosPiratas.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReiDosPiratas.Domain.Interfaces
{
    public interface IAuditLogRepository
    {
        Task RegistrarAcessoAsync(AuditLog log);
        Task<IEnumerable<AuditLog>> ObterUltimosAcessosAsync(int quantidade = 50);
    }
}