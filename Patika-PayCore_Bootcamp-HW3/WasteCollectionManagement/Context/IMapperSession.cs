using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
namespace WasteCollectionManagement.Context
{
    //Veritabanı ile bağlantı sırasında yapılan işlemleri generic olarak tanımlayan imterface
    public interface IMapperSession<T> where T : class, new()
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
        void CloseTransaction();
        void Save(T entity);
        void Update(T entity);
        void Delete(int id);
        List<T> GetAll();
        T GetById(int id);
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        IEnumerable<T> Where(Expression<Func<T, bool>> where);

        IQueryable<T> Entities { get; }
    }
}
