﻿using MenCore.CrossCuttingConserns.Exceptions.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MenCore.CrossCuttingConserns.Exceptions.HttpProblemDetails;

public class ValidationProblemDetails : ProblemDetails
{
    public ValidationProblemDetails(IEnumerable<ValidationExceptionModel> errors)
    {
        Title = "Validation Error(s)";
        Detail = "One or more validation errors occured.";
        Errors = errors;
        Status = StatusCodes.Status400BadRequest;
        Type = "https://example.com/propbs/validaiton";
    }

    public IEnumerable<ValidationExceptionModel> Errors { get; init; }
}