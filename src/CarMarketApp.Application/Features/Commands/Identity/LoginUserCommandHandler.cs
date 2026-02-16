using CarMarketApp.Application.Abstractions.Identity;
using CarMarketApp.Application.Extensions;
using CarMarketApp.Application.Models;
using CarMarketApp.Application.Models.ResultPattern;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Users;

public sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<LoginResponse>>
{
    private readonly IValidator<LoginUserCommand> _validator;
    private readonly IUserService _userService;

    public LoginUserCommandHandler(IValidator<LoginUserCommand> validator, IUserService userService)
    {
        _validator = validator;
        _userService = userService;
    }

    public async Task<Result<LoginResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result<LoginResponse>.Fail("Validation Error", validationResult.GetErrors());

        return await _userService.LoginUserAsync(request.LoginUserDto, cancellationToken);
    }
}
