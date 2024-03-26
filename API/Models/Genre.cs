using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Genre
    {
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        public ICollection<Artist> Arstits {  get; set; } 
    }
}
