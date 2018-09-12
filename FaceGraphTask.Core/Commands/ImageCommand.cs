
using FrameWork.Command.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceGraphTask.Core.Commands
{
    public class AddImage : BaseCommand
    {
        #region prop
        public string Url { set; get; }
        public Guid UserId { set; get; }
        #endregion
    }

    public class EditImage : BaseCommand
    {
        #region prop
        public string Url { set; get; }
        public Guid UserId { set; get; }
        #endregion
    }

    public class DeleteImage : BaseCommand
    {
        #region prop
        #endregion
    }
}
