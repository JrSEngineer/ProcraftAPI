using Microsoft.EntityFrameworkCore;
using ProcraftAPI.Data.Context;

namespace ProcraftAPI.Data.Settings
{
    public class ProcraftDbContextFactory : IDbContextFactory<ProcraftDbContext>
    {

        protected readonly DbContextOptions<ProcraftDbContext> _options;
        public ProcraftDbContextFactory(DbContextOptions<ProcraftDbContext> options)
        {
            _options = options;
        }

        public ProcraftDbContext CreateDbContext()
        {
            return new ProcraftDbContext(_options);
        }
    }
}
