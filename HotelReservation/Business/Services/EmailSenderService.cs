using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Schema;
using Business.Exceptions;
using Business.Helpers;
using Business.Interfaces;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<bool> CheckVerificationCode(Guid userId, string verificationCode)
        {
            var userEntity = await _userRepository.GetAsync(userId);
            if (userEntity == null)
            {
                _logger.LogError($"user with {userId} id not exists");
                throw new NotFoundException($"user with {userId} id not exists");
            }

            var verificationEmailEntities = _emailVerificationRepository.FindByUserId(userId);
            var verificationEmailEntity = verificationEmailEntities.First();
            var verificationEmailEntitiesIds = new List<Guid>();
            var secondsMinimum = DateTime.Now.Subtract(verificationEmailEntities.First().ExpiresOn).TotalSeconds;

            foreach (var verificationEmail in verificationEmailEntities)
            {
                verificationEmailEntitiesIds.Add(verificationEmail.Id);
                var tempSeconds = DateTime.Now.Subtract(verificationEmail.ExpiresOn).TotalSeconds;
                if (!(tempSeconds < secondsMinimum)) continue;
                secondsMinimum = tempSeconds;
                verificationEmailEntity = verificationEmail;
            }

            foreach (var id in verificationEmailEntitiesIds.Where(id => id != verificationEmailEntity.Id))
            {
                await _emailVerificationRepository.DeleteAsync(id);
            }
            if (verificationEmailEntity == null)
            {
                _logger.LogError($"verification code for user with {userId} id not exists");
                throw new NotFoundException($"verification code for user with {userId} id not exists");
            }
            if (verificationEmailEntity.VerificationCode != verificationCode)
            {
                throw new BadRequestException("verification code is incorrect");
            }

            if (verificationEmailEntity.ExpiresOn < DateTime.Now)
            {

                throw new BadRequestException("verification code is expired");
            }


            userEntity.IsVerified = true;
            await _userRepository.UpdateAsync(userEntity);
            await _emailVerificationRepository.DeleteAsync(verificationEmailEntity.Id);
            return true;
        }
        private void SendLetterWithVerificationCode(string name,string email,string verificationCode)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Hotel reservation system company", _options.AppEmailLogin));
                message.To.Add(new MailboxAddress(name, email));
                message.Subject = "Email verification";
                message.Body = new BodyBuilder { HtmlBody = $"<div>Your verification code: {verificationCode} . It will be expire in 5  minutes</div>" }.ToMessageBody();

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
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

