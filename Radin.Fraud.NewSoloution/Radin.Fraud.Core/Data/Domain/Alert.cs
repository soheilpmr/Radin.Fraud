using BackEndInfrsastructure.Domain;
using Radin.Fraud.Core.Data.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Radin.Fraud.Core.Data.Domain
{
	public class Alert : Auditable<int>
	{
		[JsonPropertyName("نام")] // System.Text.Json equivalent
		public required string Name { get; set; } // 'required' ensures it is never null upon creation (C# 11+)

		[JsonPropertyName("متن")]
		public required string Text { get; set; }

		[JsonIgnore]
		public SendingType SendingType { get; set; }

		[NotMapped]
		[JsonPropertyName("نوع ارسال")]
		public string SendingTypeStr => SendingType.DisplayName();

		[JsonPropertyName("زمان ارسال")]
		public string? TimeToSend { get; set; } // Marked as nullable (?) to suppress compiler warnings

		[JsonPropertyName("نمایش در پورتال")]
		public bool ShowInPortal { get; set; }

		[JsonIgnore]
		public AlertType AlertType { get; set; }

		[NotMapped]
		[JsonPropertyName("نوع هشدار")]
		// Addressed your "//todo: do better" using modern LINQ and Enum methods
		public string AlertTypeStr => string.Join(" - ",
		Enum.GetValues<AlertType>()
			.Where(flag => flag != 0 && AlertType.HasFlag(flag))
			.Select(flag => flag.DisplayName()));

		[JsonPropertyName("هشدار برای هوش مصنوعی")]
		public bool ForSuspicious { get; set; }

		[JsonPropertyName("فعال")]
		public bool IsEnabled { get; set; }

		public int? RuleRiskFrom { get; set; }
		public int? RuleRiskTo { get; set; }

		public int RepeatNumber { get; set; }
		public long Duration { get; set; }


		[NotMapped]
		[JsonIgnore]
		public TimeSpan DurationTicks
		{
			get => TimeSpan.FromTicks(Duration);
			set => Duration = value.Ticks; // Modernized to expression-bodied setter
		}

		// Consider changing this to an Enum in the future instead of using a magic number comment!
		// 1 for count of rulebreaktransactions - 2 for amount of rulebreaktransactions
		public int ConditionType { get; set; }
		public int? ConstantValue { get; set; }
		public int? ConditionRecentDays { get; set; }
		public int? ConditionMultiplier { get; set; }

	}
}
