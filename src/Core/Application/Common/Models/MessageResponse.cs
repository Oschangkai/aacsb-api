namespace AACSB.WebApi.Application.Common.Models;

public record MessageResponse(bool succeeded, string? message = null);