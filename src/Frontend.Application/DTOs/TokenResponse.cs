namespace Frontend.Application.DTOs;

public sealed record TokenResponse(
    string Token,
    DateTime ExpiresAt
);
