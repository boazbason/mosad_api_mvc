using Microsoft.EntityFrameworkCore;
using ProjectMosadApi.Models;


namespace MosadMVC.DataContext

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
        
        
    }
}