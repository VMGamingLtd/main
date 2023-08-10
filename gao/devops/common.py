
def createTempFolderOnServer(sconn):
    result = sconn.run("mktemp -d")
    return result.stdout.strip()

def runCommands(c, commandsStr):
    commands = [cmd.strip() for cmd in commandsStr.strip().splitlines()]
    for cmd in commands:
        c.run(cmd)
