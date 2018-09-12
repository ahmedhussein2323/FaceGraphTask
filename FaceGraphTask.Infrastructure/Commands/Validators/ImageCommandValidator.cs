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
    public class AddImageCommandValidator : ICommandValidator<AddImage>
    {
        #region properties
        public ValidationResult Result { get; set; }
        #endregion

        #region constructors
        public AddImageCommandValidator()
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
        public virtual ValidationResult Validate(AddImage command)
        {
            return Result;
        }
        #endregion

        #region methods

        #endregion


    }
    public class EditImageCommandValidator : ICommandValidator<EditImage>
    {
        #region propreties
        public ValidationResult Result { get; set; }
        #endregion

        #region constructors
        public EditImageCommandValidator()
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
        public ValidationResult Validate(EditImage command)
        {
            return Result;
        }
        #endregion

        #region methods

        #endregion
    }
    public class DeleteImageCommandValidator : ICommandValidator<DeleteImage>
    {
        #region propreties
        public ValidationResult Result { get; set; }
        #endregion

        #region constructors
        public DeleteImageCommandValidator()
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
        public ValidationResult Validate(DeleteImage command)
        {
            return Result;
        }
        #endregion

        #region methods

        #endregion
    }
}
