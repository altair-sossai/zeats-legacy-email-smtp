namespace Zeats.Legacy.Email.SMTP.Models
{
    public class ConfiguracaoSmtp
    {
        public string Host { get; set; }
        public int Porta { get; set; }
        public string Remetente { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public bool Ssl { get; set; }
        public bool CredenciaisPadrao { get; set; }
    }
}