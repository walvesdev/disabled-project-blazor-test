using MediatR;

using System.Collections.Generic;

using Sindvamb.Domain.DTO;
using Sindvamb.Events.BuildingBlocks.Messages;
using Sindvamb.Service.Handlers.PartnerUnits;
using Sindvamb.Utils.Attributes;

namespace Sindvamb.Service.Commands.Partner
{
    [DependencyInjection(
        ServiceType = typeof(IRequestHandler<GetPartnerUnitByFilterCommand, List<PartnerUnitDTO>>),
        ImplementType = typeof(PartnerUnitCommandHandler))
    ]
    public class GetPartnerUnitByFilterCommand : Command<GetPartnerUnitByFilterCommand, FilterDTO, List<PartnerUnitDTO>>
    {
        public FilterDTO Filter { get; set; }
        public GetPartnerUnitByFilterCommand(FilterDTO dto) => Filter = dto;
    }
}
