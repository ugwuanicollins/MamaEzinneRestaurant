using Core.DATABASE;
using Core.Models;
using Logic.IHelpers;
using Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Helpers
{
    public class EmailHelper : IEmailHelper
    {

        private readonly IEmailService _emailService;
        private readonly IEmailConfiguration _emailConfiguration;
        public IUserHelper _userHelper;
        private readonly AppDbContext _context;
        public EmailHelper(IEmailService emailService, IEmailConfiguration emailConfiguration, IUserHelper userHelper, AppDbContext context)
        {
            _emailService = emailService;
            _emailConfiguration = emailConfiguration;
            _userHelper = userHelper;
            _context = context;
        }

        public void SendApprovalEmailToCustomer(string email)
        {
            try
            {
                if (email != null)
                {
                    string toEmail = email;
                    string subject = "Payment Confirmation";
                    string message = "We got your transaction, Thank you for patronizing Us";
                    _emailService.SendEmail(toEmail, subject, message);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void SendDeclinationEmailToCustomer(string email)
        {
            try
            {
                if (email != null)
                {
                    string toEmail = email;
                    string subject = "Payment Confirmation";
                    string message = "Sorry we didn't receive your transaction, check your balance";
                    _emailService.SendEmail(toEmail, subject, message);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
