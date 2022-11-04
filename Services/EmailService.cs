using System.Net.Mail;
using CadastroService.Models;
using MailKit.Net.Smtp;
using MimeKit;
namespace CadastroService.Services
{
    public class EmailService
    {
  
			IConfiguration _configuracao;
			public EmailService(IConfiguration config)
			{
				_configuracao = config;
			}

		public bool ValidaEmail(string email)
		{
			var valid = true;

			try
			{
				var emailAddress = new MailAddress(email);
			}
			catch
			{
				valid = false;
			}

			return valid;
		}
		public void EnviarEmail(string[] destinatarios, string assunto, string conteudo)
			{
				Mensagem email = new Mensagem(destinatarios, assunto, conteudo);

				var mensagemDeEmail = new MimeMessage();
				mensagemDeEmail.From.Add(new MailboxAddress("CadastroService",_configuracao.GetValue<string>("EmailSettings:From")));
				mensagemDeEmail.To.AddRange(email.Destinatarios);
				mensagemDeEmail.Subject = email.Assunto;
				mensagemDeEmail.Body = new TextPart(MimeKit.Text.TextFormat.Text)
				{
					Text = email.Conteudo
				};

				using (var client = new MailKit.Net.Smtp.SmtpClient())
				{
					try
					{
						client.CheckCertificateRevocation = false;
						client.Connect(_configuracao.GetValue<string>("EmailSettings:SmtpServer"),
									   _configuracao.GetValue<int>("EmailSettings:Port"),
									  true);

						client.Authenticate(_configuracao.GetValue<string>("EmailSettings:From"),
										   _configuracao.GetValue<string>("EmailSettings:Password"));
						client.Send(mensagemDeEmail);
					}
					catch
					{
						throw;
					}
					finally
					{
						client.Disconnect(true);
						client.Dispose();
					}
				}
			}
		}
    }

