using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Business.Exceptions;
using Business.Helpers;
using Business.Interfaces;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using MailKit;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Business.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IEmailVerificationRepository _emailVerificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly EmailServiceOptions _options;
        private readonly ILogger<EmailSenderService> _logger;
        public EmailSenderService(ILogger<EmailSenderService> logger, IOptions<EmailServiceOptions> options,IEmailVerificationRepository emailVerificationRepository, IUserRepository userRepository)
        {
            _options = options.Value;
            _logger = logger;
            _emailVerificationRepository = emailVerificationRepository;
            _userRepository = userRepository;
        }

        public async Task CreateEmailVerificationCode(Guid userId)
        {

            var userEntity = await _userRepository.GetAsync(userId);
            if (userEntity == null)
            {
                _logger.LogError($"user with {userId} id not exists");
                throw new NotFoundException($"user with {userId} id not exists");
            }
            var emailVerificationCode = RandomStringGenerator.GetRandomString(_options.VerificationCodeLength);
            var emailVerificationEntity = new EmailVerificationEntity
            {
                UserId = userId,
                VerificationCode = emailVerificationCode,
                ExpiresOn = DateTime.Now.AddMinutes(_options.VerificationCodeLifeTime)
            };

            await _emailVerificationRepository.CreateAsync(emailVerificationEntity);
            SendLetterWithVerificationCode(userEntity.Name, userEntity.Email,emailVerificationCode);
        }
        private void SendLetterWithVerificationCode(string name,string email,string verificationCode)
        {
            try
            {
                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress("Hotel reservation system company", _options.AppEmailLogin)); //отправитель сообщения
                message.To.Add(new MailboxAddress(name,email));
                message.Subject = "Email verification"; //тема сообщения
                message.Body = new BodyBuilder() { HtmlBody = $"<div>Your verification code:{verificationCode} . It will be expire in 5  minutes</div>" }.ToMessageBody(); //тело сообщения (так же в формате HTML)

                using (MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect(_options.GoogleServer, 465, true);
                    client.Authenticate(_options.AppEmailLogin, _options.AppEmailPassword);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.GetBaseException().Message);
            }
            
        }
    }
    }

