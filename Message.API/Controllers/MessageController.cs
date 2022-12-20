using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Message.API.Models;
using Message.API.Services;
using System.Net;

namespace Message.API.Controllers
{
	[Route("messages")]
	[ApiController]
	public class MessageController : ControllerBase
	{
		// private readonly MessageContext _context;
		private readonly IMessageService _messageService;

		public MessageController(IMessageService messageService)
		{
			_messageService = messageService;
		}

		[HttpGet(Name = "GetMessages")]
		[ProducesResponseType(typeof(IEnumerable<MessageModel>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<MessageModel>>> GetMessages()
		{
			var users = await _messageService.GetAll();
        	return Ok(users);
		}

		[HttpGet("{id}", Name = "GetMessage")]
		[ProducesResponseType(typeof(MessageModel), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<MessageModel>> GetMessage(string id)
		{
			var message = await _messageService.GetById(id);

			if (message == null)
			{
				return NotFound();
			}

			return Ok(message);
		}

		[HttpPut("{id}", Name = "PutMessage")]
		public async Task<IActionResult> PutMessage(string id, MessageModel message)
		{
			if (id != message.Id)
			{
				return BadRequest();
			}

			try
			{
				await _messageService.Update(id, message);
			}
			catch (DbUpdateConcurrencyException)
			{
				var messageCheck = await _messageService.GetById(id);
				if (messageCheck != null)
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		[HttpPost(Name = "PostMessage")]
		public async Task<ActionResult<MessageModel>> PostMessage(MessageModel message)
		{
			message.Id = Guid.NewGuid().ToString();
			await _messageService.Create(message);

			return CreatedAtAction("GetMessage", new { id = message.Id }, message);
		}

		[HttpDelete("{id}", Name = "DeleteMessage")]
		public async Task<IActionResult> DeleteMessage(string id)
		{
			if (await _messageService.Delete(id))
				return NoContent();
			else
				return NotFound();
		}
	}
}
