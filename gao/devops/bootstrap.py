from gao.devops.config import GAOP_PUBLIC_SSH_KEY, GAOP_PUBLIC_SSH_KEY_NAME, GAOP_PRIVATE_SSH_KEY, GAOP_SSH_KEYS_FOLDER, GAOP_REPO
from gao.devops.connection import connectionTestServer

def installAptPackages(sconn):
    sconn.run(f"""
        apt install -y git
        apt install -y python3-venv
    """)


def installGaopSshKeys(sconn):
    sconn.run(f"""
        cd
        if [ ! -d gao ]; then
            mkdir gao
        fi
        if [ ! -d {GAOP_SSH_KEYS_FOLDER} ]; then
            mkdir -p {GAOP_SSH_KEYS_FOLDER}
        fi
    """)
    sconn.put(GAOP_PUBLIC_SSH_KEY, f"{GAOP_SSH_KEYS_FOLDER}")
    sconn.put(GAOP_PRIVATE_SSH_KEY, f"{GAOP_SSH_KEYS_FOLDER}")
    sconn.run(f"""
        chmod 400 {GAOP_SSH_KEYS_FOLDER}/id_rsa
        chmod 644 {GAOP_SSH_KEYS_FOLDER}/id_rsa.pub
    """)


def addSshKeyToGit(sconn, userName, pubKeyFilePath, pubKeyName):
    sconn.put(pubKeyFilePath, f"/home/{userName}")
    sconn.run(f"""
        cd /home/{userName}
        cat id_rsa.pub | egrep {pubKeyName}
        if [ ! $? -eq 0 ]; then
            cat id_rsa.pub >> .ssh/authorized_keys
        fi
    """)

def installVenv(sconn):
    sconn.run(f"""
        set -xe
        export GIT_SSH_COMMAND="ssh -i ${{HOME}}/{GAOP_SSH_KEYS_FOLDER}/id_rsa -o UserKnownHostsFile=/dev/null -o StrictHostKeyChecking=no"
        cd
        if [ ! -d gao ]; then
            mkdir gao
        fi
        cd gao
        if [ ! -d gaop ]; then
            git clone {GAOP_REPO} 
        fi
        cd gaop
        rm -rf .venv
        python3 -m venv .venv
        source .venv/bin/activate
        pip install -r requirements.txt
    """)

def updateVenv(sconn):
    sconn.run(f"""
        set -xe
        export GIT_SSH_COMMAND="ssh -i ${{HOME}}/{GAOP_SSH_KEYS_FOLDER}/id_rsa -o UserKnownHostsFile=/dev/null -o StrictHostKeyChecking=no"
        cd gao/gaop
        git pull
        rm -rf .venv
        python3 -m venv .venv
        source .venv/bin/activate
        pip install -r requirements.txt
    """)

def test():
    #sconn = connectionTestServer()
    #installAptPackages(sconn)
    #installGaopSshKeys(sconn)
    #addSshKeyToGit(sconn, "git", GAOP_PUBLIC_SSH_KEY, GAOP_PUBLIC_SSH_KEY)
    #installVenv(sconn)
    #updateVenv(sconn)
    pass

if __name__ == "__main__":
    test()
    print("finished")
