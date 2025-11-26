using System.Collections.Generic;

namespace RBatallas.AWSSecretsIntegration.ConnectionStringReplacer.Replacers
{
    internal interface IEnvVariableNameReplacer
    {
        /// <summary>Replaces the specified variable name.</summary>
        /// <param name="variableName">Name of the variable.</param>
        /// <returns></returns>
        string Replace(string variableName);

        /// <summary>Reverses the replace.</summary>
        /// <param name="variableName">Name of the variable.</param>
        /// <returns></returns>
        string ReverseReplace(string variableName);

        /// <summary>Gets the replacements.</summary>
        /// <value>The replacements.</value>
        IReadOnlyDictionary<string, string> Replacements { get; }
    }
}
