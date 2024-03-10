﻿using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace MenCore.CrossCuttingConserns.Exceptions.Extensions;

public static class ProblemDetailsExtensions
{
    public static string AsJson<TProblemDetail> (this TProblemDetail details)
        where TProblemDetail : ProblemDetails => JsonSerializer.Serialize(details);
}