using KennelIndexer.API.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KennelIndexer.API.Services
{
    public interface IPersonLibraryRepository
    {
        IEnumerable<Person> GetPeople();
        Person GetPerson(Guid personId);
        IEnumerable<Person> GetPeople(IEnumerable<Guid> personIds);
        void AddPerson(Person person, List<string> files = null);
        void DeletePerson(Person person);
        void UpdatePerson(Person person);
        bool PersonExists(Guid personId);
        bool Save();

        //For pictures
        IEnumerable<Picture> GetPictures(Guid personId);
        Picture GetPicture(Guid personId, Guid pictureId);
        void AddPicture(Guid personId, Picture picture);
        void UpdatePicture(Picture picture);
        void DeletePicture(Picture picture);

    }
}
