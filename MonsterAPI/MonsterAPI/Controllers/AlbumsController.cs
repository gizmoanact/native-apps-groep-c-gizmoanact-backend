using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MonsterAPI.Model.DTO;
using MonsterAPI.Model.Interface;
using MonsterAPI.Model.Model_EF;

namespace MonsterAPI.Controllers
{

    [Route("api/[controller]")]
    public class AlbumController : ControllerBase
    {

        private readonly IAlbumRepository _albumRepository;
        public AlbumController(IAlbumRepository albumRepository)
        {
            _albumRepository = albumRepository;
        }

        [HttpPost]
        public IActionResult createAlbum(Album album)
        { //Geef AterlierDTO en controleer de HTTPPut
            _albumRepository.Add(album);
            _albumRepository.saveChanges();
            return Ok();
        }
        [Route("{id}")]
        [HttpDelete]
        public ActionResult<Album> DeleteAlbum(int id)
        {
            try
            {
                Album a = _albumRepository.GetById(id);

                if (a == null)
                {
                    return null;
                }

                _albumRepository.Remove(a);
                _albumRepository.saveChanges();

                return a;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<Album>> GeefAlleClients()
        {
            try
            {
                return _albumRepository.GetAll().ToList();
            }
            catch (Exception e)
            {
                 return BadRequest(e.Message);
                //return null;
            }
        }
        [Route("{id}")]
        [HttpGet]
        public ActionResult<Album> GetById(long id)
        {
            return _albumRepository.GetById(id);
        }

        [Route("{album}")]
        [HttpPut]
        public IActionResult UpdateMenu(Album album)
        {
            _albumRepository.Update(album);
            return Ok();
        }

       
    }
}
