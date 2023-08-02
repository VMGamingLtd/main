import os
import tempfile
import tarfile
from pathlib import Path
import gao.devops.config
import gao.devops.folder
import gao.devops.connection
import shutil

def createTempFolder():
    tempdir = tempfile.mkdtemp()
    return tempdir

def archiveAssetBundles():
    assetBundlesFolderPath = Path(gao.devops.config.GAO_BUILD_ASSET_BUNDLES_FOLDER)
    cwd = os.getcwd()
    archivedFolder =  os.path.basename(assetBundlesFolderPath)
    archiveName = archivedFolder + ".tar.gz"
    archiveFolder = os.path.dirname(assetBundlesFolderPath)
    tempFolder = createTempFolder()
    archiveFilePath = os.path.join(tempFolder, archiveName)

    try:
        os.chdir(archiveFolder)
        if os.path.exists(archiveName):
          os.remove(archiveName)
        with tarfile.open(archiveName, "w:gz") as tar:
            tar.add(archivedFolder)
        os.rename(archiveName, archiveFilePath)
    finally:
        os.chdir(cwd)

    return archiveFilePath

def archiveWebglBuild():
    webglBuildFolder = Path(gao.devops.config.GAO_BUILD_WEBGL_FOLDER)
    cwd = os.getcwd()
    archivedFolder =  os.path.basename(webglBuildFolder)
    archiveName = archivedFolder + ".tar.gz"
    archiveFolder = os.path.dirname(webglBuildFolder)
    tempFolder = createTempFolder()
    archiveFilePath = os.path.join(tempFolder, archiveName)

    try:
        os.chdir(archiveFolder)
        if os.path.exists(archiveName):
          os.remove(archiveName)
        with tarfile.open(archiveName, "w:gz") as tar:
            tar.add(archivedFolder)
        os.rename(archiveName, archiveFilePath)
    finally:
        os.chdir(cwd)

    return archiveFilePath

def archiveAndroidBuild():
    webglBuildFolder = Path(gao.devops.config.GAO_BUILD_ANDROID_FOLDER)
    cwd = os.getcwd()
    archivedFolder =  os.path.basename(webglBuildFolder)
    archiveName = archivedFolder + ".tar.gz"
    archiveFolder = os.path.dirname(webglBuildFolder)
    tempFolder = createTempFolder()
    archiveFilePath = os.path.join(tempFolder, archiveName)

    try:
        os.chdir(archiveFolder)
        if os.path.exists(archiveName):
          os.remove(archiveName)
        with tarfile.open(archiveName, "w:gz") as tar:
            tar.add(archivedFolder)
        os.rename(archiveName, archiveFilePath)
    finally:
        os.chdir(cwd)

    return archiveFilePath

def getVersionFolderPath(version, platform, isLocal=False):
    if platform == "webgl":
        return f"{gao.devops.folder.webglReleaseFolder(isLocal)}/{version}"
    elif platform == "android":
        return f"{gao.devops.folder.androidReleaseFolder(isLocal)}/{version}"
    elif platform == "windows":
        return f"{gao.devops.folder.windowsReleaseFolder(isLocal)}/{version}"
    elif platform == "ios":
        return f"{gao.devops.folder.iosReleaseFolder(isLocal)}/{version}"
    elif platform == "macos":
        return f"{gao.devops.folder.macosReleaseFolder(isLocal)}/{version}"
    else:
        raise Exception(f"Unknown platform {platform}")


def createVersionFolder(sconn, version, platform, isLocal=False):
    # Create new version folder

    print(f"INFO: release {platform} {version} - checking if version already exists")

    gao.devops.folder.createReleaseFolders(sconn, isLocal)

    versionFolder = getVersionFolderPath(version, platform, isLocal)
    if isLocal:
        if os.path.isdir(versionFolder):
            print(f"ERROR: release {platform} {version} - version already exists")
            raise Exception(f"release {platform} {version} - version already exists")
        else:
            print(f"INFO: release {platform} {version} - version does not exists, create version folder")
            os.makedirs(versionFolder)
        return versionFolder
    else:
        sconn.run(f"""
            if [ -d {versionFolder} ]; then
                echo "ERROR: release {platform} {version} - version already exists"
                exit 1
            else
                echo "INFO: release {platform} {version} - version does not exists, create version folder"
                mkdir {versionFolder}
            fi
        """)
        return versionFolder

