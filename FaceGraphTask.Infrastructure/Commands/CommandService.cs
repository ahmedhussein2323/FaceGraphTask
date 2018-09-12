using FaceGraphTask.Infrastructure.Commands.Interfaces;
using FaceGraphTask.Infrastructure.Queries;
using FrameWork.Command.Interfaces;

namespace FaceGraphTask.Infrastructure.Commands
{
    public class CommandService : ICommandService
    {
        #region propreties
        public ICommandExecuter Executer { get; set; }
        public UnitOfWork UnitOfWork { get; set; }
        #endregion

        #region constructors
        public CommandService(ICommandExecuter executer)
        {
            Executer = executer;
            UnitOfWork = new UnitOfWork();
        }
        #endregion
    }
}
