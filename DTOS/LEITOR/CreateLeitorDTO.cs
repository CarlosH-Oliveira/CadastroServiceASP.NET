using System.ComponentModel.DataAnnotations;

namespace CadastroService.DTOS.LEITOR
{
    public class CreateLeitorDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Senha { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
