# Unity Assets Utils
A series of utilities that makes mining and modding Unity assets easier~

## Intro
Previously, the utilities were written in **Python**, which was rather slow.
The new version is now rewritten in **C#**.

You can download an executable built for x64 Windows from [Release](https://github.com/Haoming02/Unity-Assets-Utils/releases); 
or clone the project and build it yourself; or just use the old Python version.

When launching from the console, you can add `silent` to hide certain logs, or add `timer` to print out how long each operation took.

#### Benchmark
- Ran `ByteTrimmer` on a ~2GB folder with ~2000 files
  - Old **Python** version took ~8 sec
  - New **C#** version took ~1.6 sec
  - Effectively **~5 times** faster

## Modes
For **C#**, you can choose the mode after entering a working directory; For **Python**, just use the respective script.

### Flatten Folder
- Move all files in sub-folders of a parent folder to the parent folder
- The sub-folders are deleted afterwards
- This has no depth limits. **Verify the path before proceeding!**

### Byte Trimmer
- Remove the leading bytes of all files in a folder, so that `AssetStudio` can read them
- Works for both *"normal Header with leading bytes"* and *"normal Header with a dummy Header"* variants

### Separator
- Separate all files in a folder into their own categories
  - Included categories: `audio`, `mesh`, `texture`, `anim`, `shader`, `misc`
- Does not guarantee to work. It depends on how the game handles the assets
- If no Header is found, then the file is not moved

### Dedupe
- Add `.old` to the filename for all files in a new folder that already existed in another folder

### Find Filter
- Print out the filename and its filesize for all files of which content contains a specified filter word