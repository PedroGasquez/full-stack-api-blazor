using Fina.Api.Data;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Handlers;

public class CategoryHandler(AppDbContext context) : ICategoryHandler
{
    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
    {
        var category = new Category
        {
            Title = request.Title,
            Description = request.Description,
            UserId = request.UserId
        };
        
        try
        {
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return new Response<Category?>(category, 201, "Categoria criada com Sucesso");
        }
        catch {
            
            //Serilog, OpenTelemetry
            return new Response<Category?>(null, 500, "Nao foi possivel criar uma categoria");
        }
    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        try
        {
            var category = await context
                .Categories
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            
            if(category is null)
                return new Response<Category?>(null, 404, "Categoria nao encontrada");

            category.Title = request.Title;
            category.Description = request.Description;
            
             context.Categories.Update(category);
             await context.SaveChangesAsync();
             
             return new Response<Category?>(category, 201, "Categoria atualizada com Sucesso");

        }
        catch 
        {
            return new Response<Category?>(null, 500, "Nao foi possivel atualizar uma categoria");
        }
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        try
        {
            var category = await context
                .Categories
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            
            if(category is null)
                return new Response<Category?>(null, 404, "Categoria nao encontrada");
            
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
             
            return new Response<Category?>(category, 201, "Categoria excluida com Sucesso");
        }
        catch 
        {
            return new Response<Category?>(null, 500, "Nao foi possivel remover uma categoria");
        }
    }

    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
    {
        try
        {
            var category = await context
                .Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id && 
                                          x.UserId == request.UserId);

            return category is null
                ? new Response<Category?>(null, 401, "Categoria nao encontrada")
                : new Response<Category?>(category);
        }
        catch 
        {
            return new Response<Category?>(null, 500, "Nao foi possivel recuperar uma categoria");
        }
    }

    public async Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoryRequest request)
    {
        try
        {
            var query = context
                .Categories
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .OrderBy(x => x.Title);

            var categories = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<List<Category>?>(
                categories,
                count,
                request.PageNumber,
                request.PageSize);
        }
        catch 
        {
            return new PagedResponse<List<Category>?>(null, 500, "Nao foi possivel localizar as categoria");

        }
    }
}