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

		// GET: api/Message
		[HttpGet]
		public async Task<ActionResult<IEnumerable<MessageModel>>> GetMessages()
		{
			if (_context.Messages == null)
			{
				return NotFound();
			}
			return await _context.Messages.ToListAsync();
		}

		// GET: api/Message/5
		[HttpGet("{id}")]
		public async Task<ActionResult<MessageModel>> GetMessageModel(long id)
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

		// PUT: api/Message/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutMessageModel(long id, MessageModel messageModel)
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

		// POST: api/Message
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<MessageModel>> PostMessageModel(MessageModel messageModel)
		{
			if (_context.Messages == null)
			{
				return Problem("Entity set 'MessageContext.Messages'  is null.");
			}
			_context.Messages.Add(messageModel);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetMessageModel", new { id = messageModel.Id }, messageModel);
		}

		// DELETE: api/Message/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteMessageModel(long id)
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

		private bool MessageModelExists(long id)
		{
			return (_context.Messages?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}
