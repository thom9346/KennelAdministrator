using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KennelIndexer.API.Entities
{
    public class Picture
    {
        public Guid PictureId { get; set; }
        [Required]
        public string PictureUri { get; set; }

        [ForeignKey("PersonId")]
        public Person Person { get; set; }
        public Guid PersonId { get; set; }
    }
}
