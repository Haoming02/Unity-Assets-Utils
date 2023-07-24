import os

def process(PATH:str, FILTER:str, RECUR:bool):
    for FILE in os.listdir(PATH):
        if os.path.isdir(PATH + '/' + FILE):
            if RECUR is True:
                process(PATH + '/' + FILE, FILTER, RECUR)
            else:
                continue
        else:
            with open(os.path.join(PATH, FILE), 'rb') as in_file:
                asset = in_file.read()

                if FILTER.encode('utf-8') in asset:
                    file_stats = os.stat(PATH + '/' + FILE)    
                    print(f'{FILE}: {file_stats.st_size / 1024:.2f}KB')

            try:
                with open(os.path.join(PATH, FILE), 'r') as in_file:
                    asset = in_file.read()

                    if FILTER in asset:
                        file_stats = os.stat(PATH + '/' + FILE)    
                        print(f'{FILE}: {file_stats.st_size / 1024:.2f}KB')
            except:
                pass

def main():
    PATH = str(input('Path to Assets: '))
    FILTER = str(input('Enter Filter: '))
    RECUR = bool(input('Recursive [0|1]: '))
    process(PATH, FILTER, RECUR)

if __name__ == '__main__':
    main()