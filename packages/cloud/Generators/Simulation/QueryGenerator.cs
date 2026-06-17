// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spatial.Simulation
{
    /// <summary>
    /// A <see cref="ISourceGenerator"/> for queries.
    /// </summary>
    [Generator]
    public class QueryGenerator : ISourceGenerator
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
            $@"// Copyright © Spatial Corporation. All rights reserved.
            // This is a generated source file and should not be modified directly.

            namespace Spatial.Simulation;

            public partial class Query
            {{
            {Requirements()}
            }}";

            context.AddSource("Query.g.cs", SourceText.From(source, Encoding.UTF8));
        }

        private string Requirements()
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

                var components = string.Join(
                    separator: ", ",
                    values: Enumerable.Range(1, i).Select(x => $"ref T{x} c{x}"));

                methods.Add(
                    $@" /// <summary>
                        /// Specifies that entities must have all of the specified components.
                        /// </summary>
                        /// {typeParamDocs}
                        /// <returns>The current <see cref=""Query""/> for method chaining.</returns>
                        public Query WithAll<{typeParams}>()
                            {constraints}
                        {{
                            return WithAll(Signature.Combine<{typeParams}>());
                        }}");

                methods.Add(
                    $@" /// <summary>
                        /// Specifies that entities must have at least one of the specified components.
                        /// </summary>
                        /// {typeParamDocs}
                        /// <returns>The current <see cref=""Query""/> for method chaining.</returns>
                        public Query WithAny<{typeParams}>()
                            {constraints}
                        {{
                            return WithAny(Signature.Combine<{typeParams}>());
                        }}");

                methods.Add(
                    $@" /// <summary>
                        /// Specifies that entities must not have any of the specified components.
                        /// </summary>
                        /// {typeParamDocs}
                        /// <returns>The current <see cref=""Query""/> for method chaining.</returns>
                        public Query WithNone<{typeParams}>()
                            {constraints}
                        {{
                            return WithNone(Signature.Combine<{typeParams}>());
                        }}");
            }

            return string.Join("\n\n", methods);
        }
    }
}