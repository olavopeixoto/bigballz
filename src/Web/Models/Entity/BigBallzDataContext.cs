using System.Data.Common;
using System.Data.Entity;

namespace BigBallz
{
    public class BigBallzDataContext : DbContext
    {
        public BigBallzDataContext()
            : base("name=BigBallzContext")
        {}

        public BigBallzDataContext(DbConnection connection, bool contextOwnConnection = true)
            : base(connection, contextOwnConnection)
        { }

        public virtual DbSet<Bet> Bets { get; set; }
        public virtual DbSet<Bonus> Bonus { get; set; }
        public virtual DbSet<BonusBet> BonusBets { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Match> Matches { get; set; }
        public virtual DbSet<PaymentStatus> PaymentStatus { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Stage> Stages { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserMapping> UserMappings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bonus>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Bonus>()
                .Property(e => e.Team)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<BonusBet>()
                .Property(e => e.Team)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Comment>()
                .Property(e => e.Comments)
                .IsUnicode(false);

            modelBuilder.Entity<Group>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Group>()
                .HasMany(e => e.Bonus)
                .WithOptional(e => e.Group1)
                .HasForeignKey(e => e.Group);

            modelBuilder.Entity<Group>()
                .HasMany(e => e.Teams)
                .WithRequired(e => e.Group1)
                .HasForeignKey(e => e.GroupId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Match>()
                .Property(e => e.Team1Id)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Match>()
                .Property(e => e.Team2Id)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Match>()
                .HasMany(e => e.Bets)
                .WithRequired(e => e.Match1)
                .HasForeignKey(e => e.Match)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PaymentStatus>()
                .Property(e => e.Transaction)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PaymentStatus>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentStatus>()
                .Property(e => e.SenderEmail)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentStatus>()
                .Property(e => e.SenderName)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentStatus>()
                .Property(e => e.PaymentMethod)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Roles)
                .Map(m => m.ToTable("UserRole").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<Stage>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Stage>()
                .HasMany(e => e.Matches)
                .WithRequired(e => e.Stage1)
                .HasForeignKey(e => e.StageId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Team>()
                .Property(e => e.TeamId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Team>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Team>()
                .HasMany(e => e.Bonus)
                .WithOptional(e => e.Team1)
                .HasForeignKey(e => e.Team);

            modelBuilder.Entity<Team>()
                .HasMany(e => e.BonusBets)
                .WithRequired(e => e.Team1)
                .HasForeignKey(e => e.Team)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Team>()
                .HasMany(e => e.Matches)
                .WithOptional(e => e.Team1)
                .HasForeignKey(e => e.Team1Id);

            modelBuilder.Entity<Team>()
                .HasMany(e => e.Matches1)
                .WithOptional(e => e.Team2)
                .HasForeignKey(e => e.Team2Id);

            modelBuilder.Entity<User>()
                .Property(e => e.PhotoUrl)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.AuthorizedBy)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Bets)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.BonusBets)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.PaymentStatus)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserMappings)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserMapping>()
                .Property(e => e.ProviderName)
                .IsUnicode(false);
        }
    }
}