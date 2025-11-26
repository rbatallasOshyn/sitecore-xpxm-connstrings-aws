using Sitecore.Diagnostics;
using System.Collections.Generic;

namespace RBatallas.AWSSecretsIntegration.ConnectionStringReplacer.Replacers
{
    internal class EnvVariablesNameReplacerManager
    {
        private static readonly EnvVariablesNameReplacerManager Instance = new EnvVariablesNameReplacerManager((IEnvVariableNameReplacerProvider)new DefaultEnvVariableNameReplacerProvider());

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Sitecore.Configuration.Replacers.EnvVariablesNameReplacerManager" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public EnvVariablesNameReplacerManager(IEnvVariableNameReplacerProvider provider)
        {
            Assert.ArgumentNotNull((object)provider, nameof(provider));
            this.Provider = provider;
        }

        /// <summary>Gets the provider.</summary>
        /// <value>The provider.</value>
        public virtual IEnvVariableNameReplacerProvider Provider { get; }

        /// <summary>Applies the replacers.</summary>
        /// <param name="name">The name.</param>
        /// <returns>Returns a collection of names with applied replacements.</returns>
        public static IEnumerable<string> ApplyReplacers(string name)
        {
            return EnvVariablesNameReplacerManager.Instance.Provider.ApplyReplacers(name);
        }

        /// <summary>Reverses the replacers.</summary>
        /// <param name="name">The name.</param>
        /// <returns>Returns a collection of names with reversed replacements.</returns>
        public static IEnumerable<string> ReverseReplacers(string name)
        {
            return EnvVariablesNameReplacerManager.Instance.Provider.ReverseReplacers(name);
        }
    }
}
