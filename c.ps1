## Release

# local - webgl

#invoke release-to-local --platform="webgl" --version="0.0.1" --is-forced  
#invoke release-bundles-to-local --platform="webgl" --version="0.0.1" --bundles-version="2" --is-forced

# local - android
#invoke release-to-local --platform="android" --version="0.0.1"  
#invoke release-bundles-to-local --platform="android" --version="0.0.1" --bundles-version="2" --is-forced

# test - webgl
#invoke release-to-test-server --platform="webgl" --version="0.0.1"  
#invoke release-bundles-to-test-server --platform="webgl" --version="0.0.1" --bundles-version="2"

# test - android
#invoke release-to-test-server --platform="android" --version="0.0.1"  
#invoke release-bundles-to-test-server --platform="android" --version="0.0.1" --bundles-version="2"

## Publish

# local - webgl
#invoke publish-to-local --platform="webgl" --version="0.0.1", --bundles-version="1" --is-include-build
#invoke publish-bundles-to-local --platform="webgl" --version="0.0.1", --bundles-version="2" 


# local - android
#invoke publish-to-local --platform="android" --version="0.0.1", --bundles-version="1" --is-include-build
#invoke publish-bundles-to-local --platform="android" --version="0.0.1", --bundles-version="1" 

# server - webgl
#invoke publish-to-test-server --platform="webgl" --version="0.0.1", --bundles-version="1" --is-include-build
#invoke publish-bundles-to-test-server --platform="webgl" --version="0.0.1", --bundles-version="2" 

#invoke publish-to-test-server --platform="webgl" --version="0.0.1", --bundles-version="2" --is-include-build --no-is-use-local-release 
#invoke publish-bundles-to-test-server --platform="webgl" --version="0.0.1", --bundles-version="1" --no-is-use-local-release 

# server - android
#invoke publish-to-test-server --platform="android" --version="0.0.1", --bundles-version="2" --is-include-build
#invoke publish-bundles-to-test-server --platform="android" --version="0.0.1", --bundles-version="1" 

#invoke publish-to-test-server --platform="android" --version="0.0.1", --bundles-version="1" --is-include-build --no-is-use-local-release
#invoke publish-bundles-to-test-server --platform="android" --version="0.0.1", --bundles-version="2" --no-is-use-local-release 

## Nginx

#invoke update-nginx-on-test-server
#invoke update-nginx-on-local
#invoke start-nginx-on-local
#invoke stop-nginx-on-local

## Mongo

#invoke drop-gaos-mongo-database-on-local
#invoke drop-gaos-mongo-database-on-test-server

## Gaos

#invoke deploy-gaos-to-test-server


# sandbox testing

#invoke update-nginx-on-local
#invoke start-nginx-on-local
#invoke stop-nginx-on-local

#invoke release-to-local --platform="webgl" --version="0.0.1" --is-forced  
#invoke release-bundles-to-local --platform="webgl" --version="0.0.1" --bundles-version="2" --is-forced
#invoke publish-to-local --platform="webgl" --version="0.0.1", --bundles-version="1" --is-include-build

invoke update-nginx-on-test-server
#invoke deploy-gaos-to-test-server

#invoke release-to-local --platform="webgl" --version="0.0.1" --is-forced  
#invoke publish-to-test-server --platform="webgl" --version="0.0.1", --bundles-version="1" --is-include-build 

