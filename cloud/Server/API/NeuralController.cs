// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data.Brain;
using Spatial.Cloud.Data.Brain.Neurons;
using Spatial.Cloud.Data.Brain.Synapses;
using Spatial.Cloud.Data.Scopes;
using Spatial.Cloud.ECS.Systems;
using Spatial.Identity.Authorization;
using System.Text.Json;
using System.Threading.Channels;

namespace Spatial.Cloud.API;

/// <summary>
/// A <see cref="Controller"/> for neural functions.
/// </summary>
[Path("brain")]
public class NeuralController : Controller
{
    /// <summary>
    /// Get a <see cref="Snapshot"/> of the <see cref="Hypersolver"/> network.
    /// </summary>
    /// <returns>A <see cref="Snapshot"/> of the <see cref="Hypersolver"/> network.</returns>
    [GET]
    [Authorize(Scope.Brain.Read)]
    public Task<Snapshot> GetSnapshotAsync()
    {
        return Task.FromResult(Server.Current.Hypersolver.Snapshot);
    }

    /// <summary>
    /// Stream live <see cref="Hypersolver"/> network state.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that governs cancellation of the live stream.</param>
    [GET]
    [Path("stream")]
    [Authorize(Scope.Brain.Stream)]
    public async Task StreamAsync(CancellationToken cancellationToken)
    {
        Response.Headers.Append("Content-Type", "text/event-stream");
        Response.Headers.Append("Cache-Control", "no-cache");
        Response.Headers.Append("X-Accel-Buffering", "no");

        var structural = Channel.CreateUnbounded<(string Event, string Data)>();
        var snapshots = Channel.CreateBounded<(string Event, string Data)>(new BoundedChannelOptions(1) {
            FullMode = BoundedChannelFullMode.DropOldest
        });

        var merged = Channel.CreateUnbounded<(string Event, string Data)>();

        async Task Pipe(ChannelReader<(string Event, string Data)> source)
        {
            await foreach (var item in source.ReadAllAsync(cancellationToken))
            {
                await merged.Writer.WriteAsync(item, cancellationToken);
            }
        }

        var pipes = Task.WhenAll(Pipe(structural.Reader), Pipe(snapshots.Reader));

        void Update(Snapshot snapshot) => snapshots.Writer.TryWrite(("updated", JsonSerializer.Serialize(snapshot)));
        void AddNeuron(Neuron neuron) => structural.Writer.TryWrite(("neurons.add", JsonSerializer.Serialize(neuron)));
        void UpdateNeuron(Neuron neuron) => structural.Writer.TryWrite(("neurons.update", JsonSerializer.Serialize(neuron)));
        void RemoveNeuron(Neuron neuron) => structural.Writer.TryWrite(("neurons.remove", JsonSerializer.Serialize(neuron)));
        void AddSynapse(Synapse synapse) => structural.Writer.TryWrite(("synapses.add", JsonSerializer.Serialize(synapse)));
        void UpdateSynapse(Synapse synapse) => structural.Writer.TryWrite(("synapses.update", JsonSerializer.Serialize(synapse)));
        void RemoveSynapse(Synapse synapse) => structural.Writer.TryWrite(("synapses.remove", JsonSerializer.Serialize(synapse)));

        var hypersolver = Server.Current.Hypersolver;

        hypersolver.Updated += Update;
        hypersolver.Neurons.Added += AddNeuron;
        hypersolver.Neurons.Updated += UpdateNeuron;
        hypersolver.Neurons.Removed += RemoveNeuron;
        hypersolver.Synapses.Added += AddSynapse;
        hypersolver.Synapses.Updated += UpdateSynapse;
        hypersolver.Synapses.Removed += RemoveSynapse;

        try
        {
            foreach (var neuron in hypersolver.Resources.Neurons)
            {
                await Response.WriteAsync($"event: neurons.add\ndata: {JsonSerializer.Serialize(neuron)}\n\n", cancellationToken);
            }

            foreach (var synapse in hypersolver.Resources.Synapses)
            {
                await Response.WriteAsync($"event: synapses.add\ndata: {JsonSerializer.Serialize(synapse)}\n\n", cancellationToken);
            }

            await Response.WriteAsync("event: ready\ndata: {}\n\n", cancellationToken);
            await Response.Body.FlushAsync(cancellationToken);

            await foreach (var (e, data) in merged.Reader.ReadAllAsync(cancellationToken))
            {
                await Response.WriteAsync($"event: {e}\ndata: {data}\n\n", cancellationToken);
                await Response.Body.FlushAsync(cancellationToken);
            }
        }
        catch (OperationCanceledException) { }
        finally
        {
            hypersolver.Updated -= Update;
            hypersolver.Neurons.Added -= AddNeuron;
            hypersolver.Neurons.Updated -= UpdateNeuron;
            hypersolver.Neurons.Removed -= RemoveNeuron;
            hypersolver.Synapses.Added -= AddSynapse;
            hypersolver.Synapses.Updated -= UpdateSynapse;
            hypersolver.Synapses.Removed -= RemoveSynapse;

            structural.Writer.Complete();
            snapshots.Writer.Complete();

            await pipes;
        }
    }

