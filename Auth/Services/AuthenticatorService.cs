using Auth.Helpers.Interfaces;
using Auth.Interfaces;
using Auth.Models;
using Grpc.Core;

namespace Auth.Services;

public class AuthenticatorService : Authenticator.AuthenticatorBase
{
    private readonly ILogger<AuthenticatorService> _logger;
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public AuthenticatorService(ILogger<AuthenticatorService> logger, IUserService userService,
        ITokenService tokenService)
    {
        _logger = logger;
        _userService = userService;
        _tokenService = tokenService;
    }

    public override async Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
    {
        try
        {
            var dto = new UserDto { Mail = request.Email, Password = request.Password };
            var registrationResult = await _userService.TryRegisterUser(dto);
            return new RegisterResponse
                { Message = registrationResult.Message, Status = registrationResult.Status };
        }
        catch (Exception e)
        {
            _logger.LogError("Internal server error: {e}", e);
            return new RegisterResponse { Message = e.Message, Status = RegisterResponse.Types.Status.InternalError };
        }
    }

    public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
    {
        var dto = new UserDto { Mail = request.Email, Password = request.Password };
        var validationResult = await _userService.ValidateUser(dto);
        if (validationResult.IsFailure)
        {
            return new LoginResponse
            {
                IsSuccess = false,
                LoginToken =  "",
                RefreshToken = "",
            };
        }

        return new LoginResponse
        {
            IsSuccess = true,
            LoginToken = _tokenService.GetLoginToken(validationResult.User!),
            RefreshToken = _tokenService.GetRefreshToken(validationResult.User!)
        };
    }

    public override async Task<RefreshResponse> Refresh(RefreshRequest request, ServerCallContext context)
    {
        try
        {
            var email = await _tokenService.VerifyRefreshToken(request.RefreshToken);
            var user = await _userService.GetUser(email);
            var token = _tokenService.GetRefreshToken(user);
            return new RefreshResponse
            { IsSuccess = true, Message = "Token refreshed", AuthToken = token };
        }
        catch (Exception e)
        {
            _logger.LogError("Internal server error: {e}", e);
            return new RefreshResponse { IsSuccess = false, Message = e.Message };
        }
    }
}