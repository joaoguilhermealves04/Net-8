using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Dto
{
    public class AlbumAddEditDto
    {
        
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        public string PhotoUrl { get; set; }

        public List<int> Artisid { get; set; }
    }
}
