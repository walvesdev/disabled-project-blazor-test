using System.Security.Cryptography.X509Certificates;

using Sindvamb.Utils.Extension;

using Sindvamb.Utils.Helpers;

using Sindvamb.Utils.Http;

namespace Sindvamb.Utils.Http;

public static class HttpClientMediaType
{
    public const string ApplicationJson = "application/json";
    public const string ApplicationPatchJson = "application/json-patch+json";
}

public static class HttpClientTokenType
{
    public const string XAPIToken = "x-api-key";
}

public class HttpClientService<TInput, TResponse, TKey, TTokenService>
    where TTokenService : AuthTokentService
{
    public HttpClientService(X509Certificate2 x509)
    {
        Check.NotNull(x509, "Certificado não encontrado!");

        var handler = new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate // | DecompressionMethods.All
        };

        handler.ClientCertificates.Add(x509);
        InitClient(handler);
    }

    public HttpClientService(bool automaticDecompression = true)
    {
        InitClient(automaticDecompression: automaticDecompression);
    }

    private HttpClient HttpClient { get; set; }

    public string ApiVersion { get; set; }

    public TTokenService TokenService { get; set; }

    public string Endpoint { get; set; }



    public HttpClientResult<TResponse> Result { get; set; }

    private void InitClient(HttpClientHandler? handler = null, bool automaticDecompression = true)
    {
        handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

        if (handler.IsNull() && automaticDecompression)
        {
            handler = new HttpClientHandler
            {
                AutomaticDecompression =
                    DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.All
            };
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

        }

        if (handler != null)
        {
            HttpClient = new HttpClient(handler);
        }

        HttpClient.Timeout = TimeSpan.FromMinutes(10);

        Result = new HttpClientResult<TResponse>();
    }

    public void Init(string endpoint, TTokenService tokenService, Dictionary<string, string>? headerOption = null,
        bool ignoreHeaders = true, string? baseAddress = null)
    {
        TokenService = tokenService;
        Endpoint = endpoint;

        HttpClient.BaseAddress = baseAddress != null && !baseAddress.IsNullOrWhiteSpace()
            ? new Uri(baseAddress, UriKind.Absolute)
            : new Uri(TokenService.BaseAddress, UriKind.Absolute);

        HttpClient.DefaultRequestHeaders
            .Accept
            .Add(new MediaTypeWithQualityHeaderValue(HttpClientMediaType.ApplicationJson));

        HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

        HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");

        AddBearerHeaderToken();

        if (!ignoreHeaders)
        {
            if (headerOption.IsNotNull())
            {
                foreach (var (key, value) in headerOption)
                {
                    if (key == "Authorization")
                    {
                        HttpClient.DefaultRequestHeaders.Remove(key);
                    }

                    HttpClient.DefaultRequestHeaders.TryAddWithoutValidation(key, value);
                }
            }
        }
    }

    public void AddHeader(string key, string value)
    {
        HttpClient.DefaultRequestHeaders.TryAddWithoutValidation(key, value);
    }

    public void AddBearerHeaderToken()
    {
        RemoveHeaderAuthorization();

        HttpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", TokenService.Token);
    }

    public void AddBasicAuthentication()
    {
        RemoveHeaderAuthorization();

        HttpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", TokenService.BasicCredentials);
    }

    public void RemoveHeaderAuthorization()
    {
        if (HttpClient.DefaultRequestHeaders.Contains("Authorization"))
        {
            HttpClient.DefaultRequestHeaders.Remove("Authorization");
        }
    }

    public async Task<HttpClientResult<TResponse>> Get(TKey id)
    {
        try
        {
            var response = await HttpClient.PostAsJsonAsync(Endpoint, id);

            Result.IsSuccessStatus = true;
            Result.StatusCode = response.StatusCode;

            var result = await response.Content.ReadAsStringAsync();
            Validate(response, result);

            return Result;
        }
        catch (Exception e)
        {
            Result.IsSuccessStatus = false;
            Result.Exception = e;
            return Result;
        }
    }

    public async Task<HttpClientResult<TResponse>> Get()
    {
        try
        {
            var response = await HttpClient.GetAsync(Endpoint);

            Result.IsSuccessStatus = true;
            Result.StatusCode = response.StatusCode;

            var result = await response.Content.ReadAsStringAsync();
            Validate(response, result);

            return Result;
        }
        catch (Exception e)
        {
            Result.IsSuccessStatus = false;
            Result.Exception = e;
            return Result;
        }
    }

    public async Task<HttpClientResult<byte[]>> GetByteArray()
    {
        var result = new HttpClientResult<byte[]>();

        try
        {
            var response = await HttpClient.GetAsync(Endpoint);
            var resultByteArray = await response.Content.ReadAsByteArrayAsync();

            result.IsSuccessStatus = true;
            result.StatusCode = response.StatusCode;

            if (!response.IsSuccessStatusCode)
            {
                result.IsSuccessStatus = false;
                result.ErrorMessage = await response.Content.ReadAsStringAsync();

            }
            else
            {
                result.Data = resultByteArray;
            }

            return result;
        }
        catch (Exception e)
        {
            result.IsSuccessStatus = false;
            result.Exception = e;
            return result;
        }
    }

    public async Task<HttpClientResult<TResponse>> Post(TInput entity)
    {
        try
        {
            var content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8,
                HttpClientMediaType.ApplicationJson);
            var response = await HttpClient.PostAsync(Endpoint, content);

            Result.IsSuccessStatus = true;
            Result.StatusCode = response.StatusCode;

            var result = await response.Content.ReadAsStringAsync();
            Validate(response, result);

            return Result;
        }
        catch (Exception e)
        {
            Result.IsSuccessStatus = false;
            Result.Exception = e;
            return Result;
        }
    }

    private void Validate(HttpResponseMessage response, string result)
    {
        var containsException = result.Contains("Exception");

        if (!response.IsSuccessStatusCode || containsException)
        {
            Result.IsSuccessStatus = false;
            Result.ErrorMessage = result;
        }
        else
        {
            Result.Data = result.Deserialize<TResponse>();
        }
    }

    public async Task<HttpClientResult<TResponse>> PostFormUrlEncoded(FormUrlEncodedContent content)
    {
        try
        {
            var response = await HttpClient.PostAsync(Endpoint, content);

            Result.IsSuccessStatus = true;
            Result.StatusCode = response.StatusCode;

            var result = await response.Content.ReadAsStringAsync();
            Validate(response, result);

            return Result;
        }
        catch (Exception e)
        {
            Result.IsSuccessStatus = false;
            Result.Exception = e;
            return Result;
        }
    }

    public async Task<HttpClientResult<TResponse>> PostMultipartFormData(MultipartFormDataContent content)
    {
        try
        {
            var response = await HttpClient.PostAsync(Endpoint, content);

            Result.IsSuccessStatus = true;
            Result.StatusCode = response.StatusCode;

            var result = await response.Content.ReadAsStringAsync();
            Validate(response, result);

            return Result;
        }
        catch (Exception e)
        {
            Result.IsSuccessStatus = false;
            Result.Exception = e;
            return Result;
        }
    }

    public async Task<HttpClientResult<TResponse>> Put(TInput entity)
    {
        try
        {
            var content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8,
                HttpClientMediaType.ApplicationPatchJson);
            var response = await HttpClient.PutAsync(Endpoint, content);

            Result.IsSuccessStatus = true;
            Result.StatusCode = response.StatusCode;

            var result = await response.Content.ReadAsStringAsync();
            Validate(response, result);

            return Result;
        }
        catch (Exception e)
        {
            Result.IsSuccessStatus = false;
            Result.Exception = e;
            return Result;
        }
    }

    public async Task<HttpClientResult<TResponse>> Patch(TInput entity)
    {
        try
        {
            var content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8,
                HttpClientMediaType.ApplicationPatchJson);
            var response = await HttpClient.PatchAsync(Endpoint, content);

            Result.IsSuccessStatus = true;
            Result.StatusCode = response.StatusCode;

            var result = await response.Content.ReadAsStringAsync();
            Validate(response, result);

            return Result;
        }
        catch (Exception e)
        {
            Result.IsSuccessStatus = false;
            Result.Exception = e;
            return Result;
        }
    }

    public async Task<HttpClientResult<TResponse>> Delete(TKey id)
    {
        try
        {
            var response = await HttpClient.DeleteAsync($"{Endpoint}/{id}");

            Result.IsSuccessStatus = true;
            Result.StatusCode = response.StatusCode;

            var result = await response.Content.ReadAsStringAsync();
            Validate(response, result);

            return Result;
        }
        catch (Exception e)
        {
            Result.IsSuccessStatus = false;
            Result.Exception = e;
            return Result;
        }
    }

    public async Task<HttpClientResult<TResponse>> Delete()
    {
        try
        {
            var response = await HttpClient.DeleteAsync(Endpoint);

            Result.IsSuccessStatus = true;
            Result.StatusCode = response.StatusCode;

            var result = await response.Content.ReadAsStringAsync();
            Validate(response, result);

            return Result;
        }
        catch (Exception e)
        {
            Result.IsSuccessStatus = false;
            Result.Exception = e;
            return Result;
        }
    }

    public HttpClient GetHttpClient()
    {
        return HttpClient;
    }
}
