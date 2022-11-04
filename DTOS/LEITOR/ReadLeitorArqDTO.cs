using System.ComponentModel.DataAnnotations;

namespace CadastroService.DTOS.LEITOR
{
    public class ReadLeitorArqDTO
    {
        [Required]
        public int Id { get; set; }
        public string Codigo_De_Ativacao { get; set; }

    }
}
