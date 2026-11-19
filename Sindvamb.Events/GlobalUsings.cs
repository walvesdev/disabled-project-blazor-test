#region

global using MediatR;
global using System;
global using System.Linq;
global using System.Threading.Tasks;
global using System.Collections.Generic;
global using System.Net;
global using System.Net.Http.Headers;
global using System.Text;
global using Sindvamb.Domain;
global using Sindvamb.Utils;
global using Sindvamb.Utils.Extension;
global using Sindvamb.Utils.Helpers;
global using Sindvamb.Utils.Types;
global using Sindvamb.Utils.Services.DependencyInjection;
global using Sindvamb.Utils.Services.Http;
global using Sindvamb.Utils.Filters.CustomExceptions;
global using Sindvamb.Events.BuildingBlocks.Mediator;
global using Sindvamb.Events.BuildingBlocks.Common;
global using Sindvamb.Events.BuildingBlocks.CommonMessages.DomainEvents;
global using Sindvamb.Events.BuildingBlocks.CommonMessages.Notifications;
global using Sindvamb.Events.BuildingBlocks.EventSourcing;
global using Sindvamb.Events.BuildingBlocks.Messages;

#endregion