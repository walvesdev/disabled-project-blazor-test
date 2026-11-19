
global using System;
global using System.Linq;
global using System.Threading.Tasks;
global using System.Collections.Generic;
global using Microsoft.Extensions.DependencyInjection;
global using System.Net;
global using System.Net.Http.Headers;
global using System.Text;
global using Sindvamb.Domain;
global using Sindvamb.Domain.DTO;
global using Sindvamb.Domain.Models;
global using Sindvamb.Domain.Entities;
global using Sindvamb.Domain.Enums;
global using System.Diagnostics.CodeAnalysis;

global using Blazor.SubtleCrypto;
global using Microsoft.AspNetCore.Components.Authorization;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection.Extensions;
global using Microsoft.Extensions.Options;
global using Microsoft.Extensions.Logging;

global using Sindvamb.Web.Shared.Components.Core;

global using Sindvamb.Web.Shared.Services.Http;
global using Sindvamb.Web.Shared.Services.Http.Models;
global using Sindvamb.Web.Shared.Utils;
global using Sindvamb.Web.Shared.Extensions;

global using Sindvamb.Web.Shared.Services.Customers;
global using Sindvamb.Web.Shared.Services.CustomerTypes;
global using Sindvamb.Web.Shared.Services.Orders;
global using Sindvamb.Web.Shared.Services.OrderCategories;
global using Sindvamb.Web.Shared.Services.OrdersStatus;
global using Sindvamb.Web.Shared.Services.OrderTypes;
global using Sindvamb.Web.Shared.Services.Auth;
global using Sindvamb.Web.Shared.Services.Partners;
global using Sindvamb.Web.Shared.Services.PartnerUnit;
global using Sindvamb.Web.Shared.Services.Users;
global using Sindvamb.Web.Shared.Services;
global using Sindvamb.Web.Shared.Services.SpecialNeed;
global using Sindvamb.Web.Shared.Services.OrderAreas;
global using Sindvamb.Web.Shared.Services.EducationDegrees;
global using Sindvamb.Web.Shared.Models;

global using BootstrapBlazor;
global using BootstrapBlazor.Components;