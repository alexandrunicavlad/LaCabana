using System;
using System.Collections.Generic;
using Android.Content;
using Android.Database;
using Android.Database.Sqlite;

namespace LaCabana.Services
{
	public interface IDatabaseServices
	{
		void InsertUsername (UsersModel model);

		UsersModel GetAllUsers ();

		List<CabinModel>GetAllCabins ();

		void DeleteUser ();

		void CloseDatabase ();

		void InsertCabin (CabinModel cabin);

	}
}

