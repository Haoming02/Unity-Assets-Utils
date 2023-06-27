import os

def clean(FOLDER):
	for FILE in os.listdir(FOLDER):
		if not os.path.isfile(FOLDER + '/' + FILE):
			clean(FOLDER + '/' + FILE)
		elif '.meta' in FILE:
			os.remove(FOLDER + '/' + FILE)

def main():
	PARENT_FOLDER = str(input('Path to Folder: '))

	FLAG = str(input('Type YES to remove .meta from [' + PARENT_FOLDER + ']: '))

	if not FLAG == 'YES':
		print('Process Terminated')
		return

	clean(PARENT_FOLDER)

if __name__ == '__main__':
    main()