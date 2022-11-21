using AutoMapper;
using MessagePostgre.Models;

namespace MessagePostgre.Services;

public interface IMessageService
{
	IEnumerable<MessageModel> GetAll();
	MessageModel GetById(string id);
	void Create(MessageModel model);
	void Update(string id, MessageModel model);
	void Delete(string id);
}

public class MessageService : IMessageService
{
	private MessageContext _context;
	private readonly IMapper _mapper;

	public MessageService(
			MessageContext context,
			IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public IEnumerable<MessageModel> GetAll()
	{
		return _context.Messages;
	}

	public MessageModel GetById(string id)
	{
		return getMessage(id);
	}

	public void Create(MessageModel model)
	{

		// map model to new user object
		var user = _mapper.Map<MessageModel>(model);

		// save user
		_context.Messages.Add(user);
		_context.SaveChanges();
	}

	public void Update(string id, MessageModel message)
	{
		// copy model to user and save
		// _mapper.Map(model, message);
		_context.Messages.Update(message);
		_context.SaveChanges();
	}

	public void Delete(string id)
	{
		var user = getMessage(id);
		_context.Messages.Remove(user);
		_context.SaveChanges();
	}

	// helper methods

	private MessageModel getMessage(string id)
	{
		var message = _context.Messages.Find(id);
		if (message == null) throw new KeyNotFoundException("Message not found");
		return message;
	}
}