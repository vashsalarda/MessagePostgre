using Message.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Message.API.Services;

public interface IMessageService
{
	Task<IEnumerable<MessageModel>> GetAll();
	Task<MessageModel?> GetById(string id);
	Task<bool> Create(MessageModel model);
	Task<bool> Update(string id, MessageModel model);
	Task<bool> Delete(string id);
}

public class MessageService : IMessageService
{
	private MessageContext _context;

	public MessageService(MessageContext context)
	{
		_context = context;
	}

	public async Task<IEnumerable<MessageModel>> GetAll()
	{
		return await _context.Messages.ToListAsync();
	}

	public async Task<MessageModel?> GetById(string id)
	{
		return await _context.Messages.FindAsync(id) ?? null;
	}

	public async Task<bool> Create(MessageModel message)
	{
		// save message
		_context.Messages.Add(message);
		int affected = await _context.SaveChangesAsync();
		return affected == 1 ? true : false;
	}

	public async Task<bool> Update(string id, MessageModel message)
	{
		_context.Messages.Update(message);
		int affected = await _context.SaveChangesAsync();
		return affected == 1 ? true : false;
	}

	public async Task<bool> Delete(string id)
	{
		var message = await _context.Messages.FindAsync(id);
		if(message == null)
			return false;
		var result = _context.Messages.Remove(message);
		await _context.SaveChangesAsync();
		return result != null ? true : false;

	}
}