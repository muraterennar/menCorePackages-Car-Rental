using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace MenCore.Application.Extensions;

public static class EncodingExtensions
{
    public static string Encode(this string value)
    {
        return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(value));
    }

    public static string Decode(this string value)
    {
        return Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(value));
    }
}