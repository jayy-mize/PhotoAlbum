using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace PhotoAlbum.UnitTest
{
	[TestClass]
	public class PhotoAlbumTests
	{
		[TestInitialize]
		public void TestSetUp()
		{

		}

		[TestMethod]
		public async Task HappyPathEmpty()
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

			var systemUnderTest = new PhotoAlbumComponent(validatorMock.Object, webRepoMock.Object);
			//act
			await systemUnderTest.DoPhotoAlbumWork(It.IsAny<string>());

			//asert
			validatorMock.Verify(v => v.ValidateInput(It.IsAny<string>()), Times.Once);
			webRepoMock.Verify(v => v.GetPhotoById(It.IsAny<int>()), Times.Once);
		}

		[TestMethod]
		public async Task HappyPathListWithStuff()
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
			var fakeList = GetFakeList();
			webRepoMock.Setup(w => w.GetPhotoById(It.IsAny<int>())).ReturnsAsync(fakeList);

			var systemUnderTest = new PhotoAlbumComponent(validatorMock.Object, webRepoMock.Object);
			//act
			await systemUnderTest.DoPhotoAlbumWork(It.IsAny<string>());

			//asert
			validatorMock.Verify(v => v.ValidateInput(It.IsAny<string>()), Times.Once);
			webRepoMock.Verify(v => v.GetPhotoById(It.IsAny<int>()), Times.Once);
		}

		[TestMethod]
		public async Task WhenValidationErrorsDoNotCallWebRepo()
		{
			//arrange
			var errorValidator = new PhotoAlbumValidatorModel()
			{
				Id = 1,
				ValidationResults = new List<ValidationResult>()
				{
					new ValidationResult("generic error")
				}
			};

			var validatorMock = new Mock<IPhotoAlbumInputValidator>();
			validatorMock.Setup(v => v.ValidateInput(It.IsAny<string>())).Returns(errorValidator);

			var webRepoMock = new Mock<IPhotoAlbumWebRepo>();
			webRepoMock.Setup(w => w.GetPhotoById(It.IsAny<int>())).ReturnsAsync(new List<PhotoModel>());

			var systemUnderTest = new PhotoAlbumComponent(validatorMock.Object, webRepoMock.Object);
			//act
			await systemUnderTest.DoPhotoAlbumWork(It.IsAny<string>());

			//asert
			validatorMock.Verify(v => v.ValidateInput(It.IsAny<string>()), Times.Once);
			webRepoMock.Verify(v => v.GetPhotoById(It.IsAny<int>()), Times.Never);
		}

		[TestMethod]
		public async Task HappyPathValidator()
		{
			//arrange
			var id = 3;
			var goodInput = $"photo-album {id}";
			var systemUnderTest = new PhotoAlbumInputValidator();

			//act
			var result = systemUnderTest.ValidateInput(goodInput);

			//assert
			Assert.AreEqual(id,result.Id);
			Assert.AreEqual(0 , result.ValidationResults.Count);
		}

		[TestMethod]
		public void EmptyStringReturnsValidationError()
		{
			//arrange
			var emptyInput = string.Empty;
			var systemUnderTest = new PhotoAlbumInputValidator();

			//act
			var result = systemUnderTest.ValidateInput(emptyInput);

			//assert
			Assert.AreEqual(1, result.ValidationResults.Count);
			Assert.IsTrue(result.ValidationResults.Select(r => r.ErrorMessage).Contains("Must enter valid command."));
		}

		[TestMethod]
		public void BadCommandReturnsValidationError()
		{
			//arrange
			var emptyInput = "bad-command 3";
			var systemUnderTest = new PhotoAlbumInputValidator();

			//act
			var result = systemUnderTest.ValidateInput(emptyInput);

			//assert
			Assert.AreEqual(1, result.ValidationResults.Count);
			Assert.IsTrue(result.ValidationResults.Select(r => r.ErrorMessage).Contains("Unrecognized command entered"));
		}

		[TestMethod]
		public void UnrecognizedNumberCreatesValidationError()
		{
			//arrange
			var emptyInput = "photo-album 0ne";
			var systemUnderTest = new PhotoAlbumInputValidator();

			//act
			var result = systemUnderTest.ValidateInput(emptyInput);

			//assert
			Assert.AreEqual(1, result.ValidationResults.Count);
			Assert.IsTrue(result.ValidationResults.Select(r => r.ErrorMessage).Contains("Unable to get page number from command"));
		}

		private List<PhotoModel> GetFakeList()
		{
			return new List<PhotoModel>()
			{
				new PhotoModel()
				{
					albumId = 1,
					id = 1,
					title = "title1",
					url = "url1",
					thumbnailUrl = "thumbUrl1"
				},
				new PhotoModel()
				{
					albumId = 2,
					id = 2,
					title = "title2",
					url = "url2",
					thumbnailUrl = "thumbUrl2"
				}
			};
		}
	}
}
