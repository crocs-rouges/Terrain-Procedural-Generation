# 🏔️ Terrain Procedural Generation

[![Unity](https://img.shields.io/badge/Unity-6000.3.21f1-blue.svg?logo=unity)](https://unity.com/)
[![Render Pipeline](https://img.shields.io/badge/Pipeline-URP-informational.svg)](https://unity.com/srp/universal-render-pipeline)
[![Language](https://img.shields.io/badge/C%23-12.0-239120.svg?logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![School](https://img.shields.io/badge/ISART%20Digital-GDP2-red.svg)](https://www.isart.com/)

A modular, real-time procedural terrain generation tool built in **Unity 6 (URP)** with C#. This project implements multi-octave fractal Perlin noise, biome color mapping, custom animation curve height redistribution, and procedural 3D mesh synthesis with real-time Editor updates.

Developed by **Romain Chevalier** as part of the **ISART Digital** GDP2 (Game Design & Programming) curriculum.

---

## 📸 Preview

![Terrain Procedural Generation Preview](Docs/Capture%20d'écran%202026-10-16%20135819.png)

---

## ✨ Features

- **Fractal Multi-Octave Perlin Noise**:
  - Configurable frequency and detail via **Octaves**, **Persistence**, and **Lacunarity**.
  - Centered noise sampling with customizable **Zoom (Scale)**, **Seed**, and coordinate **Offsets**.
  - Min/Max elevation tracking with dynamic $[0, 1]$ normalization (`Mathf.InverseLerp`).
  - Fallback option for raw single-layer Perlin noise (`isSimpleNoise`).

- **Biome Color & Texture Baking**:
  - Layered height regions (`TerrainType`) with arbitrary elevation thresholds (`minHeight`, `maxHeight`) and custom biome colors (water, sand, grass, rock, snow, etc.).
  - Greyscale heightmap or colored biome texture generation directly onto `Texture2D`.
  - Filter mode toggle (`isBlurr`): crisp point-filtering for sharp biomes or smooth linear filtering.

- **Procedural 3D Mesh Generation**:
  - Converts heightmap matrices into indexed 3D quad meshes with proper vertex indices, UV coordinates, and triangle winding.
  - Symmetrical world-space positioning centered at $(0, 0, 0)$.
  - Non-linear elevation shaping via `AnimationCurve` to create flat water bodies, rolling hills, or sheer cliffs.
  - Automatic normal recalculation (`RecalculateNormals`) for dynamic lighting and shadows.
  - Dynamic `MeshCollider` generation matching the procedural surface.

- **Interactive Editor Tooling**:
  - Custom Unity Inspector (`MapGeneratorEditor`) executing in-editor generation without entering Play mode.
  - Live auto-update on parameter change (`DrawDefaultInspector()`).
  - Dedicated "Generate" button for manual regeneration.

---

## 🧱 Architecture & Scripts

The project resides in the `Com.IsartDigital` namespace and is split into modular, single-responsibility components:

```
Assets/
├── Scripts/
│   ├── Noise.cs                 # Mathematical Perlin noise sampling & octave accumulation
│   ├── TextureGenerator.cs      # Heightmap & color array to Texture2D conversion
│   ├── MeshGenerator.cs         # MeshData construction (vertices, UVs, triangles, normals)
│   ├── MapDislplay.cs           # Target renderer adapter (Renderer, MeshFilter, MeshCollider)
│   ├── MapGenerator.cs          # Central orchestrator connecting noise, biomes, and mesh
│   └── Editor/
│       └── MapGeneratorEditor.cs# Custom Inspector with live-update & generate button
├── Material/                    # Material references (MapDisplayMat)
├── Scenes/                      # SampleScene with lighting, camera & terrain setup
└── Settings/                    # Universal Render Pipeline (URP) graphic profiles
```

### Detailed Script Breakdown

| Script | Type | Responsibility |
| :--- | :--- | :--- |
| `Noise.cs` | `static class` | Evaluates multi-layered Perlin noise with pseudo-random seed offsets, octaves, amplitude persistence, and frequency lacunarity. |
| `TextureGenerator.cs` | `static class` | Creates `Texture2D` instances from either a 2D float heightmap or a 1D biome color array. |
| `MeshGenerator.cs` | `static class` | Builds a `MeshData` container (vertex array, UVs, triangle buffer) scaled to height and sculpted via `AnimationCurve`. |
| `MapDislplay.cs` | `MonoBehaviour` | Applies generated textures or meshes onto scene components (`MeshFilter`, `MeshRenderer`, `MeshCollider`, `Renderer`). |
| `MapGenerator.cs` | `MonoBehaviour` | Core manager holding noise parameters, biome thresholds, and trigger logic for generation. |
| `MapGeneratorEditor.cs` | `CustomEditor` | In-editor GUI wrapper triggering `GenerateMap()` automatically on inspector changes or via button. |

---

## 📐 Mathematical Model

### 1. Fractal Brownian Motion (fBm) Perlin Noise
The height $H(x, y)$ at integer coordinates $(x, y)$ is calculated by summing successive noise layers (octaves):

$$H(x, y) = \sum_{i=0}^{\text{octaves}-1} A_i \cdot \mathcal{N}\left( \frac{x - \frac{W}{2}}{\text{scale}} \cdot F_i + \text{offset}_{x, i}, \; \frac{y - \frac{H}{2}}{\text{scale}} \cdot F_i + \text{offset}_{y, i} \right)$$

Where:
- $A_0 = 1$ and $A_i = A_{i-1} \times \text{persistence}$ (amplitude / detail contribution).
- $F_0 = 1$ and $F_i = F_{i-1} \times \text{lacunarity}$ (frequency / detail granularity).
- $\mathcal{N}(u, v) \in [-1, 1]$ is the centered Perlin noise value.

### 2. Normalization
Because multi-octave summation can exceed standard bounds, all values are clamped and remapped to $[0, 1]$:

$$\tilde{H}(x, y) = \frac{H(x, y) - H_{\min}}{H_{\max} - H_{\min}}$$

### 3. Non-Linear Elevation Shaping
Vertex elevation $Y$ is transformed using an `AnimationCurve` $C(t)$ to control topography:

$$Y(x, y) = C(\tilde{H}(x, y)) \times \text{heightMultiply}$$

---

## 🎛️ Inspector Parameters

### Generic Noise Parameters
- **`Map Width` / `Map Height`** (1 - 500): Dimensions of the noise grid in units/vertices.
- **`Noise Scale`** (1 - 150): Zoom level of the noise map.
- **`Octave`** (1 - 8): Number of noise layers accumulated.
- **`Persistence`** (0 - 1): Determines how fast amplitude decreases per octave (higher = rougher surface).
- **`Lacunarity`** (1 - 10): Determines how fast frequency increases per octave (higher = finer features).
- **`Is Blurr`**: Toggles point vs. bilinear texture filtering.
- **`Is Simple Noise`**: Toggles single-layer raw Perlin noise mode.

### Pseudo-Random
- **`Seed`**: Integer seed generating deterministic pseudo-random offsets for each octave.
- **`Offset`** ($X, Y$): Manual coordinate pan offset.

### Mesh & Display Modes
- **`Draw Mode`**:
  - `NoiseMap`: Displays the raw greyscale elevation map.
  - `ColorMap`: Displays the 2D colored biome map on a flat quad.
  - `MeshMap`: Deforms a full 3D terrain mesh using height and applies the biome texture.
- **`Height Multiply`** (0 - 100): Vertical amplitude scaling factor.
- **`Use Height Curve`**: Enables non-linear elevation remapping.
- **`Height Curve`**: `AnimationCurve` evaluating height remapping (e.g., flat water zones, steep mountain slopes).

### Regions (`TerrainType[]`)
Define altitude ranges and associated biome colors:
- **`Name`**: Region identifier (e.g. *Water Deep*, *Water Shallow*, *Sand*, *Grass*, *Rock*, *Snow*).
- **`Min Height`** / **`Max Height`**: Elevation range $[0, 1]$.
- **`Color`**: Biome tint applied to the generated texture.

---

## 🚀 Getting Started

### Prerequisites
- **Unity 6** (recommended version: `6000.3.21f1` or later).
- **Universal Render Pipeline (URP)** package installed and configured.

### Installation & Run
1. Clone the repository:
   ```bash
   git clone https://github.com/crocs-rouges/Terrain-Procedural-Generation.git
   ```
2. Open the project folder in **Unity Hub** using **Unity 6000.3.21f1**.
3. In the Project window, navigate to `Assets/Scenes/SampleScene.unity` and open it.
4. Select the `MapGenerator` GameObject in the Scene Hierarchy.
5. In the Inspector:
   - Select your desired **Draw Mode** (`MeshMap` recommended).
   - Adjust noise parameters (`Scale`, `Octaves`, `Persistence`, `Lacunarity`).
   - The scene updates in real time! Click **Generate** if needed.

---

## 📜 License & Credits

- **Author**: [Romain Chevalier](https://github.com/crocs-rouges)
- **Institution**: ISART Digital (GDP2)
- **Engine**: Unity 6 / URP
