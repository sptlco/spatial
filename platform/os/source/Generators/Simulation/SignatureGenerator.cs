// Copyright © Spatial. All rights reserved.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spatial.Simulation
{
    /// <summary>
    /// A <see cref="ISourceGenerator"/> for signatures.
    /// </summary>
    [Generator]
    public class SignatureGenerator : ISourceGenerator
    {
        /// <summary>
        /// Initializes the <see cref="ISourceGenerator"/>.
        /// </summary>
        /// <param name="context">The source generator's initialization context.</param>
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        /// <summary>
        /// Execute the <see cref="ISourceGenerator"/>.
        /// </summary>
        /// <param name="context">The source generator's execution context.</param>
        public void Execute(GeneratorExecutionContext context)
        {
            var source = 
            $@"// Copyright © Spatial. All rights reserved.
            // This is a generated source file and should not be modified directly.

            namespace Spatial.Simulation;

            public partial record struct Signature
            {{
            {Combine()}
            }}";

            context.AddSource("Signature.g.cs", SourceText.From(source, Encoding.UTF8));
        }

        private string Combine()
        {
            var methods = new List<string>();

            for (var i = 1; i <= Constants.ComponentOverloads; i++)
            {
                var typeParams = string.Join(
                    separator: ", ",
                    values: Enumerable.Range(1, i).Select(x => $"T{x}"));

                var typeParamDocs = string.Join(
                    separator: "\n\t/// ", 
                    values: Enumerable
                        .Range(1, i)
                        .Select(x => 
                        {
                            var name = $"T{x}";
                            var doc = "A <see cref=\"IComponent\"/> type.";

                            return $"<typeparam name=\"{name}\">{doc}</typeparam>";
                        }));

                var constraints = string.Join(
                    separator: "\n\t\t", 
                    values: Enumerable
                        .Range(1, i)
                        .Select(x => $"where T{x} : unmanaged, IComponent"));

                var intermediates = string.Join(
                    separator: ", ",
                    values: Enumerable.Range(1, i).Select(x => $"Signature.Of<T{x}>()"));

                methods.Add(
                    $@" /// <summary>
                        /// Combine multiple signatures into one <see cref=""Signature""/>.
                        /// </summary>
                        /// {typeParamDocs}
                        /// <returns>A <see cref=""Signature""/>.</returns>
                        public static Signature Combine<{typeParams}>()
                            {constraints}
                        {{
                            return Signature.Combine({intermediates});
                        }}");
            }

            return string.Join("\n\n", methods);
        }
    }
}