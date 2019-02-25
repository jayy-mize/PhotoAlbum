using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PhotoAlbum
{
	public interface IPhotoAlbumComponent
	{
		Task DoPhotoAlbumWork(string input);
	}

	public class PhotoAlbumComponent : IPhotoAlbumComponent
	{
		private readonly IPhotoAlbumInputValidator _photoAlbumInputValidator;
		private readonly IPhotoAlbumWebRepo _photoAlbumWebRepo;

		public PhotoAlbumComponent(IPhotoAlbumInputValidator photoAlbumInputValidator, IPhotoAlbumWebRepo photoAlbumWebRepo)
		{
			_photoAlbumInputValidator = photoAlbumInputValidator;
			_photoAlbumWebRepo = photoAlbumWebRepo;
		}

		public async Task DoPhotoAlbumWork(string input)
		{
			var result = _photoAlbumInputValidator.ValidateInput(input);
			if (result.ValidationResults.Any())
			{
				var errorMessages = result.ValidationResults.Select(r => r.ErrorMessage);
				Console.WriteLine(JsonConvert.SerializeObject(errorMessages));
				return;
			}

			var photos = await _photoAlbumWebRepo.GetPhotoById(result.Id);

			if (!photos.Any())
			{
				Console.WriteLine($"No images found in album with albumId of {result.Id}");
			}

			foreach (var photoInfo in photos)
			{
				Console.WriteLine($"[{photoInfo.id}] {photoInfo.title}");
			}
		}
	}
}
