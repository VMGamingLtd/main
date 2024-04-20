from invoke import task
import gao.devops.connection
from gao.devops.release import release, releaseBundles
from gao.devops.publish import publish, publishBundles
import gao.devops.gaos
import gao.devops.nginx
import gao.devops.mongo


def checkPlatform(platform):
    platforms = ["webgl", "windows", "macos", "android", "ios"]
    # test if platform is valid
    if platform not in platforms:
        raise Exception(f"unknown platform {platform}")

# release

@task
def release_to_local(c, platform = "webgl", version = "0.0.1", is_include_build = True, is_forced = False):
    checkPlatform(platform)
    release(None, platform, version, isLocal = True, isIncludeBuild = is_include_build, isForced = is_forced)

@task
def release_to_test_server(c, platform = "webgl", version = "0.0.1", is_include_build = True, is_forced = False):
    sconn = gao.devops.connection.connectionTestServer()
    release(sconn, platform, version, isLocal = False, isIncludeBuild = is_include_build, isForced = is_forced)

@task
def release_bundles_to_local(c, platform = "webgl", version = "0.0.1", bundles_version = "2", is_forced = False):
    bundlesVersion = bundles_version
    releaseBundles(None, platform, version, isLocal = True, bundlesVersion = "2", isForced = is_forced)

@task
def release_bundles_to_test_server(c, platform = "webgl", version = "0.0.1", bundles_version = "1", is_forced = False):
    bundlesVersion = bundles_version
    sconn = gao.devops.connection.connectionTestServer()
    releaseBundles(sconn, platform, version, isLocal = False, bundlesVersion = "1", isForced = is_forced)

# publish 

@task
def publish_to_local(c, platform = "webgl", version = "0.0.1", bundles_version = "1", is_include_build = True):
    publish(None,  platform, version, bundlesVersion = bundles_version, isLocal = True, isIncludeBuild = is_include_build)
    print("https://local.galacticodyssey.space")

@task
def publish_to_test_server(c, platform = "webgl", version = "0.0.1", bundles_version = "1",  is_include_build = True, is_use_local_release = True):
    sconn = gao.devops.connection.connectionTestServer()
    publish(sconn,  platform, version,  bundlesVersion = bundles_version, isLocal = False, isIncludeBuild = is_include_build, isUseLocalRelease = is_use_local_release)
    print("https://test.galacticodyssey.space")

@task
def publish_bundles_to_local(c, platform = "webgl", version = "0.0.1", bundles_version = "1"):
    publishBundles(None,  platform, version, bundlesVersion = bundles_version, isLocal = True)

@task
def publish_bundles_to_test_server(c, platform = "webgl", version = "0.0.1", bundles_version = "1", is_use_local_release = True):
    sconn = gao.devops.connection.connectionTestServer()
    publishBundles(sconn,  platform, version, bundlesVersion = bundles_version, isLocal = False, isUseLocalRelease = is_use_local_release)


# gaos

@task
def deploy_gaos_to_test_server(c):
    sconn = gao.devops.connection.connectionTestServer()
    gao.devops.gaos.deploy(sconn)


# nginx

@task
def update_nginx_on_local(c):
    gao.devops.nginx.update(c, isLocal = True)

@task
def update_nginx_on_test_server(c):
    sconn = gao.devops.connection.connectionTestServer()
    gao.devops.nginx.update(sconn, isLocal = False)

@task
def start_nginx_on_local(c):
    gao.devops.nginx.start(c, isLocal = True)

@task
def stop_nginx_on_local(c):
    gao.devops.nginx.stop(c, isLocal = True)

# mongo

@task
def drop_gaos_mongo_database_on_local(c):
    gao.devops.mongo.drop_gaos_database(c, isLocal = True)

@task
def drop_gaos_mongo_database_on_test_server(c):
    sconn = gao.devops.connection.connectionTestServer()
    gao.devops.mongo.drop_gaos_database(sconn, isLocal = False)

