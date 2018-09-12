using System;
using System.Collections.Generic;
using FrameWork;
using FrameWork.Command;
using FrameWork.Command.Interfaces;
using Newtonsoft.Json;

namespace FaceGraphTask.Infrastructure.Commands
{
    public class CommandExecuter : ICommandExecuter
    {
        #region fields
        protected readonly ICommandDispatcher Dispatcher;
        #endregion

        #region propreties
        #endregion

        #region constructors
        public CommandExecuter(ICommandDispatcher dispatcher)
        {
            Dispatcher = dispatcher;
        }
        #endregion

        #region Interface Implementations
        public virtual ValidationResult Execute<T>(T command) where T : ICommand
        {
            var validator = Dispatcher.GetValidator(command);
            var result = validator.Validate(command as dynamic);

            if (result != null && !result.IsValid)
                return result;

            var handler = Dispatcher.GetHandler(command);

            if (handler == null)
                throw new
                    CommandHandlerNotFoundException(
                    $"no handler found for {command.GetType().Name} command");

            try
            {
                handler.Handle(command);

                result = new ValidationResult
                {
                    IsValid = true
                };
            }
            catch (Exception e)
            {
                if (result == null)
                    return null;

                result.IsValid = false;
                // ReSharper disable once MergeConditionalExpression
                result.ErrorMessages.Add(JsonConvert.SerializeObject(e));
            }

            return result;
        }
        public virtual List<ValidationResult> Execute<T>(List<T> commands) where T : ICommand
        {
            var results = new List<ValidationResult>();

            foreach (var command in commands)
            {
                var validator = Dispatcher.GetValidator(command);
                var result = validator.Validate(command as dynamic);

                if (result != null && !result.IsValid)
                {
                    results.Add(result);
                    return results;
                }

                var handler = Dispatcher.GetHandler(command);

                if (handler == null)
                    throw new
                        CommandHandlerNotFoundException(
                        $"no handler found for {command.GetType().Name} command");

                try
                {
                    handler.Handle(command);

                    result = new ValidationResult
                    {
                        IsValid = true
                    };
                    results.Add(result);
                }
                catch (Exception e)
                {
                    if (result == null)
                        return null;

                    result.IsValid = false;
                    // ReSharper disable once MergeConditionalExpression
                    result.ErrorMessages.Add(JsonConvert.SerializeObject(e));
                    results.Add(result);
                }
            }
            return results;
        }
        public List<ValidationResult> ExecuteTransaction<T>(List<T> commands) where T : ICommand
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
