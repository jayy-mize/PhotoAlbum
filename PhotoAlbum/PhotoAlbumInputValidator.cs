using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAlbum
{
	public interface IPhotoAlbumInputValidator
	{
		PhotoAlbumValidatorModel ValidateInput(string input);
	}

	public class PhotoAlbumInputValidator : IPhotoAlbumInputValidator
	{
		public PhotoAlbumValidatorModel ValidateInput(string input)
		{
			var validationModel = new PhotoAlbumValidatorModel();

			var inputs = input?.Split(' ');

			//null commands. no valid commands
			if (inputs == null || inputs.Length < 2)
			{
				validationModel.ValidationResults.Add(new ValidationResult("Must enter valid command."));
				return validationModel;
			}

			//if command 0 is null or not photo-album. give error
			if (inputs[0] != @"photo-album")
			{
				validationModel.ValidationResults.Add(new ValidationResult("Unrecognized command entered"));
				return validationModel;
			}

			//if command 1 is null or not able to convert to an int. give error
			if (inputs.Length < 2 || !int.TryParse(inputs[1], out var id))
			{
				validationModel.ValidationResults.Add(new ValidationResult($"Unable to get page number from command"));
				return validationModel;
			}

			validationModel.Id = id;

			return validationModel;
		}
	}

	public class PhotoAlbumValidatorModel
	{
		public PhotoAlbumValidatorModel()
		{
			ValidationResults = new List<ValidationResult>();
		}

		public int Id { get; set; }
		public List<ValidationResult> ValidationResults { get; set; }
	}
}
