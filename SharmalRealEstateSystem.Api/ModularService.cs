using SharmalRealEstateSystem.Repositories.Features.Admin.Dashboard;
using SharmalRealEstateSystem.Shared.Configs;

namespace SharmalRealEstateSystem.Api;

#region Modular Service

public static class ModularService
{
    #region Add Features

    public static IServiceCollection AddFeatures(
        this IServiceCollection services,
        WebApplicationBuilder builder
    )
    {
        return services
            .AddDbContextService(builder)
            .AddJsonService()
            .AddAuthService()
            .AddCustomServices()
            .AddSwaggerAuthorizationService(builder)
            .AddCorsPolicyService(builder)
            .AddAuthenticationService(builder);
    }

    #endregion

    #region Add Db Context Service

    private static IServiceCollection AddDbContextService(
        this IServiceCollection services,
        WebApplicationBuilder builder
    )
    {
        builder.Services.AddDbContext<AppDbContext>(
            opt =>
            {
                opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                if (Deployment.IsDevelopment())
                {
                    opt.UseSqlServer(DatabaseConfig.UATDbConnectionString());
                }
                else
                {
                    opt.UseSqlServer(DatabaseConfig.ProdDbConnectionString());
                }

            },
            ServiceLifetime.Transient
        );

        return services;
    }

    #endregion

    #region Add Json Service

    private static IServiceCollection AddJsonService(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
        return services;
    }

    #endregion

    #region AddAuthService

    private static IServiceCollection AddAuthService(this IServiceCollection services)
    {
        return services.AddScoped<JWTAuth>();
    }

    #endregion

    #region Add Custom Services

    private static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        return services
            .AddTransient<AesService>()
            .AddScoped<DapperService>()
            .AddScoped<FtpService>()
            .AddTransient<TokenValidationService>();
    }

    #endregion

    #region Add Authentication Service

    private static IServiceCollection AddAuthenticationService(
        this IServiceCollection services,
        WebApplicationBuilder builder
    )
    {
        builder
            .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    //ValidAudience = builder.Configuration["Jwt:Audience"],
                    //ValidAudience = Environment.GetEnvironmentVariable("Sharmal_Audience"),
                    ValidAudience = Deployment.IsDevelopment() ? JWTConfig.UATAudience : JWTConfig.Audience,

                    //ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    //ValidIssuer = Environment.GetEnvironmentVariable("Sharmal_Issuer"),
                    ValidIssuer = Deployment.IsDevelopment() ? JWTConfig.UATIssuer : JWTConfig.Issuer,

                    IssuerSigningKey = new SymmetricSecurityKey(
                    //Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
                    Encoding.UTF8.GetBytes(Deployment.IsDevelopment() ? JWTConfig.UATJWTKey : JWTConfig.JWTKey)
                    )
                };
            });

        return services;
    }

    #endregion

    #region Add Cors Policy Service

    private static IServiceCollection AddCorsPolicyService(
        this IServiceCollection services,
        WebApplicationBuilder builder
    )
    {
        builder.Services.AddCors();
        return services;
    }

    #endregion

    #region Add Swagger Authorization

    private static IServiceCollection AddSwaggerAuthorizationService(
        this IServiceCollection services,
        WebApplicationBuilder builder
    )
    {
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = "Sharmal Real Estate Management System API",
                    Version = "v1"
                }
            );
            c.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                }
            );
            c.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                }
            );
        });

        builder.Services.AddAuthorization();

        return services;
    }

    #endregion

    public static IApplicationBuilder AddAuthenticationMiddleware(this WebApplication app)
    {
        return app.UseMiddleware<AuthenticationMiddleware>();
    }
}

#endregion

#region Admin Modular Service

public static class AdminModularService
{
    #region Add Admin Features

    public static IServiceCollection AddAdminFeatures(this IServiceCollection services)
    {
        return services
            .AddRepositoryService()
            .AddUnitOfWorkService()
            .AddBusinessLogicService()
            .AddValidatorService()
            .AddCustomMiddlewareService();
    }

    #endregion

    #region Add Business Logic Service

