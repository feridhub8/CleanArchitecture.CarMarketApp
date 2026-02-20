using CarMarketApp.Application.DTOs.Identity;
using CarMarketApp.Application.DTOs.Users;
using CarMarketApp.Application.Features.Commands.Identity;
using CarMarketApp.Application.Features.Commands.Users;
using CarMarketApp.Application.Models;
using CarMarketApp.Application.Models.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarMarketApp.WebAPI.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto, CancellationToken cancellationToken)
    {
        Result result = await _mediator.Send(new RegisterUserCommand(registerUserDto), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto, CancellationToken cancellationToken)
    {
        Result<LoginResponse> result = await _mediator.Send(new LoginUserCommand(loginUserDto), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto, CancellationToken cancellationToken)
    {
        Result<LoginResponse> result = await _mediator.Send(new RefreshTokenCommand(refreshTokenDto), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> LogoutCurrentDevice([FromBody] LogoutCurrentDeviceDto logoutCurrentDeviceDto, CancellationToken cancellationToken)
    {
        Result result = await _mediator.Send(new LogoutCurrentDeviceCommand(logoutCurrentDeviceDto), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> LogoutAllDevices(CancellationToken cancellationToken)
    {
        Claim? userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim is null)
            return Unauthorized("User not found");

        if (!Guid.TryParse(userIdClaim.Value, out var userId))
            return BadRequest("Invalid user id");

        Result result = await _mediator.Send(new LogoutAllDevicesCommand(userId), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordUserDto forgotPasswordUserDto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ForgotPasswordUserCommand(forgotPasswordUserDto), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromQuery] string resetToken, [FromBody] ResetPasswordUserDto resetPasswordUserDto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ResetPasswordUserCommand(resetPasswordUserDto, resetToken), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }
}
