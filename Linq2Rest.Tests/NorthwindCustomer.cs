namespace Linq2Rest.Tests
{
	using System.Collections.Generic;

	public class NorthwindCustomer
	{
		public string CustomerID { get; set; }

		public string CompanyName { get; set; }
		
		public string ContactName { get; set; }
		
		public string ContactTitle { get; set; }

		public string Address { get; set; }
		
		public string City { get; set; }
		
		public string Region { get; set; }
		
		public string PostalCode { get; set; }
		
		public string Country { get; set; }
		
		public string Phone { get; set; }
	}

	public class ODataResponse<T>
	{
		public ODataResult<T> d { get; set; }
	}

	public class ODataResult<T>
	{
		public List<T> results { get; set; }
	}
}