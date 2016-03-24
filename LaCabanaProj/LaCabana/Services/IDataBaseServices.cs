using System;
using System.Collections.Generic;
using Android.Content;
using Android.Database;
using Android.Database.Sqlite;

namespace LaCabana.Services
{
	public interface IDatabaseServices
	{
		void InsertUsername (string username, string password, int id);

		List<UsersModel>GetAllUsers ();

		void CloseDatabase ();

	}
}

