import os


def _flatten(parent: str, target: str):
    for obj in os.listdir(target):
        if os.path.isfile(os.path.join(target, obj)):
            os.rename(os.path.join(target, obj), os.path.join(parent, obj))
        else:
            _flatten(parent, os.path.join(target, obj))

    os.rmdir(target)


def process(path: str):
    assert os.path.isdir(path)

    confirm = str(input(f'Type YES to flatten folder "{path}": '))
    if confirm.strip() != "YES":
        print("Process Canceled...")
        return

    for obj in (os.path.join(path, obj) for obj in os.listdir(path)):
        if os.path.isdir(obj):
            _flatten(path, obj)


if __name__ == "__main__":
    path = str(input("Path to Folder: "))
    process(path.strip().strip('"').strip())
