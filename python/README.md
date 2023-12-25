<h1 align="center">Python Implementation</h1>
<p align="center">A series of Python scripts that make datamining Unity assets easier~</p>

## Functions
> All the functions below will process every file inside the given folder

### ByteTrimmer
- Remove the leading dummy bytes, so that `AssetStudio` can read the file.
- Works for both 1 header and 2 headers variants.
- Files are edited in-place. If no Header is found, then the file is skipped.

### Dedupe
- Remove files in the new target folder, if files with the same name already exist in another old cache folder.

### FillAlpha
- Remove the alpha channel (transparency) of images.
- Requires `OpenCV` package

### Check
- Print out the filename and file size of all files that contain a specified filter.

### FlattenFolder
- Move all files inside the sub-folders of the target folder, to the target folder.
- The sub-folders are then deleted afterwards.
- **Warning! This operation has no depth limit. Verify the path before proceeding!**

### InjectVersion
- Inject the specified version back into the files, thus preventing the "version has been stripped" error.
  > format: `2020.3.42f1`
- Requires `UnityPy` package

<hr>

<sup>If nothing major happens, these scripts will <b>no longer</b> be maintained...</sup>
