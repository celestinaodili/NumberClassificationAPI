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
        if (!int.TryParse(number, out int num))
        {
            return BadRequest(new { number, error = true });
        }

        var properties = GetNumberProperties(num);
        int digitSum = Math.Abs(num).ToString().Sum(c => c - '0'); // Sum of absolute digit values

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

    private bool IsPrime(int n)
    {
        if (n < 2) return false;
        for (int i = 2; i * i <= n; i++)
            if (n % i == 0) return false;
        return true;
    }

    private bool IsPerfect(int n)
    {
        if (n < 1) return false;
        int sum = 1;
        for (int i = 2; i * i <= n; i++)
            if (n % i == 0) sum += i + (i * i == n ? 0 : n / i);
        return sum == n;
    }

    private string[] GetNumberProperties(int n)
    {
        var properties = new System.Collections.Generic.List<string>();
        if (IsArmstrong(n)) properties.Add("armstrong");
        properties.Add(n % 2 == 0 ? "even" : "odd");
        return properties.ToArray();
    }

    private bool IsArmstrong(int n)
    {
        int sum = 0, temp = Math.Abs(n), digits = temp.ToString().Length;
        while (temp > 0)
        {
            int digit = temp % 10;
            sum += (int)Math.Pow(digit, digits);
            temp /= 10;
        }
        return sum == Math.Abs(n);
    }

    private async Task<string> GetFunFact(int n)
    {
        using var httpClient = new HttpClient();
        string apiUrl = $"http://numbersapi.com/{n}/math";
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