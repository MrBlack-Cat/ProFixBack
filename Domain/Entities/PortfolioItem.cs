using Domain.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PortfolioItem : BaseEntity
    {
        public int ServiceProviderProfileId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }



        public ServiceProviderProfile? ServiceProviderProfile { get; set; }
    }
}
