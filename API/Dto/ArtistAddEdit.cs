using System.ComponentModel.DataAnnotations;

namespace API.Dto
{
    public class ArtistAddEdit
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50,ErrorMessage ="Por favor numero tem que ter de {0} ate 50 caracteres.")]
        public string Name { get; set; }

        public string PhotoUrl { get; set; }

        [Required]
        public string Genre { get; set; }
    }
}
