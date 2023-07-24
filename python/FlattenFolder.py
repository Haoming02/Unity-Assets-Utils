import shutil
import os

def flatten(PARENT_FOLDER, TARGET_FOLDER):
	for FILE in os.listdir(TARGET_FOLDER):
		if os.path.isfile(TARGET_FOLDER + '/' + FILE):
			shutil.move(TARGET_FOLDER + '/' + FILE, PARENT_FOLDER + '/' + FILE)
		else:
			flatten(PARENT_FOLDER, TARGET_FOLDER + '/' + FILE)

	os.rmdir(TARGET_FOLDER)

def main():
	PARENT_FOLDER = str(input('Path to Folder: '))

	FLAG = str(input('Type YES to flatten folder "' + PARENT_FOLDER + '": '))

	if not FLAG == 'YES':
		print('Process Terminated')
		return

	for FOLDER in os.listdir(PARENT_FOLDER):
		if not os.path.isfile(PARENT_FOLDER + '/' + FOLDER):
			flatten(PARENT_FOLDER, PARENT_FOLDER + '/' + FOLDER)

if __name__ == '__main__':
    main()
