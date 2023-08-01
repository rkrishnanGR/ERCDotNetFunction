using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.KeyManagementService;
using Amazon.KeyManagementService.Model;
using Newtonsoft.Json;

namespace ERCDotNetFunction
{
    public static class SecretManager
    {
        public static string DecodeEnvironmentVariable(string environmentVariableName)
        {
            // Retrieve env var text
            var encryptedBase64Text = Environment.GetEnvironmentVariable(environmentVariableName);
            if(String.IsNullOrEmpty(encryptedBase64Text))
                  return string.Empty;

            // Convert base64-encoded text to bytes
            var encryptedBytes = Convert.FromBase64String(encryptedBase64Text);
            // Set up encryption context
            var encryptionContext = new Dictionary<string, string>();
            encryptionContext.Add("LambdaFunctionName",
                    Environment.GetEnvironmentVariable("AWS_LAMBDA_FUNCTION_NAME"));
            // Construct client
            using (var client = new AmazonKeyManagementServiceClient())
            {
                // Construct request
                var decryptRequest = new DecryptRequest
                {
                    CiphertextBlob = new MemoryStream(encryptedBytes),
                    EncryptionContext = encryptionContext          
                };
                // Call KMS to decrypt data
                var response = client.DecryptAsync(decryptRequest).Result;
                using (var plaintextStream = response.Plaintext)
                {
                    // Get decrypted bytes
                    var plaintextBytes = plaintextStream.ToArray();
                    // Convert decrypted bytes to ASCII text
                    var plaintext = Encoding.UTF8.GetString(plaintextBytes);
                    return plaintext;
                }
            }
        }
    }
}