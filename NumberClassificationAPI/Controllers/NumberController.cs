/*
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

[ApiController]
[Route("api")]
public class ClassifyNumberController : ControllerBase
{
    [HttpGet("classify-number")]
    public async Task<IActionResult> ClassifyNumber([FromQuery] string number)
    {
        // Try parsing the number as a double to handle both integers and floating-point numbers
        if (!double.TryParse(number, out double num))
        {
            return BadRequest(new { number, error = true });
        }

        // Use the absolute value for classification and calculations
        num = Math.Abs(num);

        var properties = GetNumberProperties(num);
        int digitSum = num.ToString().Sum(c => c - '0'); // Sum of absolute digit values

        string funFact = await GetFunFact(num);

        return Ok(new
        {
            number = num,
            is_prime = IsPrime(num),
            is_perfect = IsPerfect(num),
            properties,
            digit_sum = digitSum,
            fun_fact = funFact
        });
    }

    private bool IsPrime(double n)
    {
        if (n < 2) return false;
        for (int i = 2; i * i <= n; i++)
            if (n % i == 0) return false;
        return true;
    }

    private bool IsPerfect(double n)
    {
        if (n < 1) return false;
        int sum = 1;
        for (int i = 2; i * i <= n; i++)
            if (n % i == 0) sum += i + (i * i == n ? 0 : (int)(n / i));
        return sum == n;
    }

    private string[] GetNumberProperties(double n)
    {
        var properties = new System.Collections.Generic.List<string>();
        if (IsArmstrong(n)) properties.Add("armstrong");
        properties.Add(n % 2 == 0 ? "even" : "odd");
        return properties.ToArray();
    }

    private bool IsArmstrong(double n)
    {
        int sum = 0, temp = (int)Math.Abs(n), digits = temp.ToString().Length;
        while (temp > 0)
        {
            int digit = temp % 10;
            sum += (int)Math.Pow(digit, digits);
            temp /= 10;
        }
        return sum == Math.Abs(n);
    }

    private async Task<string> GetFunFact(double n)
    {
        using var httpClient = new HttpClient();
        string apiUrl = $"http://numbersapi.com/{(int)n}/math"; // Use the integer part for the fun fact
        try
        {
            return await httpClient.GetStringAsync(apiUrl);
        }
        catch
        {
            return "No fun fact available";
        }
    }
}
*/

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/numbers")]
public class NumberController : ControllerBase
{
    [HttpGet("classify/{number?}")]
    public IActionResult ClassifyNumber([FromRoute] int? number, [FromQuery] int? queryNumber)
    {
        int? finalNumber = number ?? queryNumber;

        if (!finalNumber.HasValue)
        {
            return BadRequest(new
            {
                type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                title = "One or more validation errors occurred.",
                status = 400,
                errors = new { number = new[] { "The number field is required." } }
            });
        }

        var result = new
        {
            Number = finalNumber,
            IsPrime = IsPrime(finalNumber.Value),
            IsPerfect = IsPerfect(finalNumber.Value),
            IsArmstrong = IsArmstrong(finalNumber.Value)
        };

        return Ok(result); // Returns HTTP 200 OK with JSON response
    }

    private bool IsPrime(int num)
    {
        if (num < 2) return false;
        for (int i = 2; i * i <= num; i++)
            if (num % i == 0) return false;
        return true;
    }

    private bool IsPerfect(int num)
    {
        int sum = 1;
        for (int i = 2; i * i <= num; i++)
        {
            if (num % i == 0)
            {
                sum += i;
                if (i != num / i) sum += num / i;
            }
        }
        return sum == num && num != 1;
    }

    private bool IsArmstrong(int num)
    {
        int sum = 0, temp = num, digits = num.ToString().Length;
        while (temp > 0)
        {
            sum += (int)Math.Pow(temp % 10, digits);
            temp /= 10;
        }
        return sum == num;
    }
}
