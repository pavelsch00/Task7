using System.Collections.Generic;

namespace Epam_Task7.Interfaces
{
    /// <summary>
    /// Interface describes the IBasicMethodDb.
    /// </summary>
    public interface IBasicMethodDb<T>
    {
        /// <summary>
        /// Method read collection objects table from database.
        /// </summary>
        /// <returns>Collection<T> objects.</returns>
        List<T> Read();

        /// <summary>
        /// Method read object table from database.
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns>Item</returns>
        T Read(int id);

        /// <summary>
        /// Method add object to database.
        /// </summary>
        /// <param name="collection">Objects to add to database tables.</param>
        void Create(List<T> collection);

        /// <summary>
        /// Method update object to database.
        /// </summary>
        /// <param name="id">Id object.</param>
        /// <param name="obj">Object to update to database.</param>
        void Update(int id, T obj);

        /// <summary>
        /// Method delete object from database.
        /// </summary>
        /// <param name="obj">Object.</param>
        void Delete(T obj);

    }
}
