using System;
using System.Collections.Generic;
using System.Linq;
using APIPassagem.Services;
using APIPassagem.Utils;
using Models;
using Xunit;

namespace TestPassagem
{
    public class UnitTestTicket
    {
        private PassagemService _passagemService;

        private PassagemService InitializeDataBase()
        {
            var settings = new ProjMongoDotnetDatabaseSettings();
            return new PassagemService(settings);
        }

        [Fact]
        public void GetAll()
        {
            _passagemService = InitializeDataBase();
            IEnumerable<Passagem> users = _passagemService.Get();
            Assert.Equal(2, users.Count());
        }

        [Fact]
        public void Create()
        {
            _passagemService = InitializeDataBase();
            var user = new Passagem();
            _passagemService.Create(user);
            user = _passagemService.Get(user.Id);
            Assert.NotNull(user);
        }

        [Fact]
        public void Update()
        {
            _passagemService = InitializeDataBase();
            string id = "";
            var user = _passagemService.Get(id);
            _passagemService.Update(id, new Passagem());
            var newuser = _passagemService.Get(user.Id);
            //se mudou a passagem
            Assert.Equal(user.Passageiro.Cpf,newuser.Passageiro.Cpf);
        }

        [Fact]
        public void Delete()
        {
            _passagemService = InitializeDataBase();
            var user = _passagemService.Get("");
            _passagemService.Remove(user.Id);
            user = _passagemService.Get(user.Id);
            Assert.Null(user);
        }
    }
}
