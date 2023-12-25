import os

def process(PATH:str, FILTER:str, RECUR:bool = True):
    for FILE in os.listdir(PATH):
        if os.path.isdir(os.path.join(PATH, FILE)):
            if RECUR is True:
                process(os.path.join(PATH, FILE), FILTER, True)
            else:
                continue

        else:
            with open(os.path.join(PATH, FILE), 'rb') as in_file:
                asset = in_file.read()

                if FILTER.encode('utf-8') in asset:
                    file_stats = os.stat(f'{PATH}/{FILE}')
                    print(f'{FILE}: {file_stats.st_size / 1024:.2f} KB')

def main():
    PATH = str(input('Path to Assets: '))
    FILTER = str(input('Enter Filter: '))

    print('')
    process(PATH.strip('"').strip(), FILTER)

if __name__ == '__main__':
    main()
