using FrameWork.Command.Interfaces;
using FaceGraphTask.Infrastructure.Queries;

namespace FaceGraphTask.Infrastructure.Commands.Interfaces
{
    public interface ICommandService
    {
        ICommandExecuter Executer { get; set; }
        UnitOfWork UnitOfWork { get; set; }
    }
}
