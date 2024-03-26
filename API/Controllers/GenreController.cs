using API.Data;
using API.Dto;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : Controller
    {
        private readonly ApplicationDb _connetionDatabase;
        public GenreController(ApplicationDb connectionDatabase)
        {
            _connetionDatabase = connectionDatabase;
        }
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            var geners = _connetionDatabase.Genres.ToList();
            var toReturn = new List<GenreDto>();

            foreach (var g in geners)
            {
                var genreDto = new GenreDto
                {
                    Id = g.Id,
                    Name = g.Nome
                };
                toReturn.Add(genreDto);
            }
            return Ok(toReturn);
        }
        [HttpGet("get-one/{id}")]
        public IActionResult GetOne(int id)
        {
            var genre = _connetionDatabase.Genres.Find(id);

            var toReturn = new GenreDto
            { 
                Id= genre.Id,
                Name = genre.Nome
            };

            return Ok(toReturn);
        }

        [HttpPost("Create")]
        public IActionResult Create (GenreAddDto genreAdd)
        {
            if (GenereNameExiste(genreAdd.Name)){
                return BadRequest($"Genre {genreAdd.Name} Should be unique");
            }
            var genreToAdd = new Genre
            {
                Nome = genreAdd.Name.ToLower()
            };

            _connetionDatabase.Add(genreToAdd);
            _connetionDatabase.SaveChanges();

            return NoContent();
        }

        [HttpPut("Update")]
        public IActionResult Update(GenreAddDto genre)
        {
            var fecthedGenre = _connetionDatabase.Genres.Find(genre.Id);
            if(fecthedGenre == null)
            {
                return NotFound();
            }
            if (GenereNameExiste(genre.Name))
            {
                return BadRequest("Genre name should be unique");
            }

            fecthedGenre.Nome = genre.Name.ToLower();
            _connetionDatabase.SaveChanges();
            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var fetchedObj = _connetionDatabase.Genres.Find(id);
            if (fetchedObj == null) return NotFound();

            _connetionDatabase.Genres.Remove(fetchedObj);
            _connetionDatabase.SaveChanges();
            return NoContent();
        }

        private bool GenereNameExiste(string name)
        {
            var fetchedGenre = _connetionDatabase.Genres.FirstOrDefault(x => x.Nome.ToLower()== name.ToLower());
            if (fetchedGenre != null)
            {
                return true;
            }
            return false;
        }
    }
}
