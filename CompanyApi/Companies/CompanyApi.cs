using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CompanyApi.Companies;

internal static class CompanyApi
{
    public static RouteGroupBuilder MapCompanies(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/companies");

        group.WithTags("Companies");

        group.MapGet("/", async (CompanyDbContext context) => await Get(context));

        group.MapGet("/{id}", async (Guid id, CompanyDbContext context) => await Get(id, context));

        group.MapPost("/", async (CompanyDto companyDto, CompanyDbContext context)
            => await Post(companyDto, context));

        group.MapPut("/{id}",
            async (Guid id, CompanyDto companyDto, CompanyDbContext context) => await Put(id, companyDto, context));

        group.MapDelete("/{id}", async (Guid id, CompanyDbContext context) => await Delete(id, context));
        return group;
    }
    static async Task<IEnumerable<Company>> Get(CompanyDbContext context)
    {
        return await context.Companies.ToListAsync();
    }

    static async Task<Results<Ok<CompanyDto>, NotFound>> Get(Guid id, CompanyDbContext context)
    {
        return await context.Companies.FindAsync(id) switch
        {
            { } company when company.Id == id => TypedResults.Ok(company.AsCompanyDto()),
            _ => TypedResults.NotFound()
        };
    }

    static async Task<Created<CompanyDto>> Post(CompanyDto companyDto, CompanyDbContext context)
    {
        var company = companyDto.AsCompany();
        await context.Companies.AddAsync(company);
        await context.SaveChangesAsync();

        return TypedResults.Created($"/companies/{company.Id}", company.AsCompanyDto());
    }

    static async Task<Results<Ok, NotFound, BadRequest>> Put(Guid id, CompanyDto companyDto, CompanyDbContext context)
    {
        if (id != companyDto.Id)
        {
            return TypedResults.BadRequest();
        }

        var rowsAffected = await context.Companies.Where(c => c.Id == id)
            .ExecuteUpdateAsync(
                update =>
                    update.SetProperty(c => c.Name, companyDto.Name)
                        .SetProperty(c => c.Description, companyDto.Description));

        return rowsAffected > 0 ? TypedResults.Ok() : TypedResults.NotFound();
    }

    static async Task<Results<NotFound, Ok>> Delete(Guid id, CompanyDbContext context)
    {
        var rowsAffected = await context.Companies.Where(c => c.Id == id).ExecuteDeleteAsync();

        return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.Ok();
    }
}
