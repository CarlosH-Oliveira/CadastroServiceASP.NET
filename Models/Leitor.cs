using System.ComponentModel.DataAnnotations;

namespace CadastroService.Models
{
    public class Leitor
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Senha { get; set; }
        [Required]
        public string Email { get; set; }
        public string Codigo_De_Ativacao { get; set; }
        public bool Ativo { get; set; }
        public DateTime Data_De_Criacao { get; set; }

        public Leitor()
        {
            Data_De_Criacao = DateTime.Now;
        }
    }
}
