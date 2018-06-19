using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using BigBallz.Core;
using BigBallz.Helpers;
using BigBallz.Models;
using System.Threading.Tasks;
using BigBallz.MailTemplates;
using Elmah;

namespace BigBallz.Services.L2S
{
    public class MailService : IMailService
    {
        private const string SenderName = "BigBallz";
        private static readonly JobHost JobHost = new JobHost();

        public void SendRegistration(User user, string paymentUrl, string activationUrl)
        {
            if (user.EmailAddressVerified)
                SendMail(user.UserName, user.EmailAddress, "Confirmação de Registro", PrepareMailBodyWith(EmailTemplates.RegistrationEmailVerified, "userName", user.UserName, "userEmail", user.EmailAddress, "paymentUrl", paymentUrl));
            else
                SendMail(user.UserName, user.EmailAddress, "Confirmação de Registro", PrepareMailBodyWith(EmailTemplates.RegistrationEmailNotVerified, "userName", user.UserName, "userEmail", user.EmailAddress, "paymentUrl", paymentUrl, "url", activationUrl, "price", ConfigurationHelper.Price.ToMoney()));
        }

        public void SendPaymentConfirmation(User user)
        {
            SendMail(user.UserName, user.EmailAddress, "Confirmação de Pagamento", PrepareMailBodyWith(EmailTemplates.PaymentConfirmation, "userName", user.UserName, "userEmail", user.EmailAddress));
            SendMail("Admin", "admin@bigballz.com.br", "BigBallz - Novo Usuário", $"{user.UserName} <{user.EmailAddress}> foi autorizado pelo {user.AuthorizedBy} e já está participando do BigBallz.");
        }

        public void SendBonusExpirationWarning(User user, DateTime startTime)
        {
            Debug.Write("Enviando email de aviso de fim do bonus proximo");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");

            var hoursLeft = Math.Round(startTime.AddHours(-1).Subtract(DateTime.Now.BrazilTimeZone()).TotalHours, 2);

            var parameters = new List<string>(new[] { "userName", user.UserName, "endBetTime", hoursLeft + " hora" + (hoursLeft > 1 ? "s" : "") });

            SendMail(user.UserName, user.EmailAddress, "Aviso de fim do Bonus", PrepareMailBodyWith(EmailTemplates.BonusExpirationWarning, parameters.ToArray()));
        }

        public void SendNewCommentPosted(User[] recipients, string userName, string comment)
        {
            var messageBody = PrepareMailBodyWith(EmailTemplates.NewCommentPosted, "userName", userName, "comments", comment, "date", DateTime.Now.BrazilTimeZone().FormatDateTime());

            var recipientsCollection = new MailAddressCollection();
            recipients.Where(u => u.EmailAddressVerified).ForEach(u => recipientsCollection.Add(u.EmailAddress));

            SendMail(recipientsCollection, "Novo comentário", messageBody);
        }

        public void SendBetWarning(User user, IList<Match> matches)
        {
            Debug.Write("Enviando email de aviso de fim de aposta proximo");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");

            var hoursLeft = Math.Round(matches.First().StartTime.AddHours(-1).Subtract(DateTime.Now.BrazilTimeZone()).TotalHours, 2);

            var parameters = new List<string>(new[] { "userName", user.UserName, "startTime", matches.First().StartTime.FormatDateTime(), "endBetTime", hoursLeft + " hora" + (hoursLeft > 1 ? "s" : "") });

            var sb = new StringBuilder("<table><tbody>");
            foreach (var match in matches)
            {
                sb.AppendFormat("<tr><td>{0} X {1}</td></tr>", match.Team1.Name, match.Team2.Name);
            }

            sb.Append("</tbody></table>");
            parameters.Add("bets");
            parameters.Add(sb.ToString());

            SendMail(user.UserName, user.EmailAddress, "Aviso de fim das apostas", PrepareMailBodyWith(EmailTemplates.BetTimeWarning, parameters.ToArray()));
        }

