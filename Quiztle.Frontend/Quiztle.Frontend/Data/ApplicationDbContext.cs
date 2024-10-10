//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;

//namespace Quiztle.Frontend.Data
//{
//    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
//    {
//    }
//}


using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Quiztle.Frontend.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Método OnModelCreating para customizações
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configurações personalizadas para o Identity
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.Email).HasColumnType("varchar(256)");
                entity.Property(e => e.UserName).HasColumnType("varchar(256)");
                entity.Property(e => e.NormalizedEmail).HasColumnType("varchar(256)");
                entity.Property(e => e.NormalizedUserName).HasColumnType("varchar(256)");
                entity.Property(e => e.ConcurrencyStamp).HasColumnType("text"); // text ao invés de nvarchar(max)
            });

            // Se houver mais customizações para outras entidades, adicione aqui.
        }
    }
}
