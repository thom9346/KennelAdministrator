using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KennelIndexer.API.Entities
{
    public class Person
    {
        [Key]
        public Guid PersonId { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }


        public string Address { get; set; }
        public string ReasonsForBeingOnTheList { get; set; }
        public ICollection<Picture> Pictures { get; set; }
            = new List<Picture>();
    }
}
