from fabric import Connection
import gao.devops.config

def connectionTestServer():
    conn = Connection(gao.devops.config.TEST_SERVER_NAME, user="root",  connect_kwargs={
            "key_filename": gao.devops.config.SSH_KEY_FILE_NAME
            })
    return conn
