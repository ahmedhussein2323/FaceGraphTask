using FaceGraphTask.Infrastructure.DbContext;
using FaceGraphTask.Infrastructure.Commands.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaceGraphTask.Core.Commands;
using FaceGraphTask.Core.Entities;

namespace FaceGraphTask.Infrastructure.Commands.Handlers
{
    public class AddImageCommandHandler : IFrameWorkCommandHandler<AddImage>
    {
        public void Handle(AddImage command)
        {
            new DocumentDbRepository<Image>("Image").CreateItemAsync(new Image
            {
                Id = command.Id,
                Url = command.Url,
                UserId = command.UserId
            });
        }
    }
    public class EditImageCommandHandler : IFrameWorkCommandHandler<EditImage>
    {
        public async void Handle(EditImage command)
        {
            var item = new Image
            {
                Id = command.Id,
                Url = command.Url,
                UserId = command.UserId
            };
            await new DocumentDbRepository<Image>("Image").UpdateItemAsync(item.Id, item);
        }
    }
    public class DeleteImageCommandHandler : IFrameWorkCommandHandler<DeleteImage>
    {
        public void Handle(DeleteImage command)
        {
            new DocumentDbRepository<Image>("Image").DeleteItemAsync(command.Id, "");
        }
    }
}