    private static IServiceCollection AddBusinessLogicService(this IServiceCollection services)
    {
        return services
            .AddScoped<Features.Admin.Auth.BL_Auth>()
            .AddScoped<BL_ExchangeRate>()
            .AddScoped<BL_Property>()
            .AddScoped<BL_Feature>()
            .AddScoped<BL_Inquiry>()
            .AddScoped<BL_AdsPage>()
            .AddScoped<BL_Ads>()
            .AddScoped<BL_Car>();
    }

    #endregion

    #region Add Unit Of Work Service

    private static IServiceCollection AddUnitOfWorkService(this IServiceCollection services)
    {
        return services.AddScoped<IAdminUnitOfWork, AdminUnitOfWork>();
    }

    #endregion

    #region Add Repository Service

    private static IServiceCollection AddRepositoryService(this IServiceCollection services)
    {
        return services
            .AddScoped<
                Repositories.Features.Admin.Auth.IAuthRepository,
                Repositories.Features.Admin.Auth.AuthRepository
            >()
            .AddScoped<IExchangeRateRepository, ExchangeRateRepository>()
            .AddScoped<IPropertyRepository, PropertyRepository>()
            .AddScoped<IFeatureRepository, FeatureRepository>()
            .AddScoped<IInquiryRepository, InquiryRepository>()
            .AddScoped<IAdsPageRepository, AdsPageRepository>()
            .AddScoped<IAdsRepository, AdsRepository>()
            .AddScoped<ICarRepository, CarRepository>()
            .AddScoped<IDashboardRepository, DashboardRepository>();
    }

    #endregion

    #region Add Custom Middleware Service

    private static IServiceCollection AddCustomMiddlewareService(this IServiceCollection services)
    {
        return services
            .AddTransient<CustomAuthMiddleware>()
            .AddTransient<CustomExchangeRateMiddleware>()
            .AddTransient<CustomPropertyMiddleware>()
            .AddTransient<CustomFeatureMiddleware>()
            .AddTransient<CustomInquiryMiddleware>()
            .AddTransient<CustomAdsPageMiddleware>()
            .AddTransient<CustomAdsMiddleware>()
            .AddTransient<CustomCarMiddleware>()
            .AddTransient<CustomUpdateProfileMiddleware>();
    }

    #endregion

    #region Add Validator Service

    private static IServiceCollection AddValidatorService(this IServiceCollection services)
    {
        return services
            .AddTransient<Shared.Services.ValidationServices.Admin.Auth.LoginValidator>()
            .AddTransient<Shared.Services.ValidationServices.Admin.Auth.RegisterValidator>()
            .AddTransient<ExchangeRateValidator>()
            .AddTransient<PropertyValidator>()
            .AddTransient<FeatureValidator>()
            .AddTransient<InquiryValidator>()
            .AddTransient<AdsPageValidator>()
            .AddTransient<AdsValidator>()
            .AddTransient<UpdatePropertyValidator>()
            .AddTransient<UpdateAdsValidator>()
            .AddTransient<CarValidator>()
            .AddTransient<UpdateCarValidator>()
            .AddTransient<UpdateAuthValidator>()
            .AddTransient<UpdateProfileValidator>();
    }

    #endregion

    #region Add Custom Middleware

    public static IApplicationBuilder AddCustomMiddleware(this WebApplication app)
    {
        return app.UseMiddleware<CustomMiddleware>();
    }

    #endregion
}

#endregion

#region User Modular Service

public static class UserModularService
{
    #region Add User Features

    public static IServiceCollection AddUserFeatures(this IServiceCollection services)
    {
        return services.AddBusinessLogicService().AddRepositoryService();
    }

    #endregion

    #region Add Business Logic Service

    private static IServiceCollection AddBusinessLogicService(this IServiceCollection services)
    {
        return services
            .AddScoped<Features.User.Auth.BL_Auth>()
            .AddScoped<Features.User.Property.BL_Property>()
            .AddScoped<Features.User.Car.BL_Car>();
    }

    #endregion

    #region Add Repository Service

    private static IServiceCollection AddRepositoryService(this IServiceCollection services)
    {
        return services.AddScoped<
            Repositories.Features.User.Auth.IAuthRepository,
            Repositories.Features.User.Auth.AuthRepository
        >();
    }

    #endregion
}

#endregion