        public void SendEndBetAlert(User user, IList<Bet> bets)
        {
            Debug.Write("Enviando email de aviso de jogo");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
            var parameters = new List<string>(new[] { "userName", user.UserName, "startTime", bets.First().Match1.StartTime.FormatDateTime() });

            var sb = new StringBuilder("<table><thead><tr><th>Jogador</th><td>Aposta</td></thead><tbody>");
            foreach (var bet in bets)
            {
                var matchString = string.Format("{0} {1} X {2} {3}", bet.Match1.Team1.Name, bet.Score1?.ToString(CultureInfo.InvariantCulture) ?? "-", bet.Score2.HasValue ? bet.Score2.Value.ToString(CultureInfo.InvariantCulture) : "-", bet.Match1.Team2.Name);
                sb.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", bet.User1.UserName, matchString);
            }
            sb.Append("</tbody></table>");
            parameters.Add("bets");
            parameters.Add(sb.ToString());

            SendMail(user.UserName, user.EmailAddress, "Apostas Encerradas", PrepareMailBodyWith(EmailTemplates.BetTimeAlert, parameters.ToArray()));
        }

        public void SendEndBonusAlert(User user, IList<BonusBet> bonusBets)
        {
            var parameters = new List<string>(new[] { "userName", user.UserName });

            var sb = new StringBuilder("<table><thead><tr><th>Jogador</th><td>Bonus</td><td>Aposta</td></thead><tbody>");
            foreach (var bet in bonusBets)
            {
                var bonusString = bet.Bonus11.Name;
                var bonusBetString = bet.Team1 != null ? bet.Team1.Name : string.Empty;
                sb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", bet.User1.UserName, bonusString, bonusBetString);
            }
            sb.Append("</tbody></table>");
            parameters.Add("bonusBets");
            parameters.Add(sb.ToString());

            SendMail(user.UserName, user.EmailAddress, "Apostas BONUS Encerradas", PrepareMailBodyWith(EmailTemplates.BonusTimeAlert, parameters.ToArray()));
        }

        private string PrepareMailBodyWith(string templateName, params string[] pairs)
        {
            var body = GetMailBodyOfTemplate(templateName);

            for (var i = 0; i < pairs.Length; i += 2)
            {
                body = body.Replace("<%={0}%>".FormatWith(pairs[i]), pairs[i + 1]);
            }

            body = body.Replace("<%=siteTitle%>", SenderName);
            body = body.Replace("<%=rootUrl%>", "www.bigballz.com.br");

            return body;
        }

        private string GetMailBodyOfTemplate(string templateName)
        {
            var body = ReadFileFrom(templateName);
            return body;
        }

        private string ReadFileFrom(string templateName)
        {
            var templateDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MailTemplates");
            var filePath = string.Concat(Path.Combine(templateDirectory, templateName), ".txt");

            var body = File.ReadAllText(filePath);

            return body;
        }

        public void SendMail(string name, string address, string subject, string message)
        {
            var mailAddresses = new MailAddressCollection {new MailAddress(address, name)};
            SendMail(mailAddresses, subject, message);
        }

        public void SendMail(string addresses, string subject, string message)
        {
            var mailAddresses = new MailAddressCollection();
            foreach (var address in addresses.Split(';'))
            {
                mailAddresses.Add(new MailAddress(address));
            }

            SendMail(mailAddresses, subject, message);
        }

        public void SendMail(MailAddressCollection addresses, string subject, string message)
        {
            var mailMessage = new MailMessage
            {
                IsBodyHtml = true,
                Subject = subject,
                Body = message
            };

            if (addresses.Count == 1)
            {
                mailMessage.To.Add(addresses.First());
            }
            else
            {
                foreach (var address in addresses)
                {
                    mailMessage.Bcc.Add(address);
                }
            }

            Task.Factory.StartNew(() => JobHost.DoWork(() =>
            {
                try
                {
                    var mail = new SmtpClient();
                    mail.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    ErrorLog.GetDefault(null).Log(new Error(ex));
                }
            }));
        }
    }
}