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

        // M�todo OnModelCreating para customiza��es
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configura��es personalizadas para o Identity
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.Email).HasColumnType("varchar(256)");
                entity.Property(e => e.UserName).HasColumnType("varchar(256)");
                entity.Property(e => e.NormalizedEmail).HasColumnType("varchar(256)");
                entity.Property(e => e.NormalizedUserName).HasColumnType("varchar(256)");
                entity.Property(e => e.ConcurrencyStamp).HasColumnType("text"); // text ao inv�s de nvarchar(max)
            });

            // Se houver mais customiza��es para outras entidades, adicione aqui.
        }
    }
}
