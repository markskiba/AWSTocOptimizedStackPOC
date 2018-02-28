using AWSServerlessWebApi.Model;

namespace AWSServerlessWebApi.DAL {
	public interface IDataStore {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="userID"></param>
		/// <returns></returns>
		UserModel GetUserByID(string userID);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userID"></param>
		/// <returns></returns>
		void AddUser(UserModel user);
	}
}