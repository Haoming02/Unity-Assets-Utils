import os.path
from os import listdir


def _listfiles(path: str) -> list[str]:
    files = []

    for obj in (os.path.join(path, obj) for obj in listdir(path)):
        if os.path.isdir(obj):
            files.extend(_listfiles(obj))
        else:
            files.append(obj)

    return files


def process(path: str, key: str):
    if os.path.isdir(path):
        files = _listfiles(path)
    else:
        files = [path]

    for file in files:
        with open(file, "rb") as f:
            asset = f.read()

        if key.encode("utf-8") in asset:
            file_stats = os.stat(file)
            print(f"[{file_stats.st_size / 1024:.2f} KB] {file}")


if __name__ == "__main__":
    path = str(input("Path to Assets: "))
    key = str(input("Enter Filter: "))

    print("")
    process(path.strip().strip('"').strip(), key)
