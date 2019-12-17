using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MonsterAPI.Model.Interface;
using MonsterAPI.Model.Model_EF;

namespace MonsterAPI.Controllers
{
    [Route("api/[controller]")]
    public class LiedjesController:ControllerBase
    {

        private ILiedjeRepository _liedjeRepository;
        public LiedjesController(ILiedjeRepository liedjeRepository)
        {
            _liedjeRepository = liedjeRepository;
        }

        [HttpPost]
        public IActionResult createAlbum(Liedje liedje)
        { //Geef AterlierDTO en controleer de HTTPPut
            _liedjeRepository.Add(liedje);
            _liedjeRepository.saveChanges();
            return Ok();
        }
        [Route("{id}")]
        [HttpDelete]
        public ActionResult<Liedje> DeleteAlbum(int id)
        {
            try
            {
                Liedje l = _liedjeRepository.GetById(id);

                if (l == null)
                {
                    return null;
                }

                _liedjeRepository.Remove(l);
                _liedjeRepository.saveChanges();

                return l;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<Liedje>> GeefAlleClients()
        {
            try
            {
                return _liedjeRepository.GetAll().ToList();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
                //return null;
            }
        }
        [Route("{id}")]
        [HttpGet]
        public ActionResult<Liedje> GetById(long id)
        {
            return _liedjeRepository.GetById(id);
        }

        [Route("{album}")]
        [HttpPut]
        public IActionResult UpdateMenu(Liedje liedje)
        {
            _liedjeRepository.Update(liedje);
            return Ok();
        }


    }
}

