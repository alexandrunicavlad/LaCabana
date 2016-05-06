using System;
using System.Collections.Generic;

namespace LaCabana
{
	public class UsersModel
	{
		
		public String Id { get; set; }

		public String Email { get; set; }

		public String Username { get; set; }

		public String Password { get; set; }

		public String ProfilePhoto { get; set; }

		public Dictionary<String, String> FavoriteList { get; set; }
	}
}

