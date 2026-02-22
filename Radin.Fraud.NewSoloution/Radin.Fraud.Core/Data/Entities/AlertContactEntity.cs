using Radin.Fraud.Core.Data.Domain;

namespace Radin.Fraud.Core.Data.Entities
{
	public class AlertContactEntity : AlertContact
	{
		public virtual ContactEntity Contact { get; set; }

		public virtual AlertEntity Alert { get; set; }
	}
}
