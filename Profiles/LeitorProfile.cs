using AutoMapper;
using CadastroService.DTOS.LEITOR;
using CadastroService.Models;

namespace CadastroService.Profiles
{
    public class LeitorProfile : Profile
    {
        public LeitorProfile()
        {
            CreateMap<Leitor, ReadLeitorDTO>();
            CreateMap<Leitor, ReadLeitorArqDTO>();
            CreateMap<CreateLeitorDTO, Leitor>();
            CreateMap<UpdateLeitorDTO, Leitor>();
        }
    }
}
