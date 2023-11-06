using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using BrunoZell.ModelBinding;
using KennelIndexer.API.Models;
using KennelIndexer.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KennelIndexer.API.Controllers
{
    [Route("api/people")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IPersonLibraryRepository _personLibraryRepositry;
        private readonly IMapper _mapper;

        public PeopleController(IPersonLibraryRepository personLibraryRepository, IMapper mapper)
        {
            _personLibraryRepositry = personLibraryRepository ??
                throw new ArgumentNullException(nameof(personLibraryRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public IActionResult GetPeople()
        {
            var peopleFromRepo = _personLibraryRepositry.GetPeople();
            return Ok(_mapper.Map<IEnumerable<PersonDto>>(peopleFromRepo));
        }
        [HttpGet("{personId}", Name = "GetPerson")]
        public IActionResult GetPerson(Guid personId)
        {
            try
            {
                var personFromRepo = _personLibraryRepositry.GetPerson(personId);

                if (personFromRepo == null)
                {
                    const string errorMessage = "Person was not found";
                    return NotFound(errorMessage);
                }

                return Ok(_mapper.Map<PersonDto>(personFromRepo));
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost, DisableRequestSizeLimit]
        public ActionResult<PersonDto> PostPerson([FromForm] PersonForCreationDto person)
        {

            var files = Request.Form.Files;
            var dbPaths = new List<string>();

            if (files.Any(f => f.Length > 0))
            {
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);


                foreach (var file in files)
                {
                    var fileExtension = Path.GetExtension(files.FirstOrDefault().FileName);
                    var fileName = Guid.NewGuid() + fileExtension;
                    var fullPath = Path.Combine(pathToSave, fileName);
                    dbPaths.Add(Path.Combine(folderName, fileName));

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }
            }

            var personEntity = _mapper.Map<Entities.Person>(person); //Maps PersonForCreationDto to Entites.Person. This is possible because of the mapping in PeopleProfile.cs
            _personLibraryRepositry.AddPerson(personEntity, dbPaths);
            _personLibraryRepositry.Save();

            var personToReturn = _mapper.Map<PersonDto>(personEntity);

            return CreatedAtRoute("GetPerson",
                new { personId = personToReturn.PersonId },
                personToReturn);
        }
        [HttpPut("{personId}")]
        public IActionResult UpdatePerson([FromForm] PersonDto person)
        {

            var personFromRepo = _personLibraryRepositry.GetPerson(person.PersonId);
            if (personFromRepo == null)
            {
                var personToAdd = _mapper.Map<Entities.Person>(person);

                _personLibraryRepositry.AddPerson(personToAdd);
                _personLibraryRepositry.Save();

                var personToReturn = _mapper.Map<PersonDto>(personToAdd);

                return CreatedAtRoute("GetPerson",
                  new { personId = personToReturn.PersonId },
                  personToReturn);

            }

            _mapper.Map(person, personFromRepo);

            _personLibraryRepositry.UpdatePerson(personFromRepo);
            _personLibraryRepositry.Save();
            return NoContent();
        }

        [HttpDelete("{personId}")]
        public IActionResult DeletePerson(Guid personId)
        {
            if(!_personLibraryRepositry.PersonExists(personId))
            {
                return NotFound();
            }
            var personEntity = _personLibraryRepositry.GetPerson(personId);
            _personLibraryRepositry.DeletePerson(personEntity);
            _personLibraryRepositry.Save();

            return NoContent();

        }
    }
}