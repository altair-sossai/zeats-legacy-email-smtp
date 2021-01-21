using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Zeats.Legacy.Email.Anexos;
using Zeats.Legacy.Email.Servicos;
using Zeats.Legacy.Email.SMTP.Models;

namespace Zeats.Legacy.Email.SMTP.Servicos
{
    public class ServicoEmailSmtp : ServicoEmail
    {
        private readonly ConfiguracaoSmtp _configuracaoSmtp;

        public ServicoEmailSmtp(ConfiguracaoSmtp configuracaoSmtp)
        {
            _configuracaoSmtp = configuracaoSmtp;
        }

        public override void EnviarEmail(string assunto, string corpo, List<string> destinatarios, List<Anexo> anexos = null)
        {
            destinatarios = destinatarios.Where(destinatario => !string.IsNullOrEmpty(destinatario)).Distinct().ToList();

            if (destinatarios.Count == 0)
                return;

            using (var mailMessage = new MailMessage
            {
                Subject = assunto,
                Body = corpo,
                IsBodyHtml = true
            })
            {
                foreach (var destinatario in destinatarios.Distinct())
                    mailMessage.To.Add(destinatario);

                if (_configuracaoSmtp != null)
                    mailMessage.From = new MailAddress(_configuracaoSmtp.Email, _configuracaoSmtp.Remetente);

                if (anexos != null)
                {
                    foreach (var attachment in anexos.Select(anexo => new Attachment(anexo.Conteudo, anexo.Nome, anexo.Tipo)))
                        mailMessage.Attachments.Add(attachment);
                }

                using (var smtpClient = new SmtpClient())
                {
                    if (_configuracaoSmtp != null)
                    {
                        smtpClient.Host = _configuracaoSmtp.Host;
                        smtpClient.Port = _configuracaoSmtp.Porta;
                        smtpClient.EnableSsl = _configuracaoSmtp.Ssl;
                        smtpClient.UseDefaultCredentials = _configuracaoSmtp.CredenciaisPadrao;
                        smtpClient.Credentials = new NetworkCredential(_configuracaoSmtp.Email, _configuracaoSmtp.Senha);
                    }

                    smtpClient.Send(mailMessage);
                }
            }
        }
    }
}