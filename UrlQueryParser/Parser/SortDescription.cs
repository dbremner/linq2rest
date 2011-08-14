namespace UrlQueryParser.Parser
{
	using System;
	using System.Web.UI.WebControls;

	public class SortDescription<T>
	{
		private readonly Func<T, object> _keySelector;
		private readonly SortDirection _direction;

		public SortDescription(Func<T, object> keySelector, SortDirection direction)
		{
			_keySelector = keySelector;
			_direction = direction;
		}

		public SortDirection Direction
		{
			get { return _direction; }
		}

		public Func<T, object> KeySelector
		{
			get { return _keySelector; }
		}
	}
}