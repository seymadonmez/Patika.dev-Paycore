using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WasteCollectionManagement.Models;

namespace WasteCollectionManagement.Context
{
    //Veritabanı ile bağlantı sırasında yapılan işlemleri generic olarak tanımlayan imterface'den kalıtım alır. 
    // Vehicle sınıfı için yapılan veritabanı işlemlerini belirtir. 
    public class ContainerMapperSession : IMapperSession<Container>
    {
        private readonly ISession session;
        private ITransaction transaction;

        public ContainerMapperSession(ISession session)
        {
            this.session = session;
        }

                    
        public IQueryable<Container> Entities => session.Query<Container>();

        public void BeginTransaction()
        {
            transaction = session.BeginTransaction();
        }

        public void CloseTransaction()
        {
            if (transaction != null)
            {
                transaction.Dispose();
                transaction = null;
            }
        }

        public void Commit()
        {
            transaction.Commit();
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                session.Delete(entity);
            }
        }

        public IEnumerable<Container> Find(Expression<Func<Container, bool>> expression)
        {
            return session.Query<Container>().Where(expression).ToList();
        }

        public List<Container> GetAll()
        {
            return session.Query<Container>().ToList();
        }

        public Container GetById(int id)
        {
            var entity = session.Get<Container>(id);
            return entity;
        }

        public void Rollback()
        {
            transaction.Rollback();
        }

        public void Save(Container entity)
        {
            session.Save(entity);
        }

        public void Update(Container entity)
        {
            session.Update(entity);
        }

        public IEnumerable<Container> Where(Expression<Func<Container, bool>> where)
        {
            return session.Query<Container>().Where(where).AsQueryable();
        }

    }
}
