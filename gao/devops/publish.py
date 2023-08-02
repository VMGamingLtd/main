import shutil
import os
import os.path
import tarfile
from gao.devops.config import LOCAL_RELEASE_FOLDER, RELEASE_FOLDER, WEBGL_RELEASE_SUBFOLDER, LOCAL_PUBLISH_FOLDER, PUBLISH_FOLDER

def publishWebgl(sconn, version, isLocal=False):
    platform = "webgl"
    if isLocal:
        if os.path.isdir(f"{LOCAL_RELEASE_FOLDER}/{WEBGL_RELEASE_SUBFOLDER}/{version}"):
            raise Exception(f"version {version} already exists in {LOCAL_RELEASE_FOLDER}/{WEBGL_RELEASE_SUBFOLDER}/{version}")

        cwd = os.getcwd()
        shutil.copy(f"{LOCAL_RELEASE_FOLDER}/{WEBGL_RELEASE_SUBFOLDER}/gao_{platform}__{version}__.tar.gz", f"{LOCAL_PUBLISH_FOLDER}/{WEBGL_RELEASE_SUBFOLDER}")

        tar = None
        try:
            os.chdir(f"{LOCAL_PUBLISH_FOLDER}/{WEBGL_RELEASE_SUBFOLDER}")
            tar = tarfile.open(f"gao_{platform}__{version}__.tar.gz")
            tar.extractall()
        finally:
            if tar != None:
                tar.close()
            os.remove(f"gao_{platform}__{version}__.tar.gz")
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
            if [  -d "{PUBLISH_FOLDER}/{WEBGL_RELEASE_SUBFOLDER}/{version}" ]; then
                echo "ERROR: version {version} already exists in {PUBLISH_FOLDER}/{WEBGL_RELEASE_SUBFOLDER}/{version}"
                exit 1
            fi
        """)
        sconn.run(f"""
            cp {RELEASE_FOLDER}/{WEBGL_RELEASE_SUBFOLDER}/gao_{platform}__{version}__.tar.gz {PUBLISH_FOLDER}/{WEBGL_RELEASE_SUBFOLDER}
            cd {PUBLISH_FOLDER}/{WEBGL_RELEASE_SUBFOLDER}
            tar -xzf gao_{platform}__{version}__.tar.gz
            chown -r root:root {version}
            rm -rf current
            cp -r {version} current
            echo "{version}" > version.txt
            rm gao_{platform}__{version}__.tar.gz
        """)
        pass