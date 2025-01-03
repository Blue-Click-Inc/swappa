﻿using AutoMapper;
using MediatR;
using Swappa.Data.Contracts;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Extensions;
using Swappa.Server.Queries.User;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Server.Handlers.User
{
    public class GetUsersFeedbackQueryHandler : IRequestHandler<GetUsersFeedbackQuery, ResponseModel<PaginatedListDto<UserFeedbackDto>>>
    {
        private readonly IRepositoryManager repository;
        private readonly IMapper mapper;
        private readonly ApiResponseDto response;

        public GetUsersFeedbackQueryHandler(IRepositoryManager repository, 
            IMapper mapper, ApiResponseDto response)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.response = response;
        }

        public async Task<ResponseModel<PaginatedListDto<UserFeedbackDto>>> Handle(GetUsersFeedbackQuery request, CancellationToken cancellationToken)
        {
            var feedbacks = repository.Feedback
                    .FindAsQueryable(f => true)
                    .OrderByDescending((f) => f.CreatedAt)
                    .Search(request.Dto.SearchTerm);

            var pagedList = await Task.Run(() => 
                PagedList<UserFeedback>.ToPagedList(feedbacks, request.Dto.PageNumber, request.Dto.PageSize));

            var dataList = mapper.Map<IEnumerable<UserFeedbackDto>>(pagedList);
            var pagedData = PaginatedListDto<UserFeedbackDto>.Paginate(dataList, pagedList.MetaData);
            return response.Process<PaginatedListDto<UserFeedbackDto>>(new ApiOkResponse<PaginatedListDto<UserFeedbackDto>>(pagedData));
        }
    }
}
