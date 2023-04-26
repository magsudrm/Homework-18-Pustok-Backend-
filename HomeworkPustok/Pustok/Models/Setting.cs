using System.ComponentModel.DataAnnotations;

namespace Pustok.Models
{
	public class Setting
	{
		[Required]
		[MaxLength(50)]
		public string Key { get; set; }
		[MaxLength(300)]
		public string Value { get; set; }
	}
}
