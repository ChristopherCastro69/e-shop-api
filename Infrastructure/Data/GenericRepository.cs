using System;
using System.Buffers.Text;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

// Generic repository for managing entities
public class GenericRepository<T>(StoreContext context) : IGenericRepository<T> where T : BaseEntity
{
    // Adds a new entity to the context
    public void Add(T entity)
    {
        context.Set<T>().Add(entity);
    }

    // Checks if an entity with the specified ID exists
    public bool Exists(int id)
    {
        return context.Set<T>().Any(x=>x.Id == id);
    }

    // Asynchronously retrieves an entity by its ID
    public async Task<T?> GetByIdAsync(int id)
    {
        return await context.Set<T>().FindAsync(id);
    }

    // Asynchronously retrieves an entity based on a specification
    public async Task<T?> GetEntityWithSpec(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    public async Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T, TResult> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();

    }

    // Asynchronously retrieves all entities
    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
        return await context.Set<T>().ToListAsync();
    }

    // Asynchronously retrieves entities based on a specification
    public async Task<IReadOnlyList<T?>> ListAsync(ISpecification<T> spec)
    {
       return await ApplySpecification(spec).ToListAsync(); 
    }

    //different parameter
    public async Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec)
    {
        return await ApplySpecification(spec).ToListAsync();

    }

    // Removes an entity from the context
    public void Remove(T entity)
    {
        context.Set<T>().Remove(entity);
    }

    // Asynchronously saves changes to the context
    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync()>0;
    }

    // Updates an existing entity in the context
    public void Update(T entity)
    {
        context.Set<T>().Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
    }

    // Applies a specification to the queryable set of entities
    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(context.Set<T>().AsQueryable(), spec);
    }

    
    private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> spec)
    {
        return SpecificationEvaluator<T>.GetQuery<T,TResult>(context.Set<T>().AsQueryable(), spec);
    }
}
