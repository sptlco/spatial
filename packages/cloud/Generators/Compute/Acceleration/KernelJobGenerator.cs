// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spatial.Compute.Acceleration
{
    /// <summary>
    /// A <see cref="ISourceGenerator"/> for kernel jobs.
    /// </summary>
    [Generator]
    public class KernelJobGenerator : ISourceGenerator
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

using ILGPU;
using ILGPU.Runtime;
using Spatial.Compute.Commands;

namespace Spatial.Compute.Acceleration;

{Jobs()}";

            context.AddSource("KernelJob.g.cs", SourceText.From(source, Encoding.UTF8));
        }

        private string Jobs()
        {
            var declarations = new List<string>();

            for (var i = 1; i <= Constants.KernelOverloads; i++)
            {
                declarations.Add(KernelJobClass(i));
            }

            return string.Join("\n\n", declarations);
        }

        private string KernelJobClass(int parameters)
        {
            var typeParams = string.Join(
                    separator: ", ",
                    values: Enumerable.Range(1, parameters).Select(x => $"T{x}"));

            var typeParamDocs = string.Join(
                separator: "\n/// ", 
                values: Enumerable
                    .Range(1, parameters)
                    .Select(x => 
                    {
                        var name = $"T{x}";
                        var doc = $"Parameter type of parameter {x}.";

                        return $"<typeparam name=\"{name}\">{doc}</typeparam>";
                    }));

            var constraints = string.Join(
                separator: "\n\t", 
                values: Enumerable
                    .Range(1, parameters)
                    .Select(x => $"where T{x} : struct"));

            var fields = string.Join(
                separator: "\n\t",
                values: Enumerable
                    .Range(1, parameters)
                    .Select(x => $"private readonly T{x} _arg{x};"));

            var constructorArguments = string.Join(
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

            var setters = string.Join(
                separator: "\n\t\t",
                values: Enumerable
                    .Range(1, parameters)
                    .Select(x => $"_arg{x} = arg{x};"));

            var functionArguments = string.Join(
                separator: ", ",
                values: Enumerable
                    .Range(1, parameters)
                    .Select(x => $"_arg{x}"));

            return
$@"/// <summary>
/// An accelerated <see cref=""CommandJob""/> that runs on a GPU.
/// </summary>
/// <typeparam name=""TIndex"">The type used to index the kernel.</typeparam>
/// {typeParamDocs}
internal class KernelJob<TIndex, {typeParams}> : KernelJob
    where TIndex : struct, IIndex
    {constraints}
{{
    private readonly TIndex _extent;
    {fields}

    private readonly new Action<TIndex, {typeParams}> _function;

    /// <summary>
    /// Create a new <see cref=""KernelJob{{TIndex, {typeParams}}}""/>.
    /// </summary>
    /// <param name=""accelerator"">The job's <see cref=""Accelerator""/>.</param>
    /// <param name=""extent"">The launch extent of the kernel.</param>
    /// {argumentDocs}
    /// <param name=""function"">The function to execute.</param>
    public KernelJob(Accelerator accelerator, TIndex extent, {constructorArguments}, Action<TIndex, {typeParams}> function)
        : base(accelerator)
    {{
        _extent = extent;

        {setters}

        _function = _accelerator.LoadAutoGroupedStreamKernel(function);
    }}

    /// <summary>
    /// Execute the <see cref=""KernelJob""/>.
    /// </summary>
    public override sealed void Execute()
    {{
        _function(_extent, {functionArguments});
    }}
}}";
        }
    }
}