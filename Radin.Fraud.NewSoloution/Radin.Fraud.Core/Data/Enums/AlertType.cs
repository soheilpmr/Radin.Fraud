using System.ComponentModel.DataAnnotations;

namespace Radin.Fraud.Core.Data.Enums
{
	public enum AlertType
	{
		[Display(Name = "هیچکدام")]
		None = 0x0,

		[Display(Name = "ایمیل")]
		Email = 0x1,

		[Display(Name = "پیامک")]
		SMS = 0x2,

		[Display(Name = "صدا")]
		Sound = 0x4
	}
}
