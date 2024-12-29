using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.Faq;
using Swappa.Server.Validations.Faq;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.Faq
{
    public class DeleteFaqCommandHandler : IRequestHandler<DeleteFaqCommand, ResponseModel<string>>
    {
        private readonly ApiResponseDto response;
        private readonly IRepositoryManager repository;

        public DeleteFaqCommandHandler(ApiResponseDto response,
            IRepositoryManager repository)
        {
            this.response = response;
            this.repository = repository;
        }

        public async Task<ResponseModel<string>> Handle(DeleteFaqCommand request, CancellationToken cancellationToken)
        {
            if (request.Id.IsEmpty())
            {
                return response.Process<string>(new BadRequestResponse("Invalid request"));
            }

            var faqToDelete = await repository.Faq.FindByIdAsync(request.Id);
            if (faqToDelete.IsNull())
            {
                return response.Process<string>(new NotFoundResponse("Could not find record with this Id"));
            }

            faqToDelete.IsDeprecated = true;
            faqToDelete.UpdatedAt = DateTime.UtcNow;

            await repository.Faq.EditAsync(f => f.Id.Equals(request.Id), faqToDelete);
            return response.Process<string>(new ApiOkResponse<string>("FAQ record successfully deleted."));
        }
    }
}
