import os

CHECK = str(input('Enter new Path: '))
CACHE = str(input('Enter old Path: '))

for FILE in os.listdir(CHECK):
	if os.path.exists(CACHE + '/' + FILE):
		os.remove(CHECK + '/' + FILE)
