import os

currentPath = str(input('Path to Assets: '))
FILTER = str(input('Enter Filter: '))

for FILE in os.listdir(currentPath):
    flag = False
    with open(os.path.join(currentPath, FILE), 'rb') as in_file:
        asset = in_file.read()

        if FILTER.encode('utf-8') in asset:
            flag = True
            file_stats = os.stat(currentPath + '/' + FILE)
            print(f'{FILE}: {file_stats.st_size / 1024:.2f}KB')
    
    if flag is not True:
        try:
            with open(os.path.join(currentPath, FILE), 'r') as in_file:
                asset = in_file.read()

                if FILTER in asset:
                    file_stats = os.stat(currentPath + '/' + FILE)    
                    print(f'{FILE}: {file_stats.st_size / 1024:.2f}KB')
        except:
            pass
