using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WasteCollectionManagement.Models;

namespace WasteCollectionManagement.Context
{
    //Veritabanı ile bağlantı sırasında yapılan işlemleri generic olarak tanımlayan imterface'den kalıtım alır. 
    // Container sınıfı için yapılan veritabanı işlemlerini belirtir. 
    public class VehicleMapperSession : IMapperSession<Vehicle>
    {
        private readonly ISession session;
        private ITransaction transaction;

        public VehicleMapperSession(ISession session)
        {
            this.session = session;
        }


        public IQueryable<Vehicle> Entities => session.Query<Vehicle>();

        public void BeginTransaction()
        {
            transaction = session.BeginTransaction();
        }

        public void Commit()
        {
            transaction.Commit();
        }

        public void Rollback()
        {
            transaction.Rollback();
        }

        public void CloseTransaction()
        {
            if (transaction != null)
            {
                transaction.Dispose();
                transaction = null;
            }
        }

        public void Save(Vehicle entity)
        {
            session.Save(entity);
        }
        public void Update(Vehicle entity)
        {
            session.Update(entity);
        }
        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                session.Delete(entity);
            }
        }


        public List<Vehicle> GetAll()
        {
            return session.Query<Vehicle>().ToList();
        }

        public Vehicle GetById(int id)
        {
            var entity = session.Get<Vehicle>(id);
            return entity;
        }

        public IEnumerable<Vehicle> Find(Expression<Func<Vehicle, bool>> expression)
        {
            return session.Query<Vehicle>().Where(expression).ToList();
        }

        public IEnumerable<Vehicle> Where(Expression<Func<Vehicle, bool>> where)
        {
            return session.Query<Vehicle>().Where(where).AsQueryable();
        }
    }
}
