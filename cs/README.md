<h1 align="center">C# Implementation</h1>
<p align="center">A series of .NET scripts that make datamining Unity assets easier~</p>

### Byte Trimmer
- Remove the leading dummy bytes, so that `AssetStudio` can read the file
- Works for both 1 header and 2 headers variants
- Files are edited in-place; If no Header is found, the file is skipped

### Dedupe
- Remove duplicated files in the new target folder, if the same filename already exist in an old cache folder

### Find Filter
- Print out the name and size of all files whose content contains a specified filter

### Find File
- Print out the files and folders whose name contains a specified filter

### Flatten Folder [*](#modes)

- Move all files under sub-folders in the parent folder to the parent folder
- The sub-folders are then deleted afterwards

> [!Important]
> This operation has no depth limit. Verify the path before proceeding!

### Separator
- Move all non-asset files inside a sub-folder

<hr>

### Mode
- If your assets are in the following folder structure, switch to the **Alt.** mode and run the **FlattenFolder** function first
    ```
    XXX
     |--  YYY
     |     |--  __data
     |     |--  __info
     |
     |--  ZZZ
     |     |--  __data
     |     |--  __info
    ```

<hr>

### Args
If you launch the program via commandline, you can optionally add:
- **silent** to hide certain logs
- **alt** to directly launch in `Alt.` mode
