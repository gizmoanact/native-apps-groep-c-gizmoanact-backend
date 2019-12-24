using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MonsterAPI.Model.Interface;
using MonsterAPI.Model.Model_EF;

namespace MonsterAPI.Controllers
{
    [Route("api/[controller]")]
    public class ArtistController : ControllerBase
    {
        
        private IArtistRepository _artistRepository;
        public ArtistController(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }

        [Route("add")]
        [HttpPost]
        public IActionResult createArtist(Artist artist)
        { //Geef AterlierDTO en controleer de HTTPPut
            _artistRepository.Add(artist);
            _artistRepository.saveChanges();
            return Ok();
        }

        [Route("{id}")]
        [HttpDelete]
        public ActionResult<Artist> DeleteAlbum(int id)
        {
            try
            {
                Artist a = _artistRepository.GetById(id);

                if (a == null)
                {
                    return null;
                }

                _artistRepository.Remove(a);
                _artistRepository.saveChanges();

                return a;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<Artist>> GeefAlleClients()
        {
            try
            {
                return _artistRepository.GetAll().ToList();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
                //return null;
            }
        }
        [Route("{id}")]
        [HttpGet]
        public ActionResult<Artist> GetById(long id)
        {
            return _artistRepository.GetById(id);
        }

        [Route("put/{artist}")]
        [HttpPut]
        public ActionResult<Artist> UpadateArtist(Artist artist)
        {
            _artistRepository.Update(artist);
            return artist;
        }


    }
}

