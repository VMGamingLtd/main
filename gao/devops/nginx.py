import os
import shutil
import tarfile
import tempfile
from gao.devops.config import LOCAL_TEST_ENVIRONMENT_DOCUMENT_ROOT, LOCAL_TEST_ENVIRONMENT_NGINX_CONF
from gao.devops.common import runCommands, removeFolder

def update(sconn, isLocal = False):

    removeFolder("../nginx_conf/html/.vs")
    removeFolder("../nginx_conf/html/.vscode")
    removeFolder("../nginx_conf/html/.idea")

    if isLocal:
       commandStr = f"""
            echo "INFO: stop nginx.exe"
            taskkill /F /IM nginx.exe 2>$nul & set errorlevel=0

            echo "INFO: remove document root"
            rd /s /q ..\\nginx\\html

            echo "INFO: copy over document root"
            mkdir ..\\nginx\\html 
            xcopy /e ..\\nginx_conf\\html ..\\nginx\\html

            echo "INFO: copy over nginx.conf"
            copy ..\\nginx_conf\\nginx.conf ..\\nginx\\conf\\nginx.conf
       """

       runCommands(sconn, commandStr)

       pass
    else:
        sconn.run(
        f"""
            set -e

            echo "INFO: stop nginx service"
            systemctl stop nginx

            echo "INFO: remove document root"
            rm -rf /var/www/html/*
        """
        )

        cwd = os.getcwd()

        print(f"INFO: copy over document root archive")
        tmpFolder = tempfile.mkdtemp()
        shutil.copytree(LOCAL_TEST_ENVIRONMENT_DOCUMENT_ROOT, f"{tmpFolder}/html")

        os.chdir(tmpFolder)

        # create html.tar.gz archive
        with tarfile.open("html.tar.gz", "w:gz") as tar:
            tar.add("html")

        # copy archive to remote
        sconn.put("html.tar.gz", "/tmp/html.tar.gz")

        os.chdir(cwd)

        print(f"INFO: copy over nginx.conf")
        sconn.put(LOCAL_TEST_ENVIRONMENT_NGINX_CONF, "/tmp/nginx.conf")

        sconn.run(
        f"""
            set -e

            echo "INFO: update document root"

            rm -rf /var/www/html
            cp -r /tmp/html.tar.gz /var/www/html.tar.gz
            cd /var/www
            tar -xzf html.tar.gz
            rm -rf html.tar.gz
            chown -R www-data:www-data html/

            find html/ -type d -exec chmod 755 {{}} +
            find html/ -type f -exec chmod 644 {{}} +

            echo "INFO: update mginx.conf"
            cd /etc/nginx
            cp /tmp/nginx.conf nginx.conf
            chmod 644 nginx.conf

            echo "INFO: start nginx service"
            systemctl start nginx

            systemctl status nginx

        """
        )
        pass

def start(sconn, isLocal = False):
    if isLocal:
       commandStr = f"""
            echo "INFO: start nginx.exe"
            cd ..\\nginx && start nginx.exe
       """
       runCommands(sconn, commandStr)

       pass
    else:
        raise Exception("isLocal=True not supported yet")

def stop(sconn, isLocal = False):
    if isLocal:
       commandStr = f"""
            echo "INFO: stop nginx.exe"
            taskkill /F /IM nginx.exe & set errorlevel=0
       """
       runCommands(sconn, commandStr)

       pass
    else:
        raise Exception("isLocal=True not supported yet")