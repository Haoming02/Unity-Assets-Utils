import os

def process(CHECK:str, CACHE:str):
	for FILE in os.listdir(CHECK):
		if os.path.exists(f'{CACHE}/{FILE}'):
			os.remove(f'{CHECK}/{FILE}')

def main():
	CHECK = str(input('Enter new Path: '))
	CACHE = str(input('Enter old Path: '))
	process(CHECK, CACHE)

if __name__ == '__main__':
    main()
