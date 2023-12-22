<h1 align="center">C# Implementation</h1>

A series of .NET scripts that make mining Unity assets easier~

### Modes
- **Normal:** Default behavior. Works for files under a folder.
- **Alt:** Created specifically for `FlattenFolder` to work for assets in the following structure:
    ```
    XXXXXX
     |- YYYY
     |   | __data
     |   | __info
     |
     |- ZZZZ
         | __data
         | __info
    ```
    - Has no effect on other functions, as such you should run `FlattenFolder` first.

### Args
If you launch the program using console, you can optionally add `silent` to hide certain logs; or add `timer` to print out the time each operation took; or add `alt` to directly launch in Alt mode.

<h2 align="center">Functions</h2>

> All the functions below will work on every file inside the specified folder.

### Byte Trimmer
- Remove the leading dummy bytes, so that `AssetStudio` can read the file.
- Works for both 1 header and 2 headers variants.
- Files are edited in-place. If no Header is found, then the file is skipped.

### Dedupe
- Remove files in the target folder, if files with the same name already exist in an old cache folder.

### Find Filter
- Print out the filename and file size of all files that contain a specified filter.

### Flatten Folder
- Move all files inside the sub-folders of the target folder, to the target folder.
- The sub-folders are then deleted afterwards.
- **Warning!** This operation has no depth limit. **Verify the path before proceeding!**

### Separator
- ~~Attempt to~~ separate all files into their respective categories.
- Doesn't really work most of the time... It depends on how the game handles the assets.
- You can use `FlattenFolder` to reverse this operation...
