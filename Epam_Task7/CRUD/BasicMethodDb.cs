using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Epam_Task7.Interfaces;

namespace Epam_Task7.CRUD
{
    /// <summary>
    /// Interface describes the IBasicMethodDb.
    /// </summary>
    public class BasicMethodDb<T> : IBasicMethodDb<T> where T : class
    {
        /// <summary>
        /// Method add object to database.
        /// </summary>
        /// <param name="collection">Objects to add to database tables.</param>
        public void Create(List<T> collection)
        {
            var StudentsDataContext = new StudentsDataContext();

            foreach (var item in collection)
            {
                StudentsDataContext.GetTable<T>().InsertOnSubmit(item);
            }

            StudentsDataContext.SubmitChanges();
        }

        /// <summary>
        /// Method read collection objects table from database.
        /// </summary>
        /// <returns>Collection<T> objects.</returns>
        public List<T> Read() => new StudentsDataContext().GetTable<T>().ToList();

        /// <summary>
        /// Method read object table from database.
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns>Item</returns>
        public T Read(int id)
        {
            var Students = new StudentsDataContext();

            ParameterExpression ParameterExpression = Expression.Parameter(typeof(T), "item");
            var expression = Expression.Lambda<Func<T, bool>>
                (
                Expression.Equal(
                    Expression.Property(
                        ParameterExpression,
                        "Id"
                        ),
                    Expression.Constant(id)
                    ),
                new[] { ParameterExpression }
                );

            T newItem = Students.GetTable<T>().FirstOrDefault(expression);

            return newItem;

        }

        /// <summary>
        /// Method update object to database.
        /// </summary>
        /// <param name="id">Object id</param>
        /// <param name="obj">Object that inherits the class BaseModel</param>
        public void Update(int id, T obj)
        {
            var students = new StudentsDataContext();

            List<PropertyInfo> propertys = typeof(T).GetProperties()
                .Where(item => (!item.PropertyType.IsClass || (item.PropertyType == typeof(string)))
                && (item.Name != "Id")).ToList();

            var parameter = Expression.Parameter(typeof(T), "item");
            var expression = Expression.Lambda<Func<T, bool>>
                (
                Expression.Equal(
                    Expression.Property(
                        parameter,
                        "id"
                        ),
                    Expression.Constant(id)
                    ),
                new[] { parameter }
                );

            T newItem = students.GetTable<T>().First(expression);

            foreach (PropertyInfo item in propertys)
            {
                item.SetValue(newItem, item.GetValue(obj));
            }

            students.SubmitChanges();
        }

        /// <summary>
        /// Method delete object from database.
        /// </summary>
        /// <param name="obj">Object.</param>
        public void Delete(T item)
        {
            var studentsDataContext = new StudentsDataContext();
            studentsDataContext.GetTable<T>().DeleteOnSubmit(item);

            studentsDataContext.SubmitChanges();
        }
    }
}