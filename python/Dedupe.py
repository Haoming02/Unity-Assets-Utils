import os

def process(CHECK:str, CACHE:str):
	for FILE in os.listdir(CHECK):
		if os.path.exists(os.path.join(CACHE, FILE)):
			os.remove(os.path.join(CHECK, FILE))

def main():
	CHECK = str(input('Enter new Path: '))
	CACHE = str(input('Enter old Path: '))
	process(CHECK.strip('"').strip(), CACHE.strip('"').strip())

if __name__ == '__main__':
    main()
