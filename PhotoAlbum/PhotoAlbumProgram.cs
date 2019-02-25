using System;
using System.Security.Authentication.ExtendedProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;

namespace PhotoAlbum
{
	public class PhotoAlbumProgram
	{
		public static void Main(string[] args)
		{
			//DI
			var services = new ServiceCollection()
				.AddSingleton<IReaderComponent, ReaderComponent>()
				.AddSingleton<IPhotoAlbumComponent, PhotoAlbumComponent>()
				.AddSingleton<IPhotoAlbumInputValidator, PhotoAlbumInputValidator>()
				.AddSingleton<IPhotoAlbumWebRepo, PhotoAlbumWebRepo>()
				.BuildServiceProvider();

			var reader = services.GetService<IReaderComponent>();
			var photoAlbumComponent = services.GetService<IPhotoAlbumComponent>();
			// END DI


			while (true)
			{
				try
				{
					//prompt user
					var input = reader.ReadLine();
					//do work(wait for async)
					photoAlbumComponent.DoPhotoAlbumWork(input).Wait();
					Console.WriteLine("Ready for input.");
				}

				catch (Exception ex)
				{
					//log errors
					Console.WriteLine(JsonConvert.SerializeObject(ex.Message));
				}
			}

		}

		
	}
}
