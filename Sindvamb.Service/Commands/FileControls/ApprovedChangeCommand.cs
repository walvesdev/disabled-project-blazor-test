using System;
using Sindvamb.Domain.DTO;
using Sindvamb.Events.BuildingBlocks.Messages;

namespace Sindvamb.Service.Commands.FileControls
{
    public class ApprovedChangeCommand : Command<ApprovedChangeCommand, Guid, bool>
    {
        public FileControlDTO FileControl { get; set; }

        public ApprovedChangeCommand(FileControlDTO fileControl)
        {
            FileControl = fileControl;
        }
    }
}