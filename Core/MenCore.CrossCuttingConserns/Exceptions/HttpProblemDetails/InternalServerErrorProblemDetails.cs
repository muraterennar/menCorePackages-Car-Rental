using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MenCore.CrossCuttingConserns.Exceptions.HttpProblemDetails;

public class InternalServerErrorProblemDetails : ProblemDetails
{
    public InternalServerErrorProblemDetails(string detail)
    {
        Title = "Internal Server Error";
        Detail = detail;
        Status = StatusCodes.Status500InternalServerError;
        Type = "https://excample.com/probs/internal";
    }
}