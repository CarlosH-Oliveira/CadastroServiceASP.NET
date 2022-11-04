using MimeKit;

namespace CadastroService.Models
{
    public class Mensagem
    {
        
        public List<MailboxAddress> Destinatarios { get; set; }
        public string Assunto { get; set; }
        public string Conteudo { get; set; }

        public Mensagem(IEnumerable<string> destinatario, string assunto,
            string conteudo)
        {
            Destinatarios = new List<MailboxAddress>();
            Destinatarios.AddRange(destinatario.Select(d => new MailboxAddress("",d)));
            Assunto = assunto;
            Conteudo = conteudo;
        }
        
    }
}
