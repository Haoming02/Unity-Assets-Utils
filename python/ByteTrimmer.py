import os
THRESHOLD = 2048

def process(path:str):
    for FILE in os.listdir(path):
        if '.py' in FILE or not os.path.isfile(os.path.join(path, FILE)):
            continue

        with open(os.path.join(path, FILE), 'rb+') as BYTES:
            secondPass = False

            DATA = BYTES.read()

            limit = min(len(DATA), THRESHOLD)
            pos = 0

            for i in range(limit):
                BYTES.seek(i)
                byte = BYTES.read(7)

                if byte == b'UnityFS':
                    if not secondPass:
                        pos = BYTES.tell() - 7
                        secondPass = True
                    else:
                        pos = BYTES.tell() - 7
                        break

            if not pos == 0:
                BYTES.seek(0)
                BYTES.write(DATA[pos:])
                BYTES.truncate()

def main():
    path = str(input('Path to Assets: '))
    process(path)

if __name__ == '__main__':
    main()
