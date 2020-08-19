import sys, os, platform
import shutil

dst_path = ""
release_path = ""

if (platform.system() == "Darwin"):
	print("installing on mac")
	dst_path = "/usr/local/share/DataKeep/"
	release_path = "./mac/"
elif (platform.system() == "Windows"):
	print("installing on windows")
	dst_path = "C:\\DataKeep\\"
	release_path = "./windows/"

try:
	print("Copying folder from {} to {}".format(release_path, dst_path))
	shutil.copytree(release_path, dst_path)
	print("Install complete.")
	print("Read the adding_to_system_path_guide.txt file.")
except:
	print("Installation failed. (try running as admin or sudo command for mac?)")