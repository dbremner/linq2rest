// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace Linq2Rest.MvcSite.Models
{
	using System.Data.Entity;

	public class SimpleContext : DbContext
	{
		public DbSet<SimpleDto> SimpleDtos { get; set; }
	}
}
