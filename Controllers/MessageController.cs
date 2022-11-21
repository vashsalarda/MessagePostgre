using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MessagePostgre.Models;

namespace MessagePostgre.Controllers
{
	[Route("messages")]
	[ApiController]
	public class MessageController : ControllerBase
	{
		private readonly MessageContext _context;

		public MessageController(MessageContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<MessageModel>>> GetMessages()
		{
			if (_context.Messages == null)
			{
				return NotFound();
			}
			return await _context.Messages.ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<MessageModel>> GetMessageModel(string id)
		{
			if (_context.Messages == null)
			{
				return NotFound();
			}
			var messageModel = await _context.Messages.FindAsync(id);

			if (messageModel == null)
			{
				return NotFound();
			}

			return messageModel;
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutMessageModel(string id, MessageModel messageModel)
		{
			if (id != messageModel.Id)
			{
				return BadRequest();
			}

			_context.Entry(messageModel).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MessageModelExists(id))
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

		[HttpPost]
		public async Task<ActionResult<MessageModel>> PostMessageModel(MessageModel message)
		{
			if (_context.Messages == null)
			{
				return Problem("Entity set 'MessageContext.Messages'  is null.");
			}

			message.Id = Guid.NewGuid().ToString();
			_context.Messages.Add(message);

			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMessageModel", new { id = message.Id }, message);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteMessageModel(string id)
		{
			if (_context.Messages == null)
			{
				return NotFound();
			}
			var messageModel = await _context.Messages.FindAsync(id);
			if (messageModel == null)
			{
				return NotFound();
			}

			_context.Messages.Remove(messageModel);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool MessageModelExists(string id)
		{
			return (_context.Messages?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}
