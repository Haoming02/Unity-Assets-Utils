<h1 align="center">Unity Assets Utils</h1>
<p align="right">A series of utilities that assist in making datamining Unity assets faster and simpler.</p>
<p align="right"><b>v3</b></p>

## Intro
This repo provides some tools that can trivialize some steps during the datamine process.

The tools were originally written in **Python**; the new version is now rewritten in **C#**.
The two versions function more or less the same, refer to the respective page for the specific details.

<details>
<summary>Benchmark</summary>

Running **ByteTrimmer** on a ~9 GB folder with ~18k files:
  - **C#** version took ~62.5s
  - **Python** version took ~66.6s
</details>

## Usage
You can download an executable built for x64 Windows from [Release](https://github.com/Haoming02/Unity-Assets-Utils/releases) to use it directly; 
alternatively, clone the repo and use the Python version instead.

## Implementations
- [C#](src/)
- [Python](python/)
