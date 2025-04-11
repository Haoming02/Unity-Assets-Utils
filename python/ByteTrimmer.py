import os.path
from concurrent.futures import ThreadPoolExecutor
from os import listdir

HEADER = b"UnityFS"


def _trim(file: str):
    with open(file, "rb") as fs:
        data = fs.read()

    chunks = data.split(HEADER, 2)

    if len(chunks) == 1:
        return

    if len(chunks) == 2 and data.startswith(HEADER):
        return

    with open(file, "wb") as fs:
        fs.write(HEADER + chunks[-1])


def _listfiles(path: str) -> list[str]:
    files = []

    for obj in (os.path.join(path, obj) for obj in listdir(path)):
        if os.path.isdir(obj):
            files.extend(_listfiles(obj))
        else:
            files.append(obj)

    return files


def process(path: str):
    assert os.path.isdir(path)
    files = _listfiles(path)

    with ThreadPoolExecutor(max_workers=8) as executor:
        executor.map(_trim, files)


if __name__ == "__main__":
    path = str(input("Path to Assets: "))
    process(path.strip().strip('"').strip())
