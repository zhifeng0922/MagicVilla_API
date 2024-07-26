﻿using MagicVilla_VillaAPI.Controllers.Models;
using MagicVilla_VillaAPI.Controllers.Models.Dto;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.logging;
//using Microsoft.AspNetCore.JsonPatch;
//using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/villaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        //改换Register to the Container， 1st Removed default logger
        //private readonly ILogger<VillaAPIController> _logger;
        private readonly ILogging _logger;
        //public VillaAPIController(ILogger<VillaAPIController> logger)
        //改换Register to the Container， 1st Removed default logger
        public VillaAPIController(ILogging logger)
        {
            //改换Register to the Container， 1st Removed default logger
            //_logger = logger;
            //改换Register to the Container， 1st Removed default logger，2nd delete log folder
            _logger = logger;

        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult <IEnumerable<VillaDTO>> GetVillas() 
        {
            _logger.Log("Getting all villad", "");

            return Ok(VillaStore.villaList);
          
        }
        [HttpGet("{id:int}",Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        //[ProducesResponseType(200, Type =typeof(VillaDTO))]
        //[ProducesResponseType(404)]
        //[ProducesResponseType(400)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.Log("Get Villa Error with Id" + id, "error");

               return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
               return NotFound();
            }
            return Ok(VillaStore.villaList.FirstOrDefault(u => u.Id == id));
        }

        [HttpPost]

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDTO)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            if (VillaStore.villaList.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null) 
                {
                ModelState.AddModelError("CustomError", "Villa already Exist!");
                return BadRequest(ModelState);
                }
            if (villaDTO == null) 
            {
                return BadRequest(villaDTO);
            }
            if (villaDTO.Id == 0) 
            { 
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            villaDTO.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            VillaStore.villaList.Add(villaDTO);

            return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);        
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
            //public IActionResult DeleteVilla(int id) 
            public IActionResult DeleteVilla(int id)
            {
                if(id == 0)
            {
                return BadRequest(); // Return BadRequest if id is 0
            }
            {
                 var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
                if (villa == null)
                {
                    return NotFound();
                }
                VillaStore.villaList.Remove(villa);
                //return NoContent(); // 1:11:25
                return NoContent();
                }
            }
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)

        {
            if (villaDTO == null || id != villaDTO.Id) 
                {
                return BadRequest();
                }
                var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            villa.Name = villaDTO.Name;
            villa.Sqft = villaDTO.Sqft;
            villa.Occupancy = villaDTO.Occupancy;

            return NoContent();
        }
        //[HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        ////[HttpPatch("{id:int}"), Name = "UpdatePartialVillar")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]

        //public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
        //{
        //    if (patchDTO == null || id == 0 )
        //    {
        //        return BadRequest();
        //    }
        //    var villa = VillaStore.villaList.FirstOrDefault(u => u.Id ==id);
        //    if (villa == null)
        //    {
        //        return BadRequest();
        //    }
        //    patchDTO.ApplyTo(villaDTO, ModelState);
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    return NoContent() ;
        //}

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
        
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }

            var villaDTO = new VillaDTO
            {
                Id = villa.Id,
                Name = villa.Name,
                Sqft = villa.Sqft,
                Occupancy = villa.Occupancy
                // Add other properties as needed
            };

            patchDTO.ApplyTo(villaDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Apply changes back to the villa object
            villa.Name = villaDTO.Name;
            villa.Sqft = villaDTO.Sqft;
            villa.Occupancy = villaDTO.Occupancy;
            // Update other properties as needed

            return NoContent();
        }
    }
}
