<h1 align="center">Deprecated</h1>
<h3 align="center">No Longer Maintained</h3>

## Python Implementation
A series of Python scripts that makes mining and modding Unity assets easier~

## FlattenFolder
- Move all files in sub-folders of a parent folder to the parent folder
- The sub-folders are deleted afterwards
- This has no depth limits. **Verify the path before proceeding!**
- **How to Use**
  1. `>python FlattenFolder.py`
  2. Enter the path to the folder containing all the folders
  3. Confirm the path then enter **YES**

## ByteTrimmer
- Remove the leading bytes of all files in a folder, so that `AssetStudio` can read them.
- Works for both normal Header with leading bytes, and normal Header with Dummy Header variants.
- **How to Use**
  1. `>python ByteTrimmer.py`
  2. Enter the path to the folder containing all the assets
  3. Files are edited in-place. If no Header is found, then the file is unchanged.

## Separator
- Separate all files in a folder into their own categories
  - Included categories: `audio`, `mesh`, `texture`, `anim`, `shader`, `misc`
- Does not guarantee to work, depending on how the game handles the assets.
- **How to Use**
  1. `>python Separator.py`
  2. Enter the path to the folder containing all the assets
  3. If no Header is found, then the file is not moved.

## Dedupe
- Remove files in a new folder if files with the same name already exist in a old folder
- **How to Use**
  1. `>python Dedupe.py`
  2. Enter the **new** folder
  3. Enter the **old** folder

## Check
- Print out all filenames of which content contains a specified filter word
- Also print out its file size
- **How to Use**
  1. `>python Check.py`
  2. Enter the path to the folder containing all the assets
  3. Enter a **filter**
