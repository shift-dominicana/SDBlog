using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using SDBlog.BusinessLayer.Interfaces.Base;
using SDBlog.Core.Base;
using SDBlog.Core.Classes;
using SDBlog.Core.Extensions;
using SDBlog.DataModel.Classes;
using SDBlog.DataModel.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace SDBlog.BusinessLayer.Repositories.Base
{
    public class BaseRepository<TModel> :
        IBaseRepository<TModel>
        where TModel : EntityBase, new()
    {

        private readonly DbSet<TModel> _dbSet;
        private readonly DbContext _context;
        private IValidator<TModel> _validator;


        public BaseRepository(MainDbContext context, IValidator<TModel> validator, IMapper mapper)
        {
            _context = context;
            _dbSet = context.Set<TModel>();
            _validator = validator;
        }

        public IValidator<TModel> Validator
        {
            get { return _validator; }
            set { _validator = value; }
        }


        protected IQueryable<TModel> PrepareQuery(
            IQueryable<TModel> query,
            Expression<Func<TModel, bool>> predicate = null,
            Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> include = null,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
            int? take = null
        )
        {
            try
            {
                if (include != null)
                    query = include(query);

                if (predicate != null)
                    query = query.Where(predicate);

                if (orderBy != null)
                    query = orderBy(query);

                if (take.HasValue)
                    query = query.Take(Convert.ToInt32(take));

                return query;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método PrepareQuery de BusinessLayer.", ex);
            }
        }

        public async Task AddAsync(TModel entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método AddAsync de BusinessLayer.", ex);
            }

        }

        public virtual async Task<TModel> GeTModelByIdAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método GetModelByIdAsync de BusinessLayer.", ex);
            }
        }

        public async Task<bool> AnyAsync(Expression<Func<TModel, bool>> predicate)
        {
            try
            {
                return await _dbSet.AnyAsync(predicate);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método AnyAsync de BusinessLayer.", ex);
            }
        }

        public bool Any(Expression<Func<TModel, bool>> predicate)
        {
            try
            {
                return _dbSet.Any(predicate);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método Any de BusinessLayer.", ex);
            }
        }

        public async Task<IEnumerable<TModel>> GetList(Expression<Func<TModel, bool>> predicate)
        {

            try
            {
                return await _dbSet.Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método GetList de BusinessLayer.", ex);
            }
        }

        public virtual async Task<IEnumerable<TModel>> GetAllAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método GetAllAsync de BusinessLayer.", ex);
            }
        }
        public IQueryable<TModel> FindBy(Expression<Func<TModel, bool>> predicate)
        {
            try
            {
                IQueryable<TModel> query = _dbSet.Where(predicate);
                return query;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método FindBy de BusinessLayer.", ex);
            }
        }

        public async Task<TModel> GetAsync(Expression<Func<TModel, bool>> predicate)
        {
            try
            {
                return await _dbSet.Where(predicate).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método GetAsync de BusinessLayer.", ex);
            }
        }

        public IQueryable<TModel> GetQueryable(Expression<Func<TModel, bool>> predicate)
        {
            try
            {
                return _dbSet.Where(predicate).AsQueryable();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método GetQueryable de BusinessLayer.", ex);
            }
        }

        public IQueryable<TModel> GetQueryable()
        {
            try
            {
                return _dbSet.AsQueryable();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método GetQueryable() de BusinessLayer.", ex);
            }
        }

        public TModel UpdateEntity(TModel entity)
        {
            try
            {
                return _context.Update(entity).Entity;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método UpdateEntity de BusinessLayer.", ex);
            }
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            try
            {
                TModel entity = await _dbSet.FindAsync(id);
                var type = entity.GetType();
                var prop = type.GetProperty("Borrado");
                prop?.SetValue(entity, true);
                return await CommitAsync();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método DeleteByIdAsync de BusinessLayer.", ex);
            }
        }

        public void DeleteByEntity(TModel entity)
        {
            try
            {
                Type type = entity.GetType();
                var prop = type.GetProperty("Borrado");
                prop?.SetValue(entity, true);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método DeleteByEntity de BusinessLayer.", ex);
            }
        }


        public void RemoveRange(IEnumerable<TModel> entities)
        {
            try
            {
                _dbSet.RemoveRange(entities);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método RemoveRange de BusinessLayer.", ex);
            }
        }

        public bool Commit()
        {
            try
            {
                return _context.SaveChanges() > 0;
            }
            catch (Exception e)
            {
                throw new ArgumentException("Error al hacer commit en la Base de datos a nivel de EF", e);
            }
        }


        public async Task<bool> CommitAsync()
        {

            try
            {
                var savedRegistries = await _context.SaveChangesAsync();
                bool succeeded = savedRegistries > 0;
                return succeeded;
            }
            catch (Exception e)
            {
                throw new ArgumentException("Error al hacer CommitAsync en la Base de datos a nivel de EF", e);
            }
        }

        public virtual async Task<OperationResult> Add(TModel entity)
        {
            try
            {
                var results = _validator.Validate(entity);
                if (results.IsValid)
                {
                    var entry = await _dbSet.AddAsync(entity);
                    return new OperationResult() { Success = true, StatusCode = HttpStatusCode.Created };
                }
                var errosMsg = results.Errors.ToMessage();
                throw new ArgumentException("Error de Validación Realizando operación Add al crear un registro nuevo de BusinessLayer. --> " + errosMsg);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método Add de BusinessLayer.", ex);
            }
        }

        public virtual IEnumerable<OperationResult> AddRange(IEnumerable<TModel> entityEnumerable)
        {
            //TODO Try Catch sin el  (yield)
            foreach (var ent in entityEnumerable)
            {
                var results = _validator.Validate(ent);
                if (!results.IsValid)
                {
                    var errosMsg = results.Errors.ToMessage();
                    yield return new OperationResult() { Success = false, StatusCode = HttpStatusCode.BadRequest, Message = errosMsg };
                }
            }
            _dbSet.AddRange(entityEnumerable);
            yield return new OperationResult() { StatusCode = HttpStatusCode.Created, Success = true };
        }

        public virtual async Task<TModel> Find(int id, params Expression<Func<TModel, object>>[] includeProperties)
        {
            try
            {
                IQueryable<TModel> query = _dbSet.AsQueryable();
                foreach (Expression<Func<TModel, object>> includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
                return await query.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método Find de BusinessLayer.", ex);
            }
        }

        public virtual IQueryable<TModel> FindBy(Expression<Func<TModel, bool>> predicate, params Expression<Func<TModel, object>>[] includeProperties)
        {
            try
            {
                IQueryable<TModel> query = _dbSet.Where(predicate);
                foreach (Expression<Func<TModel, object>> includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
                return query.AsNoTracking();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método FindBy de BusinessLayer.", ex);
            }

        }

        public virtual IQueryable<TModel> GetAll(params Expression<Func<TModel, object>>[] includeProperties)
        {
            try
            {
                IQueryable<TModel> query = _dbSet.AsQueryable();
                foreach (Expression<Func<TModel, object>> includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
                return query;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método GetAll de BusinessLayer.", ex);
            }
        }

        public virtual OperationResult Remove(TModel entity)
        {
            try
            {
                EntityEntry dbEntityEntry = _context.Entry(entity);
                entity.Deleted = true;
                dbEntityEntry.State = EntityState.Modified;
                return new OperationResult() { StatusCode = HttpStatusCode.OK, Success = true };
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método Remove de BusinessLayer.", ex);
            }

        }

        public virtual OperationResult Update(TModel entity)
        {
            try
            {
                var results = _validator.Validate(entity);
                if (results.IsValid)
                {
                    this.ModifyInnerEntities(entity);

                    _context.Entry(entity).State = EntityState.Modified;

                    return new OperationResult() { StatusCode = HttpStatusCode.OK, Success = true, Message = "Registro fue Modificado Exitosamente" };
                }
                return new OperationResult() { Success = false, StatusCode = HttpStatusCode.BadRequest, Message = results.Errors.ToMessage() };
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método Update de BusinessLayer.", ex);
            }
        }

        public virtual IEnumerable<OperationResult> UpdateRange(IEnumerable<TModel> value)
        {
            //TODO Try Catch sin el  (yield)
            foreach (var entity in value)
            {
                yield return this.Update(entity);
            }
        }

        public void ModifyInnerEntities(EntityBase entity)
        {
            try
            {
                Type entityType = entity.GetType();

                foreach (var prop in entityType.GetProperties())
                {
                    var value = prop.GetValue(entity);
                    if (value != null)
                    {
                        if (value is IEnumerable<EntityBase>)
                        {
                            foreach (EntityBase v in value as System.Collections.IEnumerable)
                            {
                                this.AssignEntityStateUpdate(v);
                            }
                        }
                        else if (value is EntityBase)
                        {
                            this.AssignEntityStateUpdate(value as EntityBase);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método ModifyInnerEntities de BusinessLayer.", ex);
            }

        }

        private void AssignEntityStateUpdate(EntityBase obj)
        {
            try
            {
                if (_context.ChangeTracker.Entries().Any(e => e.Entity == obj)) return;
                _context.Entry(obj).State = obj.Id > 0 ? EntityState.Modified : EntityState.Added;
                this.ModifyInnerEntities(obj);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método AssignEntityStateUpdate de BusinessLayer.", ex);
            }
        }

        public virtual async Task<OperationResult> SaveAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return new OperationResult() { Success = true, StatusCode = HttpStatusCode.InternalServerError };
            }
            catch (DbUpdateException ex)
            {
                throw new ArgumentException("Error en método SaveAsync de BusinessLayer.", ex);
                //Log the error (uncomment ex variable name and write a log.)
                //ModelState.AddModelError("", "Unable to save changes. " +
                //"Try again, and if the problem persists, " +
                // "see your system administrator.");
            }
        }

        public virtual async Task<PageCollection<TModel>> GetPagedAsync(
            int page,
            int take,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy,
            Expression<Func<TModel, bool>> predicate = null,
            Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> include = null
        )
        {
            try
            {
                var query = _context.Set<TModel>().AsQueryable();
                var originalPages = page;

                page--;

                if (page > 0)
                    page *= take;

                query = PrepareQuery(query, predicate, include, orderBy);

                var result = new PageCollection<TModel>
                {
                    Items = await query.Skip(page).Take(take).ToListAsync(),
                    TotalItems = await query.CountAsync(),
                    CurrentPage = originalPages
                };

                if (result.TotalItems > 0)
                {
                    result.Pages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(result.TotalItems) / take));
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método GetPagedAsync de BusinessLayer.", ex);
            }
        }

        public virtual PageCollection<TModel> GetPaged(
            int page,
            int take,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy,
            Expression<Func<TModel, bool>> predicate = null,
            Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> include = null
        )
        {
            try
            {
                var query = _context.Set<TModel>().AsQueryable();
                var originalPages = page;

                page--;

                if (page > 0)
                    page *= take;

                query = PrepareQuery(query, predicate, include, orderBy);

                var result = new PageCollection<TModel>
                {
                    Items = query.Skip(page).Take(take).ToList(),
                    TotalItems = query.Count(),
                    CurrentPage = originalPages
                };

                if (result.TotalItems > 0)
                {
                    result.Pages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(result.TotalItems) / take));
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método GetPaged de BusinessLayer.", ex);
            }
        }

        public virtual async Task<decimal?> SumAsync(
            Expression<Func<TModel, bool>> predicate = null
        )
        {
            try
            {
                var query = _context.Set<TModel>().AsQueryable();
                query = PrepareQuery(query, predicate);
                return await ((IQueryable<decimal?>)query).SumAsync();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método SumAsync de BusinessLayer.", ex);
            }
        }

        public virtual decimal? Sum(
            Expression<Func<TModel, bool>> predicate = null
        )
        {
            try
            {
                IQueryable<TModel> query = _context.Set<TModel>().AsQueryable();
                query = PrepareQuery(query, predicate);
                return ((IQueryable<decimal?>)query).Sum();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método Sum de BusinessLayer.", ex);
            }
        }

        public virtual async Task<int> CountAsync(
            Expression<Func<TModel, bool>> predicate = null
        )
        {
            try
            {
                IQueryable<TModel> query = _context.Set<TModel>().AsQueryable();
                query = PrepareQuery(query, predicate);
                return await query.CountAsync();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método CountAsync de BusinessLayer.", ex);
            }
        }

        public virtual int Count(
            Expression<Func<TModel, bool>> predicate = null
        )
        {
            try
            {
                IQueryable<TModel> query = _context.Set<TModel>().AsQueryable();
                query = PrepareQuery(query, predicate);
                return query.Count();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método Count de BusinessLayer.", ex);
            }
        }



        public virtual TModel First(
            Expression<Func<TModel, bool>> predicate,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
            Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> include = null
        )
        {
            try
            {
                IQueryable<TModel> query = _context.Set<TModel>().AsQueryable();
                query = PrepareQuery(query, predicate, include, orderBy);
                return query.First();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método GetAsync de BusinessLayer.", ex);
            }
        }

        public virtual async Task<TModel> FirstAsync(
            Expression<Func<TModel, bool>> predicate,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
            Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> include = null
        )
        {
            try
            {
                IQueryable<TModel> query = _context.Set<TModel>().AsQueryable();
                query = PrepareQuery(query, predicate, include, orderBy);
                return await query.FirstAsync();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método FirstAsync de BusinessLayer.", ex);
            }
        }

        public virtual TModel FirstOrDefault(
            Expression<Func<TModel, bool>> predicate,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
            Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> include = null
        )
        {
            try
            {
                IQueryable<TModel> query = _context.Set<TModel>().AsQueryable();
                query = PrepareQuery(query, predicate, include, orderBy);
                return query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método FirstOrDefault de BusinessLayer.", ex);
            }
        }

        public virtual async Task<TModel> FirstOrDefaultAsync(
            Expression<Func<TModel, bool>> predicate,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
            Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> include = null
        )
        {
            try
            {
                IQueryable<TModel> query = _context.Set<TModel>().AsQueryable();
                query = PrepareQuery(query, predicate, include, orderBy);
                return await query.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método FirstOrDefaultAsync de BusinessLayer.", ex);
            }
        }


        public virtual TModel Single(
            Expression<Func<TModel, bool>> predicate,
            Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> include = null
        )
        {
            try
            {
                IQueryable<TModel> query = _context.Set<TModel>().AsQueryable();
                query = PrepareQuery(query, predicate, include);
                return query.Single();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método Single de BusinessLayer.", ex);
            }
        }

        public virtual async Task<TModel> SingleAsync(
            Expression<Func<TModel, bool>> predicate,
            Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> include = null
        )
        {
            try
            {
                IQueryable<TModel> query = _context.Set<TModel>().AsQueryable();
                query = PrepareQuery(query, predicate, include);
                return await query.SingleAsync();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método SingleAsync de BusinessLayer.", ex);
            }
        }

        public virtual TModel SingleOrDefault(
            Expression<Func<TModel, bool>> predicate,
            Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> include = null
        )
        {
            try
            {
                IQueryable<TModel> query = _context.Set<TModel>().AsQueryable();
                query = PrepareQuery(query, predicate, include);
                return query.SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método SingleOrDefault de BusinessLayer.", ex);
            }
        }

        public virtual async Task<TModel> SingleOrDefaultAsync(
            Expression<Func<TModel, bool>> predicate,
            Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> include = null
        )
        {
            try
            {
                IQueryable<TModel> query = _context.Set<TModel>().AsQueryable();
                query = PrepareQuery(query, predicate, include);
                return await query.SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error en método SingleOrDefaultAsync de BusinessLayer.", ex);
            }
        }

        protected OperationResult Respuesta(string mensaje, bool Success, HttpStatusCode statusRequestCode, Exception ex)
        {
            return new OperationResult()
            {
                StatusCode = statusRequestCode,
                Message = mensaje + (
                            (ex != null) ?
                                ("\n Mensaje de respuesta  - - -> \n" + ((ex.InnerException != null) ? ex.InnerException.Message : ex.Message))
                            : ""),
                Success = Success
            };

        }
    }
}
