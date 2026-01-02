<h1 align="center">Unity Assets Utils</h1>
<p align="center">
A series of blazingly fast utilities that trivializes datamining Unity assets
<br>
<a href="https://rust-lang.org/"><img src="https://img.shields.io/badge/rust-010101?logo=rust" /></a> 
</p>

## How to Use
- Download the built `.exe` from [Release](https://github.com/Haoming02/Unity-Assets-Utils/releases)

## Features

- **Byte Trimmer**
    - Trim the files to the `UnityFS` header and remove the leading dummy bytes
    - Support both variants with one header and two headers
    - Edit the files in-place; File is untouched if no header is found

- **Flatten Folder**
    - Move all files under subdirectories up to the project folder
    - The subdirectories are deleted afterwards

> [!Caution]
> Be sure to verify the project path, as this operation has no depth limit and no undo

- **Filter by Content**
    - List out the filename of all files, of which the content contains the specified filter

- **Filter by Filename**
    - List out the file and folder names, which contain the specified filter

<hr>

- **See Also**
    - [AssetStudio](https://github.com/aelurum/AssetStudio)
    - [UnityPy](https://github.com/K0lb3/UnityPy)
