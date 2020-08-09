using Microsoft.EntityFrameworkCore;

namespace Songer.WebAPI.Models
{
    public class TestAuthContext : DbContext
    {
        public TestAuthContext(DbContextOptions<TestAuthContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Playlist> Playlists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Set user-song many-to-many

            modelBuilder.Entity<UserSong>().HasKey(sc => new { sc.UserId, sc.SongId });

            modelBuilder.Entity<UserSong>()
                        .HasOne<User>(sc => sc.User)
                        .WithMany(s => s.UserSongs)
                        .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<UserSong>()
                        .HasOne<Song>(sc => sc.Song)
                        .WithMany(s => s.UserSongs)
                        .HasForeignKey(sc => sc.SongId);

            #endregion

            #region Set playlist-song many-to-many

            modelBuilder.Entity<PlaylistSong>().HasKey(sc => new { sc.PlaylistId, sc.SongId });

            modelBuilder.Entity<PlaylistSong>()
                        .HasOne<Playlist>(sc => sc.Playlist)
                        .WithMany(s => s.PlaylistSongs)
                        .HasForeignKey(sc => sc.PlaylistId);

            modelBuilder.Entity<PlaylistSong>()
                        .HasOne<Song>(sc => sc.Song)
                        .WithMany(s => s.PlaylistSongs)
                        .HasForeignKey(sc => sc.SongId);

            #endregion
        }
    }
}
