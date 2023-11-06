using KennelIndexer.API.DbContexts;
using KennelIndexer.API.Entities;
using KennelIndexer.API.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KennelIndexer.API.Services
{
    public class PersonLibraryRepository : IPersonLibraryRepository, IDisposable
    {
        private readonly PersonLibraryContext _context;

        public PersonLibraryRepository(PersonLibraryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public void AddPerson(Person person, List<string> pictureUrls = null)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }

            person.PersonId = Guid.NewGuid(); //API is responsibile for creating new IDS.

            if (pictureUrls.Count > 0)
            {
                person.Pictures = new List<Picture>();
                foreach (var file in pictureUrls)
                {
                    //var personId = Guid.NewGuid();
                    person.Pictures.Add(new Picture { PersonId = person.PersonId, PictureUri = file });
                }

            }
            _context.People.Add(person);

        }
        public void AddPicture(Guid personId, Picture picture)
        {
            if (personId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(personId));
            }
            if (picture == null)
            {
                throw new ArgumentNullException(nameof(picture));
            }

            //always set the pictureId to the passed-in pictureid.
            picture.PersonId = personId;
            _context.Pictures.Add(picture);
        }
        public void UploadPicture(Picture picture)
        {
            if (picture == null)
            {
                throw new ArgumentNullException(nameof(picture));
            }

        }

        public void DeletePerson(Person person)
        {
            _context.People.Remove(person);
        }

        public void DeletePicture(Picture picture)
        {
            _context.Pictures.Remove(picture);
        }


        public IEnumerable<Person> GetPeople()
        {
            return _context.People.ToList<Person>();
        }

        public IEnumerable<Person> GetPeople(IEnumerable<Guid> personIds)
        {
            if (personIds == null)
            {
                throw new ArgumentNullException(nameof(personIds));
            }

            return _context.People.Where(p => personIds.Contains(p.PersonId))
                .OrderBy(p => p.FirstName)
                .OrderBy(p => p.LastName)
                .ToList();
        }

        public Person GetPerson(Guid personId)
        {
            if (personId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(personId));
            }
            return _context.People.FirstOrDefault(p => p.PersonId == personId);
        }
        //Get specific picture from specific person
        public Picture GetPicture(Guid personId, Guid pictureId)
        {
            if (personId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(personId));
            }
            if (pictureId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(pictureId));
            }

            return _context.Pictures
                .Where(p => p.PersonId == personId && p.PictureId == pictureId).FirstOrDefault();
        }
        //get all pictures from one person
        public IEnumerable<Picture> GetPictures(Guid personId)
        {
            if (personId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(personId));
            }
            return _context.Pictures
                .Where(p => p.PersonId == personId);
        }


        public bool PersonExists(Guid personId)
        {
            if (personId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(personId));
            }
            return _context.People.Any(p => p.PersonId == personId);
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdatePerson(Person person)
        {

            if(person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }
            if (person.PersonId == null || person.PersonId == Guid.Empty)
            {
                throw new ArgumentNullException("Id is needed to update a person.");
            }
            _context.People.Update(person);
        }

        public void UpdatePicture(Picture picture)
        {
            //throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //Dispose resources when needed
            }
        }
    }
}
