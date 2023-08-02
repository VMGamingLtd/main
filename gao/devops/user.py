from fabric import Connection
import gao.devops.config
import gao.devops.connection

def readUsers(c):
    # read all users from /etc/passwd
    result = c.run("getent passwd | cut -d: -f1")
    users = result.stdout.strip().split("\n")
    return users

def readGroups(c):
    result = c.run("getent group | cut -d: -f1")
    groups = result.stdout.strip().split("\n")
    return groups

def removeUser(c, userName):
    c.run(f"userdel {userName}")

def createUser(c, userName):
    users = readUsers(c)
    if not userName in users:
        c.run(f"useradd --system --user-group --shell /bin/false {userName}")

def test():
    conn = gao.devops.connection.connectionTestServer()
    createUser(conn, gao.devops.config.GAO_USER_NAME)

if __name__ == "__main__":
    test()
    print("finished")