using Application.CQRS.Reviews.Queries.Requests;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Reviews.Queries.Handlers
{
    public sealed class GetAverageRatingHandler(
        IUnitOfWork unitOfWork)
        : IRequestHandler<GetAverageRatingQuery, ResponseModel<double>>
    {
        public async Task<ResponseModel<double>> Handle(GetAverageRatingQuery request, CancellationToken cancellationToken)
        {
            var avg = await unitOfWork.ReviewRepository.GetAverageRatingByProviderIdAsync(request.ProviderId);

            return ResponseModel<double>.Success(avg);
        }
    }

}
