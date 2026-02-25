using CarMarketApp.Application.DTOs.Identity;
using CarMarketApp.Application.DTOs.Users;
using CarMarketApp.Application.Features.Commands.Identity;
using CarMarketApp.Application.Features.Commands.Users;
using CarMarketApp.Application.Features.Queries.Identity;
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
        Result result = await _mediator.Send(new ForgotPasswordUserCommand(forgotPasswordUserDto), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromQuery] string resetToken, [FromBody] ResetPasswordUserDto resetPasswordUserDto, CancellationToken cancellationToken)
    {
        Result result = await _mediator.Send(new ResetPasswordUserCommand(resetPasswordUserDto, resetToken), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("{userId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddModerator([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        Result result = await _mediator.Send(new AddModeratorUserCommand(userId), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserDto updateUserDto, CancellationToken cancellationToken)
    {
        Claim? userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim is null)
            return Unauthorized("User not found");

        if (!Guid.TryParse(userIdClaim.Value, out var userId))
            return BadRequest("Invalid user id");

        Result result = await _mediator.Send(new UpdateUserCommand(userId, updateUserDto), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{userId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RestoreUser([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        Result result = await _mediator.Send(new RestoreUserCommand(userId), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{userId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveModerator([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        Result result = await _mediator.Send(new RemoveModeratorUserCommand(userId), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteProfile(CancellationToken cancellationToken)
    {
        Claim? userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim is null)
            return Unauthorized("User not found");

        if (!Guid.TryParse(userIdClaim.Value, out var userId))
            return BadRequest("Invalid user id");

        Result result = await _mediator.Send(new DeleteUserCommand(userId), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{userId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        Result result = await _mediator.Send(new DeleteUserCommand(userId), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> GetAllUsers([FromQuery] int page, [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        Result<PagedList<UserDto>> result = await _mediator.Send(new GetAllUsersQuery(page, pageSize), cancellationToken);

        return result.Success ? Ok(result) : BadRequest(result);
    }
}
