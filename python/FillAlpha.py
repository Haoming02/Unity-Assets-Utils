import cv2
import os

def process(path:str):
	if os.path.isfile(path):
		rgb = cv2.imread(path, cv2.IMREAD_COLOR)
		cv2.imwrite(path, rgb)
	else:
		for FILE in os.listdir(path):
			if '.py' in FILE or not os.path.isfile(os.path.join(path, FILE)):
				continue

			rgb = cv2.imread(f'{path}/{FILE}', cv2.IMREAD_COLOR)
			cv2.imwrite(f'{path}/{FILE}', rgb)

def main():
    path = str(input('Path to Assets: '))
    process(path)

if __name__ == '__main__':
    main()
