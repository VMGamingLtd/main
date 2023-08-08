import shutil
import os
import os.path
import tarfile
import tempfile
from gao.devops.config import LOCAL_RELEASE_FOLDER, RELEASE_FOLDER, WEBGL_RELEASE_SUBFOLDER, WINDOWS_RELEASE_SUBFOLDER, MACOS_RELEASE_SUBFOLDER, ANDROID_RELEASE_SUBFOLDER, IOS_RELEASE_SUBFOLDER, LOCAL_PUBLISH_FOLDER, PUBLISH_FOLDER
from gao.devops.connection import connectionTestServer
from gao.devops.common import createTempFolderOnServer

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

def publish(sconn, platform, version, bundlesVersion, **kwargs):
    isLocal = False
    if "isLocal" in kwargs:
        isLocal = kwargs["isLocal"]

    isIncludeBuild = False
    if "isIncludeBuild" in kwargs:
        isIncludeBuild = kwargs["isIncludeBuild"]
    else:
        if platform == "webgl":
            isIncludeBuild = True

    isUseLocalRelease = True
    if "isUseLocalRelease" in kwargs:
        isUseLocalRelease = kwargs["isUseLocalRelease"]
        

    releaseSubfolder = getReleaseSubfolder(platform)
    if isLocal:
        cwd = os.getcwd()

        os.chdir(f"{LOCAL_PUBLISH_FOLDER}/{releaseSubfolder}")
        if os.path.isdir("build"):
            shutil.rmtree("build")
        if os.path.isfile("version.txt"):
            os.remove("version.txt")
        if os.path.isdir("AssetBundles"):
            shutil.rmtree("AssetBundles")
        if os.path.isfile("versionBundles.txt"):
            os.remove("versionBundles.txt")

        print(f"INFO: copy build")
        if isIncludeBuild:
            os.chdir(cwd)
            shutil.copytree(f"{LOCAL_RELEASE_FOLDER}/{releaseSubfolder}/{version}/build", f"{LOCAL_PUBLISH_FOLDER}/{releaseSubfolder}/build")
            os.chdir(f"{LOCAL_PUBLISH_FOLDER}/{releaseSubfolder}")
        else:
            os.makedirs("build")
        with open("version.txt", "w") as text_file:
            text_file.write(f"{version}")

        print(f"INFO: copy AssetBundles")
        os.chdir(cwd)
        shutil.copytree(f"{LOCAL_RELEASE_FOLDER}/{releaseSubfolder}/{version}/AssetBundles/{bundlesVersion}", f"{LOCAL_PUBLISH_FOLDER}/{releaseSubfolder}/AssetBundles")
        os.chdir(f"{LOCAL_PUBLISH_FOLDER}/{releaseSubfolder}")
        with open("versionBundles.txt", "w") as text_file:
            text_file.write(f"{bundlesVersion}")

        print(f"INFO: finsished publishing of build: platform: {platform}, version: {version}, bundles version: 1")

        os.chdir(cwd)
    else:
        tmpFolderServer = createTempFolderOnServer(sconn)
        if isUseLocalRelease:
            cwd = os.getcwd()
            tmpFolder = tempfile.mkdtemp()
            print(f"INFO: archive build")
            shutil.copytree(f"{LOCAL_RELEASE_FOLDER}/{releaseSubfolder}/{version}/build", f"{tmpFolder}/build")
            os.chdir(tmpFolder)
            try:
                with tarfile.open("build.tar.gz", "w:gz") as tar:
                    tar.add("build")
            finally:
                pass

            print(f"INFO: archive AssetBundles")
            os.chdir(cwd)
            shutil.copytree(f"{LOCAL_RELEASE_FOLDER}/{releaseSubfolder}/{version}/AssetBundles/{bundlesVersion}", f"{tmpFolder}/AssetBundles")
            os.chdir(tmpFolder)
            try:
                with tarfile.open("AssetBundles.tar.gz", "w:gz") as tar:
                    tar.add("AssetBundles")
            finally:
                pass
            os.chdir(cwd)

            print(f"INFO: upload build archive to server")
            sconn.put(f"{tmpFolder}/build.tar.gz", tmpFolderServer)
            print(f"INFO: upload AssetBundles archive to server")
            sconn.put(f"{tmpFolder}/AssetBundles.tar.gz", tmpFolderServer)

            sconn.run(f"""
            cd {PUBLISH_FOLDER}/{releaseSubfolder}
            rm -rf build AssetBundles version.txt versionBundles.txt

            cp {tmpFolderServer}/build.tar.gz .
            tar -xvf build.tar.gz
            rm build.tar.gz
            chown root:root build
            echo "{version}" > version.txt

            cp {tmpFolderServer}/AssetBundles.tar.gz .
            tar -xvf AssetBundles.tar.gz
            rm AssetBundles.tar.gz
            chown root:root AssetBundles
            echo "{bundlesVersion}" > versionBundles.txt

            echo "INFO: finsished publishing of build: platform: {platform}, version: {version}, bundles version: {bundlesVersion}"
            """)
        else:
            sconn.run(f"""
                echo "INFO: copy build"

                cd {PUBLISH_FOLDER}/{releaseSubfolder}
                rm -rf build AssetBundles version.txt versionBundles.txt

                cp -r {RELEASE_FOLDER}/{releaseSubfolder}/{version}/build .
                chown root:root build
                echo "{version}" > version.txt

                echo "INFO: copy asset bundles"

                cp -r {RELEASE_FOLDER}/{releaseSubfolder}/{version}/AssetBundles/{bundlesVersion} AssetBundles
                chown root:root AssetBundles
                echo "{bundlesVersion}" > versionBundles.txt

                echo "INFO: finsished publishing of build: platform: {platform}, version: {version}, bundles version: {bundlesVersion}"
            """)
        pass

