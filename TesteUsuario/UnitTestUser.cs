using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using APIUsuarios.Service;
using APIUsuarios.Services;
using APIUsuarios.Utils;
using Microsoft.EntityFrameworkCore;
using Models;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace TesteUsuario
{
    public class UnitTestUser
    {
        private UsuarioService _usuarioService;

        private UsuarioService InitializeDataBase()
        {
            var settings = new ProjMongoDotnetDatabaseSettings();
            return new UsuarioService(settings);
        }

        [Fact] 
        public void GetAll()
        {
            _usuarioService =InitializeDataBase();
            IEnumerable<Usuario> users = _usuarioService.Get();
            Assert.Equal(2, users.Count());
        }

        [Fact]
        public void GetLogin()
        {
            _usuarioService = InitializeDataBase();
            Assert.Equal("Admin", _usuarioService.GetLogin("Admin").Login);
        }

        [Fact]
        public void Create()
        {
            _usuarioService = InitializeDataBase();
            var user = new Usuario();
            _usuarioService.Create(user);
            user = _usuarioService.Get(user.Id);
            Assert.NotNull(user);
        }

        [Fact]
        public void Update()
        {
            _usuarioService = InitializeDataBase();
            string id = "";
            var user = _usuarioService.Get(id);
            _usuarioService.Update(id, new Usuario());
            var newuser = _usuarioService.Get(user.Id);
            //se mudou a passageiro
            Assert.Equal(user.Login, newuser.Login);
        }

        [Fact]
        public void Delete()
        {
            _usuarioService = InitializeDataBase();
            var user = _usuarioService.Get("");
            _usuarioService.Remove(user.Id);
            user = _usuarioService.Get(user.Id);
            Assert.Null(user);
        }
    }
}
