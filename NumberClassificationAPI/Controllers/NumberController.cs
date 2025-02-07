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

