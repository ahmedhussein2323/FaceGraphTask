
using FrameWork.Command.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceGraphTask.Core.Commands
{
    public class AddUser : BaseCommand
    {
        #region prop
        public string Name { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
        public string Role { set; get; }
        #endregion
    }

    public class EditUser : BaseCommand
    {
        #region prop
        public string Name { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
        public string Role { set; get; }
        #endregion
    }

    public class DeleteUser : BaseCommand
    {
        #region prop
        #endregion
    }
}
