using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using PcWeb.Api.Data;
using PcWeb.Api.Dtos;
using PcWeb.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<GroupDbContext>(options => options.UseInMemoryDatabase("BasicDb"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGroupsEndpoints();
app.MapMembersEndpoints();
app.MapTransactionsEndpoints();
app.MapDebtsEndpoints();

app.SetupDb();

app.Run();


