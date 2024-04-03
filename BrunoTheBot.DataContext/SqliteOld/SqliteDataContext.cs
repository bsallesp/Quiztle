//using BrunoTheBot.CoreBusiness;
//using BrunoTheBot.CoreBusiness.Entities;
//using Microsoft.EntityFrameworkCore;
//using System.Diagnostics;

//namespace BrunoTheBot.DataContext
//{
//    public class SqliteDataContext : DbContext
//    {
//        // Magic string.
//        public static readonly string RowVersion = nameof(RowVersion);

//        // Magic strings.
//        public static readonly string BrunoTheBotDb = nameof(BrunoTheBotDb).ToLower();

//        // Inject options.
//        // options: The DbContextOptions{ContactContext} for the context.
//        public SqliteDataContext(DbContextOptions<SqliteDataContext> options)
//            : base(options)
//        {
//            Debug.WriteLine($"{ContextId} context created.");
//        }

//        public DbSet<School>? Schools { get; set; }
//        public DbSet<Topic>? Topics { get; set; }
//        public DbSet<Author>? Authors { get; set; }
//        public DbSet<Question>? Questions { get; set; }
//        public DbSet<Option>? Options { get; set; }
//        public DbSet<Answer>? Answers { get; set; }
//        public DbSet<AILog>? AILogs { get; set; }

//        // Define the model.
//        // modelBuilder: The ModelBuilder.
//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            // This property isn't on the C# class,
//            // so we set it up as a "shadow" property and use it for concurrency.
//            modelBuilder.Entity<School>()
//                .Property<byte[]>(RowVersion)
//                .IsRowVersion();
//            modelBuilder.Entity<Topic>()
//                .Property<byte[]>(RowVersion)
//                .IsRowVersion();

//            modelBuilder.Entity<Question>()
//                .Property<byte[]>(RowVersion)
//                .IsRowVersion();

//            modelBuilder.Entity<Author>()
//            .Property<byte[]>(RowVersion)
//            .IsRowVersion();

//            modelBuilder.Entity<Option>()
//                .Property<byte[]>(RowVersion)
//                .IsRowVersion();

//            modelBuilder.Entity<Answer>()
//                .Property<byte[]>(RowVersion)
//                .IsRowVersion();

//            modelBuilder.Entity<AILog>()
//            .Property<byte[]>(RowVersion)
//            .IsRowVersion();

//            base.OnModelCreating(modelBuilder);
//        }

//        // Dispose pattern.
//        public override void Dispose()
//        {
//            Debug.WriteLine($"{ContextId} context disposed.");
//            base.Dispose();
//        }

//        // Dispose pattern.
//        public override ValueTask DisposeAsync()
//        {
//            Debug.WriteLine($"{ContextId} context disposed async.");
//            return base.DisposeAsync();
//        }
//    }
//}
