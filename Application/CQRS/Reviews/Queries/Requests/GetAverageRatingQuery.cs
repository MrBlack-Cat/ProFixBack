using Common.GlobalResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Reviews.Queries.Requests
{
    public record struct GetAverageRatingQuery(int ProviderId)
        : IRequest<ResponseModel<double>>;

}