def releaseBundles(sconn, version, platform, isLocal=False):
    versionFolder = getVersionFolderPath(version, platform, isLocal)

    if isLocal:
        # Before releasing the bundles, platform version must already exists
        if not os.path.isdir(versionFolder):
            print(f"ERROR: release {platform} {version} - platform version deas not exists")
            raise Exception("Platform version does not exists")


        # Create bundle archive and copy it to release folder

        print(f"INFO: release {platform} {version} - creating bundle archive")

        archiveFilePath = archiveAssetBundles()
        shutil.copy(archiveFilePath, versionFolder)
        archiveBaseName = os.path.basename(archiveFilePath).split(".")[0]

        # Expand bundle archive in release folder

        print(f"INFO: release {platform} {version} - exapnding bundle archive in release folder")

        cwd = os.getcwd()
        os.chdir(versionFolder)
        tar = None
        try:
            tar = tarfile.open(f"{archiveBaseName}.tar.gz")
            tar.extractall()
        finally:
            if tar != None:
                tar.close()
            if os.path.exists(f"{archiveBaseName}.tar.gz"):
                os.remove(f"{archiveBaseName}.tar.gz")
            os.chdir(cwd)

        print(f"INFO: release {platform} {version} - releasing of bundles finished: {versionFolder}/{archiveBaseName}")
    else:
        # Before releasing the bundles, platform version must already exists
        sconn.run(f"""
            if [ ! -d {versionFolder} ]; then
                echo "ERROR: release {platform} {version} - platform version does not exists"
                exit 1
            fi
        """)


        # Create bundle archive and upload to server

        print(f"INFO: release {platform} {version} - creating bundle archive")


        archiveFilePath = archiveAssetBundles()
        sconn.put(archiveFilePath, versionFolder)
        archiveBaseName = os.path.basename(archiveFilePath).split(".")[0]


        # Expand bundle archive on server

        print(f"INFO: release {platform} {version} - exapnding bundle archive in release folder")

        sconn.run(f"""
            cd {versionFolder}
            tar -xvf {archiveBaseName}.tar.gz
            chown -R gao:gao {archiveBaseName}
            chmod -R 664 {archiveBaseName}
            rm -rf {archiveBaseName}.tar.gz
        """)

        print(f"INFO: release {platform} {version} - releasing of bundles finished: {versionFolder}/{archiveBaseName}")

def releaseWebgl(sconn, version, isLocal=False):
    platform = "webgl"

    versionFolder = createVersionFolder(sconn, version, platform, isLocal)
    releaseBundles(sconn, version, platform, isLocal)


    if isLocal:
        # Create webgl build archive and copy it to release folder

        print(f"INFO: release {platform} {version} - creating build archive")

        archiveFilePath = archiveWebglBuild()
        shutil.copy(archiveFilePath, versionFolder)
        archiveBaseName = os.path.basename(archiveFilePath).split(".")[0]

        # Expand build archive in release folder

        print(f"INFO: release platform {version} - exapnding bundle archive in release folder")

        cwd = os.getcwd()
        os.chdir(versionFolder)
        tar = None
        try:
            tar = tarfile.open(f"{archiveBaseName}.tar.gz")
            tar.extractall()
        finally:
            if tar != None:
                tar.close()
            os.rename(archiveBaseName, "build")
            if os.path.exists(f"{archiveBaseName}.tar.gz"):
                os.remove(f"{archiveBaseName}.tar.gz")
            os.chdir(cwd)

        tar = None
        try:
            os.chdir(versionFolder)
            os.chdir("..")
            with tarfile.open(f"gao_{platform}__{version}__.tar.gz", "w:gz") as tar:
                tar.add(version)
        finally:
            if tar != None:
                tar.close()
            os.chdir(cwd)

        print(f"INFO: release {platform} {version} - releasing of build finished: {versionFolder}/{archiveBaseName}")
    else:
        # Create webgl build archive and upload to server

        print(f"INFO: release {platform} {version} - creating build archive")

        archiveFilePath = archiveWebglBuild()
        sconn.put(archiveFilePath, versionFolder)
        archiveBaseName = os.path.basename(archiveFilePath).split(".")[0]

        # Expand build archive on server

        print(f"INFO: release {platform} {version} - uploading build archive to server")

        sconn.run(f"""
            cd {versionFolder}
            tar -xvf {archiveBaseName}.tar.gz
            chown -R gao:gao {archiveBaseName}
            chmod -R 664 {archiveBaseName}
            mv {archiveBaseName} build
            rm -rf {archiveBaseName}.tar.gz
            cd ..
            tar -cvf gao_{platform}__{version}__.tar {version}
            gzip gao_{platform}__{version}__.tar  
        """)

        print(f"INFO: release {platform} {version} - releasing of build finished: {versionFolder}/{archiveBaseName}")

