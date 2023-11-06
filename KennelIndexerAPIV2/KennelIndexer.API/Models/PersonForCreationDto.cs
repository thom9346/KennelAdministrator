using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KennelIndexer.API.Models
{
    public class PersonForCreationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string ReasonsForBeingOnTheList { get; set; }

        public ICollection<PictureForCreationDto> Pictures { get; set; }
            = new List<PictureForCreationDto>();


    }
}
