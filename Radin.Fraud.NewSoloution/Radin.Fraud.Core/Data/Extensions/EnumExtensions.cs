using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

public static class EnumExtensions
{
	public static string DisplayName(this Enum value)
	{
		// Get the field information for the current enum value
		FieldInfo? field = value.GetType().GetField(value.ToString());

		if (field == null) return value.ToString();

		// Look for the [Display] attribute
		var displayAttribute = field.GetCustomAttribute<DisplayAttribute>();

		// Return the Name property if the attribute exists, otherwise fall back to the enum's variable name
		return displayAttribute?.Name ?? value.ToString();
	}
}