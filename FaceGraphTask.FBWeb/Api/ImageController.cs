using FaceGraphTask.Core.Commands;
using FaceGraphTask.Core.Entities;
using FaceGraphTask.Infrastructure.Commands.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using FaceGraphTask.Infrastructure.Service;
using Utilities.Security;

namespace FaceGraphTask.FBWeb.Api
{
    [Authorize]
    public class ImageController : BaseApiController
    {
        public ImageController(ICommandService cCommandService) : base(cCommandService)
        {
        }

        [HttpPost]
        public HttpResponseMessage AddImage()
        {
            try
            {
                var file = HttpContext.Current.Request.Files[0];
                var storageService = new StorageService("images");
                var userId = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier)?.Value;

                var url = storageService.UploadFile(file);
                if (userId != null)
                {
                    var vs = CommandService.Executer.Execute(new AddImage
                    {
                        Id = Guid.NewGuid(),
                        Url = url,
                        UserId = new Guid(userId)
                    });

                    if (vs.IsValid)
                        return Request.CreateResponse(HttpStatusCode.OK, "Done");
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, vs);
                }
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "User Not Found");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetImageList()
        {
            try
            {
                var res = CommandService.UnitOfWork.ImageRepository.Get();
                if (res == null)
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error");
                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage DeleteImage(Guid id)
        {
            try
            {
                var image = CommandService.UnitOfWork.ImageRepository.Get(e => e.Id == id);
                if (image == null || image.Count() == 0)
                    return Request.CreateResponse(HttpStatusCode.NotFound, "");

                var vs = CommandService.Executer.Execute(new DeleteImage
                {
                    Id = id
                });

                if (vs.IsValid)
                {
                    var storageService = new StorageService("images");
                    storageService.DeleteBlob(image.FirstOrDefault().Url);
                    return Request.CreateResponse(HttpStatusCode.OK, "Done");
                }
                return Request.CreateResponse(HttpStatusCode.InternalServerError, vs);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
