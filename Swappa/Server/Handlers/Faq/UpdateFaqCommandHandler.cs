using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.Faq;
using Swappa.Server.Validations.Faq;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.Faq
{
    public class UpdateFaqCommandHandler : IRequestHandler<UpdateFaqCommand, ResponseModel<string>>
    {
        private readonly ApiResponseDto response;
        private readonly IRepositoryManager repository;

        public UpdateFaqCommandHandler(ApiResponseDto response,
            IRepositoryManager repository)
        {
            this.response = response;
            this.repository = repository;
        }

        public async Task<ResponseModel<string>> Handle(UpdateFaqCommand request, CancellationToken cancellationToken)
        {
            if (request.Request.IsNull())
            {
                return response.Process<string>(new BadRequestResponse("Invalid request"));
            }

            var validator = new FaqValidator();
            var validatorResult = validator.Validate(request.Request);
            if (!validatorResult.IsValid)
            {
                return response.Process<string>(new BadRequestResponse(validatorResult.Errors?.FirstOrDefault()?.ErrorMessage ?? "An error occurred."));
            }

            var faqToUpdate = await repository.Faq.FindByIdAsync(request.Id);
            if (faqToUpdate.IsNull())
            {
                return response.Process<string>(new NotFoundResponse("Could not find record with this Id"));
            }

            faqToUpdate.Title = request.Request.Title;
            faqToUpdate.Details = request.Request.Details;
            faqToUpdate.UpdatedAt = DateTime.UtcNow;

            await repository.Faq.EditAsync(f => f.Id.Equals(request.Id), faqToUpdate);
            return response.Process<string>(new ApiOkResponse<string>("FAQ record successfully updated."));
        }
    }
}
