using CarMarketApp.Application.Abstractions.Identity;
using CarMarketApp.Application.Extensions;
using CarMarketApp.Application.Models.ResultPattern;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace CarMarketApp.Application.Features.Commands.Identity;

public sealed class ResetPasswordUserCommandHandler : IRequestHandler<ResetPasswordUserCommand, Result>
{
    private readonly IValidator<ResetPasswordUserCommand> _validator;
    private readonly IUserService _userService;

    public ResetPasswordUserCommandHandler(IValidator<ResetPasswordUserCommand> validator, IUserService userService)
    {
        _validator = validator;
        _userService = userService;
    }

    public async Task<Result> Handle(ResetPasswordUserCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
            return Result.Fail("Request cannot be null");

        ValidationResult validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result.Fail(null, validationResult.GetErrors());

        return await _userService.ResetPasswordAsync(request.ResetPasswordUserDto, request.ResetToken);
    }
}
