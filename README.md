# Vertex Flow

A cross-platform library that allows you to transfer BIM data directly into your application and maintain a live link between them.

<details open><summary><b>Video Demonstration</b></summary>
<br />

https://user-images.githubusercontent.com/28132516/134523455-40196416-a56f-48b0-9205-52637cf92137.mp4

</details>

## Table of Contents

- [About](#about)
- [How It Works](#how-it-works)
- [Installation](#installation)
    - [Azure Cosmos DB Emulator](#azure-cosmos-db-emulator)
    - [Revit Addin](#revit-addin)
    - [Unity Plugin](#unity-plugin)
- [Examples](#examples)
    - [Send Mesh Data](#send-mesh-data)
    - [Get Mesh Data](#get-mesh-data)
    - [Listen Mesh Data](#listen-mesh-data)
    - [Custom Json Serializer](#custom-json-serializer)
- [How To Use](#how-to-use)
    - [Export Mesh From Revit](#export-geometry-from-revit)
    - [Import Mesh To Unity](#import-mesh-to-unity)
    - [Import Mesh To Unigine](#import-mesh-to-unigine)
- [Optimizations](#optimizations)
    - [Benchmarks](#benchmarks)
- [License](#license)

## About

Vertex Flow is designed to speed up your building information modeling (BIM) workflows. No longer necessary to juggle multiple tools and take dozens of steps to prepare and optimize BIM data for creating 3D experiences.

**Vertex Flow SDK** can help you create real-time BIM applications by building on top of any 3D engine.

## How It Works

Vertex Flow makes the process of bringing BIM models into real-time 3D extremely simple by unlocking the ability to stream data directly into your application.

> The first integration is with Revit, but you can integrate with any Autodesk products, as well as other industry tools.

## Installation

### Azure Cosmos DB Emulator

1. Install the latest version of [Azure Cosmos DB Emulator](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator?tabs=ssl-netstd21#install-the-emulator)
2. [Start the Azure Cosmos DB Emulator](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator?tabs=ssl-netstd21#run-on-windows)

### Revit Addin

1. Install [Revit 2022](https://www.autodesk.com/products/revit/free-trial)
2. Open the `VertexFlow.addin` file and replace the `Assembly` section with your output directory
3. Open the `VertexFlow.RevitAddin.csproj` file and replace the `DestinationFolder` in the target `AfterBuild` section

Once you build the solution and launch Revit, you should find the `Vertex Flow` pannel in the `Add-Ins` tab.

<details><summary>Show Screenshot</summary>
<br />
    
![RevitVertexFlowAddinPanel](https://user-images.githubusercontent.com/28132516/136948224-1506cb61-52c6-4b51-b10b-770a36fb741b.png)

</details>

> **Note:** If you don't want to install Revit, just unload the `src/VertexFlow.RevitAddin` project from the solution.

### Unity Plugin

1. Create `Assembly Definition` in the `Assets/Plugins/VertexFlow` folder with `VertexFlow` name
2. Copy following libraries to the `Assets/Plugins/VertexFlow/libs` directory
    - VertexFlow.Core.dll
    - VertexFlow.Contracts.dll
    - VertexFlow.SDK.dll
    - VertexFlow.SDK.Extensions.dll
    - VertexFlow.SDK.Listeners.dll
3. Create `signalr.ps1` file in the `Assets/Plugins/VertexFlow/libs` folder
    <details><summary>signalr.ps1</summary>
    <br />

    ```powershell
    $srcVersion = "3.1.20"
    $stjVersion = "4.7.2"
    $target = "netstandard2.0"

    $outDir = ".\temp"
    $pluginDir = ".\"

    nuget install Microsoft.AspNetCore.SignalR.Client -Version $srcVersion -OutputDirectory $outDir
    nuget install System.Text.Json -Version $stjVersion -OutputDirectory $outDir

    $packages = Get-ChildItem -Path $outDir
    foreach ($p in $packages) {
        $dll = Get-ChildItem -Path "$($p.FullName)\lib\$($target)\*.dll"
        if (!($dll -eq $null)) {
            $d = $dll[0]
            if (!(Test-Path "$($pluginDir)\$($d.Name)")) {
                Move-Item -Path $d.FullName -Destination $pluginDir
            }
        }
    }

    Remove-Item $outDir -Recurse
    ```

    </details>
4. Check if [NuGet CLI](https://docs.microsoft.com/en-us/nuget/reference/nuget-exe-cli-reference) is installed locally
5. Execute the following command in PowerShell from the `Assets/Plugins/VertexFlow/libs` directory to import the target .dll files
    ```powershell
    ./signalr.ps1
    ```

<details><summary>Additional Links</summary>
<br />
    
- [Unity WebGL SignalR](https://github.com/evanlindsey/Unity-WebGL-SignalR)
- [Communicate with SignalR from Unity](https://dev.to/masanori_msl/unity-communicate-with-signalr-from-unity-3739)
    
</details>

## Examples

> In the `samples/VertexFlow.SDK.Sample` project you will find full example.

First of all, create a class to store mesh data.

```csharp
using VertexFlow.Contracts.Models;

public struct CustomVector3
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
}

public class CustomMesh : MeshData<CustomVector3>
{
}
```
> You can use default `Vector3` from `VertexFlow.Core`

### Send Mesh Data

`IMeshFlow<TMeshData>` provides methods for adding `SendAsync` or replacing `UpdateAsync` existing meshes in a database.

```csharp
static async Task Main()
{
    using var vertexFlow = new VertexFlow("https://localhost:5001");

    var meshFlow = vertexFlow.CreateMeshFlow<CustomMesh>();
    
    var meshData = GetMeshData();
    
    // Adds a mesh data to the database.
    // Note: Will fail if there already is a mesh data with the same id.
    await meshFlow.SendAsync(meshData);
    
    // Updates a mesh data in the database.
    // Note: Will add or replace any mesh data with the specified id.
    await meshFlow.UpdateAsync(meshData.Id, meshData);
}

private static CustomMesh GetMeshData()
{
    ...
}
```

### Get Mesh Data

`IMeshStore<TMeshData>` provides methods for reading `GetAsync`, `GetAllAsync`, or deleting `DeleteAsync` existing meshes from a database.

```csharp
static async Task Main()
{
    using var vertexFlow = new VertexFlow("https://localhost:5001");

    var meshStore = vertexFlow.CreateMeshStore<CustomMesh>();

    foreach (var mesh in await meshStore.GetAllAsync())
    {
        Console.WriteLine($"Mesh '{mesh.Id}' downloaded.");
    }
}
```

### Listen Mesh Data

`IMeshFlowListener` invokes `MeshCreated` or `MeshUpdated` in response to mesh data changes in a database.

```csharp
using VertexFlow.SDK.Listeners;

static async Task Main()
{
    using var vertexFlow = new VertexFlow("https://localhost:5001");
            
    using var meshFlowListener = vertexFlow.CreateMeshFlowListener();

    // Occurs when new mesh data has been added to the database.
    meshFlowListener.MeshCreated += (sender, meshId) =>
    {
        Console.WriteLine($"Mesh '{meshId}' created.");
    };

    // Occurs when new mesh data has been updated in the database.
    meshFlowListener.MeshUpdated += (sender, meshId) =>
    {
        Console.WriteLine($"Mesh '{meshId}' updated.");
    };

    // Starts listening for changes in a database.
    await meshFlowListener.StartAsync();
    
    ...
    
    // Stops listening for changes in a database.
    await meshFlowListener.StopAsync();
}
```

Use `VertexFlow.SDK.Extensions` to automate the process of loading mesh data on adding `OnMeshCreated` or updating `OnMeshUpdated` a database.

```csharp
using System.Net.Http;
using VertexFlow.SDK.Listeners;
using VertexFlow.SDK.Extensions;

static async Task Main()
{
    using var vertexFlow = new VertexFlow("https://localhost:5001");
            
    var meshStore = vertexFlow.CreateMeshStore<CustomMesh>();

    using var meshFlowListener = await vertexFlow
        .CreateMeshFlowListener()
        .WithStore(meshStore)
        .OnMeshCreated(mesh => Console.WriteLine($"Mesh '{mesh.Id}' created."))
        .OnMeshUpdated(mesh => Console.WriteLine($"Mesh '{mesh.Id}' updated."))
        .ContinueOnCapturedContext(false)
        .StartAsync(exception => throw new HttpRequestException(exception.Message));

    ...

    await meshFlowListener.StopAsync();
}
```
> **Note:** `MeshCreated` & `MeshUpdated` will provide only the id of the mesh as a parameter, while `OnMeshCreated` & `OnMeshUpdated` will provide the full mesh data.

### Custom Json Serializer

You can control how the mesh data is encoded into JSON.

> ...

## How To Use

### Export Geometry From Revit

> In the `src/VertexFlow.RevitAddin` project you will find full example.

1. Create a class to store mesh data
    - [RevitMesh](https://github.com/ChebanovDD/VertexFlow/blob/develop/src/VertexFlow.RevitAddin/Exporter/Models/RevitMesh.cs)
2. Extract `Mesh` from an `Element`
    - [MeshExtractor](https://github.com/ChebanovDD/VertexFlow/blob/develop/src/VertexFlow.RevitAddin/Exporter/MeshExtractor.cs)
3. Construct `Mesh Data` from the extracted `Mesh`
    - [MeshDataConstructor](https://github.com/ChebanovDD/VertexFlow/blob/develop/src/VertexFlow.RevitAddin/Exporter/MeshDataConstructor.cs)
4. Send the `Mesh Data`
    - [GeometryExporter](https://github.com/ChebanovDD/VertexFlow/blob/develop/src/VertexFlow.RevitAddin/Exporter/GeometryExporter.cs)

> **Note:** `MeshDataConstructor` mirrors geometry along the `X` axis due to the `Unity` coordinate system. This approach avoids any transformation on the `Unity` side. But if you want to use this geometry in different 3D engines at the same time, it usually takes a trade-off to decide where to manually mirror it back.

### Import Mesh To Unity

Once you've [configured](#unity-plugin) your unity project:
1. Create a class to store mesh data
    <details><summary>UnityMesh</summary>
    <br />
    
    ```csharp
    using UnityEngine;
    using VertexFlow.Contracts.Models;

    public class UnityMesh : MeshData<Vector3>
    {
    }
    ```
    
    </details>
2. Implement the `MeshCreator` class
    <details><summary>MeshCreator</summary>
    <br />
    
    ```csharp
    using UnityEngine;

    public class MeshCreator
    {
        private readonly Material _meshMaterial;
        private readonly Transform _meshContainer;

        public MeshCreator(Transform meshContainer, Material meshMaterial)
        {
            _meshMaterial = meshMaterial;
            _meshContainer = meshContainer;
        
            // Rotate due to Revit coordinate system.
            _meshContainer.rotation = Quaternion.Euler(-90, 0, 0);
        }

        public MeshFilter CreateMesh(UnityMesh meshData)
        {
            var gameObj = new GameObject(meshData.Id);

            // Sets mesh data to the game object.
            var meshFilter = gameObj.AddComponent<MeshFilter>();
            RebuildMesh(meshFilter.mesh, meshData);

            // Sets default material.
            gameObj.AddComponent<MeshRenderer>().sharedMaterial = _meshMaterial;

            // Sets parent to the game object.
            gameObj.transform.SetParent(_meshContainer, false);

            return meshFilter;
        }

        public void RebuildMesh(Mesh mesh, UnityMesh meshData)
        {
            mesh.Clear();
            mesh.vertices = meshData.Vertices;
            mesh.triangles = meshData.Triangles;

            if (meshData.Normals.Length == meshData.Vertices.Length)
            {
                mesh.normals = meshData.Normals;
            }
            else
            {
                mesh.RecalculateNormals();
            }

            mesh.Optimize();
        }
    }
    ```
    
    </details>
3. Implement the `UnityMeshProvider` class
    <details><summary>UnityMeshProvider</summary>
    <br />
    
    ```csharp
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine;
    using VertexFlow.SDK.Extensions;
    using VertexFlow.SDK.Interfaces;
    using VertexFlow.SDK.Listeners;
    using VertexFlow.SDK.Listeners.Interfaces;

    public class UnityMeshProvider : IDisposable
    {
        private readonly MeshCreator _meshCreator;
        private readonly Dictionary<string, MeshFilter> _meshes;

        private readonly IMeshStore<UnityMesh> _meshStore;
        private readonly IMeshFlowListener _meshFlowListener;
        private readonly VertexFlow.SDK.VertexFlow _vertexFlow;

        public UnityMeshProvider(Transform meshContainer, Material defaultMaterial)
        {
            _meshes = new Dictionary<string, MeshFilter>();
            _meshCreator = new MeshCreator(meshContainer, defaultMaterial);

            _vertexFlow = new VertexFlow.SDK.VertexFlow("https://localhost:5001");
            _meshStore = _vertexFlow.CreateMeshStore<UnityMesh>();
            _meshFlowListener = _vertexFlow
                .CreateMeshFlowListener()
                .WithStore(_meshStore)
                .OnMeshCreated(CreateMesh)
                .OnMeshUpdated(RebuildMesh);
        }

        public async Task StartMeshFlowListenerAsync()
        {
            await _meshFlowListener.StartAsync();
        }

        public async Task LoadAllMeshesAsync()
        {
            foreach (var meshData in await _meshStore.GetAllAsync())
            {
                CreateMesh(meshData);
            }
        }

        public async Task StopMeshFlowListenerAsync()
        {
            await _meshFlowListener.StopAsync();
        }

        public void Dispose()
        {
            _meshFlowListener?.Dispose();
            _vertexFlow?.Dispose();
        }

        private void CreateMesh(UnityMesh meshData)
        {
            _meshes.Add(meshData.Id, _meshCreator.CreateMesh(meshData));
        }

        private void RebuildMesh(UnityMesh meshData)
        {
            _meshCreator.RebuildMesh(_meshes[meshData.Id].mesh, meshData);
        }
    }
    ```
    
    </details>
4. Use the `UnityMeshProvider` class as following
    <details><summary>App</summary>
    <br />
    
    ```csharp
    using Extensions;
    using UnityEngine;

    public class App : MonoBehaviour
    {
        [SerializeField] private Material _meshMaterial;
        [SerializeField] private Transform _meshContainer;

        private UnityMeshProvider _unityMeshProvider;

        private void Awake()
        {
            _unityMeshProvider = new UnityMeshProvider(_meshContainer, _meshMaterial);
        }

        private void OnEnable()
        {
            _unityMeshProvider.StartMeshFlowListenerAsync().Forget();
        }

        private void Start()
        {
            _unityMeshProvider.LoadAllMeshesAsync().Forget();
        }

        private void OnDisable()
        {
            _unityMeshProvider.StopMeshFlowListenerAsync().Forget();
        }

        private void OnDestroy()
        {
            _unityMeshProvider.Dispose();
        }
    }
    ```
    
    </details>
        
### Import Mesh To Unigine

> ...

## Optimizations

> In the `benchmarks/VertexFlow.SDK.Benchmark/JsonSerializers` directory you will find two custom json serializers.

You can optimize performance and memory usage by writing a [custom json serializer](#custom-json-serializer).

### Benchmarks

> In the `benchmarks/VertexFlow.SDK.Benchmark` project you will find all benchmarks.

The benchmarks were run on the [dataset](https://drive.google.com/file/d/1HbUXdPlHLjy1aB7gvVrgopWeLg9Ebi4U/view?usp=sharing) with realistic mesh data. The tests compare the `JsonSerializer` in the `Newtonsoft.Json` namespace (used by default) with two custom serializers based on `JsonSerializer` in the `System.Text.Json` namespace.

<details><summary>Environment</summary>
<br />
<pre>
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19041.1165 (2004/May2020Update/20H1)
Intel Core i7-8700 CPU 3.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=5.0.301
  [Host]     : .NET 5.0.7 (5.0.721.25508), X64 RyuJIT
  DefaultJob : .NET 5.0.7 (5.0.721.25508), X64 RyuJIT
</pre>
</details>

#### Get All Async

<pre>
|                          Method |     Mean |    Error |   StdDev | Ratio |     Gen 0 |     Gen 1 |     Gen 2 | Allocated |
| ------------------------------- |---------:|---------:|---------:|------:|----------:|----------:|----------:|----------:|
|               Newtonsoft_Stream | 150.1 ms |  2.88 ms |  2.41 ms |  1.00 | 3000.0000 | 1000.0000 |         - |     24 MB |
|           SystemTextJson_Stream | 114.2 ms |  2.02 ms |  1.89 ms |  0.76 |  400.0000 |  200.0000 |         - |      4 MB |
| SystemTextJson_RecyclableStream | 113.9 ms |  1.61 ms |  1.51 ms |  0.76 |  400.0000 |  200.0000 |         - |      4 MB |
</pre>

#### Send All Async

<pre>
|                          Method |     Mean |    Error |   StdDev | Ratio |     Gen 0 |     Gen 1 |     Gen 2 | Allocated |
|-------------------------------- |---------:|---------:|---------:|------:|----------:|----------:|----------:|----------:|
|               Newtonsoft_Stream | 933.5 ms |  7.35 ms |  6.88 ms |  1.00 | 2000.0000 | 1000.0000 | 1000.0000 |     17 MB |
|           SystemTextJson_Stream | 934.8 ms | 11.53 ms | 10.22 ms |  1.00 | 1000.0000 | 1000.0000 | 1000.0000 |      7 MB |
| SystemTextJson_RecyclableStream | 931.6 ms |  6.41 ms |  5.68 ms |  1.00 |         - |         - |         - |      1 MB |
</pre>

> **Note:** Make sure your database contains data before running the `VertexFlow.SDK.Benchmark` project.

## License

Usage is provided under the [MIT License](LICENSE).