def releaseAndroid(sconn, version, isLocal=False):
    platform = "android"

    versionFolder = createVersionFolder(sconn, version, platform, isLocal)
    releaseBundles(sconn, version, platform, isLocal)

    if isLocal:
        # Create webgl build archive and copy it to release folder

        print(f"INFO: release {platform} {version} - creating build archive")

        archiveFilePath = archiveWebglBuild()
        shutil.copy(archiveFilePath, versionFolder)
        archiveBaseName = os.path.basename(archiveFilePath).split(".")[0]

        # Expand build archive in release folder

        print(f"INFO: release {platform} {version} - exapnding bundle archive in release folder")

        cwd = os.getcwd()
        os.chdir(versionFolder)
        tar = None
        try:
            tar = tarfile.open(f"{archiveBaseName}.tar.gz")
            tar.extractall()
        finally:
            if tar != None:
                tar.close()
            os.rename(archiveBaseName, "build")
            if os.path.exists(f"{archiveBaseName}.tar.gz"):
                os.remove(f"{archiveBaseName}.tar.gz")
            os.chdir(cwd)

        tar = None
        try:
            os.chdir(versionFolder)
            os.chdir("..")
            versionFolderBaseName = os.path.basename(versionFolder)
            with tarfile.open(f"gao_{platform}__{version}__.tar.gz", "w:gz") as tar:
                tar.add(version)
        finally:
            if tar != None:
                tar.close()
            os.chdir(cwd)

        print(f"INFO: release {platform} {version} - releasing of build finished: {versionFolder}/{archiveBaseName}")
    else:
        # Create webgl build archive and upload to server

        print(f"INFO: release {platform} {version} - creating build archive")

        archiveFilePath = archiveAndroidBuild()
        sconn.put(archiveFilePath, versionFolder)
        archiveBaseName = os.path.basename(archiveFilePath).split(".")[0]

        # Expand bundle archive on server

        print(f"INFO: release {platform} {version} - uploading build archive to server")

        sconn.run(f"""
            cd {versionFolder}
            tar -xvf {archiveBaseName}.tar.gz
            chown -R gao:gao {archiveBaseName}
            chmod -R 664 {archiveBaseName}
            mv {archiveBaseName} build
            rm -rf {archiveBaseName}.tar.gz
            cd ..
            tar -cvf gao_{platform}__{version}__.tar {version}
            gzip gao_{platform}__{version}__.tar  
        """)

        print(f"INFO: release {platform} {version} - releasing of build finished: {versionFolder}/build")



def test():
    # remote
    #sconn = gao.devops.connection.connectionTestServer()
    #releaseWebgl(sconn, "1.0.0")
    #releaseAndroid(sconn, "1.0.0")

    # local
    releaseWebgl(None, "1.0.0", True)
    #releaseAndroid(None, "1.0.0", True)

if __name__ == "__main__":
    test()
    print("finished")

