using FrameWork.Command.Interfaces;

namespace FaceGraphTask.Infrastructure.Commands.Interfaces
{
    public interface IFrameWorkCommandHandler<in T> where T : ICommand
    {
        void Handle(T command);
    }
}
