using System;
using System.Collections.Generic;

namespace LaCabana
{
	public class CabinModel
	{

		public String Name { get; set; }

		public double Latitude { get; set; }

		public double Longitude { get; set; }

		public int Phone { get; set; }

		public String PhoneType { get; set; }

		public String Email { get; set; }

		public String EmailType { get; set; }

		public float Price{ get; set; }

		public String PriceType{ get; set; }

		public int Rating { get; set; }

		public List<string> Photo{ get; set; }

		public String Details { get; set; }

		public String IdAdded{ get; set; }

	}
}

