using CarMarketApp.Application.Abstractions.Identity;
using CarMarketApp.Application.Extensions;
using CarMarketApp.Application.Features.Commands.Users;
using CarMarketApp.Application.Models.ResultPattern;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Identity;

public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
{
    private readonly IValidator<UpdateUserCommand> _validator;
    private readonly IUserService _userService;

    public UpdateUserCommandHandler(IValidator<UpdateUserCommand> validator, IUserService userService)
    {
        _validator = validator;
        _userService = userService;
    }

    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return Result.Fail(null, validationResult.GetErrors());

        return await _userService.UpdateUserAsync(request.UserId, request.UpdateUserDto);
    }
}
