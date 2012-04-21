namespace Linq2Rest.Provider.Writers
{
	using System;

	internal interface IValueWriter
	{
		Type Handles { get; }

		string Write(object value);
	}
}