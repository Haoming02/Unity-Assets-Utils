import os.path
from os import listdir

from PIL import Image, UnidentifiedImageError


def _listfiles(path: str) -> list[str]:
    files = []

    for obj in (os.path.join(path, obj) for obj in listdir(path)):
        if os.path.isdir(obj):
            files.extend(_listfiles(obj))
        else:
            files.append(obj)

    return files


def process(path: str):
    if os.path.isdir(path):
        files = _listfiles(path)
    else:
        files = [path]

    for file in files:
        try:
            img: Image.Image = Image.open(file)
            img.convert("RGB").save(path)
        except UnidentifiedImageError:
            continue


if __name__ == "__main__":
    path = str(input("Path to Assets: "))
    process(path.strip().strip('"').strip())
