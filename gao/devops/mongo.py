import gao.devops.common

def drop_gaos_database(sconn, isLocal):
    if isLocal:
        sconn.invoke(f"""
            \\w1\mongosh\\bin\\mongosh.exe scripts\mongo\drop_gaos_database.js
        """)
    else:
        tempFoldar = gao.devops.common.createTempFolderOnServer(sconn)
        sconn.put("scripts/mongo/drop_gaos_database.js", tempFoldar)
        sconn.run(f"""
            /usr/bin/mongosh {tempFoldar}/drop_gaos_database.js
        """)
        pass
