using Radin.Fraud.Core.Data.Domain;
using System.Text.Json.Serialization; // Replaced Newtonsoft.Json

namespace Radin.Fraud.Core.Data.Entities; // File-scoped namespace (C# 10+)

public class AlertEntity : Alert
{

	// Navigation Properties
	// Auto-initialized to prevent frustrating NullReferenceExceptions when adding items
	[JsonIgnore]
	public virtual ICollection<AlertContactEntity> AlertContacts { get; set; } = new List<AlertContactEntity>();

	[JsonIgnore]
	public virtual ICollection<AlertRuleEntity> AlertRules { get; set; } = new List<AlertRuleEntity>();

	[JsonIgnore]
	public virtual ICollection<AlertContactGroupEntity> AlertContactGroups { get; set; } = new List<AlertContactGroupEntity>();

	[JsonIgnore]
	public virtual ICollection<NotificationEntity> AlertNotifications { get; set; } = new List<NotificationEntity>();

	//public override string GetTableName() => "Alert";
}