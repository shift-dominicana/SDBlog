using AutoMapper;
using Ecommerce.Api.Controllers.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using SDBlog.DataModel.Entities.Users;
using SDBlog.BusinessLayer.Interfaces.Users;

namespace MEPyDBase.Testing
{
    public class UserControllerTest
    {
        readonly Mock<IUserService> repositoryStub = new();
        readonly Mock<IMapper> mapperStub = new();
        readonly private Random rand = new();
        //Para nombrar los metodos
        //UnitOfWork_StateUnderTest_ExpectedBehavior
        [Fact]
        public async Task GetUserById_WithUnexistingItem_ReturnNotFoundAsync()
        {

            //Arrange
            repositoryStub.Setup(repo => repo.GeTModelByIdAsync(rand.Next(2000)))
                .ReturnsAsync((User)null);

            var controller = new UserController(repositoryStub.Object, mapperStub.Object);

            //Act
            var result = await controller.GetById(rand.Next(2000));

            //Assert

            result.Should().BeOfType<NoContentResult>();
            //Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetUserById_WitExistingItem_ReturnExpectedItem()
        {
            int value = rand.Next(2000);
            //Arrage
            var expectedUser = CreateRandomUser();
            repositoryStub.Setup(repo => repo.GeTModelByIdAsync(value))
               .ReturnsAsync(expectedUser);

            var controller = new UserController(repositoryStub.Object, mapperStub.Object);

            //Act
            var result = await controller.GetById(value);

            //Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);

            Assert.IsType<User>(okResult.Value);
            //El controlador no devuelve el DTO
            okResult.Value.Should().BeEquivalentTo(expectedUser, options => options.ComparingByMembers<User>());

            //Fluent Assertions hace este trabajo Automaticamente.
            //User dto = okResult.Value as User
            //Assert.Equal(expectedUser.Id, dto.Id);
            //Assert.Equal(expectedUser.FirstName, dto.FirstName);
            //Assert.Equal(expectedUser.LastName, dto.LastName);
            //Assert.Equal(expectedUser.PersonalId, dto.PersonalId);
            //Assert.Equal(expectedUser.Telephone, dto.Telephone);

        }

        [Fact]
        public void GetAll_WitExistingItem_ReturnAllItems()
        {
            //Arrange
            List<User> expectedUsers = new() { CreateRandomUser(), CreateRandomUser(), CreateRandomUser() };

            repositoryStub.Setup(repo => repo.GetAll()).Returns((IQueryable<User>)expectedUsers);

            var controller = new UserController(repositoryStub.Object, mapperStub.Object);

            //Act
            var actualUsers = controller.Get();

            //Assert

            var response = actualUsers.Where(a => a.Id > 0);
            response.Should().BeEquivalentTo(expectedUsers, options => options.ComparingByMembers<User>());
        }

        private User CreateRandomUser()
        {
            return new()
            {
                Id = rand.Next(20000),
                PersonalId = Guid.NewGuid().ToString(),
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                Dob = RandomDay(),
                Email = Guid.NewGuid().ToString(),
                NickName = Guid.NewGuid().ToString(),
                CreadoPor = Guid.NewGuid().ToString(),
                ModificadoPor = Guid.NewGuid().ToString(),
                Confirmed = true,
                CellPhone = Guid.NewGuid().ToString(),
                Telephone = Guid.NewGuid().ToString(),
                Estatus = "A",
                Password = Guid.NewGuid().ToString(),
                FechaRegistro = RandomDay(),
                Borrado = false,
                FechaModificacion = RandomDay()

            };
        }

        private DateTime RandomDay()
        {
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(rand.Next(range));
        }

    }
}
