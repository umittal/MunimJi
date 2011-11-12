using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using munimji.core.lib.extentions;
using NHibernate;

namespace munimji.core.persistance {
    public abstract class EntityContextBase : IDisposable {
        protected ISession Session { get; private set; }

        protected EntityContextBase(ISession session) {
            Session = session;
        }

        public IQueryable<T> Set<T>() where T : EntityBase {
            var query = GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.PropertyType == typeof (IQueryable<T>));
            if (query.IsEmpty()) {
                return null;
            }
            return query.First().GetValue(this, null) as IQueryable<T>;
        }

        #region LookupById

        public T LookupById<T>(long id) where T : EntityBase {
            return LookupById<T>(id, false);
        }

        public T LookupById<T>(long id, bool strict) where T : EntityBase {
            return LookupFirst<T>(x => x.Id == id, false, strict);
        }

        #endregion

        #region LookupFirst

        public T LookupFirst<T>(Expression<Func<T, bool>> whereClause, bool strict) where T : EntityBase {
            return LookupFirst(whereClause, false, strict);
        }

        /// <summary>
        ///   Minimalistic approach: Look for Top 1 only
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "whereClause"></param>
        /// <param name = "deleted"></param>
        /// <param name = "strict"></param>
        /// <returns></returns>
        public T LookupFirst<T>(Expression<Func<T, bool>> whereClause, bool deleted, bool strict)
            where T : EntityBase {
            try {
                var query = Set<T>()
                    .Where(whereClause)
                    //.Where(x => x.IsDeleted == deleted)
                    .Single();

                return query;
            }
            catch {
                if (strict) {
                    throw;
                }
                return null;
            }
        }

        #endregion

        #region Lookup<T>

        public IEnumerable<T> Lookup<T>(Expression<Func<T, bool>> whereClause) where T : EntityBase {
            return Lookup(whereClause, false);
        }


        public IEnumerable<T> Lookup<T>(Expression<Func<T, bool>> whereClause, bool deleted) where T : EntityBase {
            IEnumerable<T> result = Set<T>()
                .Where(whereClause)
                //.Where(x => x.IsDeleted == deleted)
                .Select(x => x);

            return result;
        }

        #endregion

        /// <summary>
        ///   Helper to process Added/Modified/Deleted entities at one go.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "entities"></param>
        public void Save<T>(IEnumerable<T> entities) where T : EntityBase {
            using (var transaction = Session.BeginTransaction()) {
                foreach (var entity in entities) {
                    Session.SaveOrUpdate(entity);
                }
                transaction.Commit();
            }
        }

        /// <summary>
        ///   Helper to process Added/Modified/Deleted entities at one go.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "entities"></param>
        public void Delete<T>(IEnumerable<T> entities) where T : EntityBase {
            using (var transaction = Session.BeginTransaction()) {
                foreach (var entity in entities) {
                    Session.Delete(entity);
                }
                transaction.Commit();
            }
        }

        public void Dispose() {
            if (Session == null) {
                return;
            }
            Session.Dispose();
            Session = null;
        }
    }
}