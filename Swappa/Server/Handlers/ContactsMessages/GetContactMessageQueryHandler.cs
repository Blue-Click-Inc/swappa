using Mailjet.Client.Resources;
using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Extensions;
using Swappa.Server.Queries.ContactMessages;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.ContactsMessages
{
    public class GetContactMessageQueryHandler : IRequestHandler<GetContactMessageQuery, ResponseModel<PaginatedListDto<ContactMessageToReturnDto>>>
    {
        private readonly ApiResponseDto response;
        private readonly IRepositoryManager repository;

        public GetContactMessageQueryHandler(ApiResponseDto response,
            IRepositoryManager repository)
        {
            this.response = response;
            this.repository = repository;
        }

        public async Task<ResponseModel<PaginatedListDto<ContactMessageToReturnDto>>> Handle(GetContactMessageQuery request, CancellationToken cancellationToken)
        {
            if (request.IsNull() || request.Query.IsNull())
            {
                return response.Process<PaginatedListDto<ContactMessageToReturnDto>>(new BadRequestResponse("Invalid request"));
            }

            var messageQuery = repository.ContactMessage.FindAsQueryable(m => !m.IsDeprecated)
                .OrderByDescending(m => m.CreatedAt)
                .ThenBy(m => m.IsRead)
                .Search(request.Query.SearchTerm);

            var pagedList = await Task.Run(() =>
                PagedList<ContactMessage>.ToPagedList(messageQuery, request.Query.PageNumber, request.Query.PageSize));

            var pagedData = PaginatedListDto<ContactMessageToReturnDto>.Paginate(ToContactMessageReturnDto(pagedList), pagedList.MetaData);
            return response.Process<PaginatedListDto<ContactMessageToReturnDto>>(new ApiOkResponse<PaginatedListDto<ContactMessageToReturnDto>>(pagedData));
        }

        private List<ContactMessageToReturnDto> ToContactMessageReturnDto(PagedList<ContactMessage> pagedList)
        {
            var result = new List<ContactMessageToReturnDto>();
            pagedList.ForEach(message =>
            {
                result.Add(new ContactMessageToReturnDto
                {
                    Id = message.Id,
                    Name = message.Name,
                    Message = message.Message,
                    Email = message.EmailAddress,
                    Phone = message.PhoneNumber,
                    IsRead = message.IsRead,
                    DateAdded = message.CreatedAt
                });
            });

            return result;
        }
    }
}
