import os
import shutil

def createTempFolderOnServer(sconn):
    result = sconn.run("mktemp -d")
    return result.stdout.strip()

def runCommands(c, commandsStr):
    commands = [cmd.strip() for cmd in commandsStr.strip().splitlines()]
    for cmd in commands:
        c.run(cmd)

def removeFolder(folderPath):
    if os.path.exists(folderPath):
        shutil.rmtree(folderPath)