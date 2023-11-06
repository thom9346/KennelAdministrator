using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KennelIndexer.API.Models
{
    public class PictureDto
    {
        public Guid PictureId { get; set; }
        public string PictureUri { get; set; }
        public Guid PersonId { get; set; }

    }
}
