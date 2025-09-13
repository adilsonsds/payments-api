using Payments.Application;
using Payments.Application.Queries.v1.GetProfiles;
using Payments.Infra;
using Microsoft.EntityFrameworkCore;
using Payments.Application.Queries.v1.GetPayments;
using Payments.Application.Commands.v1.CreateProfile;
using Payments.Application.Commands.v1.CreatePayment;
using Payments.Application.Commands.v1.UpdatePayment;
using Payments.Application.Queries.v1.GetPaymentById;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddDbContext<PaymentsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddControllers();
// builder.Services.AddOpenApi();

builder.Services.AddScoped<IQueryHandler<GetProfilesQuery, GetProfilesQueryResponse>, GetProfilesQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetPaymentsQuery, GetPaymentsQueryResponse>, GetPaymentsQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetPaymentByIdQuery, GetPaymentByIdQueryResponse>, GetPaymentByIdQueryHandler>();
builder.Services.AddScoped<ICommandHandler<CreateProfileCommand>, CreateProfileCommandHandler>();
builder.Services.AddScoped<ICommandHandler<CreatePaymentCommand>, CreatePaymentCommandHandler>();
builder.Services.AddScoped<ICommandHandler<UpdatePaymentCommand>, UpdatePaymentCommandHandler>();
builder.Services.AddScoped<CqrsDispatcher>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();
app.Run();