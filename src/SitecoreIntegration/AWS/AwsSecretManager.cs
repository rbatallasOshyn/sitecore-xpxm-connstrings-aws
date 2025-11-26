using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Newtonsoft.Json;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace RBatallas.AWSSecretsIntegration.ConnectionStringReplacer.AWS
{
    internal sealed class AwsSecretManager
    {
        private readonly IAmazonSecretsManager _amazonSecretsManager;

        /// <summary>
        /// Singleton instance of the AwsSecretManager class.
        /// </summary>
        public static AwsSecretManager Instance { get; } = new AwsSecretManager();

        public AwsSecretManager()
        {
            string region = GetAppSetting("aws:region", "us-west-1");
            _amazonSecretsManager = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));
        }

        public string GetSecret(string secretName)
        {
            try
            {
                var request = new GetSecretValueRequest()
                {
                    SecretId = secretName,
                    VersionStage = GetAppSetting("aws:versionStorage", "AWSCURRENT"), // VersionStage defaults to AWSCURRENT if unspecified.
                };

                var response = _amazonSecretsManager.GetSecretValue(request);
                var decodedString = DecodeString(response);

                var secretDictionary = JsonConvert.DeserializeObject<Dictionary<string,string>>(decodedString);

                string connectionString = secretDictionary != null && secretDictionary.Any() ? secretDictionary.FirstOrDefault().Value : string.Empty;

                Log.Info($"Successfully retrieved secret {secretName} from AWS Secrets Manager. The value is {connectionString}", this);
                return connectionString;
            }
            catch (Exception ex)
            {
                Log.Error($"Error retrieving secret {secretName} from AWS Secrets Manager: {ex.Message}", ex, this);
            }
            return string.Empty;
        }

        public List<string> GetAllSecrets()
        {
            var secrets = new List<string>();

            var request = new ListSecretsRequest()
            {
                MaxResults = 40
            };

            var response = _amazonSecretsManager.ListSecrets(request);

            foreach (var secret in response.SecretList)
            {
                secrets.Add(GetSecret(secret.Name));
            }

            return secrets;
        }


        /// <summary>
        /// Decodes the secret returned by the call to GetSecretValueAsync and
        /// returns it to the calling program.
        /// </summary>
        /// <param name="response">A GetSecretValueResponse object containing
        /// the requested secret value returned by GetSecretValueAsync.</param>
        /// <returns>A string representing the decoded secret value.</returns>
        private string DecodeString(GetSecretValueResponse response)
        {
            // Decrypts secret using the associated AWS Key Management Service
            // Customer Master Key (CMK.) Depending on whether the secret is a
            // string or binary value, one of these fields will be populated.
            if (response.SecretString != null)
            {
                var secret = response.SecretString;
                return secret;
            }
            else if (response.SecretBinary != null)
            {
                var memoryStream = response.SecretBinary;
                var reader = new StreamReader(memoryStream);
                string decodedBinarySecret = Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
                return decodedBinarySecret;
            }
            else
            {
                return string.Empty;
            }
        }

        private string GetAppSetting(string key, string defaultValue)
        {
            var value = ConfigurationManager.AppSettings[key];
            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }
    }

}
