using FaceGraphTask.Infrastructure.Commands;
using FaceGraphTask.Infrastructure.Commands.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FaceGraphTask.FBWeb.Api
{
    public class BaseApiController : ApiController
    {
        #region Prop
        protected CommandService CommandService { get; }
        #endregion

        #region constructor
        public BaseApiController(ICommandService cCommandService)
        {
            CommandService = cCommandService as CommandService;
        }
        #endregion

        #region Helper

        #endregion

    }
}
