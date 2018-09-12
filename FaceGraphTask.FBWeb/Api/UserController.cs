using FaceGraphTask.Core.Commands;
using FaceGraphTask.Core.Entities;
using FaceGraphTask.Infrastructure.Commands.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using Utilities.Security;

namespace FaceGraphTask.FBWeb.Api
{
    [Authorize]
    public class UserController : BaseApiController
    {
        public UserController(ICommandService cCommandService) : base(cCommandService)
        {
        }

        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage AddUser(User model)
        {
            try
            {
                var check = CommandService.UnitOfWork.UserRepository.Get(e => e.Email == model.Email);
                if (check != null && check.Count() > 0)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Email Exist");
                var vs = CommandService.Executer.Execute(new AddUser
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    Email = model.Email,
                    Password = PasswordHash.CreateHash(model.Password),
                    Role = model.Role
                });

                if (vs.IsValid)
                    return Request.CreateResponse(HttpStatusCode.OK, "Done");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, vs);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetUserList()
        {
            try
            {
                var res = CommandService.UnitOfWork.UserRepository.Get();
                if (res == null)
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error");
                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
