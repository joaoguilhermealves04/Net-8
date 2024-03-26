using API.Data;
using API.Dto;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : Controller
    {
        private readonly ApplicationDb _db;
        public AlbumController(ApplicationDb applicationDb)
        {
            _db = applicationDb;
        }

        [HttpGet("Get-all")]
        public async Task<ActionResult<List<AlbumDto>>> GetAll()
        {
            var album = await _db.albums
                .Select(x => new AlbumDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    PhotoUrl = x.PhotoUrl,
                    artists = x.ArtistAlbums.Select(a => new ArtistDto
                    {
                        Id = a.Artist.Id,
                        Name = a.Artist.Name,
                        PhotoUrl = a.Artist.PhotoUrl,
                        Genre = a.Artist.genre.Nome

                    }).ToList(),

                }).ToListAsync();

            if (album == null) return NotFound();
            return album;
        }

        [HttpPut("Update")]
        public async Task <IActionResult> Update(AlbumAddEditDto editDto)
        {
            var fethedAlbum = await _db.albums.Include(x => x.ArtistAlbums).FirstOrDefaultAsync(x=> x.Id == editDto.id);
            if(fethedAlbum == null) return NotFound();  

            if(fethedAlbum.Name != editDto.Name.ToLower() && await AlbumExiste(editDto.Name))
            {
                return BadRequest("Album name should be unique");

            }

            //clean all existing Artists
            foreach(var artist in fethedAlbum.ArtistAlbums )
            {
                var fetchedArtistAlbumBridge = await _db.ArtistAlbumBrigers
                   .SingleOrDefaultAsync(x => x.ArtistId== artist.ArtistId && x.AlbumId == fethedAlbum.Id);
                _db.ArtistAlbumBrigers.Remove(fetchedArtistAlbumBridge);
            }
            await _db.SaveChangesAsync();

            fethedAlbum.Name = editDto.Name;
            fethedAlbum.PhotoUrl = editDto.PhotoUrl;

            await AssingArtistsToAlbumAsync(fethedAlbum.Id, editDto.Artisid);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public async Task <IActionResult> Delete(int id)
        {
            var fethedAlbum = await _db.albums.Include(x => x.ArtistAlbums).FirstOrDefaultAsync(x => x.Id == id);
            if (fethedAlbum == null) return NotFound();

            //clean all existing Artists
            foreach (var artist in fethedAlbum.ArtistAlbums)
            {
                var fetchedArtistAlbumBridge = await _db.ArtistAlbumBrigers
                   .SingleOrDefaultAsync(x => x.ArtistId == artist.ArtistId && x.AlbumId == fethedAlbum.Id);
                _db.ArtistAlbumBrigers.Remove(fetchedArtistAlbumBridge);
            }
            await _db.SaveChangesAsync();

            _db.albums.Remove(fethedAlbum);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(AlbumAddEditDto album)
        {
            if(await AlbumExiste(album.Name))
            {
                return BadRequest("Album name sould be unique");
            }
            if (album.Artisid.Count == 0)
            {
                return BadRequest("At laest one artist id should be seleceted");
            }
            var albumToAdd = new Album
            {
                Name = album.Name,
                PhotoUrl = album.PhotoUrl,
            };
            _db.albums.Add(albumToAdd);
            await _db.SaveChangesAsync();

            await AssingArtistsToAlbumAsync(albumToAdd.Id, album.Artisid);
            await _db.SaveChangesAsync();

            return NoContent();
        }


        private async Task<bool> AlbumExiste(string album)
        {
            return await _db.albums.AnyAsync(x=> x.Name.ToLower()== album.ToLower());
        }

        private async Task AssingArtistsToAlbumAsync(int albunId, List<int> artistIds)
        {
            artistIds = artistIds.Distinct().ToList();
            foreach (var artistid in artistIds)
            {
                var artist = await _db.Artists.FindAsync(artistIds);
                if (artist == null)
                {
                    var artistAlbumBridgeToAdd = new ArtistAlbumBriger
                    {
                        AlbumId = artistid,
                        ArtistId = artistid,
                    };
                    _db.ArtistAlbumBrigers.Add(artistAlbumBridgeToAdd);
                }
            }
        }
        
    }
}
