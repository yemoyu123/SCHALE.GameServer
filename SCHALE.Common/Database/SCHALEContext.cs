using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using SCHALE.Common.Database.Models;

namespace SCHALE.Common.Database
{
    public class SCHALEContext : DbContext
    {
        public DbSet<GuestAccount> GuestAccounts { get; set; }
        public DbSet<AccountDB> Accounts { get; set; }
        public DbSet<MissionProgressDB> MissionProgresses { get; set; }

        public DbSet<ItemDB> Items { get; set; }
        public DbSet<CharacterDB> Characters { get; set; }
        public DbSet<EquipmentDB> Equipment { get; set; }
        public DbSet<WeaponDB> Weapons { get; set; }
        public DbSet<GearDB> Gears { get; set; }

        public DbSet<MemoryLobbyDB> MemoryLobbies { get; set; }
        public DbSet<ScenarioHistoryDB> Scenarios { get; set; }

        public DbSet<EchelonDB> Echelons { get; set; }
        public DbSet<AccountTutorial> AccountTutorials { get; set; }

        public static SCHALEContext Create(string connectionString) =>
            new(new DbContextOptionsBuilder<SCHALEContext>()
                .UseSqlServer(connectionString)
                .Options);

        public SCHALEContext()
        {
        }

        public SCHALEContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GuestAccount>().Property(x => x.Uid).ValueGeneratedOnAdd();

            modelBuilder.Entity<AccountDB>().Property(x => x.ServerId).ValueGeneratedOnAdd();
            modelBuilder.Entity<AccountDB>()
                .HasMany(x => x.Items)
                .WithOne(x => x.Account)
                .HasForeignKey(x => x.AccountServerId)
                .IsRequired();
            modelBuilder.Entity<AccountDB>()
                .HasMany(x => x.Characters)
                .WithOne(x => x.Account)
                .HasForeignKey(x => x.AccountServerId)
                .IsRequired();
            modelBuilder.Entity<AccountDB>()
                .HasMany(x => x.MissionProgresses)
                .WithOne(x => x.Account)
                .HasForeignKey(x => x.AccountServerId)
                .IsRequired();
            modelBuilder.Entity<AccountDB>()
                .HasMany(x => x.Equipment)
                .WithOne(x => x.Account)
                .HasForeignKey(x => x.AccountServerId)
                .IsRequired();
            modelBuilder.Entity<AccountDB>()
                .HasMany(x => x.Weapons)
                .WithOne(x => x.Account)
                .HasForeignKey(x => x.AccountServerId)
                .IsRequired();
            modelBuilder.Entity<AccountDB>()
                .HasMany(x => x.Gears)
                .WithOne(x => x.Account)
                .HasForeignKey(x => x.AccountServerId)
                .IsRequired();
            modelBuilder.Entity<AccountDB>()
                .HasMany(x => x.MemoryLobbies)
                .WithOne(x => x.Account)
                .HasForeignKey(x => x.AccountServerId)
                .IsRequired();
            modelBuilder.Entity<AccountDB>()
                .HasMany(x => x.Scenarios)
                .WithOne(x => x.Account)
                .HasForeignKey(x => x.AccountServerId)
                .IsRequired();

            modelBuilder.Entity<AccountDB>(x => x.Property(b => b.RaidInfo).HasJsonConversion());
            modelBuilder.Entity<ItemDB>().Property(x => x.ServerId).ValueGeneratedOnAdd();
            modelBuilder.Entity<EquipmentDB>().Property(x => x.ServerId).ValueGeneratedOnAdd();
            modelBuilder.Entity<WeaponDB>().Property(x => x.ServerId).ValueGeneratedOnAdd();
            modelBuilder.Entity<GearDB>().Property(x => x.ServerId).ValueGeneratedOnAdd();

            modelBuilder.Entity<EchelonDB>().Property(x => x.ServerId).ValueGeneratedOnAdd();

            modelBuilder.Entity<CharacterDB>().Property(x => x.ServerId).ValueGeneratedOnAdd();
            modelBuilder.Entity<CharacterDB>().Property(x => x.EquipmentSlotAndDBIds).HasJsonConversion();
            modelBuilder.Entity<CharacterDB>().Property(x => x.PotentialStats).HasJsonConversion();

            modelBuilder.Entity<AccountTutorial>().Property(x => x.AccountServerId).ValueGeneratedNever();

            modelBuilder.Entity<MissionProgressDB>().Property(x => x.ServerId).ValueGeneratedOnAdd();
            modelBuilder.Entity<MissionProgressDB>().Property(x => x.ProgressParameters).HasJsonConversion();
        }
    }

    public class SCHALESqliteContext : SCHALEContext
    {
        public SCHALESqliteContext() { }

        public SCHALESqliteContext(DbContextOptions options) : base(options)
        {
        }

        public static SCHALESqliteContext Create(string connectionString) =>
            new(new DbContextOptionsBuilder<SCHALESqliteContext>()
                .UseSqlite(connectionString).Options);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=schale.sqlite3");
        }
    }

    public static class PropertyBuilderExtensions
    {
        public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder) where T : class, new()
        {
            ValueConverter<T, string> converter = new
            (
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<T>(v) ?? new T()
            );

            propertyBuilder.HasConversion(converter);
            propertyBuilder.Metadata.SetValueConverter(converter);

            return propertyBuilder;
        }
    }
}
