using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using MongoDB.EntityFrameworkCore.Extensions;
using SCHALE.Common.Database.Models;

namespace SCHALE.Common.Database
{
    public class SCHALEContext : DbContext
    {
        public DbSet<GuestAccount> GuestAccounts { get; set; }
        public DbSet<AccountDB> Accounts { get; set; }
        public DbSet<MissionProgressDB> MissionProgresses { get; set; }
        public DbSet<Counter> Counters { get; set; }

        public SCHALEContext(DbContextOptions<SCHALEContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GuestAccount>().Property(x => x.Uid).HasValueGenerator<GuestAccountAutoIncrementValueGenerator>();
            modelBuilder.Entity<GuestAccount>().ToCollection("guest_accounts");

            modelBuilder.Entity<AccountDB>().Property(x => x.ServerId).HasValueGenerator<AccountAutoIncrementValueGenerator>();
            modelBuilder.Entity<AccountDB>().ToCollection("accounts");

            modelBuilder.Entity<MissionProgressDB>().Property(x => x.ServerId).HasValueGenerator<MissionProgressAutoIncrementValueGenerator>();
            modelBuilder.Entity<MissionProgressDB>().ToCollection("mission_progresses");

            modelBuilder.Entity<Counter>().ToCollection("counters");
        }

        private class AccountAutoIncrementValueGenerator : AutoIncrementValueGenerator
        {
            protected override string Collection => "account";
        }

        private class GuestAccountAutoIncrementValueGenerator : AutoIncrementValueGenerator
        {
            protected override string Collection => "guest_account";
        }

        private class MissionProgressAutoIncrementValueGenerator : AutoIncrementValueGenerator
        {
            protected override string Collection => "mission_progress";
        }

        private abstract class AutoIncrementValueGenerator : ValueGenerator<long>
        {
            protected abstract string Collection { get; }
            public override bool GeneratesTemporaryValues => false;

            public override long Next(EntityEntry entry)
            {
                if (entry.Context is not SCHALEContext)
                {
                    throw new ArgumentNullException($"{nameof(AutoIncrementValueGenerator)} is only implemented for {nameof(SCHALEContext)}");
                }

                var context = ((SCHALEContext)entry.Context);
                var counter = context.Counters.SingleOrDefault(x => x.Id == Collection);

                if (counter is null)
                {
                    counter = new Counter() { Id = Collection, Seq = 0 };
                    context.Add(counter);
                }

                counter.Seq++;
                context.SaveChanges();

                return counter.Seq;
            }
        }
    }
}
