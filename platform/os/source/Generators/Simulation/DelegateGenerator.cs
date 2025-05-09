// Copyright © Spatial. All rights reserved.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spatial.Simulation
{
    /// <summary>
    /// A <see cref="ISourceGenerator"/> for delegates.
    /// </summary>
    [Generator]
    public class DelegateGenerator : ISourceGenerator
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

            {Functions()}";

            context.AddSource("Delegates.g.cs", SourceText.From(source, Encoding.UTF8));
        }

        private string Functions()
        {
            var methods = new List<string>
            {
                @"/// <summary>
                /// Mutate an <see cref=""Entity""/>.
                /// </summary>
                /// <param name=""future"">The future state of the <see cref=""Space""/>.</param>
                /// <param name=""entity"">The target <see cref=""Entity""/>.</param>
                public delegate void MutateFunction(Future future, in Entity entity);"
            };

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

                var paramDocs = string.Join(
                    separator: "\n\t/// ", 
                    values: Enumerable
                        .Range(1, i)
                        .Select(x => 
                        {
                            var type = $"T{x}";
                            var name = $"c{x}";
                            var doc = $"A component of type <typeparamref name=\"{type}\"/>.";

                            return $"<param name=\"{name}\">{doc}</param>";
                        }));

                var constraints = string.Join(
                    separator: "\n\t", 
                    values: Enumerable
                        .Range(1, i)
                        .Select(x => $"where T{x} : unmanaged, IComponent"));

                var parameters = string.Join(
                    separator: ", ",
                    values: Enumerable.Range(1, i).Select(x => $"ref T{x} c{x}"));

                methods.Add(
                    $@"/// <summary>
                    /// Mutate an <see cref=""Entity""/>.
                    /// </summary>
                    /// {typeParamDocs}
                    /// <param name=""future"">The future state of the <see cref=""Space""/>.</param>
                    /// <param name=""entity"">The target <see cref=""Entity""/>.</param>
                    /// {paramDocs}
                    public delegate void MutateFunction<{typeParams}>(Future future, in Entity entity, {parameters})
                        {constraints};");
            }

            return string.Join("\n\n", methods);
        }
    }
}