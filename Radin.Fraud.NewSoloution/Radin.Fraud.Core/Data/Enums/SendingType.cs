using System.ComponentModel.DataAnnotations;

namespace Radin.Fraud.Core.Data.Enums
{
	public enum SendingType
	{
		[Display(Name = "انتخاب نوع زمان ارسال")]
		All = 0,
		[Display(Name = "در لحظه‌ی رخداد تخطی")]
		InstantSending = 1,
		[Display(Name = "در ساعت مشخص", ShortName = "در ساعتِ")]
		Fixed = 2
	}
}
