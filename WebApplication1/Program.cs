using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Company
{
    public string Name { get; set; }
    public int YearFounded { get; set; }
    public double Revenue { get; set; }
    public int Employees { get; set; }
}

[ApiController]
[Route("[controller]")]
public class CompanyController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private static readonly Random _random = new Random();

    public CompanyController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet("info")]
    public ActionResult<Company> GetCompanyInfo()
    {
        // For simplicity, returning a dummy company
        var company = new Company
        {
            Name = "Example Company",
            YearFounded = 2022,
            Revenue = 1000000.0,
            Employees = 100 // Dummy value, should be replaced with real data
        };

        return Ok(company);
    }

    [HttpGet("randomNumber")]
    public ActionResult<int> GetRandomNumber()
    {
        int randomNumber = _random.Next(0, 101);

        return Ok(randomNumber);
    }

    [HttpGet("companyWithMostEmployees")]
    public ActionResult<string> GetCompanyWithMostEmployees()
    {
        // Retrieve company with most employees from configuration files
        var companyService = new CompanyService(_configuration);
        var companyWithMostEmployees = companyService.GetCompanyWithMostEmployees();

        return Ok(companyWithMostEmployees);
    }

    [HttpGet("myInfo")]
    public ActionResult<JsonElement> GetMyInfo()
    {
        // Read data from JSON configuration file
        var myInfoFile = "my_config.json";
        var myInfoText = System.IO.File.ReadAllText(myInfoFile);
        var myInfo = JsonSerializer.Deserialize<JsonElement>(myInfoText);

        return Ok(myInfo);
    }
}

public class CompanyService
{
    private readonly IConfiguration _configuration;

    public CompanyService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetCompanyWithMostEmployees()
    {
        var companyEmployees = new Dictionary<string, int>();

        // Read data from XML configuration file
        var xmlCompanies = _configuration.GetSection("Companies:Xml").GetChildren();
        foreach (var company in xmlCompanies)
        {
            var name = company.GetValue<string>("Name");
            var employees = company.GetValue<int>("Employees");
            companyEmployees[name] = employees;
        }

        // Read data from JSON configuration file
        var jsonCompanies = _configuration.GetSection("Companies:Json").GetChildren();
        foreach (var company in jsonCompanies)
        {
            var name = company.GetValue<string>("Name");
            var employees = company.GetValue<int>("Employees");
            if (!companyEmployees.ContainsKey(name))
                companyEmployees[name] = 0; // Ensure entry exists before adding
            companyEmployees[name] += employees;
        }

        // Read data from INI configuration file
        var iniCompanies = _configuration.GetSection("Companies:Ini").GetChildren();
        foreach (var company in iniCompanies)
        {
            var name = company.GetValue<string>("Name");
            var employees = company.GetValue<int>("Employees");
            if (!companyEmployees.ContainsKey(name))
                companyEmployees[name] = 0; // Ensure entry exists before adding
            companyEmployees[name] += employees;
        }

        // Find company with most employees
        var maxEmployeesCompany = companyEmployees.OrderByDescending(x => x.Value).First();
        return maxEmployeesCompany.Key;
    }
}

class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();

        // Add controllers
        builder.Services.AddControllers();

        // Add configuration
        builder.Configuration.AddXmlFile("companies.xml");
        builder.Configuration.AddJsonFile("companies.json");
        builder.Configuration.AddIniFile("companies.ini");
        builder.Configuration.AddJsonFile("my_config.json");
        // Add CompanyService
        builder.Services.AddSingleton<CompanyService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();

        // Map controllers
        app.MapControllers();

        app.Run();
    }
}
