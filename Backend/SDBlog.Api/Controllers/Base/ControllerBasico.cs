using Microsoft.AspNetCore.Mvc;
using SDBlog.Core.Classes;
using System;
using System.Net;

namespace SDBlog.Api.Controllers.Base
{
    public class ControllerBasico : ControllerBase
    {
        protected OperationResult Respuesta(string mensaje, bool Success, HttpStatusCode statusRequestCode, Exception ex)
        {
            return new OperationResult()
            {
                StatusCode = statusRequestCode,
                Message = mensaje + (
                            (ex != null) ?
                                ("\n Mensaje de respuesta  - - -> \n" + ((ex.InnerException != null) ? ex.InnerException.Message : ex.Message))
                            : ""),
                Success = Success
            };

        }
    }
}
