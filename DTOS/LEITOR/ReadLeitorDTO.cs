using System.ComponentModel.DataAnnotations;

namespace CadastroService.DTOS.LEITOR
{
    public class ReadLeitorDTO
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public bool Ativo { get; set; }
        public string Codigo_De_Ativacao { get; set; }
        public DateTime Data_De_Criacao { get; set; }

    }
}
