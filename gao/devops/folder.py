import os
import gao.devops.config
import gao.devops.connection

def windowsReleaseFolder(isLocal):
    if isLocal:
        return f"{gao.devops.config.LOCAL_RELEASE_FOLDER}/{gao.devops.config.WINDOWS_RELEASE_SUBFOLDER}"
    else:
        return f"{gao.devops.config.RELEASE_FOLDER}/{gao.devops.config.WINDOWS_RELEASE_SUBFOLDER}"

def androidReleaseFolder(isLocal):
    if isLocal:
        return f"{gao.devops.config.LOCAL_RELEASE_FOLDER}/{gao.devops.config.ANDROID_RELEASE_SUBFOLDER}"
    else:
        return f"{gao.devops.config.RELEASE_FOLDER}/{gao.devops.config.ANDROID_RELEASE_SUBFOLDER}"

def macosReleaseFolder(isLocal):
    if isLocal:
        return f"{gao.devops.config.LOCAL_RELEASE_FOLDER}/{gao.devops.config.MACOS_RELEASE_SUBFOLDER}"
    else:
        return f"{gao.devops.config.RELEASE_FOLDER}/{gao.devops.config.MACOS_RELEASE_SUBFOLDER}"

def iosReleaseFolder(isLocal):
    if isLocal:
        return f"{gao.devops.config.LOCAL_RELEASE_FOLDER}/{gao.devops.config.IOS_RELEASE_SUBFOLDER}"
    else:
        return f"{gao.devops.config.RELEASE_FOLDER}/{gao.devops.config.IOS_RELEASE_SUBFOLDER}"

def webglReleaseFolder(isLocal):
    if isLocal:
        return f"{gao.devops.config.LOCAL_RELEASE_FOLDER}/{gao.devops.config.WEBGL_RELEASE_SUBFOLDER}"
    else:
        return f"{gao.devops.config.RELEASE_FOLDER}/{gao.devops.config.WEBGL_RELEASE_SUBFOLDER}"

def createReleaseFolders(c, isLocal):
    if isLocal:
        releaseFolder = gao.devops.config.LOCAL_RELEASE_FOLDER;
        if not os.path.isdir(gao.devops.config.LOCAL_RELEASE_FOLDER):
            print(f"INFO: Creating local release folder: {gao.devops.config.LOCAL_RELEASE_FOLDER}")
            os.makedirs(gao.devops.config.LOCAL_RELEASE_FOLDER)
        if not os.path.isdir(windowsReleaseFolder(isLocal)):
            print(f"INFO: Creating windows release folder: {windowsReleaseFolder(isLocal)}")
            os.makedirs(windowsReleaseFolder(isLocal))
        if not os.path.isdir(androidReleaseFolder(isLocal)):
            print(f"INFO: Creating android release folder: {androidReleaseFolder(isLocal)}")
            os.makedirs(androidReleaseFolder(isLocal))
        if not os.path.isdir(macosReleaseFolder(isLocal)):
            print(f"INFO: Creating macos release folder: {macosReleaseFolder(isLocal)}")
            os.makedirs(macosReleaseFolder(isLocal))
        if not os.path.isdir(iosReleaseFolder(isLocal)):
            print(f"INFO: Creating ios release folder: {iosReleaseFolder(isLocal)}")
            os.makedirs(iosReleaseFolder(isLocal))
        if not os.path.isdir(webglReleaseFolder(isLocal)):
            print(f"INFO: Creating webgl release folder: {webglReleaseFolder(isLocal)}")
            os.makedirs(webglReleaseFolder(isLocal))
    else:
        releaseFolder = gao.devops.config.RELEASE_FOLDER;
        c.run(f"""
        if [ ! -d "{releaseFolder}" ]; then
            echo "INFO: Creating release folder: {releaseFolder}"
            mkdir -p {releaseFolder}
            chown {gao.devops.config.GAO_USER_NAME}:{gao.devops.config.GAO_USER_NAME} {gao.devops.config.RELEASE_FOLDER}
        fi
        """)
        c.run(f"""
        if [ ! -d "{windowsReleaseFolder(isLocal)}" ]; then
            echo "INFO: Creating windows release folder: {windowsReleaseFolder(isLocal)}"
            mkdir -p {windowsReleaseFolder(isLocal)}
            chown {gao.devops.config.GAO_USER_NAME}:{gao.devops.config.GAO_USER_NAME} {windowsReleaseFolder(isLocal)}
        fi
        """)
        c.run(f"""
        if [ ! -d "{androidReleaseFolder(isLocal)}" ]; then
            echo "INFO: Creating android release folder: {androidReleaseFolder(isLocal)}"
            mkdir -p {androidReleaseFolder(isLocal)}
            chown {gao.devops.config.GAO_USER_NAME}:{gao.devops.config.GAO_USER_NAME} {androidReleaseFolder(isLocal)}
        fi
        """)
        c.run(f"""
        if [ ! -d "{macosReleaseFolder(isLocal)}" ]; then
            echo "INFO: Creating macos release folder: {macosReleaseFolder(isLocal)}"
            mkdir -p {macosReleaseFolder(isLocal)}
            chown {gao.devops.config.GAO_USER_NAME}:{gao.devops.config.GAO_USER_NAME} {macosReleaseFolder(isLocal)}
        fi
        """)
        c.run(f"""
        if [ ! -d "{iosReleaseFolder(isLocal)}" ]; then
            echo "INFO: Creating ios release folder: {iosReleaseFolder(isLocal)}"
            mkdir -p {iosReleaseFolder(isLocal)}
            chown {gao.devops.config.GAO_USER_NAME}:{gao.devops.config.GAO_USER_NAME} {iosReleaseFolder(isLocal)}
        fi
        """)
        c.run(f"""
        if [ ! -d "{webglReleaseFolder(isLocal)}" ]; then
            echo "INFO: Creating webgl release folder: {webglReleaseFolder(isLocal)}"
            mkdir -p {webglReleaseFolder(isLocal)}
            chown {gao.devops.config.GAO_USER_NAME}:{gao.devops.config.GAO_USER_NAME} {webglReleaseFolder(isLocal)}
        fi
        """)

def test():
    conn = gao.devops.connection.connectionTestServer()
    createReleaseFolders(conn)

if __name__ == "__main__":
    test()
    print("finished")
