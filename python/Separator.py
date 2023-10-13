import shutil
import os

def process(path:str):
    for assetType in ["audio", "mesh", "texture", "anim", "shader", "misc"]:
        if not os.path.exists(os.path.join(path, assetType)):
            os.makedirs(os.path.join(path, assetType))

    for FILE in os.listdir(path):
        if '.py' in FILE or not os.path.isfile(os.path.join(path, FILE)):
            continue

        with open(os.path.join(path, FILE), 'rb') as in_file:
            asset = in_file.read()

        if not asset[0:7] == b'UnityFS':
            continue

        if b'AudioClip' in asset:
            shutil.move(os.path.join(path, FILE), os.path.join(os.path.join(path, 'audio'), FILE))
        elif b'Mesh' in asset or b'BlendShape' in asset:
            shutil.move(os.path.join(path, FILE), os.path.join(os.path.join(path, 'mesh'), FILE))
        elif b'texture' in asset or b'Texture' in asset or b'sprite' in asset or b'Sprite' in asset:
            shutil.move(os.path.join(path, FILE), os.path.join(os.path.join(path, 'texture'), FILE))
        elif b'Keyframe' in asset or b'.anim' in asset:
            shutil.move(os.path.join(path, FILE), os.path.join(os.path.join(path, 'anim'), FILE))
        elif b'Shader' in asset:
            shutil.move(os.path.join(path, FILE), os.path.join(os.path.join(path, 'shader'), FILE))
        else:
            shutil.move(os.path.join(path, FILE), os.path.join(os.path.join(path, 'misc'), FILE))

def main():
    path = str(input('Path to Assets: '))
    process(path)

if __name__ == '__main__':
    main()
