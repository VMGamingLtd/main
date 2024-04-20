def deploy(sconn):
    dotnetEnvironment = "Test"
    sconn.run(
    f"""
        set -e

        echo "INFO: set environment"
        export PATH=${{PATH}}:$HOME/.dotnet/tools
        export ASPNETCORE_ENVIRONMENT={dotnetEnvironment}

        echo "INFO: stoping gaos service"
        systemctl stop gaos

        echo "INFO: pulling from git"
        cd /opt/gaos
        git reset --hard
        git pull

        echo "INFO: drop and update gaos database"
        dotnet ef database drop --force
        rm -rf /opt/gaos/Migrations
        dotnet ef migrations add InitialCreate
        dotnet ef database update

        echo "INFO: build release"
        dotnet publish --configuration Release
        chown -R gaos:gaos /opt/gaos/bin/Release/net7.0/publish

        echo "INFO: start gaos service"
        systemctl start gaos
    """)
    pass