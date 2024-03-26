using API.Data;
using API.Dto;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : Controller
    {
        private readonly ApplicationDb _connetionDatabase;
        public ArtistController(ApplicationDb connectionDatabase)
        {
            _connetionDatabase = connectionDatabase;
        }

        [HttpGet("get-All")]
        public ActionResult<List<ArtistDto>> GetAll()
        {
            var artits = _connetionDatabase.Artists
                .Select( x=> new ArtistDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Genre = x.genre.Nome
                }).ToList();

            return artits;
        }

        [HttpGet("get-one/{id}")]
        public ActionResult<ArtistDto> Get(int id)
        {
            var artist = _connetionDatabase.Artists
                .Where(x => x.Id == id)
                .Select(x => new ArtistDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Genre = x.genre.Nome
                }).FirstOrDefault();
                
            return artist;
        }

        [HttpPost("Create")]
        public ActionResult Create(ArtistAddEdit artist)
        {

            if (ArtisteNameExiste(artist.Name.ToLower()))
            {
                return BadRequest($"Artiste {artist.Name} Should be unique");
            }

            var fechtGenre = ExisteGenere(artist.Genre);
            if(fechtGenre == null)
            {
                return BadRequest();
            }

            var artistCriar = new Artist
            {
                Name = artist.Name.ToLower(),
                genre = fechtGenre,
                PhotoUrl = artist.PhotoUrl,
            };

            _connetionDatabase.Add(artistCriar);
            _connetionDatabase.SaveChanges();

            return NoContent();
        }

        [HttpPut("Update")]
        public ActionResult Update(ArtistAddEdit addEdit)
        {
            var fetchedArtist = _connetionDatabase.Artists.Find(addEdit.Id);
            if (fetchedArtist == null)
            {
                return NotFound();
            }

            if(fetchedArtist.Name!= addEdit.Name.ToLower() && ArtisteNameExiste(addEdit.Name)) 
            {
                return BadRequest($"Artiste {addEdit.Name} Should be unique");
            }

            var fetchedGenre = ExisteGenere(addEdit.Genre);
            if(fetchedGenre == null)
            {
                return BadRequest();
            }

            fetchedArtist.Name = addEdit.Name.ToLower();
            fetchedArtist.genre = fetchedGenre;
            fetchedArtist.PhotoUrl = fetchedArtist.PhotoUrl;
            _connetionDatabase.SaveChanges();

            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public ActionResult Delete(int id)
        {
            var fetchedArtista = _connetionDatabase.Artists.Find(id);
            if (fetchedArtista == null)
            {
                return NotFound();
            }

            _connetionDatabase.Artists.Remove(fetchedArtista);
            _connetionDatabase.SaveChanges();
            return NoContent();
        }

        private bool ArtisteNameExiste(string name)
        {
           return _connetionDatabase.Artists.Any(x => x.Name.ToLower() == name.ToLower());
        }
        private Genre ExisteGenere(string genere)
        {
            return _connetionDatabase.Genres.SingleOrDefault(x=> x.Nome.ToLower() == genere.ToLower());
        }
    }
}
