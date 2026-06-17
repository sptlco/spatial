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
    public class SpaceGenerator : ISourceGenerator
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

            using Spatial.Compute;
            using System.Runtime.CompilerServices;

            namespace Spatial.Simulation;

            public sealed partial class Space
            {{
            {Create()}
            }}";

            context.AddSource("Space.g.cs", SourceText.From(source, Encoding.UTF8));
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
                    values: Enumerable.Range(1, i).Select(x => $"in T{x} c{x}"));

                var constraints = string.Join(
                    separator: "\n\t\t", 
                    values: Enumerable
                        .Range(1, i)
                        .Select(x => $"where T{x} : unmanaged, IComponent"));

                var queries = string.Join(
                    separator: "\n\t\t\t\t\t",
                    values: Enumerable.Range(1, i).Select(x => $"ref var c{x} = ref chunk.Ref<T{x}>(handle.Index);"));

                var mutators = string.Join(
                    separator: "\n\t\t",
                    values: Enumerable.Range(1, i).Select(x => $"Set(entity, c{x});"));

                var args = string.Join(
                    separator: ", ",
                    values: Enumerable.Range(1, i).Select(x => $"ref c{x}"));

                var values = string.Join(
                    separator: ", ",
                    values: Enumerable.Range(1, i).Select(x => $"c{x}"));

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
                            Reserve(Signature.Combine<{typeParams}>(), count);
                        }}");

                methods.Add(
                    $@" /// <summary>
                        /// Create an <see cref=""Entity""/>.
                        /// </summary>
                        /// {typeParamDocs}
                        /// <returns>An <see cref=""Entity""/>.</returns>
                        [MethodImpl(MethodImplOptions.AggressiveInlining)]
                        public Entity Create<{typeParams}>()
                            {constraints}
                        {{
                            return Create(Signature.Combine<{typeParams}>());
                        }}");

                methods.Add(
                    $@" /// <summary>
                        /// Create an <see cref=""Entity""/>.
                        /// </summary>
                        /// {typeParamDocs}
                        /// {paramDocs}
                        /// <returns>An <see cref=""Entity""/>.</returns>
                        [MethodImpl(MethodImplOptions.AggressiveInlining)]
                        public Entity Create<{typeParams}>({components})
                            {constraints}
                        {{
                            var entity = Create<{typeParams}>();

                            {mutators}

                            return entity;
                        }}");

                methods.Add(
                    $@" /// <summary>
                        /// Mutate the <see cref=""Space""/>.
                        /// </summary>
                        /// {typeParamDocs}
                        [MethodImpl(MethodImplOptions.AggressiveInlining)]
                        public unsafe void Mutate<{typeParams}>(Query query, MutateFunction<{typeParams}> function)
                            {constraints}
                        {{
                            var process = (Archetype archetype, int m, int n) => {{
                                var chunk = archetype.Chunks[m];

                                if (n < chunk.Count)
                                {{
                                    ref var entity = ref chunk.Entities[n];
                                    ref var handle = ref _entities[entity];

                                    {queries}

                                    function(_future, handle, {args});
                                }}
                            }};

                            foreach (var (_, archetype) in _archetypeMap)
                            {{
                                if (!query.Matches(archetype.Signature))
                                {{
                                    continue;
                                }}

                                if (query.Accelerated)
                                {{
                                    Job.ParallelFor2D(
                                        width: archetype.Chunks.Length,
                                        height: (int) archetype.Chunks.Max(c => c.Count),
                                        function: (m, n) => process(archetype, m, n)).Wait();
                                }}
                                else
                                {{
                                    for (var m = 0; m < archetype.Chunks.Length; m++)
                                    {{
                                        for (var n = 0; n < archetype.Chunks[m].Count; n++)
                                        {{
                                            process(archetype, m, n);
                                        }}
                                    }}
                                }}
                            }}

                            _future.Commit(this);
                        }}");

                if (i <= 15)
                {
                    methods.Add(
                    $@" /// <summary>
                        /// Mutate the <see cref=""Space""/>.
                        /// </summary>
                        /// {typeParamDocs}
                        [MethodImpl(MethodImplOptions.AggressiveInlining)]
                        public unsafe void Mutate<{typeParams}>(Query query, Func<Entity, {typeParams}, bool> filter, MutateFunction<{typeParams}> function)
                            {constraints}
                        {{
                            var process = (Archetype archetype, int m, int n) => {{
                                var chunk = archetype.Chunks[m];

                                if (n < chunk.Count)
                                {{
                                    ref var entity = ref chunk.Entities[n];
                                    ref var handle = ref _entities[entity];

                                    {queries}

                                    if (filter(handle, {values}))
                                    {{
                                        function(_future, handle, {args});
                                    }}
                                }}
                            }};

                            foreach (var (_, archetype) in _archetypeMap)
                            {{
                                if (!query.Matches(archetype.Signature))
                                {{
                                    continue;
                                }}

                                if (query.Accelerated)
                                {{
                                    Job.ParallelFor2D(
                                        width: archetype.Chunks.Length,
                                        height: (int) archetype.Chunks.Max(c => c.Count),
                                        function: (m, n) => process(archetype, m, n)).Wait();
                                }}
                                else
                                {{
                                    for (var m = 0; m < archetype.Chunks.Length; m++)
                                    {{
                                        for (var n = 0; n < archetype.Chunks[m].Count; n++)
                                        {{
                                            process(archetype, m, n);
                                        }}
                                    }}
                                }}
                            }}

                            _future.Commit(this);
                        }}");
                }
            }

            return string.Join("\n\n", methods);
        }
    }
}