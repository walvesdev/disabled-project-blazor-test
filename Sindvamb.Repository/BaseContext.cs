using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Sindvamb.Domain.Entities;
using Sindvamb.Repository.Extensions;
using Sindvamb.Repository.Models;
using Sindvamb.Utils;
using Sindvamb.Utils.Attributes;
using Sindvamb.Utils.Converters;
using Sindvamb.Utils.Extension;

using System.Globalization;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Sindvamb.Repository;

// Add-Migration AddDependentIdFileControl -Context SindvambDbContext -StartupProject Application\Sindvamb.Application.Host 
// Update-Database -Context SindvambDbContext -StartupProject Application\Sindvamb.Application.Host
// Remove-Migration -Context SindvambDbContext -StartupProject Application\Sindvamb.Application.Host

public class BaseContext : DbContext, IBaseContext
{
    public virtual DbSet<Audit> Audit { get; set; }
    public virtual DbSet<LoginHistory> LoginHistory { get; set; }
    public virtual DbSet<EmailHistory> EmailHistory { get; set; }

    public DbContext DbContext { get; set; }
    public string ConxtexName { get; set; }
    public Guid? UserId { get; set; }
    public string IpAddress { get; set; }
    public string ConnectionStringName { get; set; }
    public bool UseProvider { get; set; }
    public bool EnableSoftDelete { get; set; }
    public HttpContext HttpContext { get; set; }

    public CultureInfo Culture { get; set; }  = new CultureInfo("pt-BR");

    public string pattern = "yyyy-MM-dd HH-mm-ss";

    public BaseContext()
    {
        EnableSoftDelete = true;
        HttpContext = AppServices.GetHttpContext();
    }

