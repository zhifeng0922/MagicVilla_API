using MagicVilla_VillaAPI.Controllers.Models;
using MagicVilla_VillaAPI.Controllers.Models.Dto;
using MagicVilla_VillaAPI.Data;
//using MagicVilla_VillaAPI.logging;
//using Microsoft.AspNetCore.JsonPatch;
//using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MagicVilla_VillaAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/villaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        //改换Register to the Container， 1st Removed default logger
        //private readonly ILogger<VillaAPIController> _logger;
        //private readonly ILogging _logger;
        //public VillaAPIController(ILogger<VillaAPIController> logger)
        //改换Register to the Container， 1st Removed default logger

        private readonly ApplicationDbContext _db;
        public VillaAPIController(/*ILogging logger*/ApplicationDbContext db)
        {
            //改换Register to the Container， 1st Removed default logger
            //_logger = logger;
            //改换Register to the Container， 1st Removed default logger，2nd delete log folder
            //_logger = logger;
            _db = db;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            //_logger.Log("Getting all villad", "");

            return Ok(_db.Villas.ToList());

        }
        [HttpGet("{id:int}", Name = "GetVilla")]
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
                //_logger.Log("Get Villa Error with Id" + id, "error");

                return BadRequest();
            }
            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            return Ok(/*VillaStore.villaList.FirstOrDefault(u => u.Id == id)*/villa);
        }

        [HttpPost]

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDTO)
        //{
        //    //if (!ModelState.IsValid)
        //    //{
        //    //    return BadRequest(ModelState);
        //    //}
        //    if (_db.Villas.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null) 
        //        {
        //        ModelState.AddModelError("CustomError", "Villa already Exist!");
        //        return BadRequest(ModelState);
        //        }
        //    if (villaDTO == null) 
        //    {
        //        return BadRequest(villaDTO);
        //    }
        //    if (villaDTO.Id == 0) 
        //    { 
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //    //villaDTO.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
        //    //VillaStore.villaList.Add(villaDTO);

        //    Villa model = new()
        //    {
        //        Amenity = villaDTO.Amenity,
        //        Details = villaDTO.Details,
        //        Id = villaDTO.Id,
        //        ImageUrl = villaDTO.ImageUrl,
        //        Name = villaDTO.Name,
        //        Occupancy = villaDTO.Occupancy,
        //        Rate = villaDTO.Rate,
        //        Sqft = villaDTO.Sqft
        //    };
        //    _db.Villas.Add(model);
        //    _db.SaveChanges();

        //    return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);        
        //}
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villaDTO)
        {
            if (_db.Villas.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already Exists!");
                return BadRequest(ModelState);
            }
            if (villaDTO == null)
            {
                return BadRequest(villaDTO);
            }
            //    if (villaDTO.Id > 0) 
            //    { 
            //        return StatusCode(StatusCodes.Status500InternalServerError);
            //    }
            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl,
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft
            };

            _db.Villas.Add(model);
            _db.SaveChanges();

            return CreatedAtRoute("GetVilla", new { id = model.Id }, villaDTO);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        //public IActionResult DeleteVilla(int id) 
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest(); // Return BadRequest if id is 0
            }
            {
                var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
                if (villa == null)
                {
                    return NotFound();
                }
                _db.Villas.Remove(villa);
                _db.SaveChanges();
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
            //    var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            //villa.Name = villaDTO.Name;
            //villa.Sqft = villaDTO.Sqft;
            //villa.Occupancy = villaDTO.Occupancy;

            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                Id = villaDTO.Id,
                ImageUrl = villaDTO.ImageUrl,
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft
            };
            _db.Villas.Update(model);
            _db.SaveChanges();

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

        //[HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)

        //{
        //    if (patchDTO == null || id == 0)
        //    {
        //        return BadRequest();
        //    }
        //    var villa = _db.Villas.FirstOrDefault(u => u.Id == id);

        //    VillaDTO villaDTO = new VillaDTO
        //    {
        //        Id = villa.Id,
        //        Name = villa.Name,
        //        Details = villa.Details,
        //        Rate = villa.Rate,
        //        Sqft = villa.Sqft,
        //        Occupancy = villa.Occupancy,
        //        ImageUrl = villa.ImageUrl,
        //        Amenity = villa.Amenity
        //    };

        //    if (villa == null)
        //    {
        //        return BadRequest();
        //    }

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

            var villa = _db.Villas.AsNoTracking().FirstOrDefault(u => u.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            //var villaDTO = new VillaDTO
            //{
            //    Id = villa.Id,
            //    Name = villa.Name,
            //    Sqft = villa.Sqft,
            //    Occupancy = villa.Occupancy
            //    // Add other properties as needed
            //};



            //patchDTO.ApplyTo(villaDTO, ModelState);

            //Villa model = new Villa()
            //{
            //    Amenity = villaDTO.Amenity,
            //    Details = villaDTO.Details,
            //    Id = villaDTO.Id,
            //    ImageUrl = villaDTO.ImageUrl,
            //    Name = villaDTO.Name,
            //    Occupancy = villaDTO.Occupancy,
            //    Rate = villaDTO.Rate,
            //    Sqft = villaDTO.Sqft
            //};

            //_db.Villas.Update(model);
            //_db.SaveChanges();

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            VillaDTO villaDTO = new VillaDTO
            {
                Id = villa.Id,
                Name = villa.Name,
                Details = villa.Details,
                Rate = villa.Rate,
                Sqft = villa.Sqft,
                Occupancy = villa.Occupancy,
                ImageUrl = villa.ImageUrl,
                Amenity = villa.Amenity
            };

            patchDTO.ApplyTo(villaDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //// Apply changes back to the villa object
            //villa.Name = villaDTO.Name;
            //villa.Sqft = villaDTO.Sqft;
            //villa.Occupancy = villaDTO.Occupancy;
            //// Update other properties as needed

            //            return NoContent();
            //        }
            //    }
            //}

            Villa model = _db.Villas.Find(id);
            if (model == null)
            {
                return NotFound();
            }

            model.Name = villaDTO.Name;
            model.Details = villaDTO.Details;
            model.Rate = villaDTO.Rate;
            model.Sqft = villaDTO.Sqft;
            model.Occupancy = villaDTO.Occupancy;
            model.ImageUrl = villaDTO.ImageUrl;
            model.Amenity = villaDTO.Amenity;

            _db.SaveChanges();

            return NoContent();
        }
    }
}