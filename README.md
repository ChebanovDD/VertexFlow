# Vertex Flow

Cross-platform library that allows you to transfer BIM data directly into your application and maintain a live link between them.

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

> ### Azure Cosmos DB Emulator

> ### Revit Addin

> ### Unity Plugin

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

`IMeshFlow<TMeshData>` is used for adding `SendAsync` or replacing `UpdateAsync` existing meshes in a database.

```csharp
static async Task Main()
{
    using var vertexFlow = new VertexFlow("https://localhost:5001");

    var meshFlow = vertexFlow.CreateMeshFlow<CustomMesh>();
    
    var meshData = GetMeshData();
    
    // Adds a mesh data to the database.
    await meshFlow.SendAsync(meshData); // Note: Will fail if there already is a mesh data with the same id.
    
    // Updates a mesh data in the database.
    await meshFlow.UpdateAsync(meshData.Id, meshData); // Note: Will add or replace any mesh data with the specified id.
}

private static CustomMesh GetMeshData()
{
    ...
}
```

### Get Mesh Data

`IMeshStore<TMeshData>` is used for reading `GetAsync` `GetAllAsync` or deleting `DeleteAsync` existing meshes from a database.

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

`IMeshFlowListener` is used to listen for changes in a database.

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

Use `VertexFlow.SDK.Extensions` to set actions to monitor adding `OnMeshCreated` or updating `OnMeshUpdated` a mesh data in a database.

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

## How To Use

> ### Export Geometry From Revit

> ### Import Mesh To Unity

> ### Import Mesh To Unigine

## Optimizations

> ### Benchmarks

## License

Usage is provided under the [MIT License](https://github.com/ChebanovDD/VertexFlow/blob/main/LICENSE).
