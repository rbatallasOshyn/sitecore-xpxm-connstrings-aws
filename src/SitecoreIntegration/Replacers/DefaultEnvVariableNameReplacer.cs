using Sitecore.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace RBatallas.AWSSecretsIntegration.ConnectionStringReplacer.Replacers
{
    internal class DefaultEnvVariableNameReplacer : IEnvVariableNameReplacer
    {
        private readonly IReadOnlyDictionary<string, string> _replacements;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sitecore.Configuration.Replacers.DefaultEnvVariableNameReplacer" /> class.
        /// </summary>
        /// <param name="replacements">The replacements.</param>
        public DefaultEnvVariableNameReplacer(IDictionary<string, string> replacements)
        {
            Assert.ArgumentNotNull((object)replacements, nameof(replacements));
            this._replacements = (IReadOnlyDictionary<string, string>)new ReadOnlyDictionary<string, string>(replacements);
        }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> Replacements => this._replacements;

        /// <inheritdoc />
        public virtual string Replace(string variableName)
        {
            StringBuilder stringBuilder = new StringBuilder(variableName);
            foreach (string key in this._replacements.Keys)
                stringBuilder = stringBuilder.Replace(key, this._replacements[key]);
            return stringBuilder.ToString();
        }

        /// <inheritdoc />
        public string ReverseReplace(string variableName)
        {
            StringBuilder stringBuilder = new StringBuilder(variableName);
            foreach (KeyValuePair<string, string> replacement in (IEnumerable<KeyValuePair<string, string>>)this._replacements)
                stringBuilder = stringBuilder.Replace(replacement.Value, replacement.Key);
            return stringBuilder.ToString();
        }
    }
}
