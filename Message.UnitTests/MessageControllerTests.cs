using Message.API.Controllers;
using Message.API.Models;
using Message.API.Services;

namespace Message.UnitTests;

public class MessageControllerUnitTest
{
    private readonly Mock<IMessageService> messageService;

    public MessageControllerUnitTest()
    {
        messageService = new Mock<IMessageService>();
    }

    [Fact]  
    public async void GetMessages_Should_Return_Messages()  
    {  
        
        // Arrange
        var mockList = GetMessagesData();
        messageService.Setup(x => x.GetAll()).ReturnsAsync(mockList);

        var controller = new MessageController(messageService.Object);

        // Act
        var result = await controller.GetMessages();
        var resultType = (OkObjectResult?)result?.Result;
        var resultList = resultType?.Value as List<MessageModel>;
        int totalRes = resultList?.Count ?? 0;
        var StatusCode = resultType?.StatusCode;
        var messagesList = resultType?.Value;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(GetMessagesData().Count(), totalRes);
        Assert.True(mockList.Equals(resultList));
        Assert.Equal(StatusCode, (int)System.Net.HttpStatusCode.OK);
        Assert.IsType<List<MessageModel>>(messagesList);
        
    }

    [Fact]
    public async void GetMessageByID_Message()
    {
        
        // Arrange
        string id = "1001";
        var mockList = GetMessagesData();
        messageService.Setup(x => x.GetById(id)).ReturnsAsync(mockList.Where(i => i.Id == id).AsEnumerable().FirstOrDefault() ?? mockList[1]);
        var controller = new MessageController(messageService.Object);

        // Act
        var result = await controller.GetMessage(id);
        var resultType = result.Result;
        var resultObj = result.Result as OkObjectResult;
        var resultItem = resultObj?.Value as MessageModel;
        var Id = resultItem?.Id?.ToString();
        var StatusCode = resultObj?.StatusCode;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(resultItem);
        Assert.IsType<MessageModel>(resultItem);
        Assert.Equal(id, Id);
        Assert.Equal((int)System.Net.HttpStatusCode.OK, StatusCode);

    }

    [Fact]
    public async void Create_Message_Success()
    {
        // Arrange
        var mockList = GetMessagesData();
        messageService.Setup(x => x.Create(mockList[1])).ReturnsAsync(true);
        var controller = new MessageController(messageService.Object);

        // Act
        var result = await controller.PostMessage(mockList[1]);
        var resultValue = result.Result as CreatedAtActionResult;
        var resultObj = resultValue?.Value as MessageModel;
        var StatusCode = resultValue?.StatusCode;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<MessageModel>(resultObj);
        Assert.Equal(mockList[1].Id, resultObj?.Id);
        Assert.True(mockList[1].Message == resultObj?.Message);
        Assert.Equal((int)System.Net.HttpStatusCode.Created, StatusCode);
    }

    [Fact]
    public async void Update_Message_Success()
    {
        // Arrange
        string id = "1003";
        MessageModel message = new()
        {
            Id = "1003",
            Message = "Final",
            MessageType = "Type 4",
            Date = DateTime.Now,
            Status = "NotOK"
        };
        messageService.Setup(x => x.Update(id, message)).ReturnsAsync(true);

        // Act
        var controller = new MessageController(messageService.Object);
        var result = await controller.PutMessage(id, message);
        var resultValue = result as NoContentResult;
        var StatusCode = resultValue?.StatusCode;
        Console.WriteLine(result);
        Console.WriteLine(resultValue);

        // Assert
        Assert.NotNull(result);
        Assert.Equal((int)System.Net.HttpStatusCode.NoContent, StatusCode);
    }

    [Fact]
    public async void Delete_Message_Success()
    {
        
        // Arrange
        string id = "1002";
        messageService.Setup(x => x.Delete(id)).ReturnsAsync(true);

        // Act
        var controller = new MessageController(messageService.Object);
        var result = await controller.DeleteMessage(id);
        var resultValue = result as NoContentResult;
        var StatusCode = resultValue?.StatusCode;

        // Assert
        Assert.NotNull(result);
        Assert.Equal((int)System.Net.HttpStatusCode.NoContent, StatusCode);
    }

    private List<MessageModel> GetMessagesData()
    {
        List<MessageModel> mockList = new List<MessageModel>
        {
            new MessageModel
            {
                Id = "1001",
                Message = "Hello",
                MessageType = "Type 1",
                Date = DateTime.Now,
                Status = "OK"
            },
            new MessageModel
            {
                Id = "1002",
                Message = "Hello",
                MessageType = "Type 1",
                Date = DateTime.Now,
                Status = "Pending"
            },
            new MessageModel
            {
                Id = "1003",
                Message = "Test",
                MessageType = "Type 3",
                Date = DateTime.Now,
                Status = "OK"
            },
        };

        return mockList;
    }
}