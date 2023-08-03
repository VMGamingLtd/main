import shutil
import os
import os.path
import tarfile
from gao.devops.config import LOCAL_RELEASE_FOLDER, RELEASE_FOLDER, WEBGL_RELEASE_SUBFOLDER, WINDOWS_RELEASE_SUBFOLDER, MACOS_RELEASE_SUBFOLDER, ANDROID_RELEASE_SUBFOLDER, IOS_RELEASE_SUBFOLDER, LOCAL_PUBLISH_FOLDER, PUBLISH_FOLDER
from gao.devops.connection import connectionTestServer

def getReleaseSubfolder(platform):

    if platform == "webgl":
        return WEBGL_RELEASE_SUBFOLDER
    if platform == "windows":
        return WINDOWS_RELEASE_SUBFOLDER
    elif platform == "macos":
        return MACOS_RELEASE_SUBFOLDER
    elif platform == "android":
        return ANDROID_RELEASE_SUBFOLDER
    elif platform == "ios":
        return IOS_RELEASE_SUBFOLDER
    else:
        raise Exception(f"unknown platform {platform}")

def publish(sconn, platform, version, isLocal=False):
    releaseSubfolder = getReleaseSubfolder(platform)
    if isLocal:
        if os.path.isdir(f"{LOCAL_PUBLISH_FOLDER}/{releaseSubfolder}/{version}"):
            raise Exception(f"version {version} already exists in {LOCAL_PUBLISH_FOLDER}/{releaseSubfolder}/{version}")

        print(f"INFO: copy archive")
        cwd = os.getcwd()
        shutil.copy(f"{LOCAL_RELEASE_FOLDER}/{releaseSubfolder}/gao_{platform}__{version}__.tar.gz", f"{LOCAL_PUBLISH_FOLDER}/{releaseSubfolder}")

        print(f"INFO: expand archive")
        tar = None
        try:
            os.chdir(f"{LOCAL_PUBLISH_FOLDER}/{releaseSubfolder}")
            tar = tarfile.open(f"gao_{platform}__{version}__.tar.gz")
            tar.extractall()
        finally:
            if tar != None:
                tar.close()
            os.remove(f"gao_{platform}__{version}__.tar.gz")

        print(f"INFO: set current folder")
        if os.path.isdir("current"):
            shutil.rmtree("current")
        if os.path.isfile("version.txt"):
            shutil.remove("version.txt")
        shutil.copytree(f"{version}", "current")
        with open("version.txt", "w") as text_file:
            text_file.write(f"{version}")
        os.chdir(cwd)
    else:
        sconn.run(f"""
            if [  -d "{PUBLISH_FOLDER}/{releaseSubfolder}/{version}" ]; then
                echo "ERROR: version {version} already exists in {PUBLISH_FOLDER}/{releaseSubfolder}/{version}"
                exit 1
            fi
        """)
        sconn.run(f"""
            set -e
            echo "INFO: copy archive"
            cp {RELEASE_FOLDER}/{releaseSubfolder}/gao_{platform}__{version}__.tar.gz {PUBLISH_FOLDER}/{releaseSubfolder}

            echo "INFO: expand archive"
            cd {PUBLISH_FOLDER}/{releaseSubfolder}
            tar -xvf gao_{platform}__{version}__.tar.gz
            chown -R root:root {version}
            rm gao_{platform}__{version}__.tar.gz

            echo "INFO: set current folder"
            rm -rf current
            cp -r {version} current
            echo "{version}" > version.txt
        """)
        pass

def test():
    #  remote
    #sconn = connectionTestServer()
    #publish(sconn, "webgl", "1.0.0")
    #publish(sconn, "android", "1.0.0")

    # local
    #publish(None,  "webgl", "1.0.0", True)
    #publish(None,  "android", "1.0.0", True)
    pass

if __name__ == "__main__":
    test()
    print("finished")