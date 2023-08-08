from invoke import task
import gao.devops.connection
from gao.devops.release import release, releaseBundles
from gao.devops.publish import publish, publishBundles

def checkPlatform(platform):
    platforms = ["webgl", "windows", "macos", "android", "ios"]
    # test if platform is valid
    if platform not in platforms:
        raise Exception(f"unknown platform {platform}")

# release tasks

@task
def release_to_local(c, platform = "webgl", version = "0.0.1", is_include_build = True):
    checkPlatform(platform)
    release(None, platform, version, isLocal = True, isIncludeBuild = is_include_build)

@task
def release_to_test_server(c, platform = "webgl", version = "0.0.1", is_include_build = True):
    isIncludeBuild = is_include_build
    sconn = gao.devops.connection.connectionTestServer()
    release(sconn, platform, version, isLocal = False, isIncludeBuild = is_include_build)

@task
def release_bundles_to_local(c, platform = "webgl", version = "0.0.1", bundles_version = "2"):
    bundlesVersion = bundles_version
    releaseBundles(None, platform, version, isLocal = True, bundlesVersion = "2")

@task
def release_bundles_to_test_server(c, platform = "webgl", version = "0.0.1", bundles_version = "2"):
    bundlesVersion = bundles_version
    sconn = gao.devops.connection.connectionTestServer()
    releaseBundles(sconn, platform, version, isLocal = False, bundlesVersion = "2")

# publish tasks

@task
def publish_to_local(c, platform = "webgl", version = "0.0.1", bundles_version = "1", is_include_build = True):
    publish(None,  platform, version, bundlesVersion = bundles_version, isLocal = True, isIncludeBuild = is_include_build)

@task
def publish_to_test_server(c, platform = "webgl", version = "0.0.1", bundles_version = "1",  is_include_build = True, is_use_local_release = True):
    sconn = gao.devops.connection.connectionTestServer()
    publish(sconn,  platform, version,  bundlesVersion = bundles_version, isLocal = False, isIncludeBuild = is_include_build, isUseLocalRelease = is_use_local_release)

@task
def publish_bundles_to_local(c, platform = "webgl", version = "0.0.1", bundles_version = "1"):
    publishBundles(None,  platform, version, bundlesVersion = bundles_version, isLocal = True)

@task
def publish_bundles_to_test_server(c, platform = "webgl", version = "0.0.1", bundles_version = "1", is_use_local_release = True):
    sconn = gao.devops.connection.connectionTestServer()
    publishBundles(sconn,  platform, version, bundlesVersion = bundles_version, isLocal = False, isUseLocalRelease = is_use_local_release)
