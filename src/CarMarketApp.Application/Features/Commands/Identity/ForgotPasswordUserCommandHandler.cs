using CarMarketApp.Application.Abstractions.Identity;
using CarMarketApp.Application.Extensions;
using CarMarketApp.Application.Models;
using CarMarketApp.Application.Models.ResultPattern;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Identity;

public sealed class ForgotPasswordUserCommandHandler : IRequestHandler<ForgotPasswordUserCommand, Result>
{
    private readonly IValidator<ForgotPasswordUserCommand> _validator;
    private readonly IUserService _userService;
    public ForgotPasswordUserCommandHandler(IValidator<ForgotPasswordUserCommand> validator, IUserService userService)
    {
        _validator = validator;
        _userService = userService;
    }

    public async Task<Result> Handle(ForgotPasswordUserCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
            return Result<LoginResponse>.Fail("Request cannot be null");

        ValidationResult validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result<LoginResponse>.Fail("Validation Error", validationResult.GetErrors());

        return await _userService.ForgotPasswordAsync(request.ForgotPasswordUserDto);
    }
}
