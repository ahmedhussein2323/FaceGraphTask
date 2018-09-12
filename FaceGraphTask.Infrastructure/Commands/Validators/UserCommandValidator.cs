using FaceGraphTask.Core.Commands;
using FrameWork.Command;
using FrameWork.Command.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceGraphTask.Infrastructure.Commands.Validators
{
    public class AddUserCommandValidator : ICommandValidator<AddUser>
    {
        #region properties
        public ValidationResult Result { get; set; }
        #endregion

        #region constructors
        public AddUserCommandValidator()
        {
            Result = new ValidationResult
            {
                Date = DateTime.UtcNow,
                IsValid = true,
                ErrorMessages = new List<string>()
            };
        }
        #endregion

        #region Interfaces Impelementation
        public virtual ValidationResult Validate(AddUser command)
        {
            return Result;
        }
        #endregion

        #region methods

        #endregion


    }
    public class EditUserCommandValidator : ICommandValidator<EditUser>
    {
        #region propreties
        public ValidationResult Result { get; set; }
        #endregion

        #region constructors
        public EditUserCommandValidator()
        {
            Result = new ValidationResult
            {
                Date = DateTime.UtcNow,
                IsValid = true,
                ErrorMessages = new List<string>()
            };
        }
        #endregion

        #region Interfaces Impelementation
        public ValidationResult Validate(EditUser command)
        {
            return Result;
        }
        #endregion

        #region methods

        #endregion
    }
    public class DeleteUserCommandValidator : ICommandValidator<DeleteUser>
    {
        #region propreties
        public ValidationResult Result { get; set; }
        #endregion

        #region constructors
        public DeleteUserCommandValidator()
        {
            Result = new ValidationResult
            {
                Date = DateTime.UtcNow,
                IsValid = true,
                ErrorMessages = new List<string>()
            };
        }
        #endregion

        #region Interfaces Impelementation
        public ValidationResult Validate(DeleteUser command)
        {
            return Result;
        }
        #endregion

        #region methods

        #endregion
    }
}
