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
            using (var studentsDataContext = new StudentsDataContext())
            {
                foreach (var item in collection)
                {
                    studentsDataContext.GetTable<T>().InsertOnSubmit(item);
                }

                studentsDataContext.SubmitChanges();
            }
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
            var studentsDataContext = new StudentsDataContext();

            var expressionParameter = Expression.Parameter(typeof(T), "item");
            var expression = Expression.Lambda<Func<T, bool>>
                (
                Expression.Equal(
                    Expression.Property(
                        expressionParameter,
                        "Id"
                        ),
                    Expression.Constant(id)
                    ),
                new[] { expressionParameter }
                );

            T newItem = studentsDataContext.GetTable<T>().FirstOrDefault(expression);

            return newItem;

        }

        /// <summary>
        /// Method update object to database.
        /// </summary>
        /// <param name="id">Object id</param>
        /// <param name="obj">Object that inherits the class BaseModel</param>
        public void Update(int id, T obj)
        {
            using (var studentsDataContext = new StudentsDataContext())
            {
                List<PropertyInfo> propertys = typeof(T).GetProperties()
                    .Where(item => (!item.PropertyType.IsClass || (item.PropertyType == typeof(string)))
                    && (item.Name != "Id")).ToList();

                var expressionParameter = Expression.Parameter(typeof(T), "item");
                var expression = Expression.Lambda<Func<T, bool>>
                    (
                    Expression.Equal(
                        Expression.Property(
                            expressionParameter,
                            "id"
                            ),
                        Expression.Constant(id)
                        ),
                    new[] { expressionParameter }
                    );

                T newItem = studentsDataContext.GetTable<T>().First(expression);

                foreach (PropertyInfo item in propertys)
                {
                    item.SetValue(newItem, item.GetValue(obj));
                }

                studentsDataContext.SubmitChanges();
            }
        }

        /// <summary>
        /// Method delete object from database.
        /// </summary>
        /// <param name="id">Object id.</param>
        public void Delete(int id)
        {
            using (var StudentsDataContext = new StudentsDataContext())
            {
                var expressionParameter = Expression.Parameter(typeof(T), "item");
                var expression = Expression.Lambda<Func<T, bool>>
                    (
                    Expression.Equal(
                        Expression.Property(
                            expressionParameter,
                            "id"
                            ),
                        Expression.Constant(id)
                        ),
                    new[] { expressionParameter }
                    );

                T entity = StudentsDataContext.GetTable<T>().First(expression);

                StudentsDataContext.GetTable<T>().DeleteOnSubmit(entity);
                StudentsDataContext.SubmitChanges();
            }
        }
    }
}