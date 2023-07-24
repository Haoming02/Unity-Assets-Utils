import shutil
import os

currentPath = str(input('Path to Assets: '))

for assetType in ["audio", "mesh", "texture", "anim", "shader", "misc"]:
    if not os.path.exists(os.path.join(currentPath, assetType)):
        os.makedirs(os.path.join(currentPath, assetType))

for FILE in os.listdir(currentPath):
    if '.py' in FILE or not os.path.isfile(os.path.join(currentPath, FILE)):
        continue

    with open(os.path.join(currentPath, FILE), 'rb') as in_file:
        asset = in_file.read()

    if not asset[0:7] == b'UnityFS':
        continue

    if b'AudioClip' in asset:
        shutil.move(os.path.join(currentPath, FILE), os.path.join(os.path.join(currentPath, 'audio'), FILE))
    elif b'SkinnedMesh' in asset or b'Mesh' in asset:
        shutil.move(os.path.join(currentPath, FILE), os.path.join(os.path.join(currentPath, 'mesh'), FILE))
    elif b'texture' in asset or b'Texture' in asset or b'sprite' in asset or b'Sprite' in asset:
        shutil.move(os.path.join(currentPath, FILE), os.path.join(os.path.join(currentPath, 'texture'), FILE))
    elif b'Keyframe' in asset:
        shutil.move(os.path.join(currentPath, FILE), os.path.join(os.path.join(currentPath, 'anim'), FILE))
    elif b'Shader' in asset:
        shutil.move(os.path.join(currentPath, FILE), os.path.join(os.path.join(currentPath, 'shader'), FILE))
    else:
        shutil.move(os.path.join(currentPath, FILE), os.path.join(os.path.join(currentPath, 'misc'), FILE))
