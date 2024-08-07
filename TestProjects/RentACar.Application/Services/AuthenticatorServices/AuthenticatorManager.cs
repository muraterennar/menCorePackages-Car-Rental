﻿using MenCore.CrossCuttingConserns.Exceptions.Types;
using MenCore.Mailing;
using MenCore.Security.EmailAuthenticator;
using MenCore.Security.Entities;
using MenCore.Security.Enums;
using MenCore.Security.OtpAuthenticator;
using MimeKit;
using RentACar.Application.Services.Repositories;
using RentACar.Infrastructure.Mail;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RentACar.Infrastructure.Mail.Constants;
using RentACar.Infrastructure.Mail.GeneratedTemplates.EnableMailAuthenticator;

namespace RentACar.Application.Services.AuthenticatorServices
{
    public class AuthenticatorManager : IAuthenticatorService
    {
        private readonly IEmailAuthenticatorHelper _emailAuthenticatorHelper;
        private readonly IEmailAuthenticatorRepository _emailAuthenticatorRepository;
        private readonly IMailService _mailService;
        private readonly IOtpAuthenticatorHelper _otpAuthenticatorHelper;
        private readonly IOtpAuthenticatorRepository _otpAuthenticatorRepository;
        private readonly IEnableEmailAuthenticatorTemplate _mailTemplateGeneratorService;

        public AuthenticatorManager(IEmailAuthenticatorHelper emailAuthenticatorHelper,
            IEmailAuthenticatorRepository emailAuthenticatorRepository, IMailService mailService,
            IOtpAuthenticatorHelper otpAuthenticatorHelper, IOtpAuthenticatorRepository otpAuthenticatorRepository,
            IEnableEmailAuthenticatorTemplate mailTemplateGeneratorService)
        {
            _emailAuthenticatorHelper = emailAuthenticatorHelper;
            _emailAuthenticatorRepository = emailAuthenticatorRepository;
            _mailService = mailService;
            _otpAuthenticatorHelper = otpAuthenticatorHelper;
            _otpAuthenticatorRepository = otpAuthenticatorRepository;
            _mailTemplateGeneratorService = mailTemplateGeneratorService;
        }

        // Kullanıcı için bir e-posta doğrulayıcı oluşturur
        public async Task<EmailAuthenticator> CreateEmailAuthenticator(User user)
        {
            EmailAuthenticator emailAuthenticator = new()
            {
                UserId = user.Id,
                ActivationKey = await _emailAuthenticatorHelper.CreateEmailActivationKey(),
                IsVerified = false
            };
            return emailAuthenticator;
        }

        // Kullanıcı için bir OTP doğrulayıcı oluşturur
        public async Task<OtpAuthenticator> CreateOtpAuthenticator(User user)
        {
            OtpAuthenticator otpAuthenticator = new()
            {
                UserId = user.Id,
                SecretKey = await _otpAuthenticatorHelper.GenerateSecretKey(),
                IsVerified = false
            };
            return otpAuthenticator;
        }

        // Gizli anahtar, email ve issuer bilgisi ile bir OTP QR kodu oluşturur
        public async Task<string> GenerateOtpQrCode(byte[] secretKey, string email, string issuer, string filePath)
        {
            var result = await _otpAuthenticatorHelper.GenerateQrCode(secretKey, email, issuer, filePath);
            return result;
        }

        // Gizli anahtar kullanarak bir OTP kodu oluşturur
        public async Task<string> CreateOtpCode(byte[] secretKey)
        {
            string otp = await _otpAuthenticatorHelper.GenerateOtpCode(secretKey);
            return otp;
        }

        // Bir byte dizisini bir dizeye dönüştürür
        public async Task<string> ConvertSecretKeyToString(byte[] secretKey)
        {
            var result = await _otpAuthenticatorHelper.ConvertSecretKeyToString(secretKey);
            return result;
        }

        // Kullanıcıya doğrulayıcı kodu gönderir
        public async Task SendAuthenticatorCode(User user)
        {
            if (user.AuthenticatorType == AuthenticatorType.Email)
                await SendAuthenticatorCodeWithEmail(user);
        }

        // Kullanıcının doğrulayıcı kodunu doğrular
        public async Task VerifyAuthenticatorCode(User user, string authenticatorCode)
        {
            if (user.AuthenticatorType == AuthenticatorType.Email)
                await VerifyAuthenticatorCodeWithEmail(user, authenticatorCode);
            else if (user.AuthenticatorType == AuthenticatorType.Otp)
                await VerifyAuthenticatorCodeWithOtp(user, authenticatorCode);
        }

        // Kullanıcıya e-posta ile doğrulayıcı kod gönderir
        private async Task SendAuthenticatorCodeWithEmail(User user)
        {
            var emailAuthenticator = await _emailAuthenticatorRepository.GetAsync(e => e.UserId == user.Id);
            if (emailAuthenticator == null)
                throw new NotFoundException("Email Authenticator not found.");
            if (!emailAuthenticator.IsVerified)
                throw new BusinessException("Email Authenticator must be verified.");

            var authenticatorCode = await _emailAuthenticatorHelper.CreateEmailActivationCode();
            emailAuthenticator.ActivationKey = authenticatorCode;
            await _emailAuthenticatorRepository.UpdateAsync(emailAuthenticator);

            var toEmailList = new List<MailboxAddress> { new($"{user.FirstName} {user.LastName}", user.Email) };

            string mailTemplate =
               await _mailTemplateGeneratorService.GenerateBodyAsync(authenticatorCode, MailTemplateUrl.VerifyEmailAuthenticatorTemplate);

            _mailService.SendMail(
                new Mail
                {
                    ToList = toEmailList,
                    Subject = "Authenticator Code - RentACar",
                    HtmlBody = mailTemplate
                }
            );
        }

        // Kullanıcının e-posta ile doğrulayıcı kodunu doğrular
        private async Task VerifyAuthenticatorCodeWithEmail(User user, string authenticatorCode)
        {
            var emailAuthenticator = await _emailAuthenticatorRepository.GetAsync(e => e.UserId == user.Id);
            if (emailAuthenticator == null)
                throw new NotFoundException("Email Authenticator not found.");
            if (emailAuthenticator.ActivationKey != authenticatorCode)
                throw new BusinessException("Authenticator code is invalid.");
            emailAuthenticator.ActivationKey = null;
            await _emailAuthenticatorRepository.UpdateAsync(emailAuthenticator);
        }

        // Kullanıcının OTP ile doğrulayıcı kodunu doğrular
        private async Task VerifyAuthenticatorCodeWithOtp(User user, string authenticatorCode)
        {
            var otpAuthenticator = await _otpAuthenticatorRepository.GetAsync(e => e.UserId == user.Id);
            if (otpAuthenticator == null)
                throw new NotFoundException("Otp Authenticator not found.");
            bool result = await _otpAuthenticatorHelper.VerifyOtpCode(otpAuthenticator.SecretKey, authenticatorCode);
            if (!result)
                throw new BusinessException("Authenticator code is invalid.");
        }
    }
}