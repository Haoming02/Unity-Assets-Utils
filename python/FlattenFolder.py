import shutil
import os

def process(PARENT_FOLDER:str, TARGET_FOLDER:str):
	for FILE in os.listdir(TARGET_FOLDER):
		if os.path.isfile(f'{TARGET_FOLDER}/{FILE}'):
			shutil.move(f'{TARGET_FOLDER}/{FILE}', f'{PARENT_FOLDER}/{FILE}')
		else:
			process(PARENT_FOLDER, f'{TARGET_FOLDER}/{FILE}')

	os.rmdir(TARGET_FOLDER)

def main():
	PARENT_FOLDER = str(input('Path to Folder: '))

	FLAG = str(input(f'Type YES to flatten folder "{PARENT_FOLDER}": '))

	if FLAG.strip() != 'YES':
		print('Process Terminated')
		return

	for FOLDER in os.listdir(PARENT_FOLDER):
		if not os.path.isfile(f'{PARENT_FOLDER}/{FOLDER}'):
			process(PARENT_FOLDER, f'{PARENT_FOLDER}/{FOLDER}')

if __name__ == '__main__':
    main()
