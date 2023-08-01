using System.Net.Http.Headers;
using System.Text;
using Amazon.Lambda.Core;
using Amazon.SQS;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ERCDotNetFunction;

public class Function
{
    private const string _baseAuthURL = "https://gr.oktapreview.com/oauth2/ausvtetp64jEl2AuE0h7";
    private const string _taskMasterlBaseURL = "https://taskmaster-dev.dev.saas.rate.com";
    private const string _mediaType = "application/x-www-form-urlencoded";
    private const string _dataVal = $"grant_type=client_credentials&scope=taskmaster";
    private const string _borrower = "borrower";
    private const string _coborrower = "co-borrower";
    private const string _sqsUrl = "https://sqs.us-east-1.amazonaws.com/336015931235/retry-queue-ex-backoff";
    private HttpClient _httpClient = new HttpClient();

    public async Task<TaskMasterResult?> FunctionHandler(Event input, ILambdaContext context)
    {
        try
        {
            var result = new TaskMasterResult();
            var access_token = await GenerateAccessToken(context);
            if (string.IsNullOrEmpty(access_token))
            {
                Console.WriteLine("Access token is empty! Cannot proceed further");
                return result;
            }
            Console.WriteLine($"Input json string { JsonConvert.SerializeObject(input) }");
            var payload = ConstructPayload(input);
            Console.WriteLine($"Payload json String: {JsonConvert.SerializeObject(payload)}");
            result.BorrowerResponse = await InvokeTaskMaster(payload, access_token, true);
            result.CoBorrowerResponse = await InvokeTaskMaster(payload, access_token, false);
            var sqsClient = new AmazonSQSClient();
            await DeleteMessage(sqsClient, input?.Detail?.ReceiptHandle);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Task master invocation completed with error!");
            Console.WriteLine(ex.Message);
            throw ex;
        }
    }

    private static async Task DeleteMessage(
      Amazon.SQS.IAmazonSQS sqsClient, string? receiptHandle)
    {
        try 
        {
          await sqsClient.DeleteMessageAsync(_sqsUrl, receiptHandle);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Issue in deleting the SQS message");
            Console.WriteLine(ex.Message);
            throw ex;
        }
    }


    private Payload ConstructPayload(Event eventObj)
    {
        var payload = new Payload();
        var detail = eventObj.Detail;
        if (detail == null)
        {
            Console.WriteLine("Constructing details for payload remains incomplete");
            return payload;
        }
        var body = DeserializeJsonString<Body>(detail.Body);
        if (body == null)
        {
            Console.WriteLine("Constructing body for payload remains incomplete");
            return payload;
        }
        var message = DeserializeJsonString<Message>(body?.Message);
        var msg = message?.Msg;

        payload.LoanId = message?.LoanNumber;
        payload.AppUserId = msg?.EventHeader?.UserId;
        payload.XGrCompany = msg?.EventHeader?.LoanTenant;
        payload.EventName = message?.Name;
        payload.Borrowers = GetEmailsByBorrowerType(msg.Details, _borrower);
        payload.CoBorrowers = GetEmailsByBorrowerType(msg.Details, _coborrower);
        return payload;
    }

    private T? DeserializeJsonString<T>(string? body) where T : class
    {
        if (!string.IsNullOrEmpty(body))
            return JsonConvert.DeserializeObject<T>(body);
        return null;
    }

    private List<string> GetEmailsByBorrowerType(List<DetailItem> details, string type)
    {
        if (details != null && details.Any())
        {
            return details.Where(x => x.EntitiyType == type).Select(y => y.EntityEmail).ToList();
        }
        return new List<string>();
    }

    private async Task<string> GenerateAccessToken(ILambdaContext context)
    {
        try
        {
            var clientSecrets = new ClientSecrets();
            clientSecrets.ClientId = SecretManager.DecodeEnvironmentVariable("task_master_client_id");
            clientSecrets.ClientSecret = SecretManager.DecodeEnvironmentVariable("task_master_client_secret");

            if (String.IsNullOrEmpty(clientSecrets.ClientId) || String.IsNullOrEmpty(clientSecrets.ClientSecret))
            {
                Console.WriteLine($"Secrets are empty! Cannot proceed further");
                return string.Empty;
            }

            var content = new StringContent(_dataVal, Encoding.UTF8, _mediaType);
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
           "Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientSecrets.ClientId}:{clientSecrets.ClientSecret}")));
            try
            {
                var response = await _httpClient.PostAsync($"{_baseAuthURL}/v1/token", content);
                var responseData = await response.Content.ReadAsStringAsync();
                dynamic? jsonResult = JsonConvert.DeserializeObject(responseData);
                Console.WriteLine($"Access token is: {jsonResult?.access_token}");
                return jsonResult != null ? jsonResult.access_token : string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Secrets are retrieved but token not generated due to some issue");
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine("Failed to retrieve the secrets from secret manager");
            Console.WriteLine(exception.Message);
            throw exception;
        }
    }

    private async Task<TaskMasterResponse?> InvokeTaskMaster(Payload payload, string access_token, bool isBorrower)
    {
        if ((payload.Borrowers != null && !payload.Borrowers.Any() && isBorrower) || (payload.CoBorrowers != null && !payload.CoBorrowers.Any() && !isBorrower))
        {
            var borrowerType = isBorrower ? _borrower : _coborrower;
            Console.WriteLine($"{borrowerType} is not available");
            return null;
        }

        var url = $"{_taskMasterlBaseURL}/v1/loans/{payload.LoanId}/task";
        Console.WriteLine(url);
        var requestBody = new RequestBody()
        {
            TaskTitle = payload.EventName,
            TaskDescription = "Lorem ipsum dolor sit amet",
            TaskKey = "income_w2_most_recent[f87352909751f4e2b7b2ddbe076fcb1d]",
            TaskActionType = "upload",
            TaskDisplayType = "upload",
            TaskType = "income_w2_most_recent",
            TaskCategory = "income",
            TaskTemplateId = "1865b666-b25f-4b0e-b96d-4ca31451155b",
            Borrowers = isBorrower ? ConstructBorrowerDetails(payload.Borrowers) : ConstructBorrowerDetails(payload.CoBorrowers),
            AppUserId = payload.AppUserId
        };

        Console.WriteLine($"Request json String: {JsonConvert.SerializeObject(requestBody)}");
        var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
        content.Headers.Add("x-gr-company", payload.XGrCompany);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);

        try
        {
            Console.WriteLine($"Request content json string: {JsonConvert.SerializeObject(content)}");
            var response = await _httpClient.PostAsync(url, content);
            Console.WriteLine($"Response Status: {response.StatusCode.ToString()} ");
            var responseData = await response.Content.ReadAsStringAsync();
            var taskMasterResponse = JsonConvert.DeserializeObject<TaskMasterResponse>(responseData);
            if (String.IsNullOrEmpty(taskMasterResponse?.TaskId))
            {
                Console.WriteLine("Task has not been created");
            }
            else
            {
                Console.WriteLine($"Task has been created :{taskMasterResponse?.TaskId}");
            }
            return taskMasterResponse;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Task master api invocation throws exception");
            Console.WriteLine(ex.Message);
            throw ex;
        }
    }

    private List<Borrower> ConstructBorrowerDetails(List<string>? borrowerEmail)
    {
        var borrowerDetails = new List<Borrower>();
        if (borrowerEmail == null || !borrowerEmail.Any())
            return borrowerDetails;

        foreach (var email in borrowerEmail)
        {
            borrowerDetails.Add(new Borrower() { EmailAddress = email });
        }
        return borrowerDetails;
    }
}