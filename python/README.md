<h1 align="center">Python Implementation</h1>
<p align="center">A series of Python scripts that make datamining Unity assets easier~</p>

> Every script will process all files inside the given folder

### ByteTrimmer
- Remove the leading dummy bytes, so that `AssetStudio` can read the file
- Works for both 1 header and 2 headers variants
- Files are edited in-place; If no Header is found, the file is skipped

### Check
- Print out the name and size of all files whose content contains a specified filter

### Dedupe
- Remove duplicated files in the new target folder, if the same filename already exist in an old cache folder

### FillAlpha
- Remove the alpha channel *(transparency)* of images
- Requires `Pillow` package

### FlattenFolder
- Move all files under sub-folders in the parent folder to the parent folder
- The sub-folders are then deleted afterwards

> [!Important]
> This operation has no depth limit. Verify the path before proceeding!

### InjectVersion
- Inject the specified version into the assets, preventing the "version has been stripped" error
  - **format:** `2020.3.42f1`
- Requires `UnityPy` package
