import sys
import cv2
import os

currentPath = str(input('Path to Assets: '))

if os.path.isfile(currentPath):
	rgb = cv2.imread(currentPath)
	cv2.imwrite(currentPath, rgb)

else:
	for FILE in os.listdir(currentPath):
		if '.py' in FILE or not os.path.isfile(os.path.join(currentPath, FILE)):
			continue

		rgb = cv2.imread(currentPath + '/' + FILE)
		cv2.imwrite(currentPath + '/' + FILE, rgb)