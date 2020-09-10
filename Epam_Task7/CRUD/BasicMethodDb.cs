using System.Collections.Generic;
using System.Linq;

namespace Epam_Task7.CRUD
{
    public class BasicMethodDb<T> where T : class
    {
        public void Cread(List<T> collection)
        {
            var StudentsDataContext = new StudentsDataContext();

            foreach (var item in collection)
            {
                StudentsDataContext.GetTable<T>().InsertOnSubmit(item);
            }

            StudentsDataContext.SubmitChanges();
        }

        public List<T> Read() => new StudentsDataContext().GetTable<T>().ToList();

        public void Update(int id, T obj)
        {
            var studentsDataContext = new StudentsDataContext();

            T chengesItem = studentsDataContext.GetTable<T>().First(item => (int)item.GetType().GetProperty("Id").GetValue(item) == id);
            chengesItem = obj;

            studentsDataContext.SubmitChanges();
        }

        public void Delete(T item)
        {
            var studentsDataContext = new StudentsDataContext();
            studentsDataContext.GetTable<T>().DeleteOnSubmit(item);

            studentsDataContext.SubmitChanges();
        }
    }
}