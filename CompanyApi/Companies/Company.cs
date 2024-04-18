namespace CompanyApi.Companies;

public class Company
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
}

public record CompanyDto(Guid Id, string Name, string? Description);


public static class CompanyMappingExtensions
{
    public static CompanyDto AsCompanyDto(this Company company)
    {
        return new CompanyDto(company.Id, company.Name, company.Description);
    }
}

public static class CompanyDtoMappingExtensions
{
    public static Company AsCompany(this CompanyDto companyDto)
    {
        return new Company
        {
            Id = companyDto.Id, Name = companyDto.Name, Description = companyDto.Description
        };
    }
}
