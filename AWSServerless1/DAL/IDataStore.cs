using AWSServerlessWebApi.Model;

namespace AWSServerlessWebApi.DAL {
	public interface IDataStore {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="userID"></param>
		/// <returns></returns>
		User GetUserByID(string userID);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userID"></param>
		/// <returns></returns>
		void AddUser(User user);
	}
}