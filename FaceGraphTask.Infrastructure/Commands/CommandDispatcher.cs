using Autofac;
using FrameWork.Command.Interfaces;

namespace FaceGraphTask.Infrastructure.Commands
{
    public class CommandDispatcher : ICommandDispatcher
    {

        #region fields
        readonly IContainer _container;
        #endregion

        #region constructors
        public CommandDispatcher(IContainer container)
        {
            _container = container;
        }
        #endregion

        #region Interface Implementation

        dynamic ICommandDispatcher.GetHandler<TCommand>(TCommand command)
        {
            var t = typeof(Interfaces.IFrameWorkCommandHandler<>).MakeGenericType(command.GetType());
            return _container.Resolve(t);
        }

        dynamic ICommandDispatcher.GetValidator<TCommand>(TCommand command)
        {
            var t = typeof(ICommandValidator<>).MakeGenericType(command.GetType());
            return _container.Resolve(t);
        }

        #endregion
    }

}
