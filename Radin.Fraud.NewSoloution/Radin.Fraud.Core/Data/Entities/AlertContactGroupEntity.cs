using Radin.Fraud.Core.Data.Domain;

namespace Radin.Fraud.Core.Data.Entities
{
	public class AlertContactGroupEntity : AlertContactGroup
	{
		public virtual ContactGroupEntity ContactGroup { get; set; }
		public virtual AlertEntity Alert { get; set; }
	}
}
