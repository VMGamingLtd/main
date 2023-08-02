import os
import gao.devops.config
import gao.devops.connection

def windowsReleaseFolder(isLocal=False):
    if isLocal:
        return f"{gao.devops.config.LOCAL_RELEASE_FOLDER}/{gao.devops.config.WINDOWS_RELEASE_SUBFOLDER}"
    else:
        return f"{gao.devops.config.RELEASE_FOLDER}/{gao.devops.config.WINDOWS_RELEASE_SUBFOLDER}"

def androidReleaseFolder(isLocal=False):
    if isLocal:
        return f"{gao.devops.config.LOCAL_RELEASE_FOLDER}/{gao.devops.config.ANDROID_RELEASE_SUBFOLDER}"
    else:
        return f"{gao.devops.config.RELEASE_FOLDER}/{gao.devops.config.ANDROID_RELEASE_SUBFOLDER}"

def macosReleaseFolder(isLocal=False):
    if isLocal:
        return f"{gao.devops.config.LOCAL_RELEASE_FOLDER}/{gao.devops.config.MACOS_RELEASE_SUBFOLDER}"
    else:
        return f"{gao.devops.config.RELEASE_FOLDER}/{gao.devops.config.MACOS_RELEASE_SUBFOLDER}"

def iosReleaseFolder(isLocal=False):
    if isLocal:
        return f"{gao.devops.config.LOCAL_RELEASE_FOLDER}/{gao.devops.config.IOS_RELEASE_SUBFOLDER}"
    else:
        return f"{gao.devops.config.RELEASE_FOLDER}/{gao.devops.config.IOS_RELEASE_SUBFOLDER}"

def webglReleaseFolder(isLocal=False):
    if isLocal:
        return f"{gao.devops.config.LOCAL_RELEASE_FOLDER}/{gao.devops.config.WEBGL_RELEASE_SUBFOLDER}"
    else:
        return f"{gao.devops.config.RELEASE_FOLDER}/{gao.devops.config.WEBGL_RELEASE_SUBFOLDER}"

def createReleaseFolders(c, isLocal=False):

    if isLocal:
        releaseFolder = gao.devops.config.LOCAL_RELEASE_FOLDER;
        if not os.path.isdir(gao.devops.config.LOCAL_RELEASE_FOLDER):
           os.makedirs(gao.devops.config.LOCAL_RELEASE_FOLDER)
        if not os.path.isdir(windowsReleaseFolder()):
           os.makedirs(windowsReleaseFolder())
        if not os.path.isdir(androidReleaseFolder()):
           os.makedirs(androidReleaseFolder())
        if not os.path.isdir(macosReleaseFolder()):
           os.makedirs(macosReleaseFolder())
        if not os.path.isdir(iosReleaseFolder()):
           os.makedirs(iosReleaseFolder())
        if not os.path.isdir(webglReleaseFolder()):
           os.makedirs(webglReleaseFolder())
    else:
        releaseFolder = gao.devops.config.RELEASE_FOLDER;
        c.run(f"""
        if [ ! -d "{releaseFolder}" ]; then
            mkdir -p {releaseFolder}
            chown {gao.devops.config.GAO_USER_NAME}:{gao.devops.config.GAO_USER_NAME} {gao.devops.config.RELEASE_FOLDER}
        fi
        """)
        c.run(f"""
        if [ ! -d "{windowsReleaseFolder()}" ]; then
            mkdir -p {windowsReleaseFolder()}
            chown {gao.devops.config.GAO_USER_NAME}:{gao.devops.config.GAO_USER_NAME} {windowsReleaseFolder()}
        fi
        """)
        c.run(f"""
        if [ ! -d "{androidReleaseFolder()}" ]; then
            mkdir -p {androidReleaseFolder()}
            chown {gao.devops.config.GAO_USER_NAME}:{gao.devops.config.GAO_USER_NAME} {androidReleaseFolder()}
        fi
        """)
        c.run(f"""
        if [ ! -d "{macosReleaseFolder()}" ]; then
            mkdir -p {macosReleaseFolder()}
            chown {gao.devops.config.GAO_USER_NAME}:{gao.devops.config.GAO_USER_NAME} {macosReleaseFolder()}
        fi
        """)
        c.run(f"""
        if [ ! -d "{iosReleaseFolder()}" ]; then
            mkdir -p {iosReleaseFolder()}
            chown {gao.devops.config.GAO_USER_NAME}:{gao.devops.config.GAO_USER_NAME} {iosReleaseFolder()}
        fi
        """)
        c.run(f"""
        if [ ! -d "{webglReleaseFolder()}" ]; then
            mkdir -p {webglReleaseFolder()}
            chown {gao.devops.config.GAO_USER_NAME}:{gao.devops.config.GAO_USER_NAME} {webglReleaseFolder()}
        fi
        """)

def test():
    conn = gao.devops.connection.connectionTestServer()
    createReleaseFolders(conn)

if __name__ == "__main__":
    test()
    print("finished")
