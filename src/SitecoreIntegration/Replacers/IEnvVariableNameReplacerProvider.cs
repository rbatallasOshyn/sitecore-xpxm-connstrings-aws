using System.Collections.Generic;

namespace RBatallas.AWSSecretsIntegration.ConnectionStringReplacer.Replacers
{
    internal interface IEnvVariableNameReplacerProvider
    {
        /// <summary>Gets the resolvers.</summary>
        /// <value>The resolvers.</value>
        IEnumerable<IEnvVariableNameReplacer> Replacers { get; }

        /// <summary>Applies the replacers.</summary>
        /// <param name="name">The name.</param>
        /// <returns>Returns a collection of names with applied replacements.</returns>
        IEnumerable<string> ApplyReplacers(string name);

        /// <summary>Reverses the replacers.</summary>
        /// <param name="name">The name.</param>
        /// <returns>Returns a collection of names with reversed replacements.</returns>
        IEnumerable<string> ReverseReplacers(string name);
    }
}
