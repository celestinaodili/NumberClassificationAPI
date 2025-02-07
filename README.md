# NUMBER CLASSIFICATION API 

This API classifies numbers based on mathematical properties and provides a fun fact.

## FEATURES
- It checks if a number is **prime, perfect, or Armstrong**.
- It determines if the number is **odd or even**.
- It fetches a fun fact from **Numbers API**.
- It effectively handle **errors **.
- It is CORS-enabled for cross-origin requests.
- It response in JSON Format

## TECHNOLOGY STACK
C# with ASP.NET Core

## Deployment

Deployed on Azure App Service

## ENDPOINT
It is  publicly accessible at: https://numberClassificationAPI-763.azurewebsites.net/api/classify-number?number=371

## API ENDPOINT
GET https://numberClassificationAPI-763.azurewebsites.net/api/classify-number?number=371

## STEP BY STEP GUIDE
### Step 1: Set Up Your Development Environment

1. Install .NET SDK

Download and install the latest .NET SDK (Ensure at least .NET 6 or later).

Verify installation by running:

**_dotnet --version_**

2. Create a New ASP.NET Core Web API Project

**_dotnet new webapi -n NumberClassificationAPI_**

This will create a new ASP.NET Core Web API project in a folder named NumberClassificationAPI.

3. Navigate into the project directory

**_cd NumberClassificationAPI_**

### Step 2: Install Required Dependencies

1. Install Newtonsoft.Json for JSON serialization

**_dotnet add package Newtonsoft.Json_**

2. Install Cors for Cross-Origin Resource Sharing

**_dotnet add package Microsoft.AspNetCore.Cors_**

### Step 3: Implement the API Logic

1. Open the Project in a Code Editor

If using Visual Studio Code, run:

**_code ._**

If using Visual Studio, open the **_NumberClassificationAPI.sln_** file.

2. Modify Program.cs to Enable CORS

Open Program.cs and update it with the following code to include CORS support:

```
var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.WriteIndented = true; // Enables pretty-printing
    });

builder.Services.AddCors();

var app = builder.Build();

app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader()
);

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
```

3. Create the API Controller

Locate the Controllers folder. If there is none, manually create a new folder inside your project root and name it Controllers.\
Then create a new file named NumberController.cs inside the Controllers folder and add the code below:

```
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
```

### Step 4: Test the API Locally

- Save your changes

- Build and Run the API
```
dotnet build
dotnet run
```
- Test in Browser or Postman

Open: **_http://localhost:5062/api/classify-number?number=371_**

Response:

![image](https://github.com/user-attachments/assets/86f319a0-b0ca-49a2-89bf-a39cfc7aa174)

check for error handling. Try number=xyz

Expected Response:
![image](https://github.com/user-attachments/assets/5b311cf3-b545-46ce-a729-66e8343353fe)


### Step 5: Create a Repository

- Create a public repository in GitHub

- Push Code to GitHub repo

```
git init
git add .
git commit -m "Initial commit - Number classification AP"
git branch -M main
git remote add origin https://github.com/celestinaodili/web-api.git \
replace https://github.com/celestinaodili/web-api.git with your-github-repo-url.
git push -u origin main
```
### Sep 6: Deploy to a Public Endpoint

#### Deploy on Azure App Services

**Prerequisites**

Ensure you have before starting:

 - An Azure Account 

- Azure CLI Installed 

- .NET SDK Installed (

- GitHub Repository with your API code

- Git Installed (Download)

1. Login to Azure

``az login --tenant <tenantID>`` <br/>

This opens a browser for authentication. Select your Azure account.

2.  Create an Azure Resource Group

``az group create --name webappRG --location westus`` <br/>

Replace webappRG and westus with your preferred name and location.

3. Create an Azure App Service Plan

``az appservice plan create --name myAPIServicePlan --resource-group webappRG --sku F1 --is-linux`` <br/>

F1 is a Free (F1) tier (good for testing).

Use --is-linux if you're deploying a Linux-based app.

4. Create an Azure Web App

`` az webapp create --name numberClassificationAPI-763 --resource-group webappRG  --plan myAPIServicePlan --runtime "DOTNETCORE:8.0" ``

Replace numberClassificationAPI-763 with a unique app name.

 5. Configure Deployment from GitHub : link your repository:

`` az webapp deployment source config --name numberClassificationAPI-763 --resource-group webappRG --repo-url https://github.com/celestinaodili/NumberClassificationAPI.git --branch main --manual-integration ``

Replace GitHub url with yours.
Replace numberClassificationAPI-763 with your web app name.

6. Enable Continuous Deployment (CI/CD)

Set up GitHub Actions to deploy automatically when you push changes.

- Go to GitHub Repository

- Navigate to Settings → Secrets and variables → Actions

- Click New Repository Secret and add:

Name: AZURE_WEBAPP_PUBLISH_PROFILE

Value: Get it by running:

``az webapp deployment list-publishing-profiles --name numberClassificationAPI-763 --resource-group webappRG --xml``
replace numberClassificationAPI-763 with your webapp name
Copy and paste the entire XML output into GitHub.

7. Add GitHub Actions Workflow

Create a .github/workflows/deploy.yml file in your repo and add:
```
name: Deploy API to Azure

on:
  push:
    branches:
      - main

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest

    steps:
      - name: Checkout Repository
        uses: actions/checkout@v3

      - name: Setup .NET Core
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '8.0'

      - name: Build and Publish API
        run: |
          dotnet publish -c Release -o release

      - name: Deploy to Azure
        uses: azure/webapps-deploy@v2
        with:
          app-name: "numberClassificationAPI-763"
          slot-name: "production"
          publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
          package: release
```
replace numberClassificationAPI-763 with your webapp name <br/>

7. Retrive Endpoint:
- retrive base url, run
  `` az webapp show --resource-group webappRG --name numberClassificationAPI-763 --query "defaultHostName" --output tsv ``
  
- Endpoint (Full API URL)
`` https://numberClassificationAPI-763.azurewebsites.net/api/classify-number?number=371 ``
