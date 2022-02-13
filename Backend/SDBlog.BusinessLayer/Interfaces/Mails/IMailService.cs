using SDBlog.DataModel.Entities.Mails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDBlog.BusinessLayer.Interfaces.Mails
{
    public interface IMailService
    {
        Task SendEmailAsync(Mail mailRequest);
    }
}
