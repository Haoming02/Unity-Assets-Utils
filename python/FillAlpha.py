import cv2
import os

def process(path:str):
	if os.path.isfile(path):
		try:
			rgb = cv2.imread(path, cv2.IMREAD_COLOR)
			cv2.imwrite(path, rgb)
		except cv2.error:
			print(f'"{path}" is not an image...')

	else:
		for FILE in os.listdir(path):
			if not os.path.isfile(os.path.join(path, FILE)):
				continue

			try:
				rgb = cv2.imread(os.path.join(path, FILE), cv2.IMREAD_COLOR)
				cv2.imwrite(os.path.join(path, FILE), rgb)
			except cv2.error:
				continue

def main():
    path = str(input('Path to Assets: '))
    process(path.strip('"').strip())

if __name__ == '__main__':
    main()
