using FluentValidation.Results;
using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.Faq;
using Swappa.Server.Validations.Faq;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.Faq
{
    public class CreateFaqsCommandHandler : IRequestHandler<CreateFaqsCommand, ResponseModel<string>>
    {
        private readonly ApiResponseDto response;
        private readonly IRepositoryManager repository;

        public CreateFaqsCommandHandler(ApiResponseDto response,
            IRepositoryManager repository)
        {
            this.response = response;
            this.repository = repository;
        }

        public async Task<ResponseModel<string>> Handle(CreateFaqsCommand request, CancellationToken cancellationToken)
        {
            if (request.Requests.IsNullOrEmpty())
            {
                return response.Process<string>(new BadRequestResponse("Invalid request"));
            }

            var validator = new FaqValidator();
            var validations = new List<ValidationResult>();
            foreach (var item in request.Requests)
            {
                validations.Add(validator.Validate(item));
            }

            if (validations.Any(v => !v.IsValid))
            {
                return response.Process<string>(new BadRequestResponse("One or more validation failure occurred. Please review and try again!"));
            }

            var faqs = new List<Entities.Models.Faq>();
            request.Requests.ForEach(f =>
            {
                faqs.Add(new Entities.Models.Faq
                {
                    Title = f.Title,
                    Details = f.Details
                });
            });
            
            await repository.Faq.AddManyAsync(faqs);
            return response.Process<string>(new ApiOkResponse<string>("FAQs successfully added."));
        }
    }
}
