namespace API.Models
{
    public class ArtistAlbumBriger
    {
        public int AlbumId { get; set; }
        public Album album { get; set; }

        public int ArtistId { get; set; }
        public Artist Artist { get; set; }
    }
}
