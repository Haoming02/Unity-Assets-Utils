import os.path
from os import listdir

import UnityPy


def _verify_header(path: str) -> bool:
    with open(path, "rb") as file:
        header = file.read(7)

    return header == b"UnityFS"


def _listfiles(path: str) -> list[str]:
    files = []

    for obj in (os.path.join(path, obj) for obj in listdir(path)):
        if os.path.isdir(obj):
            files.extend(_listfiles(obj))
        else:
            files.append(obj)

    return files


def process(path: str, version: str):
    assert os.path.isdir(path)
    files = _listfiles(path)

    for file in files:
        if not _verify_header(file):
            continue

        env = UnityPy.load(file)

        for obj in env.assets:
            obj.unity_version = version
            try:
                obj.save()
            except Exception:
                pass

        with open(file, "wb") as f:
            f.write(env.file.save())


if __name__ == "__main__":
    path = str(input("Path to Assets: "))
    ver = str(input("Unity Version: "))
    process(path.strip().strip('"').strip(), ver)
