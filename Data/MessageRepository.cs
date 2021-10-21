using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Helpers;
using DatingApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public MessageRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddGroup(Group group)
        {
            _context.Groups.Add(group);
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Connection> GetConnection(string connectionId)
        {
            return await _context.Connections.FindAsync(connectionId);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages
                                 .Include(u => u.Sender)
                                 .Include(u => u.Recipient)
                                 .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Group> GetMessageGroup(string groupName)
        {
            return await _context.Groups
                                 .Include(x => x.Connections)
                                 .FirstOrDefaultAsync(x => x.Name == groupName);
        }

        public async Task<IEnumerable<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = _context.Messages
                                .OrderByDescending(m => m.MessageSent)
                                .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
                                .AsQueryable();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.RecipientUsername == messageParams.Username
                                    && u.RecipientDeleted == false),
                "Outbox" => query.Where(u => u.SenderUsername == messageParams.Username
                                     && u.SenderDeleted == false),
                _ => query.Where(u => u.RecipientUsername == messageParams.Username
                              && u.RecipientDeleted == false && u.DateRead == null)
            };

            var messages = await query.Select(message => new MessageDto
            {
                Id = message.Id,
                SenderId = message.SenderId,
                SenderUsername = message.SenderUsername,
                SenderPhotoUrl = message.SenderPhotoUrl,
                RecipientId = message.RecipientId,
                RecipientUsername = message.RecipientUsername,
                RecipientPhotoUrl = message.RecipientPhotoUrl,
                Content = message.Content,
                DateRead = message.DateRead,
                MessageSent = message.MessageSent,
                SenderDeleted = message.SenderDeleted,
                RecipientDeleted = message.RecipientDeleted
            }).ToListAsync();

            return messages;
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername)
        {
            //condition:1 whether currentuser received a message from the recipientuser.
            //condition:2 whether recipientuser received a message from the currentuser. 
            var messages = await _context.Messages
                .Where(m => m.Recipient.UserName == currentUsername && m.RecipientDeleted == false
                         && m.Sender.UserName == recipientUsername   //1
                         || m.Recipient.UserName == recipientUsername
                         && m.Sender.UserName == currentUsername && m.SenderDeleted == false  //2
                      )
                .OrderBy(m => m.MessageSent)
                .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var unreadMessages = messages.Where(m => m.DateRead == null && m.RecipientUsername == currentUsername)
                                         .ToList();

            if(unreadMessages.Any())
            {
                foreach(var message in unreadMessages)
                {
                    message.DateRead = DateTime.UtcNow;
                }
                await _context.SaveChangesAsync();
            }

            return messages;

        }

        public void RemoveConnection(Connection connection)
        {
            _context.Connections.Remove(connection);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0; 
        }
    }
}
