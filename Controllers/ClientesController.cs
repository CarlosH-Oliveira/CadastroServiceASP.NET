using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CadastroService.Models;
using CadastroService.Models.Context;
using CadastroService.DTOS.LEITOR;
using AutoMapper;
using CadastroService.Services;
using System.Net.Http.Headers;

namespace CadastroService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientesController : Controller
    {
        private readonly LeitorDbContext _context;
        private readonly IMapper _mapper;
        private readonly CryptoService _cryptoService;
        private readonly EmailService _emailService;

        public ClientesController(LeitorDbContext context, IMapper mapper, CryptoService criptografia, EmailService emailService)
        {
            _mapper = mapper;
            _context = context;
            _cryptoService = criptografia;
            _emailService = emailService;
        }

        [HttpGet("/clientes/")]
        public async Task<IActionResult> Index()
        {
            List<Leitor> leitores = _context.Leitores.ToList<Leitor>();
            List<ReadLeitorDTO> readLeitores = _mapper.Map<List<ReadLeitorDTO>>(leitores);
            return Ok(readLeitores);
        }

        [HttpGet("/clientes/{email}/{senha}")]
        public async Task<IActionResult> Details(string email, string senha)
        {
            if (email == null || senha == null || _context.Leitores == null)
            {
                return NotFound();
            }

            if (_emailService.ValidaEmail(email))
            {
                var leitor = await _context.Leitores
                .FirstOrDefaultAsync(m => m.Email == email && m.Senha == _cryptoService.Criptografar(senha));
                if (leitor == null)
                {
                    return NotFound();
                }
                ReadLeitorDTO readLeitor = _mapper.Map<ReadLeitorDTO>(leitor);
                return Ok(readLeitor);
            }
            else
            {
                return BadRequest("Email inválido!");
            }

            
        }

        [HttpGet("/clientes/arq/{codigo}")]
        public async Task<IActionResult> EndArquivoService(string codigo)
        {
            if (codigo == null || _context.Leitores == null)
            {
                return NotFound();
            }
            var leitor = await _context.Leitores
            .FirstOrDefaultAsync(m => m.Codigo_De_Ativacao == codigo);
            if (leitor == null)
            {
                return NotFound();
            }
            ReadLeitorArqDTO readLeitor = _mapper.Map<ReadLeitorArqDTO>(leitor);
            return Ok(readLeitor);
          


        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/clientes/create")]
        public async Task<IActionResult> Create([FromBody] CreateLeitorDTO createLeitor)
        {
            Leitor? leitor = null;
            if (createLeitor != null && _emailService.ValidaEmail(createLeitor.Email)==true)
            {
                leitor = _context.Leitores.FirstOrDefault<Leitor>((m) => m.Email == createLeitor.Email);
                if(leitor == null)
                {
                    leitor = _mapper.Map<Leitor>(createLeitor);
                    _emailService.EnviarEmail(new string[] { leitor.Email }, "Bem-Vindo a nossa plataforma!", $"Ficamos muito felizes de tê-lo em nosso sistema! \nAqui estão suas informações para que não se esqueça:\n\nEmail: {leitor.Email}\nSenha: {leitor.Senha}\n\nAtt, CadastroService");

                    leitor.Senha = _cryptoService.Criptografar(leitor.Senha);
                    leitor.Codigo_De_Ativacao = _cryptoService.Criptografar(leitor.Name + leitor.Email + leitor.Senha);
                    leitor.Ativo = true;
                    _context.Add(leitor);
                    await _context.SaveChangesAsync();

                    return Ok("Criado com sucesso!");
                }
                else
                {
                    return BadRequest("Já existe um usuário cadastrado com esse e-mail. Verifique a mensagem que lhe enviamos, lá poderá rever seus dados para que possa recordar!");
                }
                
            }
            return BadRequest("Formato de dados inválido para registro!");
            
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/clientes/edit")]
        public async Task<IActionResult> Edit([FromQuery] string Email, [FromQuery] string Senha, [FromBody] UpdateLeitorDTO updateLeitor)
        {
            string codigoDeAtivacao = "";
            int id = 0;

            if (_context.Leitores == null)
            {
                return NotFound();
            }

            Leitor? leitor = _context.Leitores
                .FirstOrDefault(m => m.Email == Email && m.Senha == _cryptoService.Criptografar(Senha));

            if (leitor==null)
            {
                return NotFound("Não foi possível localizar seu usuário.");
            }
            else if (ModelState.IsValid){

                codigoDeAtivacao = leitor.Codigo_De_Ativacao;
                id = leitor.Id;

                if (leitor.Email != updateLeitor.Email)
                {
                    leitor.Ativo = false;
                    _emailService.EnviarEmail(new string[] { updateLeitor.Email }, "Ativação de Conta", $"Estamos aqui para te avisar que houve uma alteração no email de sua conta! \nClique nesse link para ativá-la:\n \nhttps://localhost:49153/clientes/ativa/codigoDeAtivacao={leitor.Codigo_De_Ativacao}\n Att, CadastroService");               
                }
                if(leitor.Senha != updateLeitor.Email && leitor.Senha == updateLeitor.Senha)
                {
                    _emailService.EnviarEmail(new string[] { updateLeitor.Email }, "Alteração de Conta", $"Estamos aqui para te avisar que houve uma alteração na sua conta! \nEste será seu novo e-mail a partir de hoje.\n \n Att, CadastroService");
                }

                if (leitor.Senha != updateLeitor.Email && leitor.Senha != updateLeitor.Senha)
                {
                    _emailService.EnviarEmail(new string[] { updateLeitor.Email }, "Alteração de Conta", $"Estamos aqui para te avisar que houve uma alteração na sua conta! \n Aqui estão suas novas informações:\n \n Email: {updateLeitor.Email}\n Senha: {updateLeitor.Senha}\n \n Att, CadastroService");
                }

                _mapper.Map(updateLeitor, leitor);
                leitor.Codigo_De_Ativacao = codigoDeAtivacao;
                leitor.Senha = _cryptoService.Criptografar(leitor.Senha);

                await _context.SaveChangesAsync();
                return Ok(leitor);

            }
            else
            { 
                return StatusCode(500);
            }
                 
        }


        // POST: Clientes/Delete/5
        [HttpPost("/clientes/delete/")]
        public async Task<IActionResult> DeleteConfirmed([FromQuery] string Email, [FromQuery] string Senha)
        {
            if (_context.Leitores == null)
            {
                return Problem("Não existem usuários registrados.");
            }
            var leitor = await _context.Leitores.FirstOrDefaultAsync(m => m.Email == Email && m.Senha == _cryptoService.Criptografar(Senha));
            if (leitor != null)
            {
                _emailService.EnviarEmail(new string[] { leitor.Email }, "Até logo!", $"Estamos aqui para te avisar que sua conta foi deletada.\nEste será apenas um 'Até logo' e não um 'Adeus', estaremos sempre te aguardando de volta em nossos serviços!\n \n Att, CadastroService");
                _context.Leitores.Remove(leitor);
            }
            
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("/clientes/ativa/{codigoDeAtivacao}")]
        public async Task<IActionResult> AtivaConta(string codigoDeAtivacao)
        {
            var leitor = _context.Leitores.FirstOrDefault((m) => m.Codigo_De_Ativacao == codigoDeAtivacao);
            if(leitor != null && leitor.Ativo!=true)
            {
                leitor.Ativo = true;
                leitor.Codigo_De_Ativacao = _cryptoService.Criptografar(leitor.Name + leitor.Email + leitor.Senha);
                await _context.SaveChangesAsync();
                return Ok("Sua conta foi ativada com sucesso!");
            }
            else
            {
                return BadRequest("Não foi possível ativar sua conta. Por favor, verifique se seu acesso já está ativo");
            }
        }


        private bool ClienteExists(int id)
        {
          return _context.Leitores.Any(e => e.Id == id);
        }
    }
}
