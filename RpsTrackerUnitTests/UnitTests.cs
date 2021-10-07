using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using RPS.Core.Models;
using RPS.Data;
using RPS.Web.Server.Components.Backlog;
using System;
using System.Collections.Generic;
using Telerik.JustMock;
using Xunit;

namespace RpsTrackerUnitTests
{
    public class UnitTests
    {
        public List<PtUser> Users { get; set; }
        public List<PtItem> Items { get; set; }

        public UnitTests()
        {
            this.Users = new List<PtUser>();
            this.Items = new List<PtItem>();

            PtUser ptUser = new PtUser();
            ptUser.Avatar = "images/avatars/females/image-17.png";
            ptUser.DateCreated = new DateTime(2021, 1, 1);
            ptUser.DateModified = new DateTime(2021, 5, 5);
            ptUser.FullName = "John Doe";
            ptUser.Id = 21;

            this.Users.Add(ptUser);

            PtItem openItem = new PtItem();
            openItem.Assignee = ptUser;

            openItem.Description = "Open Item Description";
            openItem.Title = "Open Item Title";
            openItem.Estimate = 20;
            openItem.Type = RPS.Core.Models.Enums.ItemTypeEnum.PBI;
            openItem.Status = RPS.Core.Models.Enums.StatusEnum.Open;
            openItem.Priority = RPS.Core.Models.Enums.PriorityEnum.High;
            openItem.DateCreated = new DateTime(2021, 1, 1);

            PtItem firstClosedItem = new PtItem();
            firstClosedItem.Assignee = ptUser;

            firstClosedItem.Description = "Closed Item Description";
            firstClosedItem.Title = "Closed Item Title";
            firstClosedItem.Estimate = 20;
            firstClosedItem.Type = RPS.Core.Models.Enums.ItemTypeEnum.PBI;
            firstClosedItem.Status = RPS.Core.Models.Enums.StatusEnum.Closed;
            firstClosedItem.Priority = RPS.Core.Models.Enums.PriorityEnum.High;
            firstClosedItem.DateCreated = new DateTime(2021, 1, 1);

            PtItem secondClosedItem = new PtItem();
            secondClosedItem.Assignee = ptUser;

            secondClosedItem.Description = "Closed Item Description";
            secondClosedItem.Title = "Closed Item Title";
            secondClosedItem.Estimate = 20;
            secondClosedItem.Type = RPS.Core.Models.Enums.ItemTypeEnum.PBI;
            secondClosedItem.Status = RPS.Core.Models.Enums.StatusEnum.Closed;
            secondClosedItem.Priority = RPS.Core.Models.Enums.PriorityEnum.High;
            secondClosedItem.DateCreated = new DateTime(2021, 8, 8);

            this.Items.Add(openItem);
            this.Items.Add(firstClosedItem);
            this.Items.Add(secondClosedItem);
        }

        [Fact]
        public void BacklogOpenItem_ValidateCorrectView()
        {
            // Arrange
            var ctx = new TestContext();
            ctx.JSInterop.Mode = JSRuntimeMode.Loose;

            // mock the data that will be used for the test
            var memoryContext = Mock.Create<PtInMemoryContext>();
            Mock.Arrange(() => memoryContext.PtItems).ReturnsCollection(this.Items);
            Mock.Arrange(() => memoryContext.PtUsers).ReturnsCollection(this.Users);

            ctx.Services.AddSingleton<IPtItemsRepository>(new PtItemsRepository(memoryContext));
            ctx.Services.AddSingleton<IPtUserRepository>(new PtUserRepository(memoryContext));

            // Act
            var cut = ctx.RenderComponent<Items>(parameter => parameter.Add(p => p.PresetParam, "Open"));

            // Assert
            var firstRow = cut.Find("tr.k-master-row");
            Assert.Equal(1, firstRow.ParentElement.ChildElementCount);
        }

        [Fact]
        public void BacklogClosedItem_ValidateCorrectView()
        {
            // Arrange
            var ctx = new TestContext();
            ctx.JSInterop.Mode = JSRuntimeMode.Loose;

            // mock the data that will be used for the test
            var memoryContext = Mock.Create<PtInMemoryContext>();
            Mock.Arrange(() => memoryContext.PtItems).ReturnsCollection(this.Items);
            Mock.Arrange(() => memoryContext.PtUsers).ReturnsCollection(this.Users);

            ctx.Services.AddSingleton<IPtItemsRepository>(new PtItemsRepository(memoryContext));
            ctx.Services.AddSingleton<IPtUserRepository>(new PtUserRepository(memoryContext));

            // Act
            var cut = ctx.RenderComponent<Items>(parameter => parameter.Add(p => p.PresetParam, "Closed"));

            // Assert
            var firstRow = cut.Find("tr.k-master-row");
            Assert.Equal(2, firstRow.ParentElement.ChildElementCount);
        }
    }
}