def publishBundles(sconn, platform, version, bundlesVersion, **kwargs):
    isLocal = False
    if "isLocal" in kwargs:
        isLocal = kwargs["isLocal"]

    isUseLocalRelease = True
    if "isUseLocalRelease" in kwargs:
        isUseLocalRelease = kwargs["isUseLocalRelease"]

    releaseSubfolder = getReleaseSubfolder(platform)
    if isLocal:
        cwd = os.getcwd()



        print(f"INFO: set current release")
        os.chdir(f"{LOCAL_PUBLISH_FOLDER}/{releaseSubfolder}")

        if os.path.isdir("AssetBundles"):
            shutil.rmtree("AssetBundles")
        if os.path.isfile("versionBundles.txt"):
            os.remove("versionBundles.txt")

        os.chdir(cwd)
        shutil.copytree(f"{LOCAL_RELEASE_FOLDER}/{releaseSubfolder}/{version}/AssetBundles/{bundlesVersion}", f"{LOCAL_PUBLISH_FOLDER}/{releaseSubfolder}/AssetBundles")
        os.chdir(f"{LOCAL_PUBLISH_FOLDER}/{releaseSubfolder}")

        with open("versionBundles.txt", "w") as text_file:
            text_file.write(f"{bundlesVersion}")

        print(f"INFO: finsished publishing of AssetBunddles: platform: {platform}, version: {version}, bundles version: {bundlesVersion}")

        os.chdir(cwd)
    else:
        if isUseLocalRelease:
            cwd = os.getcwd()
            tmpFolder = tempfile.mkdtemp()
            print(f"INFO: archive AssetBundles")
            shutil.copytree(f"{LOCAL_RELEASE_FOLDER}/{releaseSubfolder}/{version}/AssetBundles/{bundlesVersion}", f"{tmpFolder}/AssetBundles")
            os.chdir(tmpFolder)
            try:
                with tarfile.open("AssetBundles.tar.gz", "w:gz") as tar:
                    tar.add("AssetBundles")
            finally:
                pass

            print(f"INFO: copy AssetBundles archive to server")
            sconn.put(f"AssetBundles.tar.gz", f"{PUBLISH_FOLDER}/{releaseSubfolder}")
            sconn.run(f"""
                cd {PUBLISH_FOLDER}/{releaseSubfolder}
                rm -rf AssetBundles versionBundles.txt
                tar -xvf AssetBundles.tar.gz
                chown -R root:root AssetBundles
                echo "{bundlesVersion}" > versionBundles.txt
                rm -f AssetBundles.tar.gz 
                echo "INFO: finsished publishing of AssetBunddles: platform: {platform}, version: {version}, bundles version: {bundlesVersion}"
            """)
        else:
            print(f"INFO: copy AssetBundles")
            sconn.run(f"""
                cd {PUBLISH_FOLDER}/{releaseSubfolder}
                rm -rf AssetBundles versionBundles.txt
                cp -r {RELEASE_FOLDER}/{releaseSubfolder}/{version}/AssetBundles/{bundlesVersion} {PUBLISH_FOLDER}/{releaseSubfolder}/AssetBundles
                chown -R root:root AssetBundles
                echo "{bundlesVersion}" > versionBundles.txt
                echo "INFO: finsished publishing of AssetBunddles: platform: {platform}, version: {version}, bundles version: {bundlesVersion}"
            """)
            pass



def test():
    #  remote
    #sconn = connectionTestServer()
    #publish(sconn, "webgl", "1.0.0")
    #publish(sconn, "android", "1.0.0")

    # local
    #publish(None,  "webgl", "1.0.0", isLocal=True)
    #publish(None,  "android", "1.0.0", isLocal=True)

    pass

if __name__ == "__main__":
    test()
    print("finished")