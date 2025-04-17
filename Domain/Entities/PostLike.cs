using Domain.BaseEntities;

namespace Domain.Entities;

public class PostLike
{
    public int PostId { get; set; }
    public int ClientProfileId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Post? Post { get; set; }
    public ClientProfile? ClientProfile { get; set; }
}
