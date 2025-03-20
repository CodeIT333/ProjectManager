using Application.Commons;

namespace Persistence
{
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ProjectManagerContext _context;

        public UnitOfWork(ProjectManagerContext context)
        {
            _context = context;
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
