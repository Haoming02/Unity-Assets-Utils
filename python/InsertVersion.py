import UnityPy
import os

try:
    major, minor, rev = UnityPy.__version__.split('.')
    assert(int(major) == 1)
    assert(int(minor) >= 10)
    assert(int(rev) >= 5)
except AssertionError:
    print('Old version of UnityPy detected. This function might not be supported...')

def verify_header(path:str):
    with open(path, 'rb') as FILE:
        header = FILE.read(7)

    return header == b'UnityFS'

def process(path:str, version:str):
    for FILE in os.listdir(path):
        if not os.path.isfile(os.path.join(path, FILE)):
            continue

        if not verify_header(os.path.join(path, FILE)):
            continue

        env = UnityPy.load(os.path.join(path, FILE))

        for obj in env.assets:
            obj.unity_version = version
            try:
                obj.save()
            except:
                pass

        with open(os.path.join(path, FILE), "wb") as f:
            f.write(env.file.save())

def main():
    path = str(input('Path to Assets: '))
    ver = str(input('Unity Version: '))
    process(path.strip('"').strip(), ver)

if __name__ == '__main__':
    main()
