using Sitecore.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace RBatallas.AWSSecretsIntegration.ConnectionStringReplacer.Replacers
{
    internal class DefaultEnvVariableNameReplacerProvider : IEnvVariableNameReplacerProvider
    {
        private readonly List<IEnvVariableNameReplacer> _replacers = new List<IEnvVariableNameReplacer>();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sitecore.Configuration.Replacers.DefaultEnvVariableNameReplacerProvider" /> class.
        /// </summary>
        public DefaultEnvVariableNameReplacerProvider()
        {
            this._replacers.Add((IEnvVariableNameReplacer)new DefaultEnvVariableNameReplacer((IDictionary<string, string>)new Dictionary<string, string>()
              {
                {
                  ":",
                  "__"
                }
              }));
            this._replacers.Add((IEnvVariableNameReplacer)new DefaultEnvVariableNameReplacer((IDictionary<string, string>)new Dictionary<string, string>()
            {
                {
                    ".",
                    "_dot_"
                }
                }));
        }

        /// <inheritdoc />
        public virtual IEnumerable<IEnvVariableNameReplacer> Replacers
        {
            get => (IEnumerable<IEnvVariableNameReplacer>)this._replacers;
        }

        /// <inheritdoc />
        public IEnumerable<string> ApplyReplacers(string name)
        {
            Assert.ArgumentNotNull((object)name, nameof(name));
            foreach (IEnvVariableNameReplacer replacer in this.Replacers)
                yield return replacer.Replace(name);
        }

        /// <inheritdoc />
        public IEnumerable<string> ReverseReplacers(string name)
        {
            Assert.ArgumentNotNull((object)name, nameof(name));
            List<string> source = new List<string>();
            foreach (IEnvVariableNameReplacer replacer in this.Replacers)
                source.Add(replacer.ReverseReplace(name));
            return source.Distinct<string>();
        }
    }
}
