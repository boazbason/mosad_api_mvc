using ProjectMosadApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ProjectMosadApi.DAL
{
    
    public class D_DbContext : DbContext
    {
        public DbSet<M_Agent> Agents { get; set; }
        public DbSet<M_Target> Targets { get; set; }
        public DbSet<M_Mission> Missions { get; set; }
        
        public D_DbContext(DbContextOptions<D_DbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        //private static DbContextOptions GetOptions(string connectionString)
        //{
        //    return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        //}
        
    }
}