using BonusSystemApplication.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BonusSystemApplication.DAL.EF
{
    public class DataContext : DbContext
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ppolocaldb");
        //    }
        //}

        public DataContext(DbContextOptions<DataContext> opts) : base(opts) { }
        public DbSet<Form> Forms { get; set; }
        public DbSet<Definition> Definitions { get; set; }
        public DbSet<ObjectiveResult> ObjectivesResults { get; set; }
        public DbSet<Conclusion> Conclusions { get; set; }
        public DbSet<Signatures> Signatures { get; set; }
        public DbSet<LocalAccess> LocalAccess { get; set; }
        public DbSet<GlobalAccess> GlobalAccess { get; set; }
        public DbSet<Workproject> Workprojects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Form configuring
            modelBuilder.Entity<Form>(f =>
            {
                f.HasOne(f => f.Definition)
                 .WithOne(d => d.Form)
                 .HasForeignKey<Definition>(d => d.FormId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Cascade);

                f.HasOne(f => f.Conclusion)
                 .WithOne(c => c.Form)
                 .HasForeignKey<Conclusion>(c => c.FormId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Cascade);

                f.HasOne(f => f.Signatures)
                 .WithOne(s => s.Form)
                 .HasForeignKey<Signatures>(s => s.FormId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Form>()
                .HasMany(f => f.ObjectivesResults)
                .WithOne(o => o.Form)
                .HasForeignKey(f => f.FormId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Form>().Property(f => f.IsObjectivesFreezed).HasDefaultValue(false);
            modelBuilder.Entity<Form>().Property(f => f.IsResultsFreezed).HasDefaultValue(false);
            #endregion

            #region Definition configuring
            modelBuilder.Entity<Definition>()
                .HasOne(d => d.Employee)
                .WithMany(u => u.EmployeeFormDefinitions)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict)      //user deletion must be forbidden (set IsActive to false instead)
                .IsRequired();

            modelBuilder.Entity<Definition>()
                .HasOne(d => d.Manager)
                .WithMany(u => u.ManagerFormDefinitions)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);      //user deletion must be forbidden (set IsActive to false instead)

            modelBuilder.Entity<Definition>()
                .HasOne(d => d.Approver)
                .WithMany(u => u.ApproverFormDefinitions)
                .HasForeignKey(d => d.ApproverId)
                .OnDelete(DeleteBehavior.Restrict);     //user deletion must be forbidden (set IsActive to false instead)

            modelBuilder.Entity<Definition>()
                .HasOne(d => d.Workproject)
                .WithMany(wp => wp.FormDefinitions)
                .HasForeignKey(d => d.WorkprojectId)
                .OnDelete(DeleteBehavior.Restrict)      //workprojects deletion must be forbidden (set IsActive to false instead)
                .IsRequired();

            modelBuilder.Entity<Definition>()
                .HasIndex(d => new { d.EmployeeId, d.WorkprojectId, d.Period, d.Year }).IsUnique();

            modelBuilder.Entity<Definition>().Property(d => d.Period).IsRequired();
            modelBuilder.Entity<Definition>().Property(d => d.Year).IsRequired();

            modelBuilder.Entity<Definition>().Property(d => d.IsWpmHox).HasDefaultValue(false);
            #endregion

            #region Conclusion configuring
            modelBuilder.Entity<Conclusion>().Property(c => c.IsProposalForBonusPayment).HasDefaultValue(false);
            #endregion

            #region LocalAccess configuring

            //defines composite key for many to many
            modelBuilder.Entity<LocalAccess>().HasKey(la => new { la.FormId, la.UserId });

            //potentially to delete because we already following naming convention for ids: FormId and UserId
            modelBuilder.Entity<LocalAccess>()
                .HasOne(la => la.Form)
                .WithMany(f => f.LocalAccesses)
                .HasForeignKey(la => la.FormId);

            modelBuilder.Entity<LocalAccess>()
                .HasOne(la => la.User)
                .WithMany(u => u.LocalAccesses)
                .HasForeignKey(la => la.UserId);

            #endregion

            #region ObjectiveResult configuring
            modelBuilder.Entity<ObjectiveResult>(or =>
            {
                or.OwnsOne(or => or.Objective, o =>
                {
                    o.Property(o => o.IsKey).HasDefaultValue(true);
                    o.Property(o => o.IsMeasurable).HasDefaultValue(true);
                });
                or.Navigation(or => or.Objective).IsRequired();

                or.OwnsOne(or => or.Result);
                or.Navigation(or => or.Result).IsRequired();

                or.HasIndex(o => new { o.FormId, o.Row }).IsUnique();
            });
            #endregion

            #region Signatures configuration
            modelBuilder.Entity<Signatures>().Property(s => s.ForObjectivesIsSignedByEmployee).HasDefaultValue(false);
            modelBuilder.Entity<Signatures>().Property(s => s.ForObjectivesIsRejectedByEmployee).HasDefaultValue(false);
            modelBuilder.Entity<Signatures>().Property(s => s.ForObjectivesIsSignedByManager).HasDefaultValue(false);
            modelBuilder.Entity<Signatures>().Property(s => s.ForObjectivesIsSignedByApprover).HasDefaultValue(false);

            modelBuilder.Entity<Signatures>().Property(s => s.ForResultsIsSignedByEmployee).HasDefaultValue(false);
            modelBuilder.Entity<Signatures>().Property(s => s.ForResultsIsRejectedByEmployee).HasDefaultValue(false);
            modelBuilder.Entity<Signatures>().Property(s => s.ForResultsIsSignedByManager).HasDefaultValue(false);
            modelBuilder.Entity<Signatures>().Property(s => s.ForResultsIsSignedByApprover).HasDefaultValue(false);
            #endregion

            #region Workproject configuring

            modelBuilder.Entity<Workproject>().HasIndex(w => w.Name).IsUnique();
            modelBuilder.Entity<Workproject>().Property(w => w.Name).IsRequired();

            modelBuilder.Entity<Workproject>().Property(w => w.IsActive).HasDefaultValue(true);

            #endregion

            #region User configuring

            modelBuilder.Entity<User>()
                .HasOne(u => u.Department)
                .WithMany(d => d.Users)
                .HasForeignKey(u => u.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Team)
                .WithMany(t => t.Users)
                .HasForeignKey(u => u.TeamId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Position)
                .WithMany(p => p.Users)
                .HasForeignKey(u => u.PositionId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<User>().HasIndex(u => u.Login).IsUnique();
            modelBuilder.Entity<User>().Property(u => u.Login).IsRequired();

            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<User>().Property(u => u.Email).IsRequired();

            modelBuilder.Entity<User>().HasIndex(u => u.Pid).IsUnique();

            modelBuilder.Entity<User>().Property(u => u.IsActive).HasDefaultValue(true);
            #endregion

            #region Department configuring

            modelBuilder.Entity<Department>().HasIndex(d => d.Name).IsUnique();
            modelBuilder.Entity<Department>().Property(d => d.Name).IsRequired();

            #endregion

            #region Position configuring

            modelBuilder.Entity<Position>().HasIndex(p => p.NameEng).IsUnique();
            modelBuilder.Entity<Position>().Property(p => p.NameEng).IsRequired();

            modelBuilder.Entity<Position>().HasIndex(p => p.NameRus).IsUnique();
            modelBuilder.Entity<Position>().Property(p => p.NameRus).IsRequired();

            #endregion

            #region Team configuring

            modelBuilder.Entity<Team>().HasIndex(t => t.Name).IsUnique();
            modelBuilder.Entity<Team>().Property(t => t.Name).IsRequired();

            #endregion

        }
    }
}
