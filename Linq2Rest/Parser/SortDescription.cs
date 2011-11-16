// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace Linq2Rest.Parser
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