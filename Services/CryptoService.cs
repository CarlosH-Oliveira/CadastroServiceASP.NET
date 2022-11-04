using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace CadastroService.Services
{
    public class CryptoService
    {
        public string Criptografar(string entrada)
        {
            var hash = SHA1.Create();
            var encoding = new ASCIIEncoding();
            byte[] entradaByte = System.Text.Encoding.UTF8.GetBytes(entrada);
            entradaByte = hash.ComputeHash(entradaByte);
            var strHexa = new StringBuilder();
            
            foreach( var item in entradaByte)
            {
                strHexa.Append(item.ToString("x2"));
            }
            return strHexa.ToString();
        }
    }
}
