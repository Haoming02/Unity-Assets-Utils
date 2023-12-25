<h1 align="center">C# Implementation</h1>
<p align="center">A series of .NET scripts that make datamining Unity assets easier~</p>

### Modes
- **Normal:** Default behavior. Works for every file inside the given folder.
- **Alt:** Specifically works for assets in the following structure:
    ```
    XXXXXX
     |  YYYY
     |   | __data
     |   | __info
     |
     |  ZZZZ
         | __data
         | __info
    ```

> **Alt.** has no effect on most other functions. Therefore, you should run `FlattenFolder` first.

### Args
If you launch the program using console, you can optionally add:
- **silent** to hide certain logs
- **timer** to print out the time each operation took
- **alt** to directly launch in `Alt.` mode.

## Functions

### Byte Trimmer
- Remove the leading dummy bytes, so that `AssetStudio` can read the file.
- Works for both 1 header and 2 headers variants.
- Files are edited in-place. If no Header is found, then the file is skipped.

### Dedupe
- Remove files in the new target folder, if files with the same name already exist in another old cache folder.

### Find Filter
- Print out the filename and file size of all files that contain a specified filter.

### Find File
- Print out the filenames and folder names that contain a specified filter.

### Flatten Folder
- Move all files inside the sub-folders of the target folder, to the target folder.
- The sub-folders are then deleted afterwards.
- **Warning! This operation has no depth limit. Verify the path before proceeding!**

### Separator
- Move all non-asset files inside a sub-folder