    /// <summary>
    /// Stimulate a <see cref="Neuron"/>.
    /// </summary>
    /// <param name="neuron">The <see cref="Neuron"/> to stimulate.</param>
    /// <param name="options">Configurable options for neural stimulation.</param>
    [POST]
    [Path("neurons/{neuron}/stimulate")]
    [Authorize(Scope.Brain.Stimulate_Neuron)]
    public Task StimulateAsync(string neuron, [Body] StimulateNeuronOptions options)
    {
        Server.Current.Hypersolver.StimulateOne(neuron, options.Charge);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Add a <see cref="Neuron"/> to the <see cref="Hypersolver"/> network.
    /// </summary>
    /// <param name="options">Configurable options for the <see cref="Neuron"/>.</param>
    /// <returns>The <see cref="Neuron"/> that was added.</returns>
    [POST]
    [Path("neurons")]
    [Authorize(Scope.Brain.Add_Neuron)]
    public Task<Neuron> AddNeuronAsync([Body] CreateNeuronOptions options)
    {
        return Task.FromResult(Server.Current.Hypersolver.AddNeuron(options));
    }

    /// <summary>
    /// Update a <see cref="Neuron"/>.
    /// </summary>
    /// <param name="neuron">The <see cref="Neuron"/> to update.</param>
    /// <param name="options">Configurable options for the <see cref="Neuron"/>.</param>
    [PATCH]
    [Path("neurons/{neuron}")]
    [Authorize(Scope.Brain.Update_Neuron)]
    public Task UpdateNeuronAsync(string neuron, [Body] UpdateNeuronOptions options)
    {
        Server.Current.Hypersolver.UpdateNeuron(neuron, options);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Remove a <see cref="Neuron"/> from the <see cref="Hypersolver"/> network.
    /// </summary>
    /// <param name="neuron">The <see cref="Neuron"/> to remove.</param>
    [DELETE]
    [Path("neurons/{neuron}")]
    [Authorize(Scope.Brain.Remove_Neuron)]
    public Task RemoveNeuronAsync(string neuron)
    {
        Server.Current.Hypersolver.RemoveNeuron(neuron);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Add a <see cref="Synapse"/> to the <see cref="Hypersolver"/> network.
    /// </summary>
    /// <param name="options">Configurable options for the <see cref="Synapse"/>.</param>
    /// <returns>The <see cref="Synapse"/> that was added.</returns>
    [POST]
    [Path("synapses")]
    [Authorize(Scope.Brain.Add_Synapse)]
    public Task<Synapse> AddSynapseAsync([Body] CreateSynapseOptions options)
    {
        return Task.FromResult(Server.Current.Hypersolver.AddSynapse(options));
    }

    /// <summary>
    /// Update a <see cref="Synapse"/>.
    /// </summary>
    /// <param name="synapse">The <see cref="Synapse"/> to update.</param>
    /// <param name="options">Configurable options for the <see cref="Synapse"/>.</param>
    [PATCH]
    [Path("synapses/{synapse}")]
    [Authorize(Scope.Brain.Update_Synapse)]
    public Task UpdateSynapseAsync(string synapse, [Body] UpdateSynapseOptions options)
    {
        Server.Current.Hypersolver.UpdateSynapse(synapse, options);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Remove a <see cref="Synapse"/> from the <see cref="Hypersolver"/> network.
    /// </summary>
    /// <param name="synapse">The <see cref="Synapse"/> to remove.</param>
    [DELETE]
    [Path("synapses/{synapse}")]
    [Authorize(Scope.Brain.Remove_Synapse)]
    public Task RemoveSynapseAsync(string synapse)
    {
        Server.Current.Hypersolver.RemoveSynapse(synapse);

        return Task.CompletedTask;
    }
}