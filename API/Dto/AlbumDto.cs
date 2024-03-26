using System.Collections.Generic;

namespace API.Dto
{
    public class AlbumDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }

        public List<ArtistDto> artists { get; set; }
    }
}
