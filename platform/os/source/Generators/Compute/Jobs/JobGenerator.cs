// Copyright © Spatial. All rights reserved.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spatial.Compute.Jobs
{
    /// <summary>
    /// A <see cref="ISourceGenerator"/> for jobs.
    /// </summary>
    [Generator]
    public class JobGenerator : ISourceGenerator
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

using ILGPU;
using ILGPU.Runtime;

using Spatial.Compute.Jobs.Acceleration;

namespace Spatial.Compute.Jobs;

public abstract partial class Job
{{
{Runners()}
}}";

            context.AddSource("Job.g.cs", SourceText.From(source, Encoding.UTF8));
        }

        private string Runners()
        {
            var methods = new List<string>();

            for (var i = 1; i <= Constants.KernelOverloads; i++)
            {
                methods.Add(KernelMethod(i));
            }

            return string.Join("\n\n", methods);
        }

        private string KernelMethod(int parameters)
        {
            var typeParams = string.Join(
                    separator: ", ",
                    values: Enumerable.Range(1, parameters).Select(x => $"T{x}"));

                var typeParamDocs = string.Join(
                    separator: "\n\t/// ", 
                    values: Enumerable
                        .Range(1, parameters)
                        .Select(x => 
                        {
                            var name = $"T{x}";
                            var doc = $"Parameter type of parameter {x}.";

                            return $"<typeparam name=\"{name}\">{doc}</typeparam>";
                        }));

                var functionArguments = string.Join(
                    separator: ", ",
                    values: Enumerable
                        .Range(1, parameters)
                        .Select(x => $"T{x} arg{x}"));

                var argumentDocs = string.Join(
                    separator: "\n\t/// ", 
                    values: Enumerable
                        .Range(1, parameters)
                        .Select(x => 
                        {
                            var name = $"arg{x}";
                            var doc = $"Argument {x} of the underlying kernel.";

                            return $"<param name=\"{name}\">{doc}</param>";
                        }));

                var args = string.Join(
                    separator: ", ",
                    values: Enumerable
                        .Range(1, parameters)
                        .Select(x => $"arg{x}"));

                var constraints = string.Join(
                    separator: "\n\t\t", 
                    values: Enumerable
                        .Range(1, parameters)
                        .Select(x => $"where T{x} : struct"));

                return
$@" /// <summary>
    /// Dispatch a job to the GPU.
    /// </summary>
    /// <param name=""accelerator"">The job's <see cref=""Accelerator""/>.</param>
    /// <param name=""extent"">The launch extent of the kernel.</param>
    /// {argumentDocs}
    /// <param name=""function"">The function to execute.</param>
    /// <typeparam name=""TIndex"">The kernel's index type.</typeparam>
    /// {typeParamDocs}
    public static void Kernel<TIndex, {typeParams}>(Accelerator accelerator, TIndex extent, {functionArguments}, Action<TIndex, {typeParams}> function)
        where TIndex : struct, IIndex
        {constraints}
    {{
        Kernel(new KernelJob<TIndex, {typeParams}>(accelerator, extent, {args}, function));
    }}";
        }
    }
}