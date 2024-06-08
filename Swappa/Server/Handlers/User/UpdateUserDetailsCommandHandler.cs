using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.User;
using Swappa.Server.Validations.User;
using Swappa.Shared.DTOs;

namespace Swappa.Server.Handlers.User
{
    public class UpdateUserDetailsCommandHandler : IRequestHandler<UpdateUserDetailCommand, ResponseModel<string>>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;
        private readonly ApiResponseDto response;

        public UpdateUserDetailsCommandHandler(UserManager<AppUser> userManager, 
            IMapper mapper, ApiResponseDto response)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.response = response;
        }

        public async Task<ResponseModel<string>> Handle(UpdateUserDetailCommand request, CancellationToken cancellationToken)
        {
            if(request == null || request.Command == null)
            {
                return response
                   .Process<string>(new BadRequestResponse("Invalid request paramters. Please try again."));
            }

            var validator = new UpdateDetailsValidator();
            var validationResult = await validator.ValidateAsync(request.Command!);
            if (!validationResult.IsValid)
                return response
                    .Process<string>(new BadRequestResponse(validationResult.Errors.FirstOrDefault()?.ErrorMessage!));

            var userForEdit = await userManager.FindByIdAsync(request.UserId.ToString());
            if(userForEdit == null)
            {
                return response.Process<string>(new BadRequestResponse($"No user found with Id: {request.UserId}"));
            }

            mapper.Map(request.Command, userForEdit);
            userForEdit.UpdatedOn = DateTime.UtcNow;

            await userManager.UpdateAsync(userForEdit);
            return response.Process<string>(new ApiOkResponse<string>("Details successfully updated."));
        }
    }
}
