namespace Radin.Fraud.Identity.Data.Entities
{
	public class ClaimDefinition
	{
		public int Id { get; set; }
		public string Type { get; set; } = "Permission";
		public string Value { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
	}
}
