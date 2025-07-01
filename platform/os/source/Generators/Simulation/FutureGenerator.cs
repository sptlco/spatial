// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spatial.Simulation
{
    /// <summary>
    /// A <see cref="ISourceGenerator"/> for spaces.
    /// </summary>
    [Generator]
    public class FutureGenerator : ISourceGenerator
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

            using Spatial.Hardware;
            using System.Runtime.CompilerServices;

            namespace Spatial.Simulation;

            public sealed partial class Future
            {{
            {Create()}
            }}";

            context.AddSource("Future.g.cs", SourceText.From(source, Encoding.UTF8));
        }

        private string Create()
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

                var components = string.Join(
                    separator: ", ",
                    values: Enumerable.Range(1, i).Select(x => $"T{x} c{x}"));

                var args = string.Join(
                    separator: ", ",
                    values: Enumerable.Range(1, i).Select(x => $"c{x}"));

                var constraints = string.Join(
                    separator: "\n\t\t", 
                    values: Enumerable
                        .Range(1, i)
                        .Select(x => $"where T{x} : unmanaged, IComponent"));

                methods.Add(
                    $@" /// <summary>
                        /// Reserve space for entities.
                        /// </summary>
                        /// {typeParamDocs}
                        /// <returns>An <see cref=""Entity""/>.</returns>
                        [MethodImpl(MethodImplOptions.AggressiveInlining)]
                        public void Reserve<{typeParams}>(uint count)
                            {constraints}
                        {{
                            _reserves.Enqueue(space => space.Reserve<{typeParams}>(count));
                        }}");

                methods.Add(
                    $@" /// <summary>
                        /// Create an <see cref=""Entity""/>.
                        /// </summary>
                        /// {typeParamDocs}
                        [MethodImpl(MethodImplOptions.AggressiveInlining)]
                        public void Create<{typeParams}>()
                            {constraints}
                        {{
                            _creates.Enqueue(space => space.Create<{typeParams}>());
                        }}");

                methods.Add(
                    $@" /// <summary>
                        /// Create an <see cref=""Entity""/>.
                        /// </summary>
                        /// {typeParamDocs}
                        /// {paramDocs}
                        [MethodImpl(MethodImplOptions.AggressiveInlining)]
                        public void Create<{typeParams}>({components})
                            {constraints}
                        {{
                            _creates.Enqueue(space => space.Create({args}));
                        }}");
            }

            return string.Join("\n\n", methods);
        }
    }
}