    public BaseContext(DbContextOptions options) : base(options)
    {
        EnableSoftDelete = true;
        HttpContext = AppServices.GetHttpContext();

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                var attributes = property.PropertyInfo?.GetCustomAttributes(typeof(EncryptedAttribute), false);

                if (attributes?.Any() == true)
                {
                    property.SetValueConverter(new EncryptedConverter());
                }
            }
        }

        foreach (var entityType in modelBuilder.Model.GetEntityTypes()) // DEV-60499
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTimeOffset))
                {
                    property.SetValueConverter(new DateTimeOffsetConverter());
                }
                else if (property.ClrType == typeof(DateTimeOffset?))
                {
                    property.SetValueConverter(new NullableDateTimeOffsetConverter());
                }
            }
        }

        OnModelCreatingBase(modelBuilder);
        modelBuilder.UseSnakeCaseNames();
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseContext).Assembly);
    }

    protected virtual void OnModelCreatingBase(ModelBuilder modelBuilder)
    {
    }

    protected sealed override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>()
            .HaveConversion(typeof(DateTimeToDateTimeUtc));

        configurationBuilder
            .Properties<DateTimeOffset>()
            .HaveConversion<DateTimeOffsetConverter>();

        configurationBuilder
            .Properties<DateTimeOffset>()
            .HaveConversion<NullableDateTimeOffsetConverter>();

    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess = true)
    {
        try
        {
            // foreach (var entry in ChangeTracker.Entries())
            // {
            //     foreach (var property in entry.Properties)
            //     {
            //
            //         if (property.Metadata.ClrType == typeof(string) && !property.Metadata.Name.Contains("password"))
            //         {
            //             if (property.CurrentValue == null) continue;
            //             property.CurrentValue = ((string)property.CurrentValue).ToUpper();
            //         }
            //
            //         if (property.Metadata.ClrType == typeof(DateTime))
            //         {
            //             //property.CurrentValue =
            //             //    DateTime.SpecifyKind((DateTime)property.CurrentValue!, DateTimeKind.Local);
            //
            //             var date = ((DateTime)property.CurrentValue!).ToString(pattern);
            //             var dateTime = DateTime.ParseExact(date, pattern, Culture);
            //             property.CurrentValue = dateTime;
            //         }
            //
            //         if (property.Metadata.ClrType == typeof(DateTime?))
            //         {
            //             //if (property.CurrentValue == null) continue;
            //             //property.CurrentValue =
            //             //    DateTime.SpecifyKind(((DateTime?)property.CurrentValue!).Value, DateTimeKind.Local);
            //
            //             if (property.CurrentValue == null) continue;
            //             var date = ((DateTime?)property.CurrentValue!).Value.ToString(pattern);
            //             var dateTime = DateTime.ParseExact(date, pattern, Culture);
            //             property.CurrentValue = dateTime;
            //         }
            //     }
            // }

            if (UserId != null && UserId.Equals(Guid.Empty))
            {
                UserId = null;
            }

            RunEntitiesAudit();
            var auditEntries = OnBeforeSaveChanges();
            var result = base.SaveChanges(acceptAllChangesOnSuccess);
            OnAfterSaveChanges(auditEntries);
            return result;
        }
        catch
        {
            return default;
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {

            // foreach (var entry in ChangeTracker.Entries())
            // {
            //     foreach (var property in entry.Properties)
            //     {
            //
            //         if (property.Metadata.ClrType == typeof(string) && !property.Metadata.Name.Contains("password"))
            //         {
            //             if (property.CurrentValue == null) continue;
            //             property.CurrentValue = ((string)property.CurrentValue).ToUpper();
            //         }
            //
            //         //if (property.Metadata.ClrType == typeof(DateTime))
            //         //{
            //         //    property.CurrentValue =
            //         //        DateTime.SpecifyKind((DateTime)property.CurrentValue!, DateTimeKind.Local);
            //         //}
            //
            //         //if (property.Metadata.ClrType == typeof(DateTime?))
            //         //{
            //         //    if (property.CurrentValue == null) continue;
            //         //    property.CurrentValue =
            //         //        DateTime.SpecifyKind(((DateTime?)property.CurrentValue!).Value, DateTimeKind.Local);
            //         //}
            //
            //         if (property.Metadata.ClrType == typeof(DateTime))
            //         {
            //             //property.CurrentValue =
            //             //    DateTime.SpecifyKind((DateTime)property.CurrentValue!, DateTimeKind.Local);
            //
            //             var date = ((DateTime)property.CurrentValue!).ToString(pattern);
            //             var dateTime = DateTime.ParseExact(date, pattern, Culture);
            //             property.CurrentValue = dateTime;
            //         }
            //
            //         if (property.Metadata.ClrType == typeof(DateTime?))
            //         {
            //             //if (property.CurrentValue == null) continue;
            //             //property.CurrentValue =
            //             //    DateTime.SpecifyKind(((DateTime?)property.CurrentValue!).Value, DateTimeKind.Local);
            //
            //             if (property.CurrentValue == null) continue;
            //             var date = ((DateTime?)property.CurrentValue!).Value.ToString(pattern);
            //             var dateTime = DateTime.ParseExact(date, pattern, Culture);
            //             property.CurrentValue = dateTime;
            //         }
            //     }
            // }

            if (UserId != null && UserId.Equals(Guid.Empty))
            {
                UserId = null;
            }

            RunEntitiesAudit();
            var auditEntries = OnBeforeSaveChanges();
            var result = await base.SaveChangesAsync(cancellationToken);
            OnAfterSaveChanges(auditEntries);
            return result;
        }
        catch
        {
            return default;
        }

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionStringName = ConxtexName.Replace("DbContext", "").Trim();

        if (!optionsBuilder!.IsConfigured)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            Console.WriteLine($"Environment: {env}");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json",
                    optional: true, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString(connectionStringName);

            optionsBuilder.UseNpgsql(connectionString,
                options =>
                {
                    options.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                });

            optionsBuilder
                .LogTo(Console.WriteLine)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging();

            base.OnConfiguring(optionsBuilder);
        }
    }

    private void RunEntitiesAudit()
    {
        var entries = ChangeTracker.Entries();

        foreach (var entry in entries)
        {
            var type = entry.Entity.GetType();

            var isEntityBaseAudit =
                entry.Entity.GetType().GetBaseClasses().Any(c => c.Name.Contains("EntityBaseAudit"));

            if (isEntityBaseAudit)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow.ToFormatedDate();
                    entry.Property("CreatorId").CurrentValue = UserId;
                    entry.Property("UpdatedAt").CurrentValue = null;
                    entry.Property("DeletedAt").CurrentValue = null;
                }

                if (entry.State == EntityState.Modified && EnableSoftDelete)
                {
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow.ToFormatedDate();
                    entry.Property("ModifierId").CurrentValue = UserId;
                    entry.Property("CreatedAt").IsModified = false;
                    entry.Property("CreatorId").IsModified = false;
                    entry.Property("DeletedAt").IsModified = false;
                    entry.Property("DeleterId").IsModified = false;
                    entry.Property("IsDeleted").IsModified = false;
                }
                else if (entry.State == EntityState.Modified && !EnableSoftDelete)
                {
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow.ToFormatedDate();
                    entry.Property("ModifierId").CurrentValue = UserId;
                    entry.Property("CreatedAt").IsModified = false;
                    entry.Property("CreatorId").IsModified = false;
                }

                if (entry.State == EntityState.Deleted && EnableSoftDelete)
                {
                    entry.Property("UpdatedAt").IsModified = false;
                    entry.Property("ModifierId").IsModified = false;
                    entry.Property("CreatedAt").IsModified = false;
                    entry.Property("CreatorId").IsModified = false;
                    entry.Property("IsDeleted").CurrentValue = true;
                    entry.Property("DeleterId").CurrentValue = UserId;
                    entry.Property("DeletedAt").CurrentValue = DateTime.UtcNow.ToFormatedDate();
                    entry.State = EntityState.Modified;
                }
            }
        }
    }

    private List<AuditEntry> OnBeforeSaveChanges()
    {
        ChangeTracker.DetectChanges();
        var auditEntries = new List<AuditEntry>();

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
            {
                continue;
            }

            var auditEntry = new AuditEntry(entry);
            auditEntry.TableName = entry.Metadata.GetTableName()!;
            auditEntry.UserId = UserId;
            auditEntry.IpAddress = IpAddress;
            auditEntries.Add(auditEntry);

            foreach (var property in entry.Properties)
            {
                if (property.IsTemporary)
                {
                    // value will be generated by the database, get the value after saving
                    auditEntry.TemporaryProperties.Add(property);
                    continue;
                }

                var propertyName = property.Metadata.Name;
                if (property.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues[propertyName] = property.CurrentValue!;
                    continue;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        auditEntry.NewValues[propertyName] = property.CurrentValue!;
                        break;

                    case EntityState.Deleted:
                        auditEntry.OldValues[propertyName] = property.OriginalValue!;
                        break;

                    case EntityState.Modified:
                        if (property.IsModified)
                        {
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                        }

                        break;
                }
            }
        }

        // Save audit entities that have all the modifications
        try
        {
            foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties))
            {
                Audit.Add(auditEntry.ToAudit());
            }
        }
        catch
        {
            return default;
        }

        // keep a list of entries where the value of some properties are unknown at this step
        return auditEntries.Where(_ => _.HasTemporaryProperties).ToList();
    }

    private int OnAfterSaveChanges(List<AuditEntry> auditEntries)
    {
        if (auditEntries == null || auditEntries.Count == 0)
        {
            return 0;
        }

        foreach (var auditEntry in auditEntries)
        {
            // Get the final value of the temporary properties
            foreach (var prop in auditEntry.TemporaryProperties)
            {
                if (prop.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                }
                else
                {
                    auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                }
            }

            // Save the Audit entry
            Audit.Add(auditEntry.ToAudit());
        }

        return SaveChanges();
    }
}

public interface IBaseContext
{
    public DbContext DbContext { get; set; }
    public HttpContext HttpContext { get; set; }
    public string ConxtexName { get; set; }
    public Guid? UserId { get; set; }
    public string IpAddress { get; set; }
    public string ConnectionStringName { get; set; }
    public bool UseProvider { get; set; }
    public bool EnableSoftDelete { get; set; }
}