using Payments.Application;
using Payments.Application.Queries.v1.GetProfiles;
using Payments.Infra;
using Microsoft.EntityFrameworkCore;
using Payments.Application.Queries.v1.GetPayments;
using Payments.Application.Commands.v1.CreateProfile;
using Payments.Application.Commands.v1.CreatePayment;
using Payments.Application.Commands.v1.UpdatePayment;
using Payments.Application.Queries.v1.GetPaymentById;
using Payments.Application.Commands.v1.DeletePayment;
using Payments.Application.Commands.v1.DeleteProfile;
using Payments.Application.Queries.v1.GetProfileById;
using Payments.Application.Queries.v1.GetPaymentsSummary;
using Payments.Application.Queries.v1.GetBackup;
using System.Reflection;
using Payments.Application.Queries.v1.GetCategories;
using Payments.Application.Queries.v1.GetBalances;
using Payments.Application.Commands.v1.CreateBalances;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddDbContext<PaymentsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddControllers();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new()
        {
            Title = "Payments API",
            Version = "v1",
            Description = "API for managing payments, profiles, planned balances and backups"
        };
        return Task.CompletedTask;
    });
});

// Configuração para incluir comentários XML na documentação
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IQueryHandler<GetBackupQuery, GetBackupQueryResponse>, GetBackupQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetBalancesQuery, GetBalancesQueryResponse>, GetBalancesQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetCategoriesQuery, GetCategoriesQueryResponse>, GetCategoriesQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetPaymentByIdQuery, GetPaymentByIdQueryResponse>, GetPaymentByIdQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetPaymentsQuery, GetPaymentsQueryResponse>, GetPaymentsQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetPaymentsSummaryQuery, GetPaymentsSummaryQueryResponse>, GetPaymentsSummaryQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetProfileByIdQuery, GetProfileByIdQueryResponse>, GetProfileByIdQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetProfilesQuery, GetProfilesQueryResponse>, GetProfilesQueryHandler>();
builder.Services.AddScoped<ICommandHandler<CreateBalancesCommand, CreateBalancesCommandResponse>, CreateBalancesCommandHandler>();
builder.Services.AddScoped<ICommandHandler<CreatePaymentCommand, CreatePaymentCommandResponse>, CreatePaymentCommandHandler>();
builder.Services.AddScoped<ICommandHandler<CreateProfileCommand, CreateProfileCommandResponse>, CreateProfileCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeletePaymentCommand, DeletePaymentCommandResponse>, DeletePaymentCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteProfileCommand, DeleteProfileCommandResponse>, DeleteProfileCommandHandler>();
builder.Services.AddScoped<ICommandHandler<UpdatePaymentCommand, UpdatePaymentCommandResponse>, UpdatePaymentCommandHandler>();

builder.Services.AddScoped<CqrsDispatcher>();

var app = builder.Build();

app.MapOpenApi();

app.MapGet("/", () => "Payments API is running!");

app.MapControllers();
app.Run();