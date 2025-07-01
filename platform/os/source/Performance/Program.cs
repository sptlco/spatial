// Copyright © Spatial Corporation. All rights reserved.

using BenchmarkDotNet.Running;

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).RunAll();