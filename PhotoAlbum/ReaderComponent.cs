using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAlbum
{
	public interface IReaderComponent
	{
		string ReadLine();
	}
	public class ReaderComponent : IReaderComponent
	{
		public string ReadLine()
		{
			return Console.ReadLine();
		}
	}
}
