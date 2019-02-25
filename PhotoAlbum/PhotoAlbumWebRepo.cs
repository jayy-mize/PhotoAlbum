using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PhotoAlbum
{
	public interface IPhotoAlbumWebRepo
	{
		Task<List<PhotoModel>> GetPhotoById(int albumId);
	}

	public class PhotoAlbumWebRepo : IPhotoAlbumWebRepo
	{
		private readonly string _photoAlbumUrl;
		private const string PhotoAlbumAppSettingName = "PhotoAlbumUrl";

		public PhotoAlbumWebRepo()
		{
			_photoAlbumUrl = ConfigurationSettings.AppSettings.Get(PhotoAlbumAppSettingName);
		}

		public async Task<List<PhotoModel>> GetPhotoById(int albumId)
		{
			//set-up client
			var client = new HttpClient();
			var photoUrl = $"{_photoAlbumUrl}?albumId={albumId}";

			//parse response into consumable object
			var response = await client.GetAsync(photoUrl);
			var content = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<List<PhotoModel>>(content);
		}
	}
}
