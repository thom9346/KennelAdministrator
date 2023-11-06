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
using System.Text;

namespace KennelIndexer.Tests
{
    public class PicturesControllerTests
    {
        [Test]
        public void IsReturningPictureModel()
        {
            var mapMock = new Mock<IMapper>();
            var map = mapMock.Object;

            var personLibraryRepositoryMock = new Mock<IPersonLibraryRepository>();
            var personRepo = personLibraryRepositoryMock.Object;

            var existingPersonId = new Guid("88C39E8C-F423-47C4-BE36-E0F29F4D96A3");

            var systemUnderTest = new PicturesController(personRepo, map);
            var result = systemUnderTest.GetPictures(existingPersonId) as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<PictureDto>>(result.Value);
        }


        [Test]
        public void IsReturning404OnPictureNotFound()
        {
            var mapMock = new Mock<IMapper>();
            var map = mapMock.Object;

            var existingPersonId = new Guid("88C39E8C-F423-47C4-BE36-E0F29F4D96A3");
            var existingPictureId = new Guid("88C39E8C-F423-47C4-BE36-E0F29F4D96A4");

            var personLibraryRepositoryMock = new Mock<IPersonLibraryRepository>();
            personLibraryRepositoryMock.Setup(repo => repo.GetPicture(existingPersonId, existingPictureId))
                .Returns(new Picture
                {
                    PersonId = Guid.NewGuid(),
                    PictureUri = "some/random/location.png",
                    PictureId = Guid.NewGuid()
                });

            var personRepository = personLibraryRepositoryMock.Object;
            var systemUnderTest = new PicturesController(personRepository, map);

            var notExistingPersonId = new Guid("88C39E8C-F423-47C4-BE36-E0F29F4D96A8");

            var result = systemUnderTest.GetPicture(existingPersonId, notExistingPersonId);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);

        }

        [Test]
        public void IsReturning500OnInternalErrorWhenGettingPicture()
        {
            var mapMock = new Mock<IMapper>();
            var map = mapMock.Object;

            var existingPersonId = new Guid("88C39E8C-F423-47C4-BE36-E0F29F4D96A3");
            var existingPictureId = new Guid("88C39E8C-F423-47C4-BE36-E0F29F4D96A4");

            var personLibraryRepositoryMock = new Mock<IPersonLibraryRepository>();
            personLibraryRepositoryMock.Setup(repo => repo.GetPicture(existingPersonId, existingPictureId))
                .Throws(new Exception());

            var personRepository = personLibraryRepositoryMock.Object;
            var systemUnderTest = new PicturesController(personRepository, map);


            var result = systemUnderTest.GetPicture(existingPersonId, existingPictureId) as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(500, result.StatusCode);

        }
    }
}
