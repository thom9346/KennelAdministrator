using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using KennelIndexer.API.Models;
using KennelIndexer.API.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KennelIndexer.API.Controllers
{
    [Route("api/pictures")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        private readonly IPersonLibraryRepository _personLibraryRepository;
        private readonly IMapper _mapper;


        public PicturesController(IPersonLibraryRepository personLibraryRepository, IMapper mapper)
        {
            _personLibraryRepository = personLibraryRepository ??
                throw new ArgumentNullException(nameof(personLibraryRepository));

            _mapper = mapper ??
                 throw new ArgumentNullException(nameof(mapper));

        }
        [HttpGet("{personId}", Name = "GetPictures")]
        public IActionResult GetPictures(Guid personId)
        {
            var picturesFromRepo = _personLibraryRepository.GetPictures(personId);
            return Ok(_mapper.Map<IEnumerable<PictureDto>>(picturesFromRepo));
        }
        [HttpGet("{personId}/{pictureId}", Name = "GetPicture")]
        public IActionResult GetPicture(Guid personId, Guid pictureId)
        {
            try
            {
                var pictureFromRepo = _personLibraryRepository.GetPicture(personId, pictureId);

                if (pictureFromRepo == null)
                {
                    const string errorMessage = "Picture could not be found.";
                    return NotFound(errorMessage);
                }
                return Ok(_mapper.Map<PictureDto>(pictureFromRepo));
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

        }
        [HttpPost]
        public ActionResult<PictureDto> PostPictures(
            [FromForm] PictureForCreationDto pictures, List<IFormFile> files)
        {
            var pictureEntity = _mapper.Map<Entities.Picture>(pictures);
            _personLibraryRepository.AddPicture(pictures.PersonId, pictureEntity);
            _personLibraryRepository.Save();

            var picturesToReturn = _mapper.Map<PictureDto>(pictureEntity);

            return CreatedAtRoute("GetPictures", new { pictureId = picturesToReturn.PictureId }, picturesToReturn);
        }
        [Route("Upload")]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            try
            {
          

                long size = files.Sum(f => f.Length);

                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                var fileExtension = Path.GetExtension(files.FirstOrDefault().FileName);
                var fileName = Guid.NewGuid() + fileExtension;
                var fullPath = Path.Combine(folderName, fileName);

                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {

                        using (var stream = System.IO.File.Create(fullPath))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                        return Ok(new { fullPath });
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}