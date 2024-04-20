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

def archiveBuild(platform):

    webglBuildFolder = None 
    if platform == "webgl":
        webglBuildFolder = Path(gao.devops.config.GAO_BUILD_WEBGL_FOLDER)
    elif platform == "android":
        webglBuildFolder = Path(gao.devops.config.GAO_BUILD_ANDROID_FOLDER)
    elif platform == "ios":
        webglBuildFolder = Path(gao.devops.config.GAO_BUILD_IOS_FOLDER)
    elif platform == "windows":
        webglBuildFolder = Path(gao.devops.config.GAO_BUILD_WINDOWS_FOLDER)
    elif platform == "macos":
        webglBuildFolder = Path(gao.devops.config.GAO_BUILD_MACOS_FOLDER)
    else:
        raise Exception("Unknown platform: " + platform)

    #if platform == "webgl":
    #    gaosModelsFolders = os.path.join(Path(gao.devops.config.GAO_BUILD_WEBGL_FOLDER), Path("Scripts/Models"))
    #    if not (os.path.exists(gaosModelsFolders) and os.path.isdir(gaosModelsFolders)):
    #        print(f"ERROR: release {platform}  - archiveBuild(),  webl build folder {gao.devops.config.GAO_BUILD_WEBGL_FOLDER} does not contain 'Scripts/Models' subfolder, plese copy '../gao/Scripts/Model' folder inside webgl build folder")
    #        raise Exception("error calling archiveBuild()")

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


def createVersionFolder(sconn, version, platform, isLocal, isForced = False):
    # Create new version folder

    print(f"INFO: release {platform} {version} - checking if version already exists")

    gao.devops.folder.createReleaseFolders(sconn, isLocal)

    versionFolder = getVersionFolderPath(version, platform, isLocal)
    if isLocal:
        if os.path.isdir(versionFolder):
            if isForced:
                print(f"INFO: release {platform} {version} - version already exists, delete version folder")
                shutil.rmtree(versionFolder)
                print(f"INFO: release {platform} {version} - create version folder")
                os.makedirs(versionFolder)
            else:
                print(f"ERROR: release {platform} {version} - version already exists")
                raise Exception(f"release {platform} {version} - version already exists")
        else:
            print(f"INFO: release {platform} {version} - version does not exists, create version folder")
            os.makedirs(versionFolder)
        return versionFolder
    else:
        if isForced:
            sconn.run(f"""
                if [ -d {versionFolder} ]; then
                    echo "INFO: release {platform} {version} - version already exists, delete version folder"
                    rm -rf {versionFolder}
                    echo "INFO: release {platform} {version} - create version folder"
                    mkdir {versionFolder}
                else
                    echo "INFO: release {platform} {version} - version does not exists, create version folder"
                    mkdir {versionFolder}
                fi
            """)
            pass
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

def releaseBundles(sconn, platform, version, **kwargs):

    isLocal = False
    if "isLocal" in kwargs:
        isLocal = kwargs["isLocal"]

    bundlesVersion = "1"
    if "bundlesVersion" in kwargs:
        bundlesVersion = kwargs["bundlesVersion"]

    isForced = False
    if "isForced" in kwargs:
        isForced = kwargs["isForced"]

    versionFolder = getVersionFolderPath(version, platform, isLocal)

    if isLocal:
        # Before releasing the bundles, platform version must already exist
        if not os.path.isdir(versionFolder):
            print(f"ERROR: release {platform} {version} - platform version deas not exist")
            raise Exception("Platform version does not exist")


        # Create bundle archive and copy it to release folder

        print(f"INFO: release {platform} {version} - creating bundle archive")

        archiveFilePath = archiveAssetBundles()
        shutil.copy(archiveFilePath, versionFolder)
        archiveBaseName = os.path.basename(archiveFilePath).split(".")[0] # remove extension tar.gz

        # Create archiveBaseName folder inside versionFolder if not exists
        if not os.path.isdir(f"{versionFolder}/{archiveBaseName}"):
            os.makedirs(f"{versionFolder}/{archiveBaseName}")

        # bundlesVersion folder must not exists inside archiveBaseName subfolder of versionFolder
        if os.path.isdir(f"{versionFolder}/{archiveBaseName}/{bundlesVersion}"):
            if isForced:
                print(f"INFO: release {platform} {version} - bundles version {bundlesVersion} already exists, delete it")
                shutil.rmtree(f"{versionFolder}/{archiveBaseName}/{bundlesVersion}")
            else:
                raise Exception(f"ERROR: release {platform} {version} - bundles version {bundlesVersion} already exists")

        # Expand bundle archive in release folder

        print(f"INFO: release {platform} {version} - exapnding bundle archive in release folder")

        cwd = os.getcwd()

        os.chdir(versionFolder)
        tar = None
        try:
            if os.path.isdir("tmp"):
                shutil.rmtree("tmp")
            os.makedirs("tmp")
            shutil.move(f"{archiveBaseName}.tar.gz", "tmp")
            os.chdir("tmp")
             
            tar = tarfile.open(f"{archiveBaseName}.tar.gz")
            tar.extractall()
        finally:
            if tar != None:
                tar.close()
            shutil.move(archiveBaseName, f"../{archiveBaseName}/{bundlesVersion}")
            if os.path.exists(f"{archiveBaseName}.tar.gz"):
                os.remove(f"{archiveBaseName}.tar.gz")
            os.chdir("..")
            shutil.rmtree("tmp")

        os.chdir(f"{archiveBaseName}")
        #with open("version.txt", "w") as text_file:
        #    text_file.write(f"{bundlesVersion}")

        os.chdir(cwd)

        print(f"INFO: release {platform} {version} - releasing of bundles finished: {versionFolder}/{archiveBaseName}/{bundlesVersion}")
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

        # Create archiveBaseName folder inside versionFolder if not exists
        sconn.run(f"""
            if [ ! -d {versionFolder}/{archiveBaseName} ]; then
                mkdir {versionFolder}/{archiveBaseName}
            fi
        """)

        if isForced:
            # Remove bundlesVersion folder if it already exists inside archiveBaseName subfolder of versionFolder
            sconn.run(f"""
                if [ -d {versionFolder}/{archiveBaseName}/{bundlesVersion} ]; then
                    echo "INFO: release {platform} {version} - bundles version {bundlesVersion} already exists, remove it"
                    rm -rf {versionFolder}/{archiveBaseName}/{bundlesVersion} 
                fi
            """)
        else:
            # Report error if bundlesVersion folder already exists inside archiveBaseName subfolder of versionFolder
            sconn.run(f"""
                if [ -d {versionFolder}/{archiveBaseName}/{bundlesVersion} ]; then
                    echo "ERROR: release {platform} {version} - bundles version {bundlesVersion} already exists"
                    exit 1
                fi
            """)


        # Expand bundle archive on server

        print(f"INFO: release {platform} {version} - exapnding bundle archive in release folder")

        sconn.run(f"""
            cd {versionFolder}
            rm -rf tmp
            mkdir tmp
            mv {archiveBaseName}.tar.gz tmp
            cd tmp
            tar -xvf {archiveBaseName}.tar.gz
            chown -R gao:gao {archiveBaseName}
            chmod -R 664 {archiveBaseName}
            mv {archiveBaseName} ../{archiveBaseName}/{bundlesVersion}
            rm -rf {archiveBaseName}.tar.gz
            cd ..
            rm -rf tmp
            cd {archiveBaseName}
            #echo {bundlesVersion} > version.txt
            cd ..
        """)

        print(f"INFO: release {platform} {version} - releasing of bundles finished: {versionFolder}/{archiveBaseName}/{bundlesVersion}")

