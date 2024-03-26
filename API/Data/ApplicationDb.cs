using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class ApplicationDb:DbContext
    {
        public ApplicationDb(DbContextOptions<ApplicationDb> options):base(options)
        {
            
        }
        public DbSet<Genre>Genres { get; set; }
        public DbSet<Artist>Artists { get; set; }
        public DbSet<ArtistAlbumBriger> ArtistAlbumBrigers { get; set; }
        public DbSet<Album> albums { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ArtistAlbumBriger>()
                .HasKey(x => new { x.ArtistId, x.AlbumId });

            modelBuilder.Entity<Artist>()
                .HasMany(x => x.ArtistAlbums)
                .WithOne(x => x.Artist)
                .HasForeignKey(x => x.ArtistId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Album>()
                .HasMany(x => x.ArtistAlbums)
                .WithOne(x => x.album)
                .HasForeignKey(x => x.AlbumId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
