// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

namespace Linq2Rest.MvcSite.Models
{
	using System.Data.Entity;

	public class SimpleContext : DbContext
	{
		public DbSet<SimpleDto> SimpleDtos { get; set; }
	}
}
