using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace JASBlazor.Data
{
    public class DBContextFactory<TContext>
        : IDbContextFactory<TContext> where TContext : DbContext
    {
        private readonly IServiceProvider provider;

        public DBContextFactory(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public TContext CreateDbContext()
        {
            if (provider == null)
            {
                throw new InvalidOperationException(
                    $"You must configure an instance of IServiceProvider");
            }

            return ActivatorUtilities.CreateInstance<TContext>(provider);
        }
    }
}