using FaceGraphTask.Core.Commands;
using FaceGraphTask.Core.Entities;
using FaceGraphTask.Infrastructure.Commands.Interfaces;
using FaceGraphTask.Infrastructure.DbContext;

namespace FaceGraphTask.Infrastructure.Commands.Handlers
{
    public class AddUserCommandHandler : IFrameWorkCommandHandler<AddUser>
    {
        public void Handle(AddUser command)
        {
            new DocumentDbRepository<User>("User").CreateItemAsync(new User
            {
                Id = command.Id,
                Email = command.Email,
                Name = command.Name,
                Password = command.Password,
                Role = command.Role
            });
        }
    }
    public class EditUserCommandHandler : IFrameWorkCommandHandler<EditUser>
    {
        public async void Handle(EditUser command)
        {
            var item = new User
            {
                Id = command.Id,
                Email = command.Email,
                Name = command.Name,
                Password = command.Password,
                Role = command.Role
            };
            await new DocumentDbRepository<User>("User").UpdateItemAsync(item.Id, item);
        }
    }
    public class DeleteUserCommandHandler : IFrameWorkCommandHandler<DeleteUser>
    {
        public void Handle(DeleteUser command)
        {
            new DocumentDbRepository<User>("User").DeleteItemAsync(command.Id, "");
        }
    }
}
