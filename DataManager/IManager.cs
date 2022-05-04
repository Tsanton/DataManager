using System.Collections.Generic;
using DataManager.Models;

namespace DataManager
{
    public interface IManager
    {

#warning Parameterize because of SQL injection
        public T ExecuteScalar<T>(string query);
        /// <summary>
        /// Filter is expected to be a valid SQL filter starting with "where..."
        /// DO NOT use in exposed services as this is highly subject to SQL injection
        /// </summary>
        public T QueryOne<T>(string filter) where T : IEntity;
        
#warning Parameterize because of SQL injection
        /// <summary>
        /// Filter is expected to be a valid SQL filter starting with "where..."
        /// DO NOT use in exposed services as this is highly subject to SQL injection
        /// </summary>
        public List<T> QueryMany<T>(string filter) where T : IEntity;
        
        public void InsertOne<T>(T entity) where T : IEntity;
        
        public void MergeOne<T>(T entity) where T : IEntity;
        
        public void InsertMany<T>(List<T> entities) where T : IEntity;
        
        public void MergeMany<T>(List<T> entities) where T : IEntity;
    }
}