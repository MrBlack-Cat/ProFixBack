using Dapper;
using Domain.Entities;
using Repository.Repositories;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Dal.SqlServer.Infrastructure
{
    public class SqlChatMessageRepository : IChatMessageRepository
    {
        private readonly IDbConnection _db;

        public SqlChatMessageRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ChatMessage>> GetMessagesByUserIdAsync(int userId)
        {
            const string sql = @"
                    SELECT Id, UserId, Sender, MessageText, CreatedAt
                    FROM ChatMessages
                    WHERE UserId = @UserId
                    ORDER BY CreatedAt ASC;
                ";

            return await _db.QueryAsync<ChatMessage>(sql, new { UserId = userId });
        }

        public async Task AddMessageAsync(ChatMessage message)
        {
            const string sql = @"
                        INSERT INTO ChatMessages (UserId, Sender, MessageText, CreatedAt)
                        VALUES (@UserId, @Sender, @MessageText, @CreatedAt);
                    ";

            await _db.ExecuteAsync(sql, new
            {
                message.UserId,
                message.Sender,
                message.MessageText,
                message.CreatedAt
            });
        }

    }
}