def release(sconn, platform, version, **kwargs):

    isLocal = False
    if "isLocal" in kwargs:
        isLocal = kwargs["isLocal"]

    isIncludeBuild = True
    if "isIncludeBuild" in kwargs:
        isIncludeBuild = kwargs["isIncludeBuild"]
    else:
        if platform == "webgl":
            isIncludeBuild = True

    isForced = False
    if "isForced" in kwargs:
        isForced = kwargs["isForced"]

    versionFolder = createVersionFolder(sconn, version, platform, isLocal, isForced)
    releaseBundles(sconn, platform, version, isLocal = isLocal, isForced = isForced)


    if isLocal:
        # Create build archive and copy it to release folder

        print(f"INFO: release {platform} {version} - creating build archive")


        if isIncludeBuild:
            archiveFilePath = archiveBuild(platform)
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

            print(f"INFO: release {platform} {version} - releasing of build finished: {versionFolder}/build")
        else:
            print(f"INFO: release {platform} {version} - releasing finished")
    else:
        # Create build archive and upload to server

        print(f"INFO: release {platform} {version} - creating build archive")

        archiveFilePath = archiveBuild(platform)
        archiveBaseName = os.path.basename(archiveFilePath).split(".")[0]

        if isIncludeBuild:
            sconn.put(archiveFilePath, versionFolder)

            # Expand build archive on server

            print(f"INFO: release {platform} {version} - uploading build archive to server")

            sconn.run(f"""
                cd {versionFolder}
                tar -xvf {archiveBaseName}.tar.gz
                chown -R gao:gao {archiveBaseName}
                chmod -R 664 {archiveBaseName}
                mv {archiveBaseName} build
                rm -rf {archiveBaseName}.tar.gz
            """)

            print(f"INFO: release {platform} {version} - releasing of build finished: {versionFolder}/build")
        else:
            print(f"INFO: release {platform} {version} - releasing finished: {versionFolder}/build")



def test():
    # remote

    #sconn = gao.devops.connection.connectionTestServer()

    #release(sconn, "webgl", "1.0.0")
    #releaseBundles(sconn, "webgl", "1.0.0", isLocal=False, bundlesVersion="2")
    #releaseBundles(sconn, "webgl", "1.0.0", isLocal=False, bindlesVersion="3")

    #release(sconn, "android", "1.0.0")
    #releaseBundles(sconn, "android", "1.0.0", isLocal=False, bundlesVersion="2")
    #releaseBundles(sconn, "android", "1.0.0", isLocal=False, bundlesVertsion="3")

    # local

    #release(None, "webgl", "1.0.0", isLocal=True)
    #releaseBundles(None, "webgl", "1.0.0", isLocal=True, bundlesVersion="2")
    #releaseBundles(None, "webgl", "1.0.0", isLocal=True, bundlesVersion="3")

    #release(None, "android", "1.0.0", isLocal=isLocal=True)
    #releaseBundles(None, "android", "1.0.0", isLocal=True, bundlesVersion="2")
    #releaseBundles(None, "android", "1.0.0", isLocal=True, bundlesVersion="3")
    pass

if __name__ == "__main__":
    test()
    print("finished")

