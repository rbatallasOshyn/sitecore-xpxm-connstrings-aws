using Microsoft.Configuration.ConfigurationBuilders;
using RBatallas.AWSSecretsIntegration.ConnectionStringReplacer.AWS;
using RBatallas.AWSSecretsIntegration.ConnectionStringReplacer.Replacers;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RBatallas.AWSSecretsIntegration.ConnectionStringReplacer.Builder
{
    public class ConnectionStringBuilder : EnvironmentConfigBuilder
    {
        public override string Name => "ConnectionStringBuilder";

        public override string Description => "Replaces tokens in connection strings with environment variable values.";

        public override ICollection<KeyValuePair<string, string>> GetAllValues(string prefix)
        {
            ICollection<KeyValuePair<string, string>> allValues = base.GetAllValues(prefix);
            Collection<KeyValuePair<string, string>> source = new Collection<KeyValuePair<string, string>>();
            foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>)allValues)
            {
                source.Add(new KeyValuePair<string, string>(keyValuePair.Key, keyValuePair.Value));
                foreach (string reverseReplacer in EnvVariablesNameReplacerManager.ReverseReplacers(keyValuePair.Key))
                {
                    string variant = reverseReplacer;
                    if (source.All<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>)(p => p.Key != variant)))
                        source.Add(new KeyValuePair<string, string>(variant, keyValuePair.Value));
                }
            }
            return (ICollection<KeyValuePair<string, string>>)source;
        }

        public override string GetValue(string key)
        {
            Assert.ArgumentNotNullOrEmpty(key, nameof(key));

            string secretValue = AwsSecretManager.Instance.GetSecret(key);

            EnvVariablesNameReplacerManager.ApplyReplacers(key).ToList().ForEach(variantKey =>
            {
                if (string.IsNullOrEmpty(secretValue))
                {
                    secretValue = base.GetValue(variantKey);
                }
            });

            return secretValue;
        }
    }
}
