using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Database;
using Android.Database.Sqlite;
using Android.Text;
using Org.Apache.Http.Protocol;
using Android.Graphics;



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

		public void InsertUsername (UsersModel user)
		{	
			var db = GetDatabase ();
			var values = new ContentValues ();
			values.Put ("Id", user.Id);
			values.Put ("Username", user.Username);
			values.Put ("Password", user.Password);
			values.Put ("Email", user.Email);
			values.Put ("ProfilePhoto", user.ProfilePhoto);
			db.Insert ("user", null, values);			
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

		public void InsertCabin (CabinModel cabin)
		{
			var db = GetDatabase ();
			var values = new ContentValues ();
			values.Put ("Name", cabin.Name);
			values.Put ("latitude", cabin.Latitude);
			values.Put ("longitude", cabin.Longitude);
			values.Put ("phone", cabin.Phone);
			values.Put ("email", cabin.Email);
			values.Put ("price", cabin.Price);
			values.Put ("rating", cabin.Rating);
			db.Insert ("cabins", null, values);
		}

		public List<CabinModel> GetAllCabins ()
		{
			var db = GetDatabase ();

			var cabins = new List<CabinModel> ();
			const string query = "Select * from cabins";
			try {
				var cursor = db.RawQuery (query, null);
				if (cursor.MoveToFirst ()) {
					do {
						var account = new CabinModel () {
							Name = cursor.GetString (0),
							Latitude = cursor.GetDouble (1),
							Longitude = cursor.GetDouble (2),
							Phone = cursor.GetInt (3),
							PhoneType = cursor.GetString (4),
							Email = cursor.GetString (5),
							EmailType = cursor.GetString (6),
							Price = cursor.GetFloat (7),
							Rating = cursor.GetInt (8)								
						};
						cabins.Add (account);
					} while (cursor.MoveToNext ());
				}

				cursor.Close ();
			} catch (SQLException s) {
				var abc = 0;
			}
			////db.Close();
			return cabins;
		
		}

		public UsersModel GetAllUsers ()
		{
			var db = GetDatabase ();

			var users = new UsersModel ();
			const string query = "Select * from user";
			try {
				var cursor = db.RawQuery (query, null);
				if (cursor.MoveToFirst ()) {
					do {
						var account = new UsersModel () {
							Id = cursor.GetString (0),
							Username = cursor.GetString (1),
							Password = cursor.GetString (2),
							Email = cursor.GetString (3),
							ProfilePhoto = cursor.GetString (4)
						};
						users = account;
					} while (cursor.MoveToNext ());
				}

				cursor.Close ();
			} catch (SQLException s) {
				var abc = 0;
			}
			////db.Close();
			return users;
		}

		public void DeleteUser ()
		{
			var db = GetDatabase ();
			const string query = "Delete from user";
			try {
				db.ExecSQL (query);
			} catch (SQLException e) {
				var abc = 0;
			}
		}
	}
}

