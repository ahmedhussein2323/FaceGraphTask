
using FrameWork.Command.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceGraphTask.Core.Commands
{
    public class BaseCommand : ICommand
    {
        public Guid Id { set; get; }
    }
}
