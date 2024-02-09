# local
#invoke drop-gaos-mongo-database-on-local
#invoke release-to-local --platform="webgl" --version="1.0.1" --is-forced
invoke publish-to-local --platform="webgl" --version="1.0.1", --bundles-version="1" --is-include-build

# test
#invoke update-nginx-on-test-server

#python -m venv .venv
#.venv\Scripts\Activate.ps1
#pip install -r requirements.txt

#.venv\Scripts\Activate.ps1
#invoke drop-gaos-mongo-database-on-test-server
#invoke release-to-local --platform="webgl" --version="0.0.1" --is-forced
#invoke publish-to-test-server --platform="webgl" --version="0.0.1", --bundles-version="1" --is-include-build

#invoke release-to-local --platform="android" --version="0.0.1" --is-forced
#invoke publish-to-test-server --platform="android" --version="0.0.1", --bundles-version="1" --is-include-build
