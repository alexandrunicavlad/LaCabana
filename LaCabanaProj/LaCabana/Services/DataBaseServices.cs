using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Database;
using Android.Database.Sqlite;
using Android.Text;
using Org.Apache.Http.Protocol;


namespace LaCabana.Services
{
	public class DatabaseServices : SQLiteOpenHelper, IDatabaseServices
	{
		private const int DatabaseVersion = 5;
		private const string DatabaseName = "LaCabanaDb";
		private static Context _currentContext;
		private SQLiteDatabase _db;


		public DatabaseServices (Context context)
			: base (context, DatabaseName, null, DatabaseVersion)
		{
			_currentContext = _currentContext ?? context;
			_db = WritableDatabase;
		}

		public override void OnCreate (SQLiteDatabase db)
		{
		}

		public override void OnUpgrade (SQLiteDatabase db, int oldVersion, int newVersion)
		{
		}

		public void InsertUsername (string username, string password, int id)
		{
			var db = GetDatabase ();

			var SqlCreateUsers =
				"CREATE TABLE users(" + "Id" + " INTEGER PRIMARY KEY AUTOINCREMENT," + "UserName" + " TEXT NOT NULL UNIQUE," +
				"UserPassword" + " TEXT NOT NULL" + ")";
			db.ExecSQL (SqlCreateUsers);
			
		}


		private SQLiteDatabase GetDatabase ()
		{
			if (_db != null && _db.IsOpen)
				return _db;
			_db = WritableDatabase;
			return _db;
		}

		public void CloseDatabase ()
		{
			if (_db == null)
				return;
			_db.Close ();
		}



		public List<UsersModel> GetAllUsers ()
		{
			var db = GetDatabase ();

			var users = new List<UsersModel> ();
			const string query = "Select * from users";
			try {
				var cursor = db.RawQuery (query, null);
				if (cursor.MoveToFirst ()) {
					do {
						var account = new UsersModel () {
							Id = cursor.GetInt (0),
							Username = cursor.GetString (1),
							Password = cursor.GetString (2),
							Email = cursor.GetString (3)
						};
						users.Add (account);
					} while (cursor.MoveToNext ());
				}

				cursor.Close ();
			} catch (SQLException s) {
				var abc = 0;
			}
			////db.Close();
			return users;
		}
	}
}

