using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace PhotoAlbum.UnitTests
{
	[TestClass]
	public class PhotoAlbumTests
	{
		[TestInitialize]
		public void TestSetUp()
		{

		}

		[TestMethod]
		public async Task HappyPath()
		{
			//arrange
			var passingValidator = new PhotoAlbumValidatorModel()
			{
				Id = 1,
				ValidationResults = new List<ValidationResult>()
			};

			var validatorMock = new Mock<IPhotoAlbumInputValidator>();
			validatorMock.Setup(v => v.ValidateInput(It.IsAny<string>())).Returns(passingValidator);

			var webRepoMock = new Mock<IPhotoAlbumWebRepo>();
			webRepoMock.Setup(w => w.GetPhotoById(It.IsAny<int>())).ReturnsAsync(new List<PhotoModel>());

			var systemUnderTest = new PhotoAlbumComponent(validatorMock.Object,webRepoMock.Object);
			//act
			await systemUnderTest.DoPhotoAlbumWork(It.IsAny<string>());

			//asert
			validatorMock.Verify(v => v.ValidateInput(It.IsAny<string>()), Times.Once);
			webRepoMock.Verify(v => v.GetPhotoById(It.IsAny<int>()), Times.Once);


		}
	}
}
