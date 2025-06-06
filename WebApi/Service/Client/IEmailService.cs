﻿using WebApi.Helper;

namespace WebApi.Service.Client
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
        Task SendFinalContractToCustomer(string toEmail, string contractUrl);

    }
}
