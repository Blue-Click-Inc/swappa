using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.Faq;
using Swappa.Server.Validations.Faq;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.Faq
{
    public class CreateFaqCommandHandler : IRequestHandler<CreateFaqCommand, ResponseModel<string>>
    {
        private readonly ApiResponseDto response;
        private readonly IRepositoryManager repository;

        public CreateFaqCommandHandler(ApiResponseDto response,
            IRepositoryManager repository)
        {
            this.response = response;
            this.repository = repository;
        }

        public async Task<ResponseModel<string>> Handle(CreateFaqCommand request, CancellationToken cancellationToken)
        {
            if (request.IsNull())
            {
                return response.Process<string>(new BadRequestResponse("Invalid request"));
            }

            var validator = new FaqValidator();
            var validatorResult = validator.Validate(request);
            if(!validatorResult.IsValid)
            {
                return response.Process<string>(new BadRequestResponse(validatorResult.Errors?.FirstOrDefault()?.ErrorMessage ?? "An error occurred."));
            }

            var faq = new Swappa.Entities.Models.Faq
            {
                Title = request.Title,
                Details = request.Details
            };

            await repository.Faq.AddAsync(faq);
            return response.Process<string>(new ApiOkResponse<string>("FAQ successfully added."));
        }
    }
}
