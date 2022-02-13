﻿
using Microsoft.AspNetCore.Mvc;
using SDBlog.BusinessLayer.Interfaces;
using SDBlog.BusinessLayer.Interfaces.Mails;
using SDBlog.DataModel.Entities.Mails;
using System;
using System.Threading.Tasks;

namespace SDBlog.Api.Controllers.Mails
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService mailService;
        public MailController(IMailService mailService)
        {
            this.mailService = mailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromForm] Mail request)
        {
            try
            {
                await mailService.SendEmailAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

    }
}
