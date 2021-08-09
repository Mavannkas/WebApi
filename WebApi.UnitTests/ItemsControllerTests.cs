using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Controllers;
using WebApi.Dtos;
using WebApi.Entities;
using WebApi.Mappers;
using WebApi.Repositories;
using Xunit;

namespace WebApi.UnitTests
{
    public class ItemsControllerTests
    {
        private readonly Mock<IItemsRepository> repositoryStub = new();
        private readonly Mock<DtoMapper> mapperStub = new();
        private readonly Random random = new();
        [Fact]
        public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFound()
        {
            // Arrange
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                          .ReturnsAsync((Item)null);

            var controller = new ItemsController(repositoryStub.Object, mapperStub.Object);

            // Act
            var result = await controller.GetItemAsync(Guid.NewGuid());

            // Assert  
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetItemAsync_WithExistingItem_ReturnsExpectedItem()
        {
            // Arrange
            var expectItem = CreateItem();
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectItem);

            var controller = new ItemsController(repositoryStub.Object, mapperStub.Object);

            // Act
            var result = await controller.GetItemAsync(Guid.NewGuid());

            // Assert
            result.Value.Should()
                .BeEquivalentTo(expectItem, options => options.ComparingByMembers<Item>());
        }

        private Item CreateItem()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Price = random.Next(1, 1000),
                CreateDate = DateTimeOffset.UtcNow
            };
        }

        [Fact]
        public async Task GetItemsAsync_WithExistingItem_ReturnsAllItem()
        {
            // Arrange
            var exprectedItems = (IEnumerable<Item>)new[]
            {
                CreateItem(),
                CreateItem(),
                CreateItem(),
                CreateItem(),
            };

            repositoryStub.Setup(repo => repo.GetItemsAsync())
                .ReturnsAsync(exprectedItems);

            var controller = new ItemsController(repositoryStub.Object, mapperStub.Object);

            // Act
            var result = await controller.GetItemsAsync();

            // Assert
            result.Should().BeEquivalentTo(
                exprectedItems,
                options => options.ComparingByMembers<Item>()
                );
        }        
        
        [Fact]
        public async Task CreateItemAsync_WithItemToCreate_ReturnsCreatedItem()
        {
            // Arrange
            var itemToCreate = new CreateItemDto()
            {
                Name = Guid.NewGuid().ToString(),
                Price = random.Next(1, 1000) 
            };

            var controller = new ItemsController(repositoryStub.Object, mapperStub.Object);

            // Act
            var result = await controller.CreateItemAsync(itemToCreate);

            // Assert
            var createdItem = (result.Result as CreatedAtActionResult).Value as ItemDto;
            itemToCreate.Should().BeEquivalentTo(
                createdItem,
                options => options.ComparingByMembers<ItemDto>()
                                .ExcludingMissingMembers()
            );
            createdItem.Id.Should().NotBeEmpty();
            createdItem.CreateDate.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        }
    }
}
