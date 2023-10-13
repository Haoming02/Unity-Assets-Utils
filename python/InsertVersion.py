import UnityPy
import os

def process(path:str, version:str):
    for FILE in os.listdir(path):
        if '.py' in FILE or not os.path.isfile(os.path.join(path, FILE)):
            continue

        env = UnityPy.load(f'{path}/{FILE}')

        for obj in env.assets:
            obj.unity_version = version
            obj.save()

        with open(f'{path}/{FILE}', "wb") as f:
            f.write(env.file.save())

def main():
    path = str(input('Path to Assets: '))
    ver = str(input('Unity Version: '))
    process(path, ver)

if __name__ == '__main__':
    main()
