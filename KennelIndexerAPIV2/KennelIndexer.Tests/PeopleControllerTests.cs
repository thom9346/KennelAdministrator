using AutoMapper;
using KennelIndexer.API.Controllers;
using KennelIndexer.API.Entities;
using KennelIndexer.API.Models;
using KennelIndexer.API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace KennelIndexer.Tests
{
    public class PeopleControllerTests
    {
    
        [Test]
        public void IsReturningPeople()
        {
            var mapMock = new Mock<IMapper>();
            var map = mapMock.Object;

            var personLibraryRepositoryMock = new Mock<IPersonLibraryRepository>();
            personLibraryRepositoryMock.Setup(repo => repo.GetPeople())
                .Returns(new List<Person>
                {
                new Person
                {
                    PersonId = Guid.NewGuid(),
                    FirstName = "Tom",
                    LastName = "Holm",
                    Address = "none"
                },
                new Person
                 {
                     PersonId = Guid.NewGuid(),
                     FirstName = "Tom 2",
                     LastName = "Holm 2",
                     Address = "none 2"
                 }
                 });

            var personRepository = personLibraryRepositoryMock.Object;
            var systemUnderTest = new PeopleController(personRepository, map);

            var result = systemUnderTest.GetPeople() as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<PersonDto>>(result.Value);

        }

        [Test]
        public void IsReturning404OnPersonNotFound()
        {
            var mapMock = new Mock<IMapper>();
            var map = mapMock.Object;

            var existingPersonId = new Guid("88C39E8C-F423-47C4-BE36-E0F29F4D96A3");

            var personLibraryRepositoryMock = new Mock<IPersonLibraryRepository>();
            personLibraryRepositoryMock.Setup(repo => repo.GetPerson(existingPersonId))
                .Returns(new Person
                {
                    PersonId = Guid.NewGuid(),
                    FirstName = "Tom",
                    LastName = "Holm",
                    Address = "none"
                });

            var personRepository = personLibraryRepositoryMock.Object;
            var systemUnderTest = new PeopleController(personRepository, map);

            var notExistingPersonId = new Guid("88C39E8C-F423-47C4-BE36-E0F29F4D96A4");

            var result = systemUnderTest.GetPerson(notExistingPersonId);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);

        }
        [Test]
        public void IsReturning500OnInternalErrorWhenGettingPerson()
        {
            var mapMock = new Mock<IMapper>();
            var map = mapMock.Object;

            var existingPersonId = new Guid("88C39E8C-F423-47C4-BE36-E0F29F4D96A3");

            var personLibraryRepositoryMock = new Mock<IPersonLibraryRepository>();
            personLibraryRepositoryMock.Setup(repo => repo.GetPerson(existingPersonId))
                .Throws(new Exception());

            var personRepository = personLibraryRepositoryMock.Object;
            var systemUnderTest = new PeopleController(personRepository, map);

            var result = systemUnderTest.GetPerson(existingPersonId) as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(500, result.StatusCode);

        }
    }
}