import os


def process(new_path: str, old_path: str):
    assert os.path.isdir(new_path) and os.path.isdir(old_path)
    for file in os.listdir(new_path):
        if os.path.isfile(os.path.join(old_path, file)):
            os.remove(os.path.join(new_path, file))


if __name__ == "__main__":
    new_path = str(input("New Target Folder: "))
    old_path = str(input("Old Cached Folder: "))
    process(new_path.strip().strip('"').strip(), old_path.strip().strip('"').strip())
