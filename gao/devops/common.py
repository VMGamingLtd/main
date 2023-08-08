
def createTempFolderOnServer(sconn):
    result = sconn.run("mktemp -d")
    return result.stdout.strip()